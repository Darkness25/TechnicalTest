using Microsoft.AspNetCore.Mvc;
using Moq;

namespace ClientPerson.Tests
{
    [TestClass()]
    public class ClientsControllerTests
    {
        private ClientsController _controller;
        private Mock<IClientService> _mockClientService;

        [TestInitialize]
        public void Setup()
        {
            _mockClientService = new Mock<IClientService>();
            _controller = new ClientsController(_mockClientService.Object);
        }

        [TestMethod]
        public async Task GetClientes_ReturnsFailResult()
        {           
            var clients = new List<Client>
        {
            new Client { ClientId = 1, Name = "John Doe" },
            new Client { ClientId = 2, Name = "Jane Smith" },
        };

            _mockClientService.Setup(s => s.GetAllClientsAsync()).ReturnsAsync(clients);

            var result = await _controller.GetClientes();

            Assert.IsNotInstanceOfType(result, typeof(IEnumerable<ActionResult>));
            
        }

        [TestMethod]
        public async Task GetCliente_ReturnsNotFoundResult_WhenClientNotFound()
        {
            int clientId = 1;
            Client client = null;

            _mockClientService.Setup(s => s.GetClientByIdAsync(clientId)).ReturnsAsync(client);

            var result = await _controller.GetCliente(clientId);

            Assert.IsInstanceOfType(result.Result, typeof(NotFoundResult));
        }
    }
}