using InstaTracker.Helpers;
using Serilog;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace InstaTracker.Services;

public class Navigation
{
    readonly ILogger logger;

    public Navigation(
        ILogger logger)
    {
        this.logger = logger;
    }


    public async Task NavigateAsync(
        Page page,
        bool animated = true)
    {
        // Check if components page
        TabbedPage mainView = (TabbedPage)Application.Current.MainPage;
        if (mainView.CurrentPage is not NavigationPage navigation)
        {
            logger.Log("Tried to navigate on default page");
            return;
        }

        logger.Log("Navigating to page");
        await navigation.PushAsync(page, animated);
    }


    public async Task GoBackAsync(
        bool toRoot = false,
        bool animated = true)
    {
        // Check if components page
        TabbedPage mainView = (TabbedPage)Application.Current.MainPage;
        if (mainView.CurrentPage is not NavigationPage navigation)
        {
            logger.Log("Tried to navigate on default page");
            return;
        }

        logger.Log("Navigating to page");
        if (toRoot)
            await navigation.PopToRootAsync(animated);
        else
            await navigation.PopAsync(animated);
    }
}