using FluentValidation;

namespace SistemaCaixa.Application.UseCases.FecharCaixa;

public class FecharCaixaValidator : AbstractValidator<FecharCaixaCommand>
{
    public FecharCaixaValidator()
    {
        RuleFor(x => x.CaixaId)
            .NotEmpty().WithMessage("ID do caixa é obrigatório");
        RuleFor(x => x.OperadorFechamento)
            .NotEmpty().WithMessage("Operador de fechamento é obrigatório")
            .MaximumLength(100);
    }
}
