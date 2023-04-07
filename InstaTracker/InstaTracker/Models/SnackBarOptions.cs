namespace InstaTracker.Models;

public class SnackBarOptions
{
    public SnackBarOptions(
        string message,
        string? buttonText = "Okay",
        int millisecondsDelay = 4000,
        bool closeOnButtonClicked = true,
        Action? onButtonClicked = null,
        Action? onClosing = null)
    {
        Message = message;
        ButtonText = buttonText;
        MillisecondsDelay = millisecondsDelay;
        CloseOnButtonClicked = closeOnButtonClicked;
        OnButtonClicked = onButtonClicked;
        OnClosing = onClosing;
    }


    public string Message { get; set; }

    public string? ButtonText { get; set; }

    public int MillisecondsDelay { get; set; } 

    public bool CloseOnButtonClicked { get; set; }

    public Action? OnButtonClicked { get; set; }

    public Action? OnClosing { get; set; }
}