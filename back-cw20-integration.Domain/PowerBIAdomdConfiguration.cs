namespace back_cw20_integration.Domain
{
    public class PowerBIAdomdConfiguration
    {
        public string ConnectionPrefix { get; set; } = default!;
        public string ConnectionString { get; set; } = default!;
        public int BusinessUnit { get; set; }
        public string ClientId { get; set; } = default!;
        public string TenantId { get; set; } = default!;
        public string ClientSecret { get; set; } = default!;
    }
}
