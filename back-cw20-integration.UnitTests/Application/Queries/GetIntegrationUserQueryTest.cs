using back_cw20_integration.Application.Common.DTO;
using back_cw20_integration.Application.Common.Interfaces.Cache;
using back_cw20_integration.Application.IntegrationUser.Queries.GetAll;
using back_cw20_integration.Domain;
using FluentAssertions;
using Moq;

namespace Application.Queries
{
    public class GetIntegrationUserQueryTest
    {
        private readonly Mock<IGetAllIntegrationUserRepository> _repositoryMock;
        private readonly Mock<ICacheService> _cacheServiceMock;
        private readonly GetIntegrationUserQueryHandler _handler;

        public GetIntegrationUserQueryTest() 
        { 
            _repositoryMock = new Mock<IGetAllIntegrationUserRepository>();
            _cacheServiceMock = new Mock<ICacheService>();
            _handler = new GetIntegrationUserQueryHandler(_repositoryMock.Object, _cacheServiceMock.Object);
        }

        [Fact]
        public async Task Handle_AnyUsers_ReturnUsers() 
        { 
            var query = new GetIntegrationUserQuery(true);

            GetIntegrationUserResponse? cachedQuery = null;

            _cacheServiceMock
                .Setup(x => x.GetByRefreshCacheAsync<GetIntegrationUserResponse>(It.IsAny<string>(), query.RefreshCache))
                .ReturnsAsync(cachedQuery);


            List<IntegrationUserModel> usersModel =
            [
                new IntegrationUserModel{Username = "user1", BusinessUnit = 1000, AccessToken = "accesstoken", Role = "INTEGRATION" },
                new IntegrationUserModel{Username = "user2", BusinessUnit = 1001, AccessToken = "accesstoken2", Role = "INTEGRATION" },
                new IntegrationUserModel{Username = "user3", BusinessUnit = 1002, AccessToken = "accesstoken3", Role = "INTEGRATION" },
            ];

            _repositoryMock
                .Setup(x => x.GetAllIntegrationUserAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(usersModel);

            List<IntegrationUserDto> usersDto =
            [
                new IntegrationUserDto("user1", "accesstoken", 1000),
                new IntegrationUserDto("user2", "accesstoken2", 1001),
                new IntegrationUserDto("user3", "accesstoken3", 1002),
            ];

            var response = GetIntegrationUserResponse.SuccessResponse(usersDto);

            _cacheServiceMock
                .Setup(x => x.SetAsync("integration_users_list_all", response, null))
                .Returns(Task.CompletedTask);

            var result = await _handler.Handle(query, It.IsAny<CancellationToken>());

            result.Should().NotBeNull();
            result.Success.Should().Be(true);
            result.ErrorMessage.Should().BeNull();
            result.Users.Should().NotBeNull();
            result.Users.Should().HaveCount(3);

            _cacheServiceMock.Verify(x => x.GetByRefreshCacheAsync<GetIntegrationUserResponse>(It.IsAny<string>(), query.RefreshCache)
            , Times.Once);

            _cacheServiceMock.Verify(x => x.SetAsync(It.Is<string>(s => s == "integration_users_list_all"),
                It.IsAny<GetIntegrationUserResponse>(),
                It.IsAny<TimeSpan?>())
            , Times.Once);

            _repositoryMock.Verify(x => x.GetAllIntegrationUserAsync(It.IsAny<CancellationToken>())
            , Times.Once);
        }
        [Fact]
        public async Task Handle_NotAnyUsers_ReturnFailure()
        {
            var query = new GetIntegrationUserQuery(true);

            GetIntegrationUserResponse? cachedQuery = null;

            _cacheServiceMock
                .Setup(x => x.GetByRefreshCacheAsync<GetIntegrationUserResponse>(It.IsAny<string>(), query.RefreshCache))
                .ReturnsAsync(cachedQuery);

            List<IntegrationUserModel> usersModel = [];

            _repositoryMock
                .Setup(x => x.GetAllIntegrationUserAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(usersModel);

            var result = await _handler.Handle(query, It.IsAny<CancellationToken>());

            result.Should().NotBeNull();
            result.Success.Should().Be(false);
            result.ErrorMessage.Should().NotBeNull();
            result.ErrorMessage.Should().Be("No se encontrarón usuarios");
            result.Users.Should().NotBeNull();
            result.Users.Should().HaveCount(0);

            _cacheServiceMock.Verify(x => x.GetByRefreshCacheAsync<GetIntegrationUserResponse>(It.IsAny<string>(), query.RefreshCache)
            , Times.Once);

            _repositoryMock.Verify(x => x.GetAllIntegrationUserAsync(It.IsAny<CancellationToken>())
            , Times.Once);
        }
    }
}
