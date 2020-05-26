using SolarMaxClient.Result;
using System;

namespace SolarMaxClient
{
    public interface ISolarMaxClient
    {
        GetStatusResult GetStatus();
        GetEnergyReportResult GetEnergyReport();


    }
}
