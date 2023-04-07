using System.Text.Json;
using InstaTracker.Helpers;
using Serilog;

namespace InstaTracker.Services;

public class JsonConverter
{
    readonly ILogger logger;

    public JsonConverter(
        ILogger logger)
    {
        this.logger = logger;

        logger.Log("Registered JsonConverter");
    }


    public string ToString(
        object input)
    {
        logger.Log("Serializing object to string");
        return JsonSerializer.Serialize(input);
    }

    public T? ToObject<T>(
        string input)
    {
        logger.Log("Deserializing string to object");
        return JsonSerializer.Deserialize<T>(input);
    }
}