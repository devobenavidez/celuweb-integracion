using back_cw20_integration.Application.Common.Interfaces;
using back_cw20_integration.Application.Common.Interfaces.Cache;
using back_cw20_integration.Application.DynamicQuery.Command.Execute;
using FluentAssertions;
using Moq;
using System.Text.Json;

namespace Application.Commands
{
    public class DynamicQueryCommandHandlerTest
    {
        private readonly Mock<ICurrentIntegrationUserService> _userServiceMock;
        private readonly Mock<IExecuteQueryRepository> _repositoryMock;
        private readonly Mock<ICacheService> _cacheServiceMock;

        private readonly ExecuteQueryCommandHandler _handler;

        public DynamicQueryCommandHandlerTest()
        {
            _userServiceMock = new Mock<ICurrentIntegrationUserService>();
            _repositoryMock = new Mock<IExecuteQueryRepository>();
            _cacheServiceMock = new Mock<ICacheService>();

            _handler = new ExecuteQueryCommandHandler(_userServiceMock.Object
                , _repositoryMock.Object
                , _cacheServiceMock.Object);
        }

        [Fact]
        public async Task Handle_ValidQueryPrefix_ShouldReturnResults() 
        {
            var command = new ExecuteQueryCommand
            {
                QueryPrefix = "test",
                QueryParams = [KeyValuePair.Create("param1", "valueParam"), KeyValuePair.Create("param2", "valueParam2")],
                PageSize = 100,
                NumPage = 1,
                RefreshCache = true
            };

            ExecuteQueryResponse? cachedQuery = null;

            _cacheServiceMock
                .Setup(x => x.GetByRefreshCacheAsync<ExecuteQueryResponse>(It.IsAny<string>(), It.IsAny<bool>()))
                .ReturnsAsync(cachedQuery);

            var businessUnit = "1000";

            _userServiceMock
                .Setup(x => x.BusinessUnit)
                .Returns(businessUnit);

            var query = "testquery";

            _repositoryMock
                .Setup(x => x.GetQueryByPrefixAsync(It.IsAny<string>(), It.IsAny<string>(), default))
                .ReturnsAsync(query);

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

            var results = data;

            _repositoryMock
                .Setup(x => x.ExecuteDynamicQueryAsync(It.IsAny<string>()
                , It.IsAny<List<KeyValuePair<string, string>>>()
                , It.IsAny<string>()
                , It.IsAny<int>()
                , It.IsAny<int>()
                , default))
                .ReturnsAsync(results);

            _cacheServiceMock
                .Setup(x => x.SetAsync(It.IsAny<string>()
                , It.IsAny<ExecuteQueryResponse>()
                , It.IsAny<TimeSpan>()))
                .Returns(Task.CompletedTask);

            var response = ExecuteQueryResponse.SuccessResponse(command, results.Count, results);

            var result = await _handler.Handle(command, default);

            result.Should().NotBeNull();
            result.Success.Should().BeTrue();
            result.ErrorMessage.Should().BeNull();
            result.Results.Should().HaveCount(2);

            _cacheServiceMock
                .Verify(x => x.GetByRefreshCacheAsync<ExecuteQueryResponse>(It.IsAny<string>(), It.IsAny<bool>())
                , Times.Once);

            _userServiceMock
                .Verify(x => x.BusinessUnit, Times.Once);

            _repositoryMock
                .Verify(x => x.GetQueryByPrefixAsync(It.IsAny<string>(), It.IsAny<string>(), default)
                , Times.Once);

            _repositoryMock
                .Verify(x => x.ExecuteDynamicQueryAsync(It.IsAny<string>()
                , It.IsAny<List<KeyValuePair<string, string>>>()
                , It.IsAny<string>()
                , It.IsAny<int>()
                , It.IsAny<int>()
                , default)
                , Times.Once);

            _cacheServiceMock
                .Verify(x => x.SetAsync(It.IsAny<string>()
                , It.IsAny<ExecuteQueryResponse>()
                , It.IsAny<TimeSpan>())
                , Times.Once);
        }

        [Fact]
        public async Task Handle_InValidQueryPrefix_ShouldFail()
        {
            var command = new ExecuteQueryCommand
            {
                QueryPrefix = "invalidtestquery",
                QueryParams = [KeyValuePair.Create("param1", "valueParam"), KeyValuePair.Create("param2", "valueParam2")],
                PageSize = 100,
                NumPage = 1,
                RefreshCache = true
            };

            ExecuteQueryResponse? cachedQuery = null;

            _cacheServiceMock
                .Setup(x => x.GetByRefreshCacheAsync<ExecuteQueryResponse>(It.IsAny<string>(), It.IsAny<bool>()))
                .ReturnsAsync(cachedQuery);

            var businessUnit = "1000";

            _userServiceMock
                .Setup(x => x.BusinessUnit)
                .Returns(businessUnit);

            var query = string.Empty;

            _repositoryMock
                .Setup(x => x.GetQueryByPrefixAsync(It.IsAny<string>(), It.IsAny<string>(), default))
                .ReturnsAsync(query);

            var response = ExecuteQueryResponse.FailureResponse($"No se encontro configuración del prefijo {command.QueryPrefix}");

            var result = await _handler.Handle(command, default);

            result.Should().NotBeNull();
            result.Success.Should().BeFalse();
            result.ErrorMessage.Should().NotBeNullOrWhiteSpace();
            result.ErrorMessage.Should().Be($"No se encontro configuración del prefijo {command.QueryPrefix}");
            result.Results.Should().BeEmpty();
            result.QuantityResults.Should().Be(0);

            _cacheServiceMock
                .Verify(x => x.GetByRefreshCacheAsync<ExecuteQueryResponse>(It.IsAny<string>(), It.IsAny<bool>())
                , Times.Once);

            _userServiceMock
                .Verify(x => x.BusinessUnit, Times.Once);

            _repositoryMock
                .Verify(x => x.GetQueryByPrefixAsync(It.IsAny<string>(), It.IsAny<string>(), default)
                , Times.Once);
        }
    }
}
