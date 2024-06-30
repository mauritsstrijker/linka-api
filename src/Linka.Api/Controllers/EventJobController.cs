using Linka.Application.Common;
using Linka.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Linka.Api.Controllers

{
    [ApiController]
    [Route("[controller]")]
    public class EventJobController(IRepository<EventJob> eventJobRepository) : ControllerBase
    {
        [HttpGet]
        [Route("{eventJobId}")]
        public async Task<EventJob> GetById
            (
            Guid eventJobId
            )
        {
            return await eventJobRepository.GetFirstAsync(a => a.Id == eventJobId, CancellationToken.None);
        }
        [HttpGet]
        public async Task<IEnumerable<EventJob>> GetAll()
        {
            return await eventJobRepository.GetAsync(CancellationToken.None);
        }
    }
}
