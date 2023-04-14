using InstaTracker.Helpers;
using Serilog;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace InstaTracker.Services;

public class SnackBar
{
    readonly ILogger logger;
    readonly Message message;

    public SnackBar(
        ILogger logger,
        Message message)
    {
        this.logger = logger;
        this.message = message;

        logger.Log("Registered SnackBar");
    }


    public Action<Exception> ErrorCallback(
        string title,
        string? errorMessage = null) =>
        async ex => await DisplayAsync(title, "More", true, async () => await message.ShowAsync(title, errorMessage is null ? ex.Message : $"{errorMessage}\n\nError: {ex.Message}"));


    public async Task RunAsync<T>(
        string message,
        Task<T> task,
        Action<Exception>? onError = null,
        Action<T>? onFinished = null,
        int hideDelay = 1000)
    {
        // Show snackbar
        Components.Page? page = SetupSnackbar(message);
        if (page is null)
            return;

        await ShowAsync(page);

        // Run task
        try
        {
            T result = await task;
            onFinished?.Invoke(result);
        }
        catch (Exception ex)
        {
            onError?.Invoke(ex);
        }

        // Hide snackbar
        if (hideDelay > 0 && page.IsSnackBarVisible)
            await Task.Delay(hideDelay);

        if (page.SnackBarLabel.Text == message)
            await HideAsync(page);
    }

    public async Task RunAsync(
        string message,
        Task task,
        Action<Exception>? onError = null,
        Action? onFinished = null,
        int hideDelay = 1000)
    {
        // Show snackbar
        Components.Page? page = SetupSnackbar(message);
        if (page is null)
            return;

        await ShowAsync(page);

        // Run task
        try
        {
            await task;
            onFinished?.Invoke();
        }
        catch (Exception ex)
        {
            onError?.Invoke(ex);
        }

        // Hide snackbar
        if (hideDelay > 0 && page.IsSnackBarVisible)
            await Task.Delay(hideDelay);

        if (page.SnackBarLabel.Text == message)
            await HideAsync(page);
    }


    public async Task DisplayAsync(
        string message,
        string? buttonText = null,
        bool closeOnButtonClicked = true,
        Action? onButtonClicked = null,
        int delay = 2000)
    {
        CancellationTokenSource cts = new();

        // Show snackbar
        Components.Page? page = SetupSnackbar(message, buttonText, comPage =>
        {
            if (closeOnButtonClicked)
                cts.Cancel();

            onButtonClicked?.Invoke();
        });
        if (page is null)
            return;

        await ShowAsync(page);

        // Wait for delay
        try
        {
            await Task.Delay(delay, cts.Token);
        }
        catch (TaskCanceledException) { }

        // Hide snackbar
        await HideAsync(page);
    }


    Components.Page? SetupSnackbar(
        string message,
        string? buttonText = null,
        Action<Components.Page>? onButtonClicked = null)
    {
        // Getting components page
        Components.Page? page = null;
        TabbedPage mainView = (TabbedPage)Application.Current.MainPage;

        if (mainView.CurrentPage is Components.Page comPage)
            page = comPage;
        if (mainView.CurrentPage is NavigationPage navigationPage)
            page = (Components.Page)navigationPage.CurrentPage;

        logger.Log("Got components page");
        if (page is null)
        {
            logger.Log("Tried running snackbar on default page");
            return null;
        }

        page.SnackBarLabel.Text = message;
        page.SnackBarButton.IsVisible = buttonText is not null;
        page.SnackBarButton.Text = buttonText;
        page.OnSnackBarButtonClicked = onButtonClicked;

        logger.Log("Set up snackbar on page");
        return page;
    }


    async Task ShowAsync(
        Components.Page page)
    {
        if (page.IsSnackBarVisible)
            await HideAsync(page);

        page.IsSnackBarVisible = true;
        await page.SnackBar.TranslateTo(0, 0, 100, Easing.Linear);

        logger.Log("Showed snackbar on page");
    }

    async Task HideAsync(
        Components.Page page)
    {
        if (!page.IsSnackBarVisible)
            return;

        await page.SnackBar.TranslateTo(0, 75, 100, Easing.Linear);
        page.IsSnackBarVisible = false;

        logger.Log("Hidden snackbar on page");
    }
}