using Xunit;
using FluentAssertions;
using Moq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using EmpresaX.POS.API.Controllers;
using EmpresaX.POS.API.Services;

namespace EmpresaX.POS.API.Tests.Controllers
{
    public class ContasControllerTests
    {
        private readonly Mock<IContaService> _serviceMock;
        private readonly Mock<ILogger<ContasController>> _loggerMock;
        private readonly ContasController _controller;

        public ContasControllerTests()
        {
            _serviceMock = new Mock<IContaService>();
            _loggerMock = new Mock<ILogger<ContasController>>();
            _controller = new ContasController(_serviceMock.Object, _loggerMock.Object);
        }

        [Fact]
        public void TestePlaceholder_DevePassar()
        {
            // Arrange
            bool condicao = true;

            // Act

            // Assert
            Assert.True(condicao);
        }
    }
}
