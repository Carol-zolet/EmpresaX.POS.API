using SistemaCaixa.Domain.ValueObjects;

namespace SistemaCaixa.Domain.Entities;

public class LancamentoBancario : Entity
{
    public Guid Id { get; private set; }
    public DateTime Data { get; private set; }
    public Dinheiro Valor { get; private set; }
    public string NumeroDocumento { get; private set; }
    public string Descricao { get; private set; }

    private LancamentoBancario() { }

    public static LancamentoBancario Criar(DateTime data, Dinheiro valor, string numeroDocumento, string descricao)
    {
        if (valor.EhNegativo() || valor.Valor == 0)
            throw new DomainException("Valor do lançamento deve ser positivo");
        if (string.IsNullOrWhiteSpace(numeroDocumento))
            throw new DomainException("Número do documento é obrigatório");
        if (string.IsNullOrWhiteSpace(descricao))
            throw new DomainException("Descrição é obrigatória");

        return new LancamentoBancario
        {
            Id = Guid.NewGuid(),
            Data = data,
            Valor = valor,
            NumeroDocumento = numeroDocumento.Trim(),
            Descricao = descricao.Trim()
        };
    }
}
