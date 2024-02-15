using BlazorEcommerce.Server.Services.CartService;
using BlazorEcommerce.Server.Data;
using BlazorEcommerce.Shared;
using System.Security.Claims;
using BlazorEcommerce.Shared.DTOs;
using BlazorEcommerce.Server.Services.AuthService;
using Microsoft.EntityFrameworkCore;

namespace BlazorEcommerce.Server.Services.OrderService
{
    public class OrderService : IOrderService
    {
        private readonly DataContext _dataContext;
        private readonly ICartService _cartService;
        private readonly IAuthService _authService;

        public OrderService(DataContext dataContext, ICartService cartService, IAuthService authService) 
        {
            _dataContext = dataContext;
            _cartService = cartService;
            _authService = authService;
        }

        /// <summary>
        /// Adds an Order to the Database
        /// </summary>
        public async Task<ServiceResponse<bool>> PlaceOrder(int userId)
        {
            var response = new ServiceResponse<bool>();
            var products = (await _cartService.GetCartProductsFromDatabase(userId)).Data;

            if(products != null && products.Count > 0) 
            {
                decimal totalprice = 0;
                products.ForEach(product => totalprice += (product.ProductPrice * product.ProductQuantity));

                var orderItems = new List<OrderItem>();
                products.ForEach(product => orderItems.Add(new OrderItem
                {
                    ProductId = product.ProductId,
                    ProductTypeId = product.ProductTypeId,
                    Quantity = product.ProductQuantity,
                    TotalPrice = (product.ProductPrice * product.ProductQuantity)
                }));

                Order order = new Order
                {
                    UserId = userId,
                    OrderDate = DateTime.Now,
                    TotalPrice = totalprice,
                    OrderItems = orderItems
                };

                _dataContext.Orders.Add(order);
                await _cartService.EmptyCart(userId);
                await _dataContext.SaveChangesAsync();

                response.Data = true;
                response.Success = true;
                response.Message = "Order created";
            }
            else
            {
                response.Data = false;
                response.Success = false;
                response.Message = "Could not fetch the products from the Database cart";
            }
            return response;
        }

        /// <summary>
        /// Returns a list of OrderResponses that represent every order for a User
        /// </summary>
        public async Task<ServiceResponse<List<OrderResponse>>> GetOrders()
        {
            var response = new ServiceResponse<List<OrderResponse>>();

            var orders = await _dataContext.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .Where(o => o.UserId == _authService.GetUserId())
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();

            if(orders != null)
            {
                response.Data = new List<OrderResponse>();
                orders.ForEach(order => response.Data.Add(new OrderResponse
                {
                    Id = order.Id,
                    OrderDate = order.OrderDate,
                    TotalPrice = order.TotalPrice,
                    ProductName = order.OrderItems.Count > 1 ?
                        order.OrderItems[0].Product.Name + $" and {order.OrderItems.Count - 1} more items" : order.OrderItems[0].Product.Name,
                    ProductImageUrl = order.OrderItems[0].Product.ImageUrl
                }));

                response.Success = true;
                response.Message = "Completed Succesfully";
            }
            else
            {
                response.Data = null;
                response.Success = false;
                response.Message = "Unable to fetch the User's orders";
            }
            return response;
        }

        public async Task<ServiceResponse<OrderDetailsResponse>> GetOrderDetails(int orderId)
        {
            var response = new ServiceResponse<OrderDetailsResponse>();

            var order = await _dataContext.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.ProductType)
                .Where(o => o.UserId == _authService.GetUserId() && o.Id == orderId)
                .OrderByDescending(o => o.OrderDate)
                .FirstOrDefaultAsync();

            if(order != null)
            {
                response.Data =  new OrderDetailsResponse
                {
                    OrderDate = order.OrderDate,
                    TotalPrice = order.TotalPrice,
                    Products = new List<OrderDetailsProductResponse>()
                };

                order.OrderItems.ForEach(item => response.Data.Products.Add(new OrderDetailsProductResponse
                {
                    ProductId = item.ProductId,
                    ImageUrl = item.Product.ImageUrl,
                    ProductName = item.Product.Name,
                    Quantity = item.Quantity,
                    ProductTypeName = item.ProductType.Name,
                    TotalPrice = item.TotalPrice
                }));

                response.Success = true;
                response.Message = "Order Details Found";
            }
            else
            {
                response.Data = null;
                response.Success = false;
                response.Message = $"Could not find Order with ID {orderId}";
            }
            return response;
        }

	}
}
