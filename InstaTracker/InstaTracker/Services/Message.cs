using InstaTracker.Helpers;
using Serilog;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace InstaTracker.Services;

public class Message
{
    readonly ILogger logger;

    public Message(
        ILogger logger)
    {
        this.logger = logger;
    }


    public Task ShowAsync(
        string title,
        string message,
        string cancelButtonText = "Okay")
    {
        logger.Log("Displaying information alert");
        return Application.Current.MainPage.DisplayAlert(title, message, cancelButtonText);
    }

    public Task<bool> ShowQuestionAsync(
        string title,
        string message,
        string cancelButtonText = "No",
        string acceptButtonText = "Yes")
    {
        logger.Log("Displaying question alert");
        return Application.Current.MainPage.DisplayAlert(title, message, acceptButtonText, cancelButtonText);
    }

    public Task<string> ShowPromptAsync(
        string title,
        string message,
        string cancelButtonText = "Cancel",
        string acceptButtonText = "Okay",
        string? placeholderText = null,
        int maxLength = -1,
        Keyboard? keyboard = null,
        string initialText = "")
    {
        logger.Log("Displaying prompt alert");
        return Application.Current.MainPage.DisplayPromptAsync(title, message, acceptButtonText, cancelButtonText, placeholderText, maxLength, keyboard, initialText);
    }
}