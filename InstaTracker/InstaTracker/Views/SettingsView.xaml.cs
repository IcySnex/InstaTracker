using InstaTracker.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Xamarin.Forms.Xaml;

namespace InstaTracker.Views;

[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class SettingsView : Components.Page
{
    public SettingsView()
    {
        InitializeComponent();

        BindingContext = App.Provider.GetRequiredService<SettingsViewModel>();
    }
}