using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Material.iOS;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(Entry), typeof(InstaTracker.iOS.Renderer.EntryRenderer), new[] { typeof(VisualMarker.MaterialVisual) })]
namespace InstaTracker.iOS.Renderer;

public class EntryRenderer : MaterialEntryRenderer
{
    protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
    {
        base.OnElementChanged(e);

        Control.TintColor = UIColor.FromRGB(152, 94, 255);
    }
}