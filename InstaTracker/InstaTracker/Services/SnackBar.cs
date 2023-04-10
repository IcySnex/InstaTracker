using InstaTracker.Helpers;
using InstaTracker.Models;
using Serilog;
using System.Threading;
using Xamarin.Forms;

namespace InstaTracker.Services;

public class SnackBar
{
    readonly ILogger logger;

    public SnackBar(
        ILogger logger)
    {
        this.logger = logger;

        logger.Log("Registered SnackBar");
    }


    bool isActive = false;
    Components.Page? currentPage = null;
    TaskCompletionSource<bool> snackBarAwaiter = new();
    CancellationTokenSource cancellationTokenSource = new();


    public async Task ShowAsync(
        string message,
        string? buttonText = "Okay",
        int millisecondsDelay = 4000,
        bool closeOnButtonClicked = true,
        Action? onButtonClicked = null,
        Action? onClosing = null,
        bool awaitPreviousSnackBar = false)
    {
        // Wait for active snackbar
        if (isActive)
        {
            if (awaitPreviousSnackBar)
                await snackBarAwaiter.Task;

            cancellationTokenSource.Cancel();
            if (currentPage is not null)
                await HideAsync(currentPage);
            onClosing?.Invoke();
        }


        // Check if components page
        TabbedPage mainView = (TabbedPage)Application.Current.MainPage;
        if (((NavigationPage)mainView.CurrentPage).CurrentPage is not Components.Page page)
        {
            logger.Log("Tried to show snackbar on default page");
            return;
        }

        // Setup snackbar
        page.SnackBarLabel.Text = message;
        page.SnackBarButton.IsVisible = buttonText is not null;
        page.SnackBarButton.Text = buttonText;
        page.OnSnackBarButtonClicked = async () =>
        {
            if (closeOnButtonClicked)
            {
                cancellationTokenSource.Cancel();
                await HideAsync(page);
                onClosing?.Invoke();
            }

            onButtonClicked?.Invoke();
        };

        // Show snackbar
        isActive = true;
        currentPage = page;
        await page.SnackBar.TranslateTo(0, 0, 100, Easing.Linear);
        logger.Log("Showed snackbar");

        // Wait for auto hide snackbar
        try
        {
            await Task.Delay(millisecondsDelay, cancellationTokenSource.Token);
        }
        catch { }

        // Hide snackbar
        if (!cancellationTokenSource.IsCancellationRequested)
        {
            await HideAsync(page);
            onClosing?.Invoke();
        }
    }

    public async void Show(
        string message,
        string? buttonText = "Okay",
        int millisecondsDelay = 4000,
        bool closeOnButtonClicked = true,
        Action? onButtonClicked = null,
        Action? onClosing = null,
        bool awaitPreviousSnackBar = false) =>
        await ShowAsync(message, buttonText, millisecondsDelay, closeOnButtonClicked, onButtonClicked, onClosing, awaitPreviousSnackBar);



    public async Task HideAsync(
        Components.Page page)
    {
        isActive = false;
        await page.SnackBar.TranslateTo(0, 75, 100, Easing.Linear);
        snackBarAwaiter.TrySetResult(true);
        snackBarAwaiter = new();
        cancellationTokenSource = new();
        logger.Log("Hidden current snackbar");
    }
}