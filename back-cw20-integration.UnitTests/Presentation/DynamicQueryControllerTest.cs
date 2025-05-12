using back_cw20_integration.API.Controllers;
using back_cw20_integration.Application.Common.Interfaces.Mediator;
using back_cw20_integration.Application.DynamicQuery.Command.Execute;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Text.Json;

namespace Presentation
{
    public class DynamicQueryControllerTest
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly DynamicQueryController _controller;

        public DynamicQueryControllerTest() 
        { 
            _mediatorMock = new Mock<IMediator>();
            _controller = new DynamicQueryController(_mediatorMock.Object);
        }

        [Fact]
        public async Task Execute_ValidQuery_ReturnsOkResult()
        {
            var command = new ExecuteQueryCommand
            {
                QueryPrefix = "test",
                QueryParams = [KeyValuePair.Create("param1", "valueParam"), KeyValuePair.Create("param2", "valueParam2")],
                PageSize = 100,
                NumPage = 1,
                RefreshCache = true
            };

            List<JsonElement> data = [];

            data.Add(JsonSerializer.SerializeToElement(new 
            {
                document = "testdocument",
                ownerId = "testownerid",
                price = 1000,
                clientId = "testclientid"
            }));

            data.Add(JsonSerializer.SerializeToElement(new
            {
                document = "testdocument1",
                ownerId = "testownerid1",
                price = 1001,
                clientId = "testclientid1"
            }));

            data.Add(JsonSerializer.SerializeToElement(new
            {
                document = "testdocument2",
                ownerId = "testownerid2",
                price = 1002,
                clientId = "testclientid2"
            }));

            var results = data;

            var response = ExecuteQueryResponse.SuccessResponse(command, results.Count, results);

            _mediatorMock
                .Setup(x => x.Send(command, default))
                .ReturnsAsync(response);

            var result = await _controller.ExecuteQuery(command, default);

            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;

            okResult.Value.Should().Be(response);

            _mediatorMock.Verify(x => x.Send(command, default), Times.Once);
        }

        [Fact]
        public async Task Execute_InvalidQuery_ReturnsBadResult()
        {
            var command = new ExecuteQueryCommand
            {
                QueryPrefix = "unexistprefix",
                QueryParams = [KeyValuePair.Create("param1", "valueParam"), KeyValuePair.Create("param2", "valueParam2")],
                PageSize = 100,
                NumPage = 1,
                RefreshCache = true
            };

            var response = ExecuteQueryResponse.FailureResponse($"No se encontro configuración del prefijo {command.QueryPrefix}");

            _mediatorMock
                .Setup(x => x.Send(command, default))
                .ReturnsAsync(response);

            var result = await _controller.ExecuteQuery(command, default);

            var badResult = result.Should().BeOfType<BadRequestObjectResult>().Subject;

            badResult.Value.Should().Be(response);

            _mediatorMock.Verify(x => x.Send(command, default), Times.Once);
        }
    }
}
