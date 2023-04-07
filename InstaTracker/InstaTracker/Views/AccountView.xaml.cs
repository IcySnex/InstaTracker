using Xamarin.Forms.Xaml;
using InstaTracker.ViewModels;
using Microsoft.Extensions.DependencyInjection;

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