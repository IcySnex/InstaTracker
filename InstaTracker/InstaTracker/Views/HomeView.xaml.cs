using InstaTracker.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Xamarin.Forms.Xaml;

namespace InstaTracker.Views;

[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class HomeView : Components.Page
{
    public HomeView()
    {
        InitializeComponent();
        BindingContext = App.Provider.GetRequiredService<HomeViewModel>();
    }
}