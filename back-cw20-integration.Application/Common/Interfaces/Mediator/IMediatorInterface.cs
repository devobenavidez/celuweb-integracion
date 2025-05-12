namespace back_cw20_integration.Application.Common.Interfaces.Mediator
{
    public interface IRequest<TResponse> { }
    public interface IRequestHandler<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken);
    }

    public interface IMediator 
    {
        Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken);
    }
    // Interfaz para el middleware de comportamiento
    public interface IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        Task<TResponse> Handle(
            TRequest request,
            CancellationToken cancellationToken,
            RequestHandlerDelegate<TResponse> next);
    }

    // Delegado para el siguiente handler en la cadena
    public delegate Task<TResponse> RequestHandlerDelegate<TResponse>();
}
