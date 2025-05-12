using back_cw20_integration.Application.Common.Interfaces.Mediator;

namespace back_cw20_integration.Application.DynamicQuery.Command.Execute
{
    public class ExecuteQueryCommand : IRequest<ExecuteQueryResponse>
    {
        public string QueryPrefix { get; set; } = default!;
        public List<KeyValuePair<string, string>> QueryParams { get; set; } = [];
        public int NumPage { get; set; } = 1;
        public int PageSize { get; set; } = 50;
        public bool RefreshCache { get; set; } = false;
    }
}
