using back_cw20_integration.Application.DynamicQuery.Command.Execute;
using FluentAssertions;

namespace Application.Validators
{
    public class DynamicQueryCommandValidatorTest
    {
        private readonly ExecuteQueryCommandValidator _validator;

        public DynamicQueryCommandValidatorTest()
        { 
            _validator = new ExecuteQueryCommandValidator();
        }

        [Fact]
        public void Validate_EmptyQueryPrefix_ShouldHaveValidationError() 
        {
            var command = new ExecuteQueryCommand
            { 
                QueryPrefix = "",
                QueryParams = [],
                PageSize = 10,
                NumPage = 1,
                RefreshCache = true,
            };

            var result = _validator.Validate(command);

            result.Errors.Should().Contain(e =>
            e.PropertyName == "QueryPrefix"
            && e.ErrorMessage == "El prefijo de la query a ejecutar es requerido");
        }
        [Fact]
        public void Validate_GreaterPageSize_ShouldHaveValidationError()
        {
            var command = new ExecuteQueryCommand
            {
                QueryPrefix = "testquery",
                QueryParams = [],
                PageSize = 0,
                NumPage = 1,
                RefreshCache = true,
            };

            var result = _validator.Validate(command);

            result.Errors.Should().Contain(e =>
            e.PropertyName == "PageSize"
            && e.ErrorMessage == "El tamaño por página no puede ser menor a 1");
        }

        [Fact]
        public void Validate_LessPageSize_ShouldHaveValidationError()
        {
            var command = new ExecuteQueryCommand
            {
                QueryPrefix = "testquery",
                QueryParams = [],
                PageSize = 1001,
                NumPage = 1,
                RefreshCache = true,
            };

            var result = _validator.Validate(command);

            result.Errors.Should().Contain(e =>
            e.PropertyName == "PageSize"
            && e.ErrorMessage == "El tamaño por página no puede superar los 1000 registros");
        }
        [Fact]
        public void Validate_GreaterNumPage_ShouldHaveValidationError()
        {
            var command = new ExecuteQueryCommand
            {
                QueryPrefix = "testquery",
                QueryParams = [],
                PageSize = 100,
                NumPage = 0,
                RefreshCache = true,
            };

            var result = _validator.Validate(command);

            result.Errors.Should().Contain(e =>
            e.PropertyName == "NumPage"
            && e.ErrorMessage == "El número de página no puede ser menor a 1");
        }

        [Fact]
        public void Validate_ValidParams_ShouldNotHaveValidationErrors() 
        {
            var command = new ExecuteQueryCommand
            {
                QueryPrefix = "testquery",
                QueryParams = [],
                PageSize = 100,
                NumPage = 1,
                RefreshCache = true,
            };

            var result = _validator.Validate(command);

            result.Errors.Should().BeEmpty();
        }
    }
}
