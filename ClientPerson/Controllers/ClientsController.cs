using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ClientPerson
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientsController : ControllerBase
    {
        private readonly IClientService _clientService;

        public ClientsController(IClientService clientService)
        {
            _clientService = clientService;
        }
        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Client>>> GetClientes()
        {
            return Ok(await _clientService.GetAllClientsAsync());
        }
        
        [HttpGet("{id}")]
        public async Task<ActionResult<Client>> GetCliente(int id)
        {
            var cliente = await _clientService.GetClientByIdAsync(id);

            if (cliente == null)
            {
                return NotFound();
            }

            return cliente;
        }
        
        [HttpPost]
        public async Task<ActionResult<Client>> PostCliente(Client cliente)
        {
            await _clientService.CreateClientAsync(cliente);
            return CreatedAtAction(nameof(GetCliente), new { id = cliente.ClientId }, cliente);
        }
        
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCliente(int id, Client cliente)
        {
            if (id != cliente.ClientId)
            {
                return BadRequest();
            }

            var result = await _clientService.UpdateClientAsync(id, cliente);
            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCliente(int id)
        {
            var result = await _clientService.DeleteClientAsync(id);
            if (!result)
            {
                return NotFound();
            }

            return Ok("Ok");
        }        
    }
}