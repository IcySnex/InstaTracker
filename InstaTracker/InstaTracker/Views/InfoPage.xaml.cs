using InstaTracker.ViewModels;
using Xamarin.Forms.Xaml;

namespace InstaTracker.Views;

[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class InfoPage : Components.Page
{
    public InfoPage(
        InfoViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}