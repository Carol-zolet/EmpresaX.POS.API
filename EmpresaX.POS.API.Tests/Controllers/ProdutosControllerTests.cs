using Xunit;
using FluentAssertions;
using Moq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using EmpresaX.POS.API.Controllers;
using EmpresaX.POS.API.Services;
using EmpresaX.POS.API.Modelos.DTOs;

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

        [Fact]
        public async Task GetById_QuandoProdutoExiste_DeveRetornarOkComProdutoDto()
        {
            var idProduto = 1;
            var produtoDto = new ProdutoDto { Id = idProduto, Nome = "Produto Teste" };
            _serviceMock.Setup(s => s.GetByIdAsync(idProduto)).ReturnsAsync(produtoDto);

            var actionResult = await _controller.GetById(idProduto);

            var okResult = actionResult.Should().BeOfType<OkObjectResult>().Subject;
            var produtoRetornado = okResult.Value.Should().BeOfType<ProdutoDto>().Subject;
            produtoRetornado.Id.Should().Be(idProduto);
        }
    }
}