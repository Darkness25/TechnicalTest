using Microsoft.EntityFrameworkCore;

namespace AccountTransactions
{
    public class ContextAccount : DbContext
    {
        public ContextAccount(DbContextOptions<ContextAccount> options)
        : base(options)
        {
        }

        public DbSet<Account> Accounts { get; set; }
        public DbSet<Transactions> Transactions { get; set; }
    }
}
