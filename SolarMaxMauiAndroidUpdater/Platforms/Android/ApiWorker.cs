using Android.Content;
using AndroidX.Work;
using Microsoft.Extensions.Logging;

namespace SolarMaxMauiAndroidUpdater;

public class ApiWorker : Worker
{    
    public ApiWorker(Context context, WorkerParameters workerParams)
            : base(context, workerParams)
    {
    }

    public override Result DoWork()
    {
        try
        {
            FetchApiData().Wait();
            return Result.InvokeSuccess();
        }
        catch (Exception ex)
        {
            //_logger.LogError(ex, "Error in Worker");

            return Result.InvokeFailure();
        }
    }

    private async Task FetchApiData()
    {
        //_logger.LogInformation("FetchApiData started");
        await Task.Delay(2000);
        //using var client = new HttpClient();
        //var response = await client.GetAsync("https://api.example.com/data");
        //if (response.IsSuccessStatusCode)
        //{
        //    var data = await response.Content.ReadAsStringAsync();
        //    Console.WriteLine($"Dati ricevuti: {data}");
        //}
        //_logger.LogInformation("FetchApiData completed");
    }
}
