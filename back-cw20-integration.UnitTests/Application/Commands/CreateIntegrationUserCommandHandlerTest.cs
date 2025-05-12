using back_cw20_integration.Application.Common.Interfaces.JWT;
using back_cw20_integration.Application.Common.Interfaces.Persistence;
using back_cw20_integration.Application.IntegrationUser.Command.Create;
using back_cw20_integration.Domain;
using FluentAssertions;
using Moq;

namespace Application.Commands
{
    public class CreateIntegrationUserCommandHandlerTest
    {
        private readonly Mock<ICreateIntegrationUserRepository> _repositoryMock;
        private readonly Mock<IJwtService> _jwtServiceMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IClaimsSettings> _claimsSettingsMock;

        private readonly CreateIntegrationUserCommandHandler _handler;

        public CreateIntegrationUserCommandHandlerTest()
        {
            _repositoryMock = new Mock<ICreateIntegrationUserRepository>();
            _jwtServiceMock = new Mock<IJwtService>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _claimsSettingsMock = new Mock<IClaimsSettings>();
            _handler = new CreateIntegrationUserCommandHandler(_repositoryMock.Object
                , _jwtServiceMock.Object
                , _unitOfWorkMock.Object
                , _claimsSettingsMock.Object);
        }

        [Fact]
        public async Task Handle_ValidUser_ShouldReturnUser()
        {
            var command = new CreateIntegrationUserCommand
            {
                Username = "testusername",
                BusinessUnit = 1000
            };

            _repositoryMock
                .Setup(x => x.FindUsernameAndBusinessUnitAsync(command.Username, command.BusinessUnit, CancellationToken.None))
                .ReturnsAsync(false);

            var token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1c2VyIjoib21hciIsImlhdCI6MTcwOTY0NzEyNn0.VJ8K6A6CzB5xY3Qj9q7FZwXtFdM1rOqyzGmHZG9V8Yk"
;

            _jwtServiceMock
                .Setup(x => x.TokenGenerateAsync(_claimsSettingsMock.Object.ClaimsToBasicToken(It.IsAny<IntegrationUserModel>())))
                .ReturnsAsync(token);

            _repositoryMock
                .Setup(x => x.CreateAsync(It.IsAny<IntegrationUserModel>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            var result = await _handler.Handle(command, CancellationToken.None);

            result.Should().NotBeNull();
            result.Success.Should().BeTrue();
            result.User.Should().NotBeNull();

            _unitOfWorkMock.Verify(x => x.BeginTransactionAsync(CancellationToken.None)
                , Times.Once);

            _unitOfWorkMock.Verify(x => x.CommitTransactionAsync(CancellationToken.None)
                , Times.Once);

            _repositoryMock.Verify(x => x.FindUsernameAndBusinessUnitAsync(command.Username, command.BusinessUnit, CancellationToken.None)
                , Times.Once);

            _repositoryMock.Verify(x => x.CreateAsync(It.IsAny<IntegrationUserModel>(), It.IsAny<CancellationToken>())
                , Times.Once);

            _jwtServiceMock.Verify(x => x.TokenGenerateAsync(_claimsSettingsMock.Object.ClaimsToBasicToken(It.IsAny<IntegrationUserModel>()))
                , Times.Once);
        }

        [Fact]
        public async Task Handle_ExistsUser_ShouldReturnFailure()
        {
            var command = new CreateIntegrationUserCommand
            {
                Username = "testusernameexist",
                BusinessUnit = 1000
            };

            var result = await _handler.Handle(command, CancellationToken.None);

            result.Should().NotBeNull();
            result.Success.Should().BeFalse();
            result.User.Should().BeNull();

            _unitOfWorkMock.Verify(x => x.BeginTransactionAsync(CancellationToken.None)
                , Times.Once());

            _repositoryMock.Verify(x => x.FindUsernameAndBusinessUnitAsync(command.Username, command.BusinessUnit, CancellationToken.None)
                , Times.Once);
        }

        [Fact]
        public async Task Handle_ExceptionUser_ShouldReturnFailure() 
        {
            var command = new CreateIntegrationUserCommand
            {
                Username = "testusernameexist",
                BusinessUnit = 1000
            };

            _repositoryMock
                .Setup(x => x.FindUsernameAndBusinessUnitAsync(command.Username, command.BusinessUnit, CancellationToken.None))
                .ReturnsAsync(false);

            var token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1c2VyIjoib21hciIsImlhdCI6MTcwOTY0NzEyNn0.VJ8K6A6CzB5xY3Qj9q7FZwXtFdM1rOqyzGmHZG9V8Yk";

            _jwtServiceMock
                .Setup(x => x.TokenGenerateAsync(_claimsSettingsMock.Object.ClaimsToBasicToken(It.IsAny<IntegrationUserModel>())))
                .ReturnsAsync(token);

            _repositoryMock
                .Setup(x => x.CreateAsync(It.IsAny<IntegrationUserModel>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception());

            var result = await _handler.Handle(command, CancellationToken.None);

            _unitOfWorkMock.Verify(x => x.BeginTransactionAsync(CancellationToken.None)
                , Times.Once);

            _unitOfWorkMock.Verify(x => x.RollbackTransactionAsync(CancellationToken.None)
                , Times.Once);

            _repositoryMock.Verify(x => x.FindUsernameAndBusinessUnitAsync(command.Username, command.BusinessUnit, CancellationToken.None)
                , Times.Once);

            _repositoryMock.Verify(x => x.CreateAsync(It.IsAny<IntegrationUserModel>(), It.IsAny<CancellationToken>())
                , Times.Once);

            _jwtServiceMock.Verify(x => x.TokenGenerateAsync(_claimsSettingsMock.Object.ClaimsToBasicToken(It.IsAny<IntegrationUserModel>()))
                , Times.Once);
        }
    }
}
