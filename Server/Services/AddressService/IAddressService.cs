using BlazorEcommerce.Shared;

namespace BlazorEcommerce.Server.Services.AddressService
{
	public interface IAddressService
	{
		Task<ServiceResponse<Address>> GetAddress(int addressId);
		Task<ServiceResponse<Address>> AddAddress(Address newAddress);
		Task<ServiceResponse<Address>> ChangeAddress(int addressId, Address newAddress);
		Task<ServiceResponse<bool>> DeleteAddress(int addressId);
	}
}
