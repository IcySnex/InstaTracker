﻿using Foundation;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

namespace InstaTracker.iOS;

[Register("AppDelegate")]
public partial class AppDelegate : FormsApplicationDelegate
{
    public override bool FinishedLaunching(
        UIApplication app,
        NSDictionary options)
    {
        Forms.Init();
        FormsMaterial.Init();
        LoadApplication(new App());

        return base.FinishedLaunching(app, options);
    }
}