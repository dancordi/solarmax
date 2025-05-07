using Android.Content;
using global::Android.App;

namespace SolarMaxMauiAndroidUpdater;

[BroadcastReceiver(Enabled = true, Exported = true)]
[IntentFilter(new[] { Intent.ActionBootCompleted })]
public class BootReceiver : BroadcastReceiver
{
    public override void OnReceive(Context? context, Intent? intent)
    {
        if (intent!.Action == Intent.ActionBootCompleted)
        {
            new SchedulerApiWorker(context!).ScheduleApiWorker();
        }
    }
}