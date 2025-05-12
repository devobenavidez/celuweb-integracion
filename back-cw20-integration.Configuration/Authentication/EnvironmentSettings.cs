using back_cw20_integration.Configuration.Authentication.Interfaces;

namespace back_cw20_integration.Configuration.Authentication
{
    public class EnvironmentSettings : IEnvironmentSettings
    {
        public string Environment { get; set; } = default!;
    }
}
