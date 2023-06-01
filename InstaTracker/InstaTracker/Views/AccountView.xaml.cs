using InstaTracker.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace InstaTracker.Views;

[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class AccountView : Components.ScrollablePage
{
    public AccountView()
    {
        InitializeComponent();
        AccountViewModel viewModel = App.Provider.GetRequiredService<AccountViewModel>();
        BindingContext = viewModel;

        // Fix for https://github.com/xamarin/Xamarin.Forms/issues/13607
        viewModel.AccountManager.PropertyChanged += (s, e) =>
        {
            if (e.PropertyName != "LoggedAccount" || viewModel.AccountManager.LoggedAccount is not null)
                return;

            BindableLayout.SetItemsSource(SavedAccountsCollection, null);
            SavedAccountsCollection.SetBinding(BindableLayout.ItemsSourceProperty, new Binding()
            {
                Source = viewModel,
                Path = "SavedAccounts",
                Mode = BindingMode.OneWay   
            });
        };
    }
}