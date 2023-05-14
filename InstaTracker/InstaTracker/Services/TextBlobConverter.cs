using SQLiteNetExtensions.Extensions.TextBlob;
using System;

namespace InstaTracker.Services;

class TextBlobConverter : ITextBlobSerializer
{
    readonly JsonConverter converter;

    public TextBlobConverter(
        JsonConverter converter)
    {
        this.converter = converter;
    }


    public object Deserialize(
        string text,
        Type type) =>
        converter.ToS

    public string Serialize(object element)
    {
        throw new NotImplementedException();
    }
}