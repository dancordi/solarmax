using SolarMaxClient;
using SolarMaxClient.Transport;
using System;
using System.Text;

namespace SolarMaxClientConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            ITransport transport = new NetworkTransport("192.168.0.151", 12345, TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(10), Encoding.UTF8);
            ISolarMaxClient solarMaxClient = new SolarMaxClient.SolarMaxClient(transport);
            var getStatusResult = solarMaxClient.GetStatus();
            if (getStatusResult.Result)
            {
                Console.WriteLine($"status={getStatusResult.Status}");
            }
            //var energyReportResult = solarMaxClient.GetEnergyReport();
        }
    }
}
