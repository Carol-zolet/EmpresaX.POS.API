using Xunit;
using FluentAssertions;
using Moq;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Collections.Generic;
using System.Threading.Tasks;
using EmpresaX.POS.API.Controllers;
// TODO: Crie e referencie os DTOs e a Interface do Serviço
// using EmpresaX.POS.API.DTOs;
// using EmpresaX.POS.API.Services;

namespace EmpresaX.POS.API.Tests.Controllers
{
    public class ContasControllerTests
    {
        private readonly Mock<IContaService> _serviceMock;
        private readonly ContasController _controller;

        public ContasControllerTests()
        {
            _serviceMock = new Mock<IContaService>();
            _controller = new ContasController(_serviceMock.Object);
        }

        [Fact]
        public async Task GetContas_QuandoExistemContas_DeveRetornarOkComLista()
        {
            // Arrange (Preparação)
            var listaDeContas = new List<ContaDto> { new ContaDto { Id = 1, Descricao = "Conta Mockada" } };
            _serviceMock.Setup(s => s.GetAllAsync()).ReturnsAsync(listaDeContas);

            // Act (Ação)
            var actionResult = await _controller.GetContas();

            // Assert (Verificação)
            var okResult = actionResult.Should().BeOfType<OkObjectResult>().Subject;
            var contas = okResult.Value.Should().BeOfType<List<ContaDto>>().Subject;
            contas.Should().BeEquivalentTo(listaDeContas);
        }

        [Fact]
        public async Task GetConta_ComIdValido_DeveRetornarOkComConta()
        {
            // Arrange
            var id = 1;
            var contaDto = new ContaDto { Id = id, Descricao = $"Conta {id}" };
            _serviceMock.Setup(s => s.GetByIdAsync(id)).ReturnsAsync(contaDto);

            // Act
            var actionResult = await _controller.GetConta(id);

            // Assert
            var okResult = actionResult.Should().BeOfType<OkObjectResult>().Subject;
            var conta = okResult.Value.Should().BeOfType<ContaDto>().Subject;
            conta.Id.Should().Be(id);
        }

        [Fact]
        public async Task GetConta_ComIdInexistente_DeveRetornarNotFound()
        {
            // Arrange
            var id = 99;
            // Configura o mock para retornar nulo, simulando que a conta não foi encontrada
            _serviceMock.Setup(s => s.GetByIdAsync(id)).ReturnsAsync((ContaDto?)null);

            // Act
            var actionResult = await _controller.GetConta(id);

            // Assert
            actionResult.Should().BeOfType<NotFoundResult>();
        }
        
        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public async Task GetConta_ComIdInvalido_DeveRetornarBadRequest(int invalidId)
        {
            // Act
            var actionResult = await _controller.GetConta(invalidId);

            // Assert
            // A validação de ID (<= 0) geralmente ocorre no controller antes de chamar o serviço.
            // Por isso, não precisamos de um _serviceMock.Setup() aqui.
            actionResult.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async Task CreateConta_ComDadosValidos_DeveRetornarCreated()
        {
            // Arrange
            var novaContaDto = new CreateContaDto { Descricao = "Nova Conta" };
            var contaCriadaDto = new ContaDto { Id = 123, Descricao = "Nova Conta" };

            // Configura o mock para retornar a conta com o novo ID quando o serviço for chamado
            _serviceMock.Setup(s => s.CreateAsync(novaContaDto)).ReturnsAsync(contaCriadaDto);

            // Act
            var actionResult = await _controller.CreateConta(novaContaDto);

            // Assert
            var createdResult = actionResult.Should().BeOfType<CreatedAtActionResult>().Subject;
            createdResult.StatusCode.Should().Be((int)HttpStatusCode.Created);
            createdResult.ActionName.Should().Be(nameof(ContasController.GetConta));
            createdResult.RouteValues["id"].Should().Be(contaCriadaDto.Id);
            
            var contaRetornada = createdResult.Value.Should().BeOfType<ContaDto>().Subject;
            contaRetornada.Should().BeEquivalentTo(contaCriadaDto);
        }

        [Fact]
        public async Task CreateConta_ComModelStateInvalido_DeveRetornarBadRequest()
        {
            // Arrange
            var novaContaDto = new CreateContaDto(); // Dados inválidos
            _controller.ModelState.AddModelError("Descricao", "Descrição é obrigatória");

            // Act
            var actionResult = await _controller.CreateConta(novaContaDto);

            // Assert
            // O controller deve retornar BadRequest ANTES de chamar o serviço.
            actionResult.Should().BeOfType<BadRequestObjectResult>();
        }
    }
}