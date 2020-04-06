using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AccessDataApi.HTTPModels;
using AccessDataApi.Repo;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AccessDataApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class ClientUserAccountController : Controller
    {
        private IClientAccountRepo _clientAccountRepo;

        public ClientUserAccountController(IClientAccountRepo clientAccountRepo)
        {
            _clientAccountRepo = clientAccountRepo;
        }

        //Finish Crud

        [HttpGet]
        public IActionResult Get()
        {
            var clients = _clientAccountRepo.GetAllClients();

            if (clients == null)
                return NotFound("Database contains No Client Accs");

            return Ok(clients);
        }

        [HttpPost]
        public IActionResult CreateClientAccount([FromBody]ClientForm clientForm)
        {
            if (!ModelState.IsValid)
                return BadRequest("Model state is not valid");

            var response = _clientAccountRepo.CreateClientAccount(clientForm);

            return Ok(response);
        }

        [HttpPost("update/{id}")]
        public IActionResult UpdateClientAccount(int id, [FromBody]ClientForm clientForm)
        {
            if (!ModelState.IsValid)
                return BadRequest("Model state is not valid");

            var response = _clientAccountRepo.UpdateClientAccount(id, clientForm);

            if (response.StartsWith("C"))
                return NotFound(response);

            return Ok(response);
        }

        [HttpDelete("remove/{id}")]
        public IActionResult RemoveClientAccount(int id)
        {
            var response = _clientAccountRepo.DeleteClientAccount(id);

            return Ok(response);
        }
        [HttpGet("get/{id}")]
        public IActionResult GetClientAccount(int id)
        {
            var response = _clientAccountRepo.GetClientAccount(id);

            if (response == null)
                return NotFound($"Client Acc : {id} does not exist");
            return Ok(response);
        }


    }
}
