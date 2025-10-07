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
    public class CategoriasControllerTests
    {
        private readonly Mock<ICategoriaService> _serviceMock;
        private readonly Mock<ILogger<CategoriasController>> _loggerMock;
        private readonly CategoriasController _controller;

        public CategoriasControllerTests()
        {
            _serviceMock = new Mock<ICategoriaService>();
            _loggerMock = new Mock<ILogger<CategoriasController>>();
            _controller = new CategoriasController(_serviceMock.Object, _loggerMock.Object);
        }

        // --- Testes GET All ---
        [Fact]
        public async Task GetAll_QuandoExistemCategorias_DeveRetornarOkComLista()
        {
            var listaDto = new List<CategoriaDto> { new CategoriaDto { Id = 1, Nome = "Bebidas" } };
            _serviceMock.Setup(s => s.GetAllAsync()).ReturnsAsync(listaDto);
            var actionResult = await _controller.GetAll();
            var okResult = actionResult.Should().BeOfType<OkObjectResult>().Subject;
            var categorias = okResult.Value.Should().BeOfType<List<CategoriaDto>>().Subject;
            categorias.Should().HaveCount(1);
        }

        [Fact]
        public async Task GetAll_QuandoNaoExistemCategorias_DeveRetornarOkComListaVazia()
        {
            _serviceMock.Setup(s => s.GetAllAsync()).ReturnsAsync(new List<CategoriaDto>());
            var actionResult = await _controller.GetAll();
            var okResult = actionResult.Should().BeOfType<OkObjectResult>().Subject;
            var categorias = okResult.Value.Should().BeOfType<List<CategoriaDto>>().Subject;
            categorias.Should().BeEmpty();
        }

        // --- Testes GET {id} ---
        [Fact]
        public async Task GetById_QuandoCategoriaExiste_DeveRetornarOkComCategoriaDto()
        {
            var categoriaDto = new CategoriaDto { Id = 1, Nome = "Laticínios" };
            _serviceMock.Setup(s => s.GetByIdAsync(1)).ReturnsAsync(categoriaDto);
            var actionResult = await _controller.GetById(1);
            var okResult = actionResult.Should().BeOfType<OkObjectResult>().Subject;
            okResult.Value.Should().BeOfType<CategoriaDto>().Which.Id.Should().Be(1);
        }

        [Fact]
        public async Task GetById_QuandoCategoriaNaoExiste_DeveRetornarNotFound()
        {
            _serviceMock.Setup(s => s.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((CategoriaDto?)null);
            var actionResult = await _controller.GetById(99);
            actionResult.Should().BeOfType<NotFoundResult>();
        }

        // --- Testes POST (Create) ---
        [Fact]
        public async Task Create_ComDadosValidos_DeveRetornarCreated()
        {
            var createDto = new CreateCategoriaDto { Nome = "Mercearia" };
            var categoriaCriadaDto = new CategoriaDto { Id = 101, Nome = "Mercearia" };
            _serviceMock.Setup(s => s.CreateAsync(createDto)).ReturnsAsync(categoriaCriadaDto);
            var actionResult = await _controller.Create(createDto);
            var createdResult = actionResult.Should().BeOfType<CreatedAtActionResult>().Subject;
            createdResult.StatusCode.Should().Be((int)HttpStatusCode.Created);
        }

        [Fact]
        public async Task Create_ComModeloInvalido_DeveRetornarBadRequest()
        {
            _controller.ModelState.AddModelError("Nome", "O campo Nome é obrigatório.");
            var actionResult = await _controller.Create(new CreateCategoriaDto { Nome = "" });
            actionResult.Should().BeOfType<BadRequestObjectResult>();
            _serviceMock.Verify(s => s.CreateAsync(It.IsAny<CreateCategoriaDto>()), Times.Never());
        }

        // --- Testes PUT (Update) ---
        [Fact]
        public async Task Update_QuandoCategoriaExiste_DeveRetornarNoContent()
        {
            var updateDto = new CategoriaDto { Id = 1, Nome = "Bebidas Alcoólicas" };
            _serviceMock.Setup(s => s.UpdateAsync(1, updateDto)).Returns(Task.CompletedTask);
            var actionResult = await _controller.Update(1, updateDto);
            actionResult.Should().BeOfType<NoContentResult>();
        }

        [Fact]
        public async Task Update_QuandoCategoriaNaoExiste_DeveRetornarNotFound()
        {
            var updateDto = new CategoriaDto { Id = 99, Nome = "Inexistente" };
            _serviceMock.Setup(s => s.UpdateAsync(99, updateDto)).ThrowsAsync(new KeyNotFoundException());
            var actionResult = await _controller.Update(99, updateDto);
            actionResult.Should().BeOfType<NotFoundResult>();
        }

        // --- Testes DELETE ---
        [Fact]
        public async Task Delete_QuandoCategoriaExiste_DeveRetornarNoContent()
        {
            _serviceMock.Setup(s => s.DeleteAsync(1)).Returns(Task.CompletedTask);
            var actionResult = await _controller.Delete(1);
            actionResult.Should().BeOfType<NoContentResult>();
        }

        [Fact]
        public async Task Delete_QuandoCategoriaNaoExiste_DeveRetornarNotFound()
        {
            _serviceMock.Setup(s => s.DeleteAsync(99)).ThrowsAsync(new KeyNotFoundException());
            var actionResult = await _controller.Delete(99);
            actionResult.Should().BeOfType<NotFoundResult>();
        }
    }
}
