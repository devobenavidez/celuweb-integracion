namespace back_cw20_integration.Application.Common.DTO
{
    public class IntegrationUserDto(string? username, string? accessToken, int businessUnit)
    {
        public string? Username { get; set; } = username;
        public string? AccessToken { get; set; } = accessToken;
        public int BusinessUnit { get; set; } = businessUnit;
    }
}
