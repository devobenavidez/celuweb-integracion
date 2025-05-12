using back_cw20_integration.Application.Authentication.Command.Login;
using back_cw20_integration.Application.Common.Interfaces.Authentication;
using back_cw20_integration.Application.Common.Interfaces.JWT;
using back_cw20_integration.Domain;
using FluentAssertions;
using Moq;
using System.Text.Json;

namespace Application.Commands
{
    public class LoginCommandHandlerTest
    {
        private readonly Mock<ILoginCommandRepository> _repositoryMock;
        private readonly Mock<IJwtService> _jwtServiceMock;
        private readonly Mock<IAuthenticationCookieService> _cookieServiceMock;
        private readonly Mock<IClaimsSettings> _claimSettingsMock;

        private readonly LoginCommandHandler _handler;

        public LoginCommandHandlerTest()
        {
            _repositoryMock = new Mock<ILoginCommandRepository>();
            _jwtServiceMock = new Mock<IJwtService>();
            _cookieServiceMock = new Mock<IAuthenticationCookieService>();
            _claimSettingsMock = new Mock<IClaimsSettings>();
            _handler = new LoginCommandHandler(_repositoryMock.Object
                , _jwtServiceMock.Object
                , _cookieServiceMock.Object
                , _claimSettingsMock.Object);
        }

        [Fact]
        public async Task Handle_ValidCredentials_ShouldReturnToken()
        {
            // Arrange
            var command = new LoginCommand
            {
                Username = "testuser",
                AccessToken = "accesstoken"
            };

            var user = new IntegrationUserModel
            {   
                UserId = "testuserid",
                Username = "testuser",
                AccessToken = "accesstoken",
                BusinessUnit = 1000,
                Role = "INTEGRATION"
            };

            _repositoryMock
                .Setup(x => x.FindUserByCredentialsAsync(command.Username, command.AccessToken, CancellationToken.None))
                .ReturnsAsync(user);

            var jsonString = "{\"sub\":\"testuserid\"}";

            var jsonPayload = JsonSerializer.Deserialize<JsonElement>(jsonString);

            _jwtServiceMock
                .Setup(x => x.ExtractPayloadAsync(It.IsAny<string>()))
                .ReturnsAsync(jsonPayload);
                

            _jwtServiceMock
                .Setup(x => x.TokenGenerateAsync(_claimSettingsMock.Object.ClaimsToNormalToken(user)))
                .ReturnsAsync("generated.jwt.token");           

            _cookieServiceMock
                .Setup(x => x.SetAuthenticationCookie("generated.jwt.token"));

            //// Act
            var result = await _handler.Handle(command, CancellationToken.None);

            //// Assert

            result.Should().NotBeNull();
            result.Token.Should().Be("generated.jwt.token");
            result.Success.Should().BeTrue();
            result.ErrorMessage.Should().BeNull();

            //// Verify interactions
            _repositoryMock.Verify(
                x => x.FindUserByCredentialsAsync(command.Username, command.AccessToken, CancellationToken.None),
                Times.Once
            );

            _jwtServiceMock.Verify(
                x => x.TokenGenerateAsync(_claimSettingsMock.Object.ClaimsToNormalToken(user)),
                Times.Once
            );

            _cookieServiceMock.Verify(
                x => x.SetAuthenticationCookie("generated.jwt.token"),
                Times.Once
            );
        }

        [Fact]
        public async Task Login_WithInvalidCredentials_ShouldFail()
        {
            // Arrange
            var command = new LoginCommand
            {
                Username = "invaliduser",
                AccessToken = "wrongaccesstoken"
            };

            IntegrationUserModel? user = null;

            // Setup: Configurar comportamiento para ValidateUser
            _repositoryMock
                .Setup(x => x.FindUserByCredentialsAsync(command.Username, command.AccessToken, CancellationToken.None))
                .ReturnsAsync(user);

            _cookieServiceMock
                .Setup(x => x.RemoveAuthenticationCookie());

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Token.Should().Be(null);
            result.ExpiresAt.Should().Be(null);
            result.Success.Should().BeFalse();
            result.ErrorMessage.Should().Be("Credenciales Invalidas");

            //// Verify interactions
            _repositoryMock.Verify(
                x => x.FindUserByCredentialsAsync(command.Username, command.AccessToken, CancellationToken.None),
                Times.Once
            );

            _cookieServiceMock.Verify(
                x => x.RemoveAuthenticationCookie(),
                Times.Once
            );

        }
    }
}
