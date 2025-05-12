using back_cw20_integration.Application.Common.Interfaces.Mediator;
using Microsoft.Extensions.DependencyInjection;

namespace back_cw20_integration.Infrastructure.Mediator
{
    public class MediatorService(IServiceProvider serviceProvider) : IMediator
    {
        private readonly IServiceProvider _serviceProvider = serviceProvider;
        public async Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken)
        {
            var requestType = request.GetType();

            var handlerType = typeof(IRequestHandler<,>).MakeGenericType(requestType, typeof(TResponse));

            var handler = _serviceProvider.GetService(handlerType)
                ?? throw new Exception($"No handler registered for {requestType.Name}");

            var handleMethod = handlerType.GetMethod("Handle") 
                ?? throw new Exception($"Handle method not found for {handlerType.Name}");

            var behaviorType = typeof(IPipelineBehavior<,>).MakeGenericType(requestType, typeof(TResponse));
            var behaviors = _serviceProvider.GetServices(behaviorType).Cast<object>().ToList();

            RequestHandlerDelegate<TResponse> next = () =>
                handleMethod.Invoke(handler, [request, cancellationToken]) as Task<TResponse>  
                ?? throw new Exception("Request handle delegate not invoke");

            for (int i = behaviors.Count - 1; i >= 0; i--) 
            {
                var behavior = behaviors[i];
                var currentNext = next;

                next = () =>
                {
                    var behaviorHandleMethod = behavior.GetType().GetMethod("Handle")
                        ?? throw new Exception($"Handle method not found for behavior {behavior.GetType().Name}");

                    return behaviorHandleMethod.Invoke(behavior, [request, cancellationToken, currentNext]) as Task<TResponse>
                        ?? throw new Exception("Request handle delegate not invoke");
                };
            }

            return await next();
        }
    }
}
