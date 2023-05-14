using InstaTracker.ViewModels;
using Xamarin.Forms.Xaml;

namespace InstaTracker.Views;

[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class InfoView : Components.Page
{
    public InfoView(
        InfoViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}