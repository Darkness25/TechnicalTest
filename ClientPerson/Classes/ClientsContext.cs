using Microsoft.EntityFrameworkCore;

namespace ClientPerson
{
    public class ClientsContext : DbContext
    {
        public ClientsContext(DbContextOptions<ClientsContext> options)
        : base(options)
        {
        }

        public DbSet<Person> Personas { get; set; }
        public DbSet<Client> Clientes { get; set; }
    }
}
