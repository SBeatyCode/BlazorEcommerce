using BlazorEcommerce.Server.Services.AddressService;
using BlazorEcommerce.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BlazorEcommerce.Server.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	[Authorize]
	public class AddressController : ControllerBase
	{
		private readonly IAddressService _addressService;

		public AddressController(IAddressService addressService) 
		{
			_addressService = addressService;
		}

		[HttpGet("get/{addressId}")]
		public async Task<ActionResult<ServiceResponse<Address>>> GetAddress(int addressId = 0)
		{
			var response = await _addressService.GetAddress(addressId);

			if (response == null)
				return NotFound(response);
			else
				return Ok(response);
		}

		[HttpPost("add/{newAddress}")]
		public async Task<ActionResult<ServiceResponse<Address>>> AddAddress(Address newAddress)
		{
			var response = await _addressService.AddAddress(newAddress);

			if (response == null)
				return BadRequest(response);
			else
				return Ok(response);
		}

		[HttpPut("change/{addressId}/{newAddress}")]
		public async Task<ActionResult<ServiceResponse<Address>>> ChangeAddress(int addressId, Address newAddress)
		{
			var response = await _addressService.ChangeAddress(addressId, newAddress);

			if (response == null)
				return NotFound(response);
			else
				return Ok(response);
		}

		[HttpDelete("delete/{addressId}")]
		public async Task<ActionResult<ServiceResponse<bool>>> DeleteAddress(int addressId)
		{
			var response = await _addressService.DeleteAddress(addressId);

			if (response == null)
				return NotFound(response);
			else
				return Ok(response);
		}
	}
}
