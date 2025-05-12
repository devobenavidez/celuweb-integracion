namespace back_cw20_integration.Application.Common.Interfaces
{
    public interface ICurrentIntegrationUserService
    {
        string? Username { get; }
        string? Role { get; }
        string? BusinessUnit { get; }
    }
}
