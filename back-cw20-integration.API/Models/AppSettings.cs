namespace back_cw20_integration.API.Models
{
    public class AppSettings
    {
        public ApplicationDetail ApplicationDetail { get; set; } = default!;
    }
    public class ApplicationDetail
    {
        public string ApplicationName { get; set; } = default!;
        public string Description { get; set; } = default!;
        public string ContactWebsite { get; set; } = default!;
        public string LicenseDetail { get; set; } = default!;
    }
}