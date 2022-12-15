using Contracts;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CompanyEmployess.Controllers
{
    [Route("api/clients")]
    [ApiController]
    public class ClientsV2Controller : ControllerBase
    {
        private readonly IRepositoryManager _repository;
        public ClientsV2Controller(IRepositoryManager repository)
        {
            _repository = repository;
        }
        [HttpGet]
        public async Task<IActionResult> GetCompanies()
        {
            var clients = await
           _repository.Client.GetAllClientsAsync(trackChanges: false);
            return Ok(clients);
        }
    }
}
