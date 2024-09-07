using Linka.Application.Common;
using Linka.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Linka.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AddressController(IRepository<Address> addressRepository) : ControllerBase
    {
        [HttpGet]
        [Route("{addressId}")]
        public async Task<Address> GetById
            (
            Guid addressId
            )
        {
            return await addressRepository.Get(addressId, CancellationToken.None);
        }
        [HttpGet]
        public async Task<IEnumerable<Address>> GetAll()
        {
            return await addressRepository.GetAll(CancellationToken.None);
        }
    }
}
