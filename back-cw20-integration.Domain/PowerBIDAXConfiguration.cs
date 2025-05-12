namespace back_cw20_integration.Domain
{
    public class PowerBIDAXConfiguration
    {
        public string QueryPrefix { get; set; } = default!;
        public string ConnectionPrefix { get; set; } = default!;
        public string QueryDax { get; set; } = default!;
        public int BusinessUnit { get; set; }
        public string TempTableName { get; set; } = default!;
    }
}