using System.Transactions;
using Microsoft.EntityFrameworkCore;

namespace AccountTransactions
{
    public interface ITransactionService
    {
        Task<IEnumerable<Transactions>> GetTransactionsAsync();
        Task<Transactions> GetTransactionByIdAsync(int id);
        Task<Transactions> CreateTransactionAsync(Transactions transaction);
        Task UpdateTransactionAsync(Transactions transaction);
        Task DeleteTransactionAsync(int id);
        Task<IEnumerable<Transactions>> GetTransactionsByAccountIdAsync(int accountId);
        Task<IEnumerable<Transactions>> GetTransactionsByAccountIdDateAsync(int accountId, DateTime fechaInicio, DateTime fechaFin);
    }
    public class TransactionsService : ITransactionService
    {
        private readonly ContextAccount _context;

        public TransactionsService(ContextAccount context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Transactions>> GetTransactionsAsync()
        {
            return await _context.Transactions.ToListAsync();
        }

        public async Task<Transactions> GetTransactionByIdAsync(int id)
        {
            return await _context.Transactions.FindAsync(id);
        }

        public async Task<Transactions> CreateTransactionAsync(Transactions transaction)
        {
            using (var transactionScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {                   
                    var account = await _context.Accounts.FirstOrDefaultAsync(a => a.AccountNumber == transaction.AccountNumber);
                    if (account == null)
                    {
                        throw new InvalidOperationException("La cuenta asociada a la transacción no existe.");
                    }
                    if (transaction.Value < 0 && account.InitialBalance < Math.Abs(transaction.Value))
                    {
                        throw new InvalidOperationException("Saldo no disponible para realizar la transacción.");
                    }
                    _context.Transactions.Add(transaction);
                    await _context.SaveChangesAsync();
                    account.InitialBalance += transaction.Value;
                    await _context.SaveChangesAsync();
                    transactionScope.Complete();

                    return transaction;
                }
                catch (InvalidOperationException)
                {                    
                    transactionScope.Dispose();
                    throw;                    
                }
            }
            
        }

        public async Task UpdateTransactionAsync(Transactions transaction)
        {            
            await _context.SaveChangesAsync();
        }

        public async Task DeleteTransactionAsync(int id)
        {
            var transaction = await _context.Transactions.FindAsync(id);
            if (transaction != null)
            {
                _context.Transactions.Remove(transaction);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Transactions>> GetTransactionsByAccountIdAsync(int accountId)
        {
            return await _context.Transactions.Where(m => m.Id == accountId).ToListAsync();
        }

        public async Task<IEnumerable<Transactions>> GetTransactionsByAccountIdDateAsync(int accountId, DateTime fechaInicio, DateTime fechaFin)
        {
            return await _context.Transactions
        .Where(m => m.Id == accountId && m.Date >= fechaInicio && m.Date <= fechaFin)
        .ToListAsync();
        }
    }
}
