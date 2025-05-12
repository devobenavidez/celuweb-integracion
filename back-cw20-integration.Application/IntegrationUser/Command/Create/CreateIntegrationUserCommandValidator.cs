using FluentValidation;

namespace back_cw20_integration.Application.IntegrationUser.Command.Create
{
    public class CreateIntegrationUserCommandValidator : AbstractValidator<CreateIntegrationUserCommand>
    {
        public CreateIntegrationUserCommandValidator() 
        {
            RuleFor(v => v.Username)
                .NotEmpty().WithMessage("El nombre de usuario es requerido")
                .MaximumLength(50).WithMessage("El nombre de usuario excede el limite permitido, máximo 50 carácteres");

            RuleFor(v => v.BusinessUnit)
                .NotNull().WithErrorCode("La unidad de negocio es requerida")
                .GreaterThan(0).WithMessage("La unidad de negocio debe ser mayor a 0")
                .LessThan(10000).WithMessage("La unidad de negocio deber ser menor a 10000");
        }
    }
}
