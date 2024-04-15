using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AccountTransactions
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionsController : ControllerBase
    {
        private readonly ITransactionService _transactionService;
        private readonly AccountService _cuentasService;

        public TransactionsController(ITransactionService transactionService, AccountService cuentasService)
        {
            _transactionService = transactionService;
            _cuentasService = cuentasService;
        }
        
        [HttpGet]
        public async Task<IActionResult> GetTransactions()
        {
            var transaction = await _transactionService.GetTransactionsAsync();
            return Ok(transaction);
        }

        [HttpPost]
        public async Task<IActionResult> CreateTransaction(Transactions transaction)
        {
            var newTransaction = await _transactionService.CreateTransactionAsync(transaction);
            return CreatedAtAction(nameof(GetTransaction), new { id = newTransaction.Id }, newTransaction);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTransaction(int id)
        {
            var transaction = await _transactionService.GetTransactionByIdAsync(id);
            if (transaction == null)
            {
                return NotFound();
            }
            return Ok(transaction);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTransaction(int id, Transactions transaction)
        {
            if (id != transaction.Id)
            {
                return BadRequest();
            }
            await _transactionService.UpdateTransactionAsync(transaction);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTransaction(int id)
        {
            await _transactionService.DeleteTransactionAsync(id);
            return NoContent();
        }
        
        [HttpGet("/reportes")]
        public async Task<IActionResult> GenerarReporteEstadoCuenta(DateTime fechaInicio, DateTime fechaFin, int clienteId)
        {
            var cuentas = await _cuentasService.GetCuentasByClientIdAsync(clienteId);

            var reporte = new List<object>();

            foreach (var cuenta in cuentas)
            {               
                var movimientos = await _transactionService.GetTransactionsByAccountIdDateAsync(cuenta.Id, fechaInicio, fechaFin);

                if (!movimientos.Any())
                {
                    return BadRequest("No se encontraron movimientos para las fechas especificadas.");
                }
              
                foreach (var movimiento in movimientos)
                {
                    var movimientoInfo = new
                    {
                        Fecha = movimiento.Date,
                        ClienteId = cuenta.ClientId,
                        NumeroCuenta = movimiento.AccountNumber,
                        Tipo = cuenta.AccountType,
                        SaldoInicial = movimiento.Balance,
                        Estado = cuenta.State,
                        Movimiento = movimiento.Value,
                        SaldoDisponible = cuenta.InitialBalance
                    };

                    reporte.Add(movimientoInfo);
                }
            }

            return Ok(reporte);
        }
    }

}
