using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AccountTransactions
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly ContextAccount _context;
        private readonly AccountService _cuentasService;

        public AccountController(ContextAccount context, AccountService cuentasService)
        {
            _context = context;
            _cuentasService = cuentasService;
        }
        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Account>>> GetCuentas()
        {
            return await _context.Accounts.ToListAsync();
        }
        
        [HttpGet("{id}")]
        public async Task<ActionResult<Account>> GetCuenta(int id)
        {
            var cuenta = await _context.Accounts.FindAsync(id);

            if (cuenta == null)
            {
                return NotFound();
            }

            return cuenta;
        }

        [HttpPost]
        public async Task<ActionResult<Account>> PostCuenta(Account cuenta)
        {
            _context.Accounts.Add(cuenta);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCuenta), new { id = cuenta.Id }, cuenta);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutCuenta(int id, Account cuenta)
        {
            if (id != cuenta.Id)
            {
                return BadRequest();
            }

            _context.Entry(cuenta).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CuentaExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCuenta(int id)
        {
            var cuenta = await _context.Accounts.FindAsync(id);
            if (cuenta == null)
            {
                return NotFound();
            }

            _context.Accounts.Remove(cuenta);
            await _context.SaveChangesAsync();

            return Ok();
        }

        private bool CuentaExists(int id)
        {
            return _context.Accounts.Any(e => e.Id == id);
        }
    }
}