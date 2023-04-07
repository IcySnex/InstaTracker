using Android.App;
using Android.OS;

namespace InstaTracker.Droid;

[Activity(
    Label = "InstaTracker",
    Theme = "@style/SplashTheme",
    Icon = "@mipmap/ic_launcher",
    MainLauncher = true,
    NoHistory = true)]
public class SplashActivity : Activity
{
    protected override void OnCreate(Bundle savedInstanceState)
    {
        base.OnCreate(savedInstanceState);

        StartActivity(typeof(MainActivity));
    }
}