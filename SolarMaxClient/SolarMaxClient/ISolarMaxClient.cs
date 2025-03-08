using SolarMaxClient.Result;

namespace SolarMaxClient
{
    public interface ISolarMaxClient
    {
        GetStatusResult GetStatus();

        GetEnergyReportResult GetEnergyReport();
    }
}