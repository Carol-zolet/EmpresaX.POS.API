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
        private readonly Mock<IProdutoService> _serviceMock;
        private readonly Mock<ILogger<ProdutosController>> _loggerMock;
        private readonly ProdutosController _controller;

        public ProdutosControllerTests()
        {
            _serviceMock = new Mock<IProdutoService>();
            _loggerMock = new Mock<ILogger<ProdutosController>>();
            _controller = new ProdutosController(_serviceMock.Object, _loggerMock.Object);
        }

        // --- Testes GET ---
        [Fact]
        public async Task GetById_QuandoProdutoExiste_DeveRetornarOkComProdutoDto()
        {
            var produtoDto = new ProdutoDto { Id = 1, Nome = "Produto Teste" };
            _serviceMock.Setup(s => s.GetByIdAsync(1)).ReturnsAsync(produtoDto);
            var actionResult = await _controller.GetById(1);
            var okResult = actionResult.Should().BeOfType<OkObjectResult>().Subject;
            okResult.Value.Should().BeOfType<ProdutoDto>().Which.Id.Should().Be(1);
        }

        [Fact]
        public async Task GetById_QuandoProdutoNaoExiste_DeveRetornarNotFound()
        {
            _serviceMock.Setup(s => s.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((ProdutoDto?)null);
            var actionResult = await _controller.GetById(99);
            actionResult.Should().BeOfType<NotFoundResult>();
        }

        // --- Testes POST ---
        [Fact]
        public async Task Create_ComDadosValidos_DeveRetornarCreated()
        {
            var createDto = new CreateProdutoDto { Nome = "Novo Produto", Preco = 10.50m };
            var produtoCriadoDto = new ProdutoDto { Id = 101, Nome = "Novo Produto", Preco = 10.50m };
            _serviceMock.Setup(s => s.CreateAsync(createDto)).ReturnsAsync(produtoCriadoDto);
            var actionResult = await _controller.Create(createDto);
            var createdResult = actionResult.Should().BeOfType<CreatedAtActionResult>().Subject;
            createdResult.StatusCode.Should().Be((int)HttpStatusCode.Created);
            createdResult.RouteValues.Should().ContainKey("id").WhoseValue.Should().Be(produtoCriadoDto.Id);
        }

        [Fact]
        public async Task Create_ComModeloInvalido_DeveRetornarBadRequest()
        {
            var createDto = new CreateProdutoDto { Nome = "" };
            _controller.ModelState.AddModelError("Nome", "O campo Nome é obrigatório.");
            var actionResult = await _controller.Create(createDto);
            actionResult.Should().BeOfType<BadRequestObjectResult>();
            _serviceMock.Verify(s => s.CreateAsync(It.IsAny<CreateProdutoDto>()), Times.Never());
        }

        // --- Testes PUT ---
        [Fact]
        public async Task Update_QuandoProdutoExiste_DeveRetornarNoContent()
        {
            var idProduto = 1;
            var updateDto = new UpdateProdutoDto { Nome = "Produto Atualizado", Preco = 12.00m };
            _serviceMock.Setup(s => s.UpdateAsync(idProduto, updateDto)).Returns(Task.CompletedTask);
            var actionResult = await _controller.Update(idProduto, updateDto);
            actionResult.Should().BeOfType<NoContentResult>();
            _serviceMock.Verify(s => s.UpdateAsync(idProduto, updateDto), Times.Once);
        }

        [Fact]
        public async Task Update_QuandoProdutoNaoExiste_DeveRetornarNotFound()
        {
            var idProduto = 99;
            var updateDto = new UpdateProdutoDto { Nome = "Produto Fantasma" };
            _serviceMock.Setup(s => s.UpdateAsync(idProduto, updateDto)).ThrowsAsync(new KeyNotFoundException());
            var actionResult = await _controller.Update(idProduto, updateDto);
            actionResult.Should().BeOfType<NotFoundResult>();
        }

        // --- Testes DELETE ---
        [Fact]
        public async Task Delete_QuandoProdutoExiste_DeveRetornarNoContent()
        {
            var idProduto = 1;
            _serviceMock.Setup(s => s.DeleteAsync(idProduto)).Returns(Task.CompletedTask);
            var actionResult = await _controller.Delete(idProduto);
            actionResult.Should().BeOfType<NoContentResult>();
            _serviceMock.Verify(s => s.DeleteAsync(idProduto), Times.Once);
        }

        [Fact]
        public async Task Delete_QuandoProdutoNaoExiste_DeveRetornarNotFound()
        {
            var idProduto = 99;
            _serviceMock.Setup(s => s.DeleteAsync(idProduto)).ThrowsAsync(new KeyNotFoundException());
            var actionResult = await _controller.Delete(idProduto);
            actionResult.Should().BeOfType<NotFoundResult>();
        }
    }
}

