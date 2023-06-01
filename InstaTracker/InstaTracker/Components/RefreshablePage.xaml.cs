using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace InstaTracker.Components;

[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class RefreshablePage : Components.Page
{
    public RefreshablePage()
    {
        InitializeComponent();
    }


    public static readonly BindableProperty IsRefreshingProperty = BindableProperty.Create(
        nameof(IsRefreshing), typeof(bool), typeof(Page), false, BindingMode.OneWay);

    public bool IsRefreshing
    {
        get => (bool)GetValue(IsRefreshingProperty);
        set => SetValue(IsRefreshingProperty, value);
    }

    public static readonly BindableProperty IsRefreshingEnabledProperty = BindableProperty.Create(
        nameof(IsRefreshingEnabled), typeof(bool), typeof(Page), false, BindingMode.OneWay);

    public bool IsRefreshingEnabled
    {
        get => (bool)GetValue(IsRefreshingEnabledProperty);
        set => SetValue(IsRefreshingEnabledProperty, value);
    }

    public static readonly BindableProperty RefreshCommandProperty = BindableProperty.Create(
        nameof(RefreshCommand), typeof(ICommand), typeof(Page), default(ICommand), BindingMode.OneTime);

    public ICommand RefreshCommand
    {
        get => (ICommand)GetValue(RefreshCommandProperty);
        set => SetValue(RefreshCommandProperty, value);
    }
}