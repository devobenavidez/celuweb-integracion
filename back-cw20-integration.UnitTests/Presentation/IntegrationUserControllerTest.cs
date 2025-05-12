using back_cw20_integration.API.Controllers;
using back_cw20_integration.Application.Common.DTO;
using back_cw20_integration.Application.Common.Interfaces.Mediator;
using back_cw20_integration.Application.IntegrationUser.Command.Create;
using back_cw20_integration.Application.IntegrationUser.Queries.GetAll;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Presentation
{
    public class IntegrationUserControllerTest
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly IntegrationUserController _controller;

        public IntegrationUserControllerTest() 
        { 
            _mediatorMock = new Mock<IMediator>();
            _controller = new IntegrationUserController(_mediatorMock.Object);
        }

        [Fact]
        public async Task Create_ValidUser_ReturnsOkResult()
        {
            // Arrange
            var command = new CreateIntegrationUserCommand
            {
                Username = "testuserintegration",
                BusinessUnit = 1000
            };

            var userDto = new IntegrationUserDto(command.Username, "token.de.acceso", command.BusinessUnit);

            var response = CreateIntegrationUserResponse.SuccessResponse(userDto);

            _mediatorMock
                .Setup(x => x.Send(command, default))
                .ReturnsAsync(response);

            // Act
            var result = await _controller.CreateIntegrationUser(command, default);

            // Assert
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            okResult.Value.Should().Be(response);

            _mediatorMock.Verify(x => x.Send(command, default), Times.Once);
        }

        [Fact]
        public async Task Create_InValidUser_ReturnsBadResult()
        {
            // Arrange
            var command = new CreateIntegrationUserCommand
            {
                Username = "userexisttest",
                BusinessUnit = 1000
            };

            var response = CreateIntegrationUserResponse.FailureResponse($"El usuario {command.Username} ya se encuentra registrado para la unidad de negocio {command.BusinessUnit}");

            _mediatorMock
                .Setup(x => x.Send(command, default))
                .ReturnsAsync(response);

            // Act
            var result = await _controller.CreateIntegrationUser(command, default);

            // Assert
            var badResult = result.Should().BeOfType<BadRequestObjectResult>().Subject;
            badResult.Value.Should().Be(response);

            _mediatorMock.Verify(x => x.Send(command, default), Times.Once);
        }

        [Fact]
        public async Task GetAll_Users_ReturnsOkResult()
        {
            // Arrange
            List<IntegrationUserDto> users =
            [
                new IntegrationUserDto("user1", "accesstoken", 1000),
                new IntegrationUserDto("user2", "accesstoken2", 1001),
                new IntegrationUserDto("user3", "accesstoken3", 1002),
            ];

            var response = GetIntegrationUserResponse.SuccessResponse(users);

            _mediatorMock
                .Setup(x => x.Send(
                    It.Is<GetIntegrationUserQuery>(q => q.RefreshCache == false),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(response);

            // Act
            var result = await _controller.IntegrationUsersGetAll(default, false);

            // Assert
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            okResult.Value.Should().Be(response);

            _mediatorMock.Verify(x => x.Send(It.Is<GetIntegrationUserQuery>(q => q.RefreshCache == false),
                     It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task GetAll_Users_ReturnsBadResult()
        {
            // Arrange

            var response = GetIntegrationUserResponse.FailureResponse("No se encontrarón usuarios");

            _mediatorMock
                .Setup(x => x.Send(
                    It.Is<GetIntegrationUserQuery>(q => q.RefreshCache == false),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(response);

            // Act
            var result = await _controller.IntegrationUsersGetAll(default, false);

            // Assert
            var badResult = result.Should().BeOfType<BadRequestObjectResult>().Subject;
            badResult.Value.Should().Be(response);

            _mediatorMock.Verify(x => x.Send(It.Is<GetIntegrationUserQuery>(q => q.RefreshCache == false),
                    It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
