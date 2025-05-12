using back_cw20_integration.Application.Common.DTO;
using back_cw20_integration.Application.Common.Interfaces.JWT;
using back_cw20_integration.Application.Common.Interfaces.Mediator;
using back_cw20_integration.Application.Common.Interfaces.Persistence;
using back_cw20_integration.Domain;

namespace back_cw20_integration.Application.IntegrationUser.Command.Create
{
    public class CreateIntegrationUserCommandHandler(ICreateIntegrationUserRepository repository
        , IJwtService jwtService
        , IUnitOfWork unitOfWork
        , IClaimsSettings claimsSettings)
        : IRequestHandler<CreateIntegrationUserCommand, CreateIntegrationUserResponse>
    {
        private readonly ICreateIntegrationUserRepository _repository = repository;
        private readonly IJwtService _jwtService = jwtService;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IClaimsSettings _claimsSettings = claimsSettings;   

        public async Task<CreateIntegrationUserResponse> Handle(CreateIntegrationUserCommand request, CancellationToken cancellationToken)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync(cancellationToken);

                var existUser = await _repository.FindUsernameAndBusinessUnitAsync(request.Username, request.BusinessUnit, cancellationToken);
                if (existUser) 
                    return CreateIntegrationUserResponse.FailureResponse($"El usuario {request.Username} ya se encuentra registrado para la unidad de negocio {request.BusinessUnit}");
                    

                var user = new IntegrationUserModel()
                {
                    UserId = Guid.NewGuid().ToString(),
                    Username = request.Username,
                    BusinessUnit = request.BusinessUnit,
                    Role = "INTEGRATION"
                };

                var token = await _jwtService.TokenGenerateAsync(_claimsSettings.ClaimsToBasicToken(user));

                user.AccessToken = token.Split('.')[1].Trim();

                bool savedUser = await _repository.CreateAsync(user, cancellationToken);

                if (!savedUser)
                    return CreateIntegrationUserResponse.FailureResponse("No se pudo guardar el usuario");

                await _unitOfWork.CommitTransactionAsync(cancellationToken);

                return CreateIntegrationUserResponse.SuccessResponse(
                    new IntegrationUserDto(request.Username, user.AccessToken, request.BusinessUnit));
            }
            catch (Exception) 
            {
                await _unitOfWork.RollbackTransactionAsync(cancellationToken);

                return CreateIntegrationUserResponse.FailureResponse($"Ocurrio un error al intentar registrar al usuario {request.Username}");
            }
        }
    }
}
