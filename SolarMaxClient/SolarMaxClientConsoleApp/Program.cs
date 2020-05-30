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

            while (true)
            {
                int lastByte = 150;
                for (int i = 0; i < 2; i++)
                {
                    string baseIpAddress = $"192.168.0.{lastByte++}";

                    Console.WriteLine($"-------------------------------");
                    Console.WriteLine($"Data from {baseIpAddress}");
                    ITransport transport = new NetworkTransport(baseIpAddress, 12345, TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(10), Encoding.UTF8);
                    ISolarMaxClient solarMaxClient = new SolarMaxClient.SolarMaxClient(transport);
                    var getStatusResult = solarMaxClient.GetStatus();
                    if (getStatusResult.Result)
                    {
                        Console.WriteLine($"status={getStatusResult.Status}");
                    }
                    else
                    {
                        Console.WriteLine($"status. Error:{getStatusResult.ErrorDescription}");
                    }
                    var energyReportResult = solarMaxClient.GetEnergyReport();
                    if (energyReportResult.Result)
                    {
                        Console.WriteLine($"pac={energyReportResult.PAC}");
                    }
                    else
                    {
                        Console.WriteLine($"pac. Error:{getStatusResult.ErrorDescription}");
                    }
                    Console.WriteLine($"-------------------------------");
                }
                System.Threading.Thread.Sleep(2000); //2 seconds sleep
            }
        }
    }
}
