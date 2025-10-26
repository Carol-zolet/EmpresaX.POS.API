using FluentValidation;

namespace SistemaCaixa.Application.UseCases.AbrirCaixa;

public class AbrirCaixaValidator : AbstractValidator<AbrirCaixaCommand>
{
    public AbrirCaixaValidator()
    {
        RuleFor(x => x.SaldoInicial)
            .GreaterThanOrEqualTo(0).WithMessage("Saldo inicial não pode ser negativo");
        RuleFor(x => x.OperadorAbertura)
            .NotEmpty().WithMessage("Operador é obrigatório")
            .MaximumLength(100);
    }
}
