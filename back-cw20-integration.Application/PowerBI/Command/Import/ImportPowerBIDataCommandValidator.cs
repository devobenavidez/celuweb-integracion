using FluentValidation;

namespace back_cw20_integration.Application.PowerBI.Command.Import
{
    public class ImportPowerBIDataCommandValidator : AbstractValidator<ImportPowerBIDataCommand>
    {
        public ImportPowerBIDataCommandValidator() 
        {
            RuleFor(v => v.QueryPrefix)
                .NotEmpty().WithMessage("El prefijo de la query a ejecutar es requerido");
        }
    }
}
