using FluentValidation;

namespace back_cw20_integration.Application.Authentication.Command.Login
{
    public class LoginCommandValidator : AbstractValidator<LoginCommand>
    {
        public LoginCommandValidator()
        {
            RuleFor(v => v.Username)
                .NotEmpty().WithMessage("El nombre de usuario es requerido");

            RuleFor(v => v.AccessToken)
                .NotEmpty().WithMessage("El token de acceso es requerido");
        }
    }
}