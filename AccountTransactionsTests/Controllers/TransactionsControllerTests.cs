using Microsoft.AspNetCore.Mvc;
using Moq;

namespace AccountTransactions.Tests
{
    [TestClass()]
    public class TransactionsControllerTests
    {
        private TransactionsController _controller;
        private Mock<ITransactionService> _mockTransactionService;

        [TestInitialize]
        public void Setup()
        {
            _mockTransactionService = new Mock<ITransactionService>();
            _controller = new TransactionsController(_mockTransactionService.Object, null);
        }

        [TestMethod]
        public async Task GetTransactions_ReturnsOkResult()
        {
            var transactions = new List<Transactions>
        {
            new Transactions { Id = 1, AccountNumber = "123456", ClientId = 1, Balance = 2000, Date = DateTime.Now, TransactionType = "Retiro", Value = 170 },
            new Transactions { Id = 2, AccountNumber = "123466", ClientId = 2, Balance = 4000, Date = DateTime.Now, TransactionType = "Retiro", Value = 170  },

        };
            _mockTransactionService.Setup(s => s.GetTransactionsAsync()).ReturnsAsync(transactions);

            var result = await _controller.GetTransactions();

            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            var okResult = result as OkObjectResult;
            Assert.IsInstanceOfType(okResult.Value, typeof(IEnumerable<Transactions>));
        }

        [TestMethod]
        public async Task GetTransaction_WithInvalidId_ReturnsNotFoundResult()
        {            
            int id = 1;
            _mockTransactionService.Setup(s => s.GetTransactionByIdAsync(id)).ReturnsAsync(null as Transactions);
          
            var result = await _controller.GetTransaction(id);

            Assert.IsInstanceOfType<NotFoundResult>(result);
        }

        [TestMethod]
        public async Task CreateTransaction_ReturnsCreatedAtActionResult()
        {            
            var newTransaction = new Transactions { Id = 1, AccountNumber = "123456", ClientId = 1, Balance = 2000, Date = DateTime.Now, TransactionType = "Retiro", Value = 170 };
            _mockTransactionService.Setup(s => s.CreateTransactionAsync(It.IsAny<Transactions>())).ReturnsAsync(newTransaction);
           
            var result = await _controller.CreateTransaction(newTransaction);

            Assert.IsInstanceOfType(result, typeof(CreatedAtActionResult));            
           
        }
    }
}