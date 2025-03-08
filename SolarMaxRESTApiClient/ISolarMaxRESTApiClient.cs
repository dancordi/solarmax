using SolarMaxRESTApiClient.Models;
using System;
using System.Threading.Tasks;

namespace SolarMaxRESTApiClient
{
    public interface ISolarMaxRestApiClient
    {
        Task<bool> AddSolarPacItemAsync(AzureFunction function, SolarPacItem solarPacItem);
        Task<SolarPacItem> GetLastSolarPacItemAsync(AzureFunction function, int inverterId);
    }
}
