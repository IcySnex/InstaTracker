using InstaTracker.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace InstaTracker.Views;

[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class AccountView : Components.Page
{
    public AccountView()
    {
        InitializeComponent();
        AccountViewModel viewModel = App.Provider.GetRequiredService<AccountViewModel>();
        BindingContext = viewModel;

        // Fix for https://github.com/xamarin/Xamarin.Forms/issues/13607
        viewModel.AccountManager.PropertyChanged += (s, e) =>
        {
            if (e.PropertyName != "LoggedUser" || viewModel.AccountManager.LoggedUser is not null)
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