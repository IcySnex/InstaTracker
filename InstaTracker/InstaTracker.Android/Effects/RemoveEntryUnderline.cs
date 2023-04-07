using Android.Graphics;
using Android.Widget;
using System;
using Xamarin.Forms.Platform.Android;

namespace InstaTracker.Droid.Effects;

public class RemoveEntryUnderline : PlatformEffect
{
    protected override void OnAttached()
    {
        EditText editText = (EditText)Control;

        if (editText is null)
            throw new NotImplementedException();

        editText.SetBackgroundColor(Color.Transparent);
    }

    protected override void OnDetached()
    {
    }
}