using System;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

namespace InstaTracker.iOS.Effects;

public class RemoveEntryBordersEffect : PlatformEffect
{
    protected override void OnAttached()
    {
        UITextField textField = (UITextField)Control;

        if (textField is null)
            throw new NotImplementedException();

        textField.BorderStyle = UITextBorderStyle.None;
        textField.BackgroundColor = Color.Transparent.ToUIColor();
    }

    protected override void OnDetached()
    {
    }
}