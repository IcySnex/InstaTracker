using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(ContentPage), typeof(InstaTracker.iOS.Renderer.SwipeBackPageRenderer))]
namespace InstaTracker.iOS.Renderer;

public class SwipeBackPageRenderer : PageRenderer
{
    public override void ViewWillAppear(bool animated)
    {
        base.ViewDidAppear(animated);

        ViewController.NavigationController.InteractivePopGestureRecognizer.Enabled = true;
        ViewController.NavigationController.InteractivePopGestureRecognizer.Delegate = new UIGestureRecognizerDelegate();
    }
}