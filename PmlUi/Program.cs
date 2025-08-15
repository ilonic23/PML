using Avalonia;
using System;
using System.Diagnostics;
using PmlUi.Models;

namespace PmlUi;

static class Program
{
    [STAThread]
    public static void Main(string[] args)
    {
        LogWriter.WriteInfo("Starting PML UI");
        try
        {
            BuildAvaloniaApp()
                .StartWithClassicDesktopLifetime(args);
        }
        catch (Exception ex)
        {
            LogWriter.WriteError($"Caught a global {ex.GetType().Name}: {ex.Message}");
            LogWriter.WriteInfo("Unhandled global exception, process will be terminated after this message, please tell the dev.");
            Process.GetCurrentProcess().Kill();
        }
        finally
        {
           LogWriter.WriteInfo("Exiting PML UI"); 
        }
    }

    // Avalonia configuration, don't remove; also used by visual designer.
    public static AppBuilder BuildAvaloniaApp()
        => AppBuilder.Configure<App>()
            .UsePlatformDetect()
            .WithInterFont()
            .LogToTrace();
}