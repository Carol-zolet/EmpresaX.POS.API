using Microsoft.AspNetCore.Mvc;

namespace EmpresaX.POS.API.Controllers
{
    [ApiController]
    [Route("api/v1/financeiro/fluxo-caixa")]
    public class FluxoCaixaController : BaseController
    {
        [HttpGet("posicao")]
        public ActionResult<object> GetPosicao()
        {
            try
            {
                return Ok(new
                {
                    sucesso = true,
                    posicaoAtual = new
                    {
                        saldoAtual = 45623.45m,
                        saldoAtualFormatado = "R$ 45.623,45",
                        dataAtualizacao = DateTime.Now,
                        entradas = 78500.00m,
                        saidas = 32876.55m
                    }
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { erro = ex.Message });
            }
        }

        [HttpPost("fechamento")]
        public ActionResult<object> FechamentoCaixa([FromBody] FechamentoRequest request)
        {
            try
            {
                return Ok(new
                {
                    sucesso = true,
                    mensagem = "Fechamento de caixa realizado com sucesso",
                    saldoFinal = 45623.45m,
                    dataFechamento = DateTime.Now
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { erro = ex.Message });
            }
        }

        [HttpGet("movimentacoes")]
        public ActionResult<object> GetMovimentacoes([FromQuery] int take = 10)
        {
            try
            {
                var movimentacoes = new[]
                {
                    new { id = 1, descricao = "Pagamento fornecedor ABC", valor = -1250.00m, data = DateTime.Now.AddDays(-1), tipo = "Saida" },
                    new { id = 2, descricao = "Recebimento cliente XYZ", valor = 3500.00m, data = DateTime.Now.AddDays(-2), tipo = "Entrada" },
                    new { id = 3, descricao = "Pagamento energia elétrica", valor = -850.30m, data = DateTime.Now.AddDays(-3), tipo = "Saida" }
                };

                return Ok(new { sucesso = true, movimentacoes = movimentacoes.Take(take) });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { erro = ex.Message });
            }
        }
    }

    public class FechamentoRequest
    {
        public DateTime DataFechamento { get; set; }
        public decimal SaldoInformado { get; set; }
        public string Observacoes { get; set; } = string.Empty;
    }
}
