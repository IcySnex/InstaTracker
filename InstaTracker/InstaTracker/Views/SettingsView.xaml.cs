using InstaTracker.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Xamarin.Forms.Xaml;

namespace InstaTracker.Views;

[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class SettingsView : Components.ScrollablePage
{
    public SettingsView()
    {
        InitializeComponent();
        BindingContext = App.Provider.GetRequiredService<SettingsViewModel>();
    }
}