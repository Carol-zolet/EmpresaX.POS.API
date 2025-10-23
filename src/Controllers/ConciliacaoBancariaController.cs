using Microsoft.AspNetCore.Mvc;

namespace EmpresaX.POS.API.Controllers
{
    [ApiController]
    [Route("api/v1/financeiro/conciliacao")]
    public class ConciliacaoBancariaController : BaseController
    {
        [HttpGet("posicao")]
        public ActionResult<object> GetPosicao()
        {
            try
            {
                var contas = new[]
                {
                    new { banco = "Banco do Brasil", agencia = "1234-5", conta = "12345-6", tipo = "Conta Corrente", saldo = 25420.80m, saldoFormatado = "R$ 25.420,80" },
                    new { banco = "Itaú", agencia = "4567", conta = "98765-4", tipo = "Conta Corrente", saldo = 18750.65m, saldoFormatado = "R$ 18.750,65" },
                    new { banco = "Santander", agencia = "8901", conta = "54321-9", tipo = "Conta Poupança", saldo = 12500.00m, saldoFormatado = "R$ 12.500,00" }
                };

                return Ok(new
                {
                    sucesso = true,
                    dataAtualizacao = DateTime.Now,
                    contas = contas,
                    totalBancos = contas.Sum(c => c.saldo),
                    totalBancosFormatado = $"R$ {contas.Sum(c => c.saldo):N2}"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { erro = ex.Message });
            }
        }

        [HttpGet("pendencias")]
        public ActionResult<object> GetPendencias()
        {
            try
            {
                var pendencias = new[]
                {
                    new { id = 1, descricao = "Transferência não identificada", valor = 1500.00m, data = DateTime.Now.AddDays(-5), banco = "Banco do Brasil" },
                    new { id = 2, descricao = "Débito automático pendente", valor = -320.00m, data = DateTime.Now.AddDays(-3), banco = "Itaú" }
                };

                return Ok(new { sucesso = true, pendencias = pendencias });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { erro = ex.Message });
            }
        }
    }
}


