using Microsoft.EntityFrameworkCore;

namespace ClientPerson
{
    public interface IClientService
    {
        Task<IEnumerable<Client>> GetAllClientsAsync();
        Task<Client> GetClientByIdAsync(int id);
        Task<Client> CreateClientAsync(Client client);
        Task<bool> UpdateClientAsync(int id, Client client);
        Task<bool> DeleteClientAsync(int id);
    }

    public class ClientService : IClientService
    {
        private readonly ClientsContext _context;

        public ClientService(ClientsContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Client>> GetAllClientsAsync()
        {
            return await _context.Clientes.ToListAsync();
        }

        public async Task<Client> GetClientByIdAsync(int id)
        {
            return await _context.Clientes.FindAsync(id);
        }

        public async Task<Client> CreateClientAsync(Client cliente)
        {
            _context.Clientes.Add(cliente);
            await _context.SaveChangesAsync();
            return cliente;
        }

        public async Task<bool> UpdateClientAsync(int id, Client cliente)
        {
            if (id != cliente.ClientId)
            {
                return false;
            }

            _context.Entry(cliente).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ClienteExists(id))
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

        public async Task<bool> DeleteClientAsync(int id)
        {
            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente == null)
            {
                return false;
            }

            _context.Clientes.Remove(cliente);
            await _context.SaveChangesAsync();

            return true;
        }

        private bool ClienteExists(int id)
        {
            return _context.Clientes.Any(e => e.ClientId == id);
        }

        

    }
}
