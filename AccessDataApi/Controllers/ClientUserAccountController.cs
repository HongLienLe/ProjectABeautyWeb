using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AccessDataApi.Repo;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AccessDataApi.Controllers
{
    [Route("api/[controller]")]
    public class ClientUserAccountController : Controller
    {
        private IClientAccountRepo _clientAccountRepo;

        public ClientUserAccountController(IClientAccountRepo clientAccountRepo)
        {
            _clientAccountRepo = clientAccountRepo;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var clients = _clientAccountRepo.GetAllClients();

            if (clients == null)
                return NoContent();
            return Ok(clients);
        }

    }
}
