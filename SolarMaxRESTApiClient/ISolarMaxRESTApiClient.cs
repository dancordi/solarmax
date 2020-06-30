using SolarMaxRESTApiClient.Models;
using System;

namespace SolarMaxRESTApiClient
{
    public interface ISolarMaxRESTApiClient
    {
        bool AddSolarPacItem(AzureFunction function, SolarPacItem solarPacItem);
        SolarPacItem GetLastSolarPacItem(AzureFunction function, int inverterId);
    }
}
