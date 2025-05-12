namespace back_cw20_integration.Domain
{
    public class IntegrationUserModel
    {
        public string UserId { get; set; } = default!;
        public string Username { get; set; } = default!;
        public int BusinessUnit { get; set; }
        public string AccessToken { get; set; } = default!;
        public string Role { get; set; } = default!;
    }
}
