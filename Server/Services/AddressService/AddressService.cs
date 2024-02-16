using BlazorEcommerce.Client.Services.AuthService;
using BlazorEcommerce.Server.Data;
using BlazorEcommerce.Shared;
using Microsoft.EntityFrameworkCore;
using Stripe.Identity;

namespace BlazorEcommerce.Server.Services.AddressService
{
	public class AddressService : IAddressService
	{
		private readonly DataContext _dataContext;
		private readonly IAuthService _authService;

		public AddressService(DataContext dataContext, IAuthService authService) 
		{
			_dataContext = dataContext;
			_authService = authService;
		}

		public async Task<ServiceResponse<Address>> GetAddress(int addressId = 0)
		{
			int userId = await _authService.GetUserId();

			if(userId > 0 && addressId > -1) 
			{
				var response = new ServiceResponse<Address>();

				var address = await _dataContext.Addresses.FirstOrDefaultAsync(add => add.Id == addressId && add.UserId == userId);

				if(address != null) 
				{
					response.Data = address;
					response.Success = true;
					response.Message = "Address found!";

					return response;
				}
				else
				{
					return new ServiceResponse<Address>
					{
						Data = null,
						Success = false,
						Message = $"Could not find Address Id {addressId}"
					};
				}
			}
			else
			{
				return new ServiceResponse<Address>
				{
					Data = null,
					Success = false,
					Message = "Could not find current user Id or Address Id"
				};
			}
		}

		public async Task<ServiceResponse<Address>> AddAddress(Address newAddress)
		{
			var id = await _authService.GetUserId();

			if (id > -1)
			{
				await _dataContext.Addresses.AddAsync(newAddress);
				await _dataContext.SaveChangesAsync();

				return new ServiceResponse<Address>
				{
					Data = newAddress,
					Success = true,
					Message = "New Address has been added!"
				};
			}
			else
				return new ServiceResponse<Address>
				{
					Data = null,
					Success = false,
					Message = "New Address has been added!"
				};
		}

		public async Task<ServiceResponse<Address>> ChangeAddress(int addressId, Address newAddress)
		{
			var addressToChange = (await GetAddress(addressId)).Data;

			if(addressToChange != null)
			{
				addressToChange.FirstName = newAddress.FirstName;
				addressToChange.LastName = newAddress.LastName;
				addressToChange.Street = newAddress.Street;
				addressToChange.City = newAddress.City;
				addressToChange.StateProvince = newAddress.StateProvince;
				addressToChange.PostalCode = newAddress.PostalCode;
				addressToChange.Country = newAddress.Country;

				await _dataContext.SaveChangesAsync();

				return new ServiceResponse<Address>
				{
					Data = addressToChange,
					Success = true,
					Message = $"The address details have been changed"
				};
			}
			else
			{
				return new ServiceResponse<Address>
				{
					Data = null,
					Success = false,
					Message = $"Could not find the Address ID {addressId} to change"
				};
			}
		}

		public async Task<ServiceResponse<bool>> DeleteAddress(int addressId)
		{
			var addressToDelete = (await GetAddress(addressId)).Data;

			if( addressToDelete != null ) 
			{
				_dataContext.Addresses.Remove(addressToDelete);
				await _dataContext.SaveChangesAsync();

				return new ServiceResponse<bool>
				{
					Data = true,
					Success = true,
					Message = "Address deleted!"
				};
			}
			else
				return new ServiceResponse<bool>
				{
					Data = false,
					Success = false,
					Message = $"An address with the Id {addressId} does not exist, could not delete"
				};
		}
	}
}
