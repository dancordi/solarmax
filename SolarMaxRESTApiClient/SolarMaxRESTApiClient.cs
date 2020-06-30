using RestSharp;
using SolarMaxRESTApiClient.Models;
using System;
using System.Net;

namespace SolarMaxRESTApiClient
{
    public class SolarMaxRESTApiClient : ISolarMaxRESTApiClient
    {
        IRestClient _RestClient { get; set; }
        System.Text.Json.JsonSerializerOptions _JsonSerializerOptions { get; set; }

        public SolarMaxRESTApiClient(string baseUrl)
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

        public bool AddSolarPacItem(AzureFunction function, SolarPacItem solarPacItem)
        {
            var restRequest = new RestRequest(Method.POST);
            restRequest.Resource = $"/{function.Url}";
            JsonObject jsonObj = new JsonObject();
            jsonObj.Add("InverterId", solarPacItem.inverterId);
            jsonObj.Add("Pac", solarPacItem.pac);
            restRequest.RequestFormat = DataFormat.Json;
            string jsonPayload = System.Text.Json.JsonSerializer.Serialize(jsonObj, _JsonSerializerOptions);
            restRequest.AddParameter("application/json; charset=utf-8", jsonPayload, ParameterType.RequestBody);

            //invio la richiesta
            var resultRestRequest = _RestClient.Execute(restRequest);

            //gestione del response
            if (resultRestRequest.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public SolarPacItem GetLastSolarPacItem(AzureFunction function, int inverterId)
        {
            var restRequest = new RestRequest(Method.POST);
            restRequest.Resource = $"/{function.Url}";
            JsonObject jsonObj = new JsonObject();
            jsonObj.Add("InverterId", inverterId);
            restRequest.RequestFormat = DataFormat.Json;
            string jsonPayload = System.Text.Json.JsonSerializer.Serialize(jsonObj, _JsonSerializerOptions);
            restRequest.AddParameter("application/json; charset=utf-8", jsonPayload, ParameterType.RequestBody);

            //invio la richiesta
            var resultRestRequest = _RestClient.Execute(restRequest);

            //gestione del response
            if (resultRestRequest.StatusCode == System.Net.HttpStatusCode.OK)
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
