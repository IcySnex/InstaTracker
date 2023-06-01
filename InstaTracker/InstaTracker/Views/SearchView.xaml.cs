using InstaTracker.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Xamarin.Forms.Xaml;

namespace InstaTracker.Views;

[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class SearchView : Components.RefreshablePage
{
    public SearchView()
    {
        InitializeComponent();
        BindingContext = App.Provider.GetRequiredService<SearchViewModel>();
    }
}