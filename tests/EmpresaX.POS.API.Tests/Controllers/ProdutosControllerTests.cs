using Xunit;
using FluentAssertions;
using Moq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System.Collections.Generic;
using EmpresaX.POS.API.Controllers;
using EmpresaX.POS.API.Services;
using EmpresaX.POS.API.Modelos.DTOs;
using System.Net;

namespace EmpresaX.POS.API.Tests.Controllers
{
    public class ProdutosControllerTests
    {
        private readonly Mock<IProdutosService> _serviceMock;
        private readonly Mock<ILogger<ProdutosController>> _loggerMock;
        private readonly ProdutosController _controller;

        public ProdutosControllerTests()
        {
            _serviceMock = new Mock<IProdutosService>();
            _loggerMock = new Mock<ILogger<ProdutosController>>();
            _controller = new ProdutosController(_serviceMock.Object, _loggerMock.Object);
        }

        // --- Testes GET ---
        [Fact]
        public async Task GetById_QuandoProdutoExiste_DeveRetornarOkComProdutoDto() { /* ... código do teste ... */ }

        [Fact]
        public async Task GetById_QuandoProdutoNaoExiste_DeveRetornarNotFound() { /* ... código do teste ... */ }

        // --- Testes POST ---
        [Fact]
        public async Task Create_ComDadosValidos_DeveRetornarCreated() { /* ... código do teste ... */ }

        [Fact]
        public async Task Create_ComModeloInvalido_DeveRetornarBadRequest() { /* ... código do teste ... */ }

        // --- Testes PUT ---
        [Fact]
        public async Task Update_QuandoProdutoExiste_DeveRetornarNoContent() { /* ... código do teste ... */ }

        [Fact]
        public async Task Update_QuandoProdutoNaoExiste_DeveRetornarNotFound() { /* ... código do teste ... */ }

        // --- TESTES DELETE ADICIONADOS ---
        [Fact]
        public async Task Delete_QuandoProdutoExiste_DeveRetornarNoContent()
        {
            // Arrange
            var idProduto = 1;
            _serviceMock.Setup(s => s.DeleteAsync(idProduto)).Returns(Task.CompletedTask);

            // Act
            var actionResult = await _controller.Delete(idProduto);

            // Assert
            actionResult.Should().BeOfType<NoContentResult>();
            _serviceMock.Verify(s => s.DeleteAsync(idProduto), Times.Once);
        }

        [Fact]
        public async Task Delete_QuandoProdutoNaoExiste_DeveRetornarNotFound()
        {
            // Arrange
            var idProduto = 99;
            _serviceMock.Setup(s => s.DeleteAsync(idProduto)).ThrowsAsync(new KeyNotFoundException());

            // Act
            var actionResult = await _controller.Delete(idProduto);

            // Assert
            actionResult.Should().BeOfType<NotFoundResult>();
        }
    }
}