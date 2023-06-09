﻿using Serilog.Core;
using Serilog.Events;
using Serilog.Formatting.Display;
using Serilog.Formatting;
using System;
using System.IO;

namespace InstaTracker.Helpers;

public class InMemorySink : ILogEventSink
{
    readonly ITextFormatter textFormatter = new MessageTemplateTextFormatter("[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}");

    public event EventHandler<string>? OnNewLog;

    public void Emit(
        LogEvent logEvent)
    {
        var renderSpace = new StringWriter();
        textFormatter.Format(logEvent, renderSpace);

        OnNewLog?.Invoke(this, renderSpace.ToString());
    }
}