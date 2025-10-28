using System.Collections.Generic;
using System.Threading.Tasks;
using EmpresaX.POS.API.Controllers;
using EmpresaX.POS.API.DTOs;
using EmpresaX.POS.API.Repositories;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace EmpresaX.POS.API.Tests
{
    public class ProdutosControllerTests
    {
        private readonly Mock<IProdutoRepository> _repoMock;
        private readonly ProdutosController _controller;

        public ProdutosControllerTests()
        {
            _repoMock = new Mock<IProdutoRepository>();
            _controller = new ProdutosController(_repoMock.Object);
        }

        [Fact]
        public async Task GetAll_DeveRetornarListaDeProdutos()
        {
            // Arrange
            var produtos = new List<ProdutoDto> { new ProdutoDto { Id = 1, Nome = "Produto 1", Preco = 10, Estoque = 5 } };
            _repoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(produtos);

            // Act
            var result = await _controller.GetAll();

            // Assert
            var ok = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(produtos, ok.Value);
        }

        [Fact]
        public async Task GetById_ProdutoExistente_DeveRetornarOk()
        {
            // Arrange
            var produto = new ProdutoDto { Id = 1, Nome = "Produto 1", Preco = 10, Estoque = 5 };
            _repoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(produto);

            // Act
            var result = await _controller.GetById(1);

            // Assert
            var ok = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(produto, ok.Value);
        }

        [Fact]
        public async Task GetById_ProdutoInexistente_DeveRetornarNotFound()
        {
            // Arrange
            _repoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync((ProdutoDto)null);

            // Act
            var result = await _controller.GetById(1);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Create_ProdutoValido_DeveRetornarCreated()
        {
            // Arrange
            var request = new CreateProdutoRequest { Nome = "Novo", Preco = 20, Estoque = 10 };
            var produto = new ProdutoDto { Id = 2, Nome = "Novo", Preco = 20, Estoque = 10 };
            _repoMock.Setup(r => r.AddAsync(request)).ReturnsAsync(produto);

            // Act
            var result = await _controller.Create(request);

            // Assert
            var created = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal(produto, created.Value);
        }

        [Fact]
        public async Task Update_ProdutoExistente_DeveRetornarOk()
        {
            // Arrange
            var request = new UpdateProdutoRequest { Nome = "Atualizado", Preco = 30, Estoque = 15 };
            var produto = new ProdutoDto { Id = 1, Nome = "Atualizado", Preco = 30, Estoque = 15 };
            _repoMock.Setup(r => r.UpdateAsync(1, request)).ReturnsAsync(produto);

            // Act
            var result = await _controller.Update(1, request);

            // Assert
            var ok = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(produto, ok.Value);
        }

        [Fact]
        public async Task Update_ProdutoInexistente_DeveRetornarNotFound()
        {
            // Arrange
            var request = new UpdateProdutoRequest { Nome = "Atualizado", Preco = 30, Estoque = 15 };
            _repoMock.Setup(r => r.UpdateAsync(1, request)).ReturnsAsync((ProdutoDto)null);

            // Act
            var result = await _controller.Update(1, request);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Delete_ProdutoExistente_DeveRetornarNoContent()
        {
            // Arrange
            _repoMock.Setup(r => r.DeleteAsync(1)).ReturnsAsync(true);

            // Act
            var result = await _controller.Delete(1);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task Delete_ProdutoInexistente_DeveRetornarNotFound()
        {
            // Arrange
            _repoMock.Setup(r => r.DeleteAsync(1)).ReturnsAsync(false);

            // Act
            var result = await _controller.Delete(1);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
    }
}
