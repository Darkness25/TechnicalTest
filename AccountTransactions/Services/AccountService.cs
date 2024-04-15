using Microsoft.EntityFrameworkCore;

namespace AccountTransactions
{
    public class AccountService
    {
        private readonly ContextAccount _context;

        public AccountService(ContextAccount context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Account>> ObtenerTodasCuentas()
        {
            return await _context.Accounts.ToListAsync();
        }

        public async Task<Account> ObtenerCuentaPorId(int id)
        {
            return await _context.Accounts.FindAsync(id);
        }

        public async Task<Account> CrearCuenta(Account cuenta)
        {
            _context.Accounts.Add(cuenta);
            await _context.SaveChangesAsync();
            return cuenta;
        }

        public async Task<bool> ActualizarCuenta(int id, Account cuenta)
        {
            if (id != cuenta.Id)
            {
                return false;
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
                    return false;
                }
                else
                {
                    throw;
                }
            }

            return true;
        }

        public async Task<bool> EliminarCuenta(int id)
        {
            var cuenta = await _context.Accounts.FindAsync(id);
            if (cuenta == null)
            {
                return false;
            }

            _context.Accounts.Remove(cuenta);
            await _context.SaveChangesAsync();

            return true;
        }

        private bool CuentaExists(int id)
        {
            return _context.Accounts.Any(e => e.Id == id);
        }

        public async Task<IEnumerable<Account>> GetCuentasByClientIdAsync(int clientId)
        {
            return await _context.Accounts.Where(a => a.ClientId == clientId).ToListAsync();
        }

    }
}
