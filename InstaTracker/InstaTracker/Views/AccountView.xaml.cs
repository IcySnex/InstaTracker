using InstaTracker.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Xamarin.Forms.Xaml;

namespace InstaTracker.Views;

[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class AccountView : Components.Page
{
    public AccountView()
    {
        InitializeComponent();
        BindingContext = App.Provider.GetRequiredService<AccountViewModel>();
    }
}