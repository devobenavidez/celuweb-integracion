using back_cw20_integration.Application.Authentication.Command.Login;
using FluentAssertions;

namespace Application.Validators
{
    public class LoginCommandValidatorTest
    {
        private readonly LoginCommandValidator _validator;

        public LoginCommandValidatorTest() 
        {
            _validator = new LoginCommandValidator();
        }

        [Fact]
        public void Validate_EmptyUsername_ShouldHaveValidationError() 
        { 
            var command = new LoginCommand { Username = "", AccessToken = "asdsafasf"};

            var result = _validator.Validate(command);

            result.Errors.Should().Contain(e =>
            e.PropertyName == "Username" &&
            e.ErrorMessage.Contains("El nombre de usuario es requerido"));
        }

        [Fact]
        public void Validate_EmptyAccessToken_ShouldHaveValidationError()
        {
            var command = new LoginCommand { Username = "superadmin", AccessToken = "" };

            var result = _validator.Validate(command);

            result.Errors.Should().Contain(e =>
            e.PropertyName == "AccessToken" &&
            e.ErrorMessage.Contains("El token de acceso es requerido"));
        }

        [Fact]
        public void Validate_EmptyUsernameAndEmptyAccessToken_ShouldHaveValidationError()
        {
            var command = new LoginCommand { Username = "", AccessToken = "" };

            var result = _validator.Validate(command);

            result.Errors.Should()
                .HaveCount(2)
                .And
                .NotBeNullOrEmpty()
                .And
                .Contain(e =>
                    e.PropertyName == "Username" 
                    && e.ErrorMessage.Contains("El nombre de usuario es requerido"))
                .And.Contain(e =>
                    e.PropertyName == "AccessToken" 
                    && e.ErrorMessage.Contains("El token de acceso es requerido")
            );
        }

        [Fact]
        public void Validate_ValidCredentials_ShouldNotHaveValidationErrors()
        {
            var command = new LoginCommand
            {
                Username = "validuser",
                AccessToken = "ValidPassword123"
            };

            var result = _validator.Validate(command);

            result.Errors.Should().BeEmpty();
        }
    }
}
