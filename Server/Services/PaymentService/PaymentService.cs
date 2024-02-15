using BlazorEcommerce.Server.Services.AuthService;
using BlazorEcommerce.Server.Services.CartService;
using BlazorEcommerce.Server.Services.OrderService;
using BlazorEcommerce.Shared;
using Microsoft.AspNetCore.Http.Connections;
using Stripe;
using Stripe.Checkout;

namespace BlazorEcommerce.Server.Services.PaymentService
{
    public class PaymentService : IPaymentService
    {
        private readonly ICartService _cartService;
        private readonly IAuthService _authService;
        private readonly IOrderService _orderService;

        //This should be the secret key linked to Strip to get WebHooks to work, but
        //since I'm having Dev Deployment issues I can't test this, so this is just a placeholder
        const string secret = "secret-key";

        public PaymentService(ICartService cartService, IAuthService authService, IOrderService orderService) 
        {
            _authService = authService;
            _cartService = cartService;
            _orderService = orderService;

            StripeConfiguration.ApiKey = "sk_test_51OkBhOB8kxWjoxVsDc46xutBmy6aiInxmHYqor43JYjptZrmE9lvWp4S8qbCkS0Rhs1RHXiGsfgPZ1bTAretOOax00mAWKkcGF";
        }
        
        /// <summary>
        /// Creates a new Checkout Session with Stripe. Returns Null if the User/Cart cannot
        /// be retrieved.
        /// </summary>
        public async Task<Session> CreateCheckoutSession()
        {
            var cartResult = await _cartService.GetCartProductsFromDatabase();

            if (cartResult != null && cartResult.Success)
            {
                var products = cartResult.Data;
                var lineItems = new List<SessionLineItemOptions>();

                products.ForEach(product => lineItems.Add(new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions()
                    {
                        UnitAmountDecimal = product.ProductPrice * 100,
                        Currency = "usd",
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = product.ProductName,
                            Images = new List<string> { product.ProductImageUrl }
                        }
                    },
                    Quantity = product.ProductQuantity
                }));

                var options = new SessionCreateOptions
                {
                    CustomerEmail = _authService.GetUserEmail(),
                    ShippingAddressCollection = new SessionShippingAddressCollectionOptions
                    {
                        AllowedCountries = new List<string>
                        {
                            "US",
                            "CA",
                            "DK",
                            "UK",
                            "JP",
                            "KR"
                        }
                    },
                    PaymentMethodTypes = new List<string>
                    {
                        "card"
                    },
                    LineItems = lineItems,
                    Mode = "payment",
                    SuccessUrl = "http://localhost:5272/order-success",
                    CancelUrl = "http://localhost:5272/cart"
                };

                var service = new SessionService();
                Session session = service.Create(options);
                return session;
            }
            else
                return null;
        }

        /// <summary>
        /// Gets an HttpRequest from the Stripe WebHook after customer places an order,
        /// and saves a copy of that order to the Database
        /// </summary>
        public async Task<ServiceResponse<bool>> FulfillOrder(HttpRequest httpRequest)
        {
            var json = await new StreamReader(httpRequest.Body).ReadToEndAsync();

            try 
            {
                var stripeEvent = EventUtility.ConstructEvent(
                    json,
                    httpRequest.Headers["Stripe-Signature"],
                    secret
                );

                if( stripeEvent.Type == Events.CheckoutSessionCompleted ) 
                {
                    var session = stripeEvent.Data.Object as Session;
                    var user = await _authService.GetUserByEmail(session.CustomerEmail);

                    await _orderService.PlaceOrder(user.Id);
                }

                return new ServiceResponse<bool> { Data = true, Success = true, Message = "Order Fulfilled" };
            }
            catch (StripeException ex) 
            {
                return new ServiceResponse<bool>
                {
                    Data = false,
                    Success = false,
                    Message = ex.Message,
                };
			}
        }

	}
}
