using SolarMaxClient;
using SolarMaxClient.Transport;
using SolarMaxRESTApiClient;
using System;
using System.Text;
using System.Threading;

namespace SolarMaxUpdater
{
    class Program
    {
        static void Main(string[] args)
        {
            ISolarMaxRESTApiClient RESTApiClient = new SolarMaxRESTApiClient.SolarMaxRESTApiClient("https://solarpacfunction20200628145749.azurewebsites.net");
            var addSolarPacItemFunction = new SolarMaxRESTApiClient.Models.AzureFunction()
            {
                Url = "api/AddSolarPacItem?code=sZDIHm0GLWIqROPQAUpHYqRR0QY2RDXxDlLVFjA3inqLkUNjUQAVLg==",
                Key = ""
            };
            int lastByte = 150;
            for (int i = 1; i <= 2; i++)
            {
                string baseIpAddress = $"192.168.1.{lastByte++}";

                ITransport transport = new NetworkTransport(baseIpAddress, 12345, TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(10), Encoding.UTF8);
                ISolarMaxClient solarMaxClient = new SolarMaxClient.SolarMaxClient(transport);
                Console.WriteLine($"Reading the PAC from {baseIpAddress} ...");
                var energyReportResult = solarMaxClient.GetEnergyReport();
                if (energyReportResult.Result)
                {
                    Console.WriteLine($"Posting the PAC {energyReportResult.PAC} for invert {i} ...");

                    var solarPacItem = new SolarMaxRESTApiClient.Models.SolarPacItem() {
                        inverterId = i, pac = energyReportResult.PAC
                    };
                    var resAdd = RESTApiClient.AddSolarPacItem(addSolarPacItemFunction, solarPacItem);
                    if (resAdd)
                    {
                        Console.WriteLine($"PAC has been posted done successfully");
                    }
                    else
                    {
                        Console.WriteLine($"PAC has *NOT* been posted");
                    }
                }
            }
            Thread.Sleep(5000);
        }
    }
}
