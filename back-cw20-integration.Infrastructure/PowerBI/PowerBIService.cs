using Azure.Core;
using back_cw20_integration.Application.PowerBI.Interfaces;
using back_cw20_integration.Domain;
using Microsoft.AnalysisServices.AdomdClient;
using Microsoft.Identity.Client;
using System.Data;

namespace back_cw20_integration.Infrastructure.PowerBI
{
    public class PowerBIService : IPowerBIService
    {
        public async Task<DataTable> AddParamBusinessUnitAsync(DataTable data, string businessUnit, CancellationToken cancellationToken)
        {
            return await Task.Run(() =>
            {
                data.Columns.Add(new DataColumn("business_unit", typeof(int)));

                foreach (DataRow row in data.Rows)
                {
                    row["business_unit"] = businessUnit;
                }

                return data;
            });
            
        }

        public async Task<DataTable> ExecuteQueryAdomdAsync(PowerBIAdomdConfiguration adomdConfiguration, PowerBIDAXConfiguration powerBIDAXConfiguration, CancellationToken cancellationToken)
        {
            return await Task.Run(async () =>
            {
                string accessToken = await GetAccessTokenWithUsernamePassword(adomdConfiguration);

                adomdConfiguration.ConnectionString = $"{adomdConfiguration.ConnectionString};Token:{accessToken}";

                using AdomdConnection connection = new(adomdConfiguration.ConnectionString);

                connection.Open();

                using AdomdCommand command = new(powerBIDAXConfiguration.QueryDax, connection);
                using AdomdDataAdapter adapter = new(command);
                DataTable resultTable = new();

                adapter.Fill(resultTable);
                return resultTable;
            });
        }

        private static async Task<string> GetAccessTokenWithUsernamePassword(PowerBIAdomdConfiguration configuration)
        {
            IConfidentialClientApplication app = ConfidentialClientApplicationBuilder
            .Create(configuration.ClientId)
            .WithClientSecret(configuration.ClientSecret)
            .WithAuthority(new Uri($"https://login.microsoftonline.com/{configuration.TenantId}"))
            .Build();

            // Scope específico para Power BI
            string[] scopes = ["https://analysis.windows.net/powerbi/api/.default"];

            try
            {
                // Adquirir token para la aplicación cliente
                AuthenticationResult result = await app.AcquireTokenForClient(scopes)
                    .ExecuteAsync();

                return result.AccessToken;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener token: {ex.Message}");
                throw;
            }
        }
    }
}
