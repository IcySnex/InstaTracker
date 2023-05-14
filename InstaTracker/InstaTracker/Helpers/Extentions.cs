using System.Runtime.CompilerServices;
using Xamarin.Forms;
using Serilog;
using Serilog.Events;
using System;
using System.Threading.Tasks;
using System.Linq;
using System.Threading;
using InstagramApiSharp.Classes;
using static SQLite.SQLite3;
using InstagramApiSharp.Classes.Models;
using InstagramApiSharp.API;

namespace InstaTracker.Helpers;
    
public static class Extentions
{
    public static void Log(this ILogger logger,
        object? message,
        Exception? exception = null,
        LogEventLevel logLevel = LogEventLevel.Information,
        [CallerFilePath] string filePath = "",
        [CallerMemberName] string memberName = "") =>
        logger.Write(exception is null ? logLevel : LogEventLevel.Error, exception, "[{filePath}-{memberName}] {message}{error}", filePath.Contains('/') ? filePath.Split('/').Last() : filePath.Split('\\').Last(), memberName, message, exception.Format());


    public static string Format(this Exception? input) =>
        input is null ? "" :
        input.InnerException is null ? $": {input.Message}" :
        $": {input.Message}\n\t{input.InnerException.Message.Replace("\n", "\n\t")}";


    public static Task<bool> WidthTo(this VisualElement input,
        double width,
        uint duration = 250,
        Easing? easing = null,
        Action<double, bool>? finishedCallback = null)
    {
        TaskCompletionSource<bool> tcs = new();

        new Animation(x => input.WidthRequest = x, input.Width, width)
            .Commit(input, "WidthAnimation", 10, duration, easing, (finalValue, finished) =>
            {
                finishedCallback?.Invoke(finalValue, finished);
                tcs.SetResult(finished);
            });

        return tcs.Task;
    }


    public static Task<bool> ColorBackgroundTo(this VisualElement input,
        Color color,
        uint length = 250,
        Easing? easing = null)
    {
        Func<double, Color> transform = (t) =>
          Color.FromRgba(input.BackgroundColor.R + t * (color.R - input.BackgroundColor.R),
                         input.BackgroundColor.G + t * (color.G - input.BackgroundColor.G),
                         input.BackgroundColor.B + t * (color.B - input.BackgroundColor.B),
                         input.BackgroundColor.A + t * (color.A - input.BackgroundColor.A));
        return ColorAnimation(input, "BackgroundColorAnimation", transform, c => input.BackgroundColor = c, length, easing);
    }

    public static Task<bool> ColorBorderTo(this Frame input,
        Color color,
        uint length = 250,
        Easing? easing = null)
    {
        Func<double, Color> transform = (t) =>
          Color.FromRgba(input.BorderColor.R + t * (color.R - input.BorderColor.R),
                         input.BorderColor.G + t * (color.G - input.BorderColor.G),
                         input.BorderColor.B + t * (color.B - input.BorderColor.B),
                         input.BorderColor.A + t * (color.A - input.BorderColor.A));
        return ColorAnimation(input, "BorderColorAnimation", transform, c => input.BorderColor = c, length, easing);
    }

    static Task<bool> ColorAnimation(
        VisualElement element,
        string name,
        Func<double, Color> transform,
        Action<Color> callback,
        uint length,
        Easing? easing)
    {
        TaskCompletionSource<bool> tcs = new();

        element.Animate(name, transform, callback, 16, length, easing, (v, c) => tcs.SetResult(c));
        return tcs.Task;
    }


    public static void ThrowIfUnauthenticated(this IInstaApi input,
        string message,
        ILogger logger)
    {
        if (input.IsUserAuthenticated)
            return;

        Exception innerException = new("No account is currently logged in.");
        logger.Log(message, innerException);

        throw new(message, innerException);
    }

    public static void ThrowIfAuthenticated(this IInstaApi input,
        string message,
        ILogger logger)
    {
        if (!input.IsUserAuthenticated)
            return;

        Exception innerException = new("An account is already logged in.");
        logger.Log(message, innerException);

        throw new(message, innerException);
    }

    public static void ThrowIfFailed<T>(this IResult<T> input,
        string message,
        ILogger logger)
    {
        if (input.Succeeded)
            return;

        Exception innerException = new($"{char.ToUpper(input.Info.Message[0])}{input.Info.Message.Substring(1).Replace('_', ' ')}.");
        logger.Log(message, innerException);

        throw new(message, innerException);
    }
}