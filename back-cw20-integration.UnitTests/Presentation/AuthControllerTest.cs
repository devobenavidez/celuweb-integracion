using back_cw20_integration.API.Controllers;
using back_cw20_integration.Application.Authentication.Command.Login;
using back_cw20_integration.Application.Common.Interfaces.Mediator;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Presentation
{
    public class AuthControllerTests
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly AuthController _controller;

        public AuthControllerTests()
        {
            _mediatorMock = new Mock<IMediator>();
            _controller = new AuthController(_mediatorMock.Object);
        }

        [Fact]
        public async Task Login_ValidCredentials_ReturnsOkResult()
        {
            // Arrange
            var loginCommand = new LoginCommand
            {
                Username = "testuser",
                AccessToken = "password"
            };

            var response = LoginResponse.SuccessResponse("token", DateTime.UtcNow.AddHours(12));

            _mediatorMock
                .Setup(x => x.Send(loginCommand, default))
                .ReturnsAsync(response);

            // Act
            var result = await _controller.Login(loginCommand, default);

            // Assert
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            okResult.Value.Should().Be(response);

            _mediatorMock.Verify(x => x.Send(loginCommand, default), Times.Once);
        }

        [Fact]
        public async Task Login_InValidCredentials_ReturnsUnauthorizedResult()
        {
            // Arrange
            var loginCommand = new LoginCommand
            {
                Username = "testuser",
                AccessToken = "password"
            };

            var response = LoginResponse.FailureResponse("Credenciales Invalidas");

            _mediatorMock
                .Setup(x => x.Send(loginCommand, default))
                .ReturnsAsync(response);

            // Act
            var result = await _controller.Login(loginCommand, default);

            // Assert
            var badResult = result.Should().BeOfType<UnauthorizedObjectResult>().Subject;
            badResult.Value.Should().Be(response);

            _mediatorMock.Verify(x => x.Send(loginCommand, default), Times.Once);
        }
    }
}
