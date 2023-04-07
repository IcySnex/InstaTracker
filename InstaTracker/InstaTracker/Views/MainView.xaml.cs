using Xamarin.Forms.Xaml;
using Xamarin.Forms;

namespace InstaTracker.Views;

[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class MainView : TabbedPage
{
    public MainView() =>
        InitializeComponent();
}