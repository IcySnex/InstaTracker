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
        textField.TintColor = UIColor.FromRGB(37, 150, 190);
    }

    protected override void OnDetached()
    {
    }
}