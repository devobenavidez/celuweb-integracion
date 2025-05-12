using FluentValidation;

namespace back_cw20_integration.Application.DynamicQuery.Command.Execute
{
    public class ExecuteQueryCommandValidator : AbstractValidator<ExecuteQueryCommand>
    {
        public ExecuteQueryCommandValidator() 
        {
            RuleFor(v => v.QueryPrefix)
                .NotEmpty().WithMessage("El prefijo de la query a ejecutar es requerido");
            RuleFor(v => v.NumPage)
                .NotNull().WithMessage("El número de página es requerido")
                .GreaterThanOrEqualTo(1).WithMessage("El número de página no puede ser menor a 1");
            RuleFor(v => v.PageSize)
                .NotNull().WithMessage("El tamaño de la página es requerido")
                .GreaterThanOrEqualTo(1).WithMessage("El tamaño por página no puede ser menor a 1")
                .LessThanOrEqualTo(1000).WithMessage("El tamaño por página no puede superar los 1000 registros");
        }
    }
}
