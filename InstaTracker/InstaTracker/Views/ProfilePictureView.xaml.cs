using System.Windows.Input;
using Xamarin.CommunityToolkit.Effects;
using Xamarin.Forms.Xaml;

namespace InstaTracker.Views;

[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class ProfilePictureView : Components.Page
{
    public ProfilePictureView(
        string imageUrl,
        ICommand goBackCommand)
    {
        InitializeComponent();

        TouchEffect.SetCommand(GoBackImage, goBackCommand);
        ImageView.Source = imageUrl;
    }
}