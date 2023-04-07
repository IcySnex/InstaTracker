using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.OS;
using Xamarin.Forms.Platform.Android;
using Xamarin.Forms;

namespace InstaTracker.Droid;

[Activity(
    Label = "InstaTracker",
    Icon = "@mipmap/ic_launcher",
    Theme = "@style/MainTheme",
    ConfigurationChanges =
        ConfigChanges.ScreenSize |
        ConfigChanges.Orientation |
        ConfigChanges.UiMode |
        ConfigChanges.ScreenLayout |
        ConfigChanges.SmallestScreenSize
    )]
public class MainActivity : FormsAppCompatActivity
{
    protected override void OnCreate(
        Bundle savedInstanceState)
    {
        base.OnCreate(savedInstanceState);

        Xamarin.Essentials.Platform.Init(this, savedInstanceState);
        Forms.Init(this, savedInstanceState);
        FormsMaterial.Init(this, savedInstanceState);
        LoadApplication(new App());
    }


    public override void OnRequestPermissionsResult(
        int requestCode,
        string[] permissions,
        [GeneratedEnum] Permission[] grantResults)
    {
        Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

        base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
    }
}