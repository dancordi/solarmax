using Android.Content;
using AndroidX.Work;

namespace SolarMaxMauiAndroidUpdater;

public class SchedulerApiWorker
{
    private readonly Context _context;
    public SchedulerApiWorker(Context context)
    {
        _context = context;
    }

    public void ScheduleApiWorker()
    {
        var prefs = _context.GetSharedPreferences("my_prefs", FileCreationMode.Private);
        bool isWorkerScheduled = prefs!.GetBoolean("worker_scheduled", false);
        if (!isWorkerScheduled)
        {
            var workRequest = PeriodicWorkRequest.Builder.From<ApiWorker>(
                TimeSpan.FromMinutes(5))
                .Build();

            WorkManager.GetInstance(_context).EnqueueUniquePeriodicWork(
                "ApiWorker",
                ExistingPeriodicWorkPolicy.Keep!,
                workRequest);

            prefs.Edit()!.PutBoolean("worker_scheduled", true)!.Apply();
        }
    }
}
