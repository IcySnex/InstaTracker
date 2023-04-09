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
    TaskCompletionSource<bool> snackBarAwaiter = new();
    CancellationTokenSource cancellationTokenSource = new();


    public async Task ShowAsync(
        SnackBarOptions options,
        bool awaitPreviousSnackBar = false)
    {
        // Wait for active snackbar
        if (isActive)
        {
            if (awaitPreviousSnackBar)
                await snackBarAwaiter.Task;

            cancellationTokenSource.Cancel();
            await HideCurrentAsync();
            options.OnClosing?.Invoke();
        }


        // Check if components page
        TabbedPage mainView = (TabbedPage)Application.Current.MainPage;
        if (((NavigationPage)mainView.CurrentPage).CurrentPage is not Components.Page page)
        {
            logger.Log("Tried to show snackbar on default page");
            return;
        }

        // Setup snackbar
        page.SnackBarLabel.Text = options.Message;
        page.SnackBarButton.IsVisible = options.ButtonText is not null;
        page.SnackBarButton.Text = options.ButtonText;
        page.OnSnackBarButtonClicked = async () =>
        {
            if (options.CloseOnButtonClicked)
            {
                cancellationTokenSource.Cancel();
                await HideCurrentAsync();
                options.OnClosing?.Invoke();
            }

            options.OnButtonClicked?.Invoke();
        };

        // Show snackbar
        isActive = true;
        await page.SnackBar.TranslateTo(0, 0, 100, Easing.Linear);
        logger.Log("Showed snackbar");

        // Wait for auto hide snackbar
        try
        {
            await Task.Delay(options.MillisecondsDelay, cancellationTokenSource.Token);
        }
        catch { }

        // Hide snackbar
        if (!cancellationTokenSource.IsCancellationRequested)
        {
            await HideCurrentAsync();
            options.OnClosing?.Invoke();
        }
    }

    public Task ShowAsync(
        string message,
        string? buttonText = "Okay",
        int millisecondsDelay = 4000,
        bool closeOnButtonClicked = true,
        Action? onButtonClicked = null,
        Action? onClosing = null,
        bool awaitPreviousSnackBar = false) =>
        ShowAsync(new(message, buttonText, millisecondsDelay, closeOnButtonClicked, onButtonClicked, onClosing), awaitPreviousSnackBar);


    public async void Show(
        SnackBarOptions options,
        bool awaitPreviousSnackBar = false) =>
        await ShowAsync(new(options.Message, options.ButtonText, options.MillisecondsDelay, options.CloseOnButtonClicked, options.OnButtonClicked, options.OnClosing), awaitPreviousSnackBar);

    public async void Show(
        string message,
        string? buttonText = "Okay",
        int millisecondsDelay = 4000,
        bool closeOnButtonClicked = true,
        Action? onButtonClicked = null,
        Action? onClosing = null,
        bool awaitPreviousSnackBar = false) =>
        await ShowAsync(new(message, buttonText, millisecondsDelay, closeOnButtonClicked, onButtonClicked, onClosing), awaitPreviousSnackBar);



    public async Task HideCurrentAsync()
    {
        TabbedPage mainView = (TabbedPage)Application.Current.MainPage;
        if (((NavigationPage)mainView.CurrentPage).CurrentPage is not Components.Page page)
        {
            logger.Log("Tried to hide current snackbar on default page");
            return;
        }

        isActive = false;
        await page.SnackBar.TranslateTo(0, 75, 100, Easing.Linear);
        snackBarAwaiter.TrySetResult(true);
        snackBarAwaiter = new();
        cancellationTokenSource = new();
        logger.Log("Hidden current snackbar");
    }
}