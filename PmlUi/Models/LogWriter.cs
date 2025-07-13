using System;

namespace PmlUi.Models;

public static class LogWriter
{
    public static void WriteMessage(string msg)
    {
        Console.ForegroundColor = ConsoleColor.White;
        Console.Write("[MSG] ");
        Console.ResetColor();
        Console.WriteLine(msg);
    }
    public static void WriteWarning(string msg)
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.Write("[WARNING] ");
        Console.ResetColor();
        Console.WriteLine(msg);
    }
    public static void WriteInfo(string msg)
    {
        Console.ForegroundColor = ConsoleColor.Blue;
        Console.Write("[INFO] ");
        Console.ResetColor();
        Console.WriteLine(msg);
    }
    public static void WriteSuccess(string msg)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.Write("[SUCCESS] ");
        Console.ResetColor();
        Console.WriteLine(msg);
    }
    public static void WriteError(string msg)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.Write("[ERROR] ");
        Console.ResetColor();
        Console.WriteLine(msg);
    }
}