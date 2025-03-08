using Microsoft.Extensions.Configuration;
using SolarMaxClient;
using SolarMaxClient.Transport;
using SolarMaxRESTApiClient;
using SolarMaxUpdater.Settings;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace SolarMaxUpdater
{
    class Program
    {
        static async Task Main(string[] args)
        {
            // Build configuration
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddUserSecrets<Program>(optional: true, reloadOnChange: true)
                .Build();

            // Bind the configuration
            var appSettings = new AppSettings();
            configuration.Bind(appSettings);
            
            if (args != null && args.Length > 0)
            {
                if (args[0] == "--version")
                {
                    var assembly = System.Reflection.Assembly.GetExecutingAssembly().GetName();
                    Console.WriteLine($"{assembly.Name} v. {assembly.Version}");
                    return;
                }
            }

            ISolarMaxRestApiClient RESTApiClient = new SolarMaxRESTApiClient.SolarMaxRestApiClient(appSettings.AzureFunctions.BaseUrl);
            var addSolarPacItemFunction = new SolarMaxRESTApiClient.Models.AzureFunction()
            {
                Url = $"api/AddSolarPacItem?code={appSettings.AzureFunctions.Code}",
                Key = ""
            };

            foreach (var inverter in appSettings.Inverters)
            {
                ITransport transport = new NetworkTransport(inverter.IpAddress, inverter.TcpPort, TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(10), Encoding.UTF8);
                ISolarMaxClient solarMaxClient = new SolarMaxClient.SolarMaxClient(transport);
                Console.WriteLine($"Reading the PAC from {inverter.IpAddress} ({inverter.Name}) ...");
                var energyReportResult = solarMaxClient.GetEnergyReport();
                if (energyReportResult.Result)
                {
                    Console.WriteLine($"Posting the PAC {energyReportResult.PAC} for inverter {inverter.Id} ...");

                    var solarPacItem = new SolarMaxRESTApiClient.Models.SolarPacItem()
                    {
                        inverterId = inverter.Id,
                        pac = energyReportResult.PAC
                    };
                    var resAdd = await RESTApiClient.AddSolarPacItemAsync(addSolarPacItemFunction, solarPacItem);
                    if (resAdd)
                    {
                        Console.WriteLine($"PAC has been posted successfully");
                    }
                    else
                    {
                        Console.WriteLine($"PAC has *NOT* been posted");
                    }
                }
                else
                {
                    Console.WriteLine($"Unable to read the PAC from {inverter.IpAddress}. Reason: {energyReportResult.ErrorDescription}");
                }
            }
        }
    }
}
