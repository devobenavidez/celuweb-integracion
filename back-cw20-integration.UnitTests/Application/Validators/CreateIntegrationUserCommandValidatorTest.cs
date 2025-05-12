using back_cw20_integration.Application.IntegrationUser.Command.Create;
using FluentAssertions;

namespace Application.Validators
{
    public class CreateIntegrationUserCommandValidatorTest
    {
        private readonly CreateIntegrationUserCommandValidator _validator;

        public CreateIntegrationUserCommandValidatorTest()
        {
            _validator = new CreateIntegrationUserCommandValidator();
        }

        [Fact]
        public void Validate_EmptyUsername_ShouldHaveValidationError()
        {
            var command = new CreateIntegrationUserCommand { Username = "", BusinessUnit = 1000 };

            var result = _validator.Validate(command);

            result.Errors.Should().Contain(e =>
            e.PropertyName == "Username" &&
            e.ErrorMessage.Contains("El nombre de usuario es requerido"));
        }

        [Fact]
        public void Validate_GreaterBusinessUnit_ShouldHaveValidationError()
        {
            var command = new CreateIntegrationUserCommand { Username = "superadmin", BusinessUnit = 0};

            var result = _validator.Validate(command);

            result.Errors.Should().Contain(e =>
            e.PropertyName == "BusinessUnit" &&
            e.ErrorMessage.Contains("La unidad de negocio debe ser mayor a 0"));
        }

        [Fact]
        public void Validate_LatestBusinessUnit_ShouldHaveValidationError()
        {
            var command = new CreateIntegrationUserCommand { Username = "superadmin", BusinessUnit = 10000};

            var result = _validator.Validate(command);

            result.Errors.Should().Contain(e =>
            e.PropertyName == "BusinessUnit" &&
            e.ErrorMessage.Contains("La unidad de negocio deber ser menor a 10000"));
        }

        [Fact]
        public void Validate_LimitUsername_ShouldHaveValidationError()
        {
            var command = new CreateIntegrationUserCommand 
            { 
                Username = "aquihaymasdecincuentacaracterescomoparametrodenombredeusuario",
                BusinessUnit = 1000
            };

            var result = _validator.Validate(command);

            result.Errors.Should().Contain(e =>
            e.PropertyName == "Username" &&
            e.ErrorMessage.Contains("El nombre de usuario excede el limite permitido, máximo 50 carácteres"));
        }

        [Fact]
        public void Validate_ValidUser_ShouldNotHaveValidationErrors()
        {
            var command = new CreateIntegrationUserCommand
            {
                Username = "superadmin",
                BusinessUnit = 1000
            };

            var result = _validator.Validate(command);

            result.Errors.Should().BeEmpty();
        }
    }
}
