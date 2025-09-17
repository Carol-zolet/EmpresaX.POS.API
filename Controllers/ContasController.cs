using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace EmpresaX.POS.API.Controllers
{
    /// <summary>
    /// Controller para gerenciamento de contas
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class ContasController : ControllerBase
    {
        /// <summary>
        /// Obtém todas as contas
        /// </summary>
        /// <returns>Lista de contas</returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<ContaDto>), StatusCodes.Status200OK)]
        public IActionResult GetContas()
        {
            var contas = new List<ContaDto>
            {
                new ContaDto { Id = 1, Descricao = "Conta Teste", Valor = 100.00m, DataVencimento = DateTime.Now.AddDays(30) },
                new ContaDto { Id = 2, Descricao = "Conta Teste 2", Valor = 250.00m, DataVencimento = DateTime.Now.AddDays(15) }
            };

            return Ok(contas);
        }

        /// <summary>
        /// Obtém uma conta específica por ID
        /// </summary>
        /// <param name="id">ID da conta</param>
        /// <returns>Dados da conta</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ContaDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetConta(int id)
        {
            if (id <= 0)
                return BadRequest("ID deve ser maior que zero");

            var conta = new ContaDto { Id = id, Descricao = $"Conta {id}", Valor = 100.00m, DataVencimento = DateTime.Now.AddDays(30) };
            return Ok(conta);
        }

        /// <summary>
        /// Cria uma nova conta
        /// </summary>
        /// <param name="conta">Dados da conta</param>
        /// <returns>Conta criada</returns>
        [HttpPost]
        [ProducesResponseType(typeof(ContaDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult CreateConta([FromBody] CreateContaDto conta)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var novaConta = new ContaDto
            {
                Id = new Random().Next(1, 1000),
                Descricao = conta.Descricao,
                Valor = conta.Valor,
                DataVencimento = conta.DataVencimento
            };

            return CreatedAtAction(nameof(GetConta), new { id = novaConta.Id }, novaConta);
        }
    }

    /// <summary>
    /// DTO para representação de conta
    /// </summary>
    public class ContaDto
    {
        /// <summary>
        /// ID da conta
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Descrição da conta
        /// </summary>
        public string Descricao { get; set; } = string.Empty;

        /// <summary>
        /// Valor da conta
        /// </summary>
        public decimal Valor { get; set; }

        /// <summary>
        /// Data de vencimento
        /// </summary>
        public DateTime DataVencimento { get; set; }
    }

    /// <summary>
    /// DTO para criação de conta
    /// </summary>
    public class CreateContaDto
    {
        /// <summary>
        /// Descrição da conta
        /// </summary>
        [Required(ErrorMessage = "Descrição é obrigatória")]
        [StringLength(200, ErrorMessage = "Descrição deve ter no máximo 200 caracteres")]
        public string Descricao { get; set; } = string.Empty;

        /// <summary>
        /// Valor da conta
        /// </summary>
        [Required(ErrorMessage = "Valor é obrigatório")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Valor deve ser maior que zero")]
        public decimal Valor { get; set; }

        /// <summary>
        /// Data de vencimento
        /// </summary>
        [Required(ErrorMessage = "Data de vencimento é obrigatória")]
        public DateTime DataVencimento { get; set; }
    }
}