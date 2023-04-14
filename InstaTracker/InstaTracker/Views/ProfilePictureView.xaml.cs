using System;
using Xamarin.Forms.Xaml;

namespace InstaTracker.Views;

[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class ProfilePictureView : Components.Page
{
    public ProfilePictureView(
        string imageUrl,
        EventHandler onGoBackClicked)
    {
        InitializeComponent();

        GoBackButton.Clicked += onGoBackClicked;
        PictureView.Source = imageUrl;
    }
}