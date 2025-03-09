using RestSharp;
using SolarMaxRESTApiClient.Models;
using System;
using System.Net;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace SolarMaxRESTApiClient
{
    public class SolarMaxRestApiClient : ISolarMaxRestApiClient
    {
        IRestClient _RestClient { get; set; }
        System.Text.Json.JsonSerializerOptions _JsonSerializerOptions { get; set; }

        public SolarMaxRestApiClient(string baseUrl)
        {
            //https://restsharp.dev/get-help/faq.html#connection-closed-with-ssl
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

            _RestClient = new RestClient(baseUrl);
            _JsonSerializerOptions = new System.Text.Json.JsonSerializerOptions()
            {
                WriteIndented = true,
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            };
        }

        public async Task<bool> AddSolarPacItemAsync(AzureFunction function, SolarPacItem solarPacItem)
        {
            var restRequest = new RestRequest($"/{function.Url}", Method.Post);
            JsonObject jsonObj = new JsonObject();
            jsonObj.Add("InverterId", solarPacItem.inverterId);
            jsonObj.Add("Pac", solarPacItem.pac);
            restRequest.RequestFormat = DataFormat.Json;
            restRequest.AddJsonBody(jsonObj);

            var resultRestRequest = await _RestClient.ExecuteAsync(restRequest);

            return resultRestRequest.IsSuccessful;
        }


        public async Task<SolarPacItem> GetLastSolarPacItemAsync(AzureFunction function, int inverterId)
        {
            var restRequest = new RestRequest($"/{function.Url}", Method.Post);
            JsonObject jsonObj = new JsonObject();
            jsonObj.Add("InverterId", inverterId);
            restRequest.RequestFormat = DataFormat.Json;
            restRequest.AddJsonBody(jsonObj);

            var resultRestRequest = await _RestClient.ExecuteAsync(restRequest);

            //gestione del response
            if (resultRestRequest.IsSuccessful)
            {
                if (string.IsNullOrWhiteSpace(resultRestRequest.Content))
                {
                    //no itempac
                    return null;
                }
                try
                {
                    //deserialize the response
                    var itemPac = System.Text.Json.JsonSerializer.Deserialize<SolarPacItem>(resultRestRequest.Content);
                    return itemPac;
                }
                catch (System.Exception ex)
                {
                    return null;
                }
            }
            else
            {
                return null;
            }

        }
    }
}
