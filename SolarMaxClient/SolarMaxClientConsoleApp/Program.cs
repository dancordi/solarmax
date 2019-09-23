using SolarMaxClient;
using SolarMaxClient.Transport;
using System;

namespace SolarMaxClientConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            ITransport transport = new NetworkTransport("192.168.1.41", 1234, TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(10));
            ISolarMaxClient solarMaxClient = new SolarMaxClient.SolarMaxClient(transport);
            bool resGetVersion = solarMaxClient.GetVersion(out string version);
        }
    }
}
