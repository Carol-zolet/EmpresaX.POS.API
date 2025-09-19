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

namespace EmpresaX.POS.API.Tests.Controllers
{
    public class CategoriasControllerTests
    {
        private readonly Mock<ICategoriasService> _serviceMock;
        private readonly Mock<ILogger<CategoriasController>> _loggerMock;
        private readonly CategoriasController _controller;

        public CategoriasControllerTests()
        {
            _serviceMock = new Mock<ICategoriasService>();
            _loggerMock = new Mock<ILogger<CategoriasController>>();
            _controller = new CategoriasController(_serviceMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task GetAll_QuandoExistemCategorias_DeveRetornarOkComLista()
        {
            // Arrange
            var listaDto = new List<CategoriaDto> { new CategoriaDto { Id = 1, Nome = "Categoria Teste" } };
            _serviceMock.Setup(s => s.GetAllAsync()).ReturnsAsync(listaDto);

            // Act
            var actionResult = await _controller.GetAll();

            // Assert
            var okResult = actionResult.Should().BeOfType<OkObjectResult>().Subject;
            var categorias = okResult.Value.Should().BeOfType<List<CategoriaDto>>().Subject;
            categorias.Should().HaveCount(1);
        }
    }
}
