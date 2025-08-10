using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Tomlyn;

namespace PmlUi.Models;

public class App
{
    private static bool _init = false;
    public static AppData AppData { get; set; }
    public static string AppPath { get; private set; }
    public static string UpdatePath { get; private set; } = "https://raw.githubusercontent.com/ilonic23/PML/"; // BRANCH/path
    public static string InstancesPath { get; private set; }

    public static string CurrentVersion { get; private set; } = "0.2A";
    public static void Initialize()
    {
        if (_init) return;
        _init = true;
        // Read appdata
        AppPath = AppDomain.CurrentDomain.BaseDirectory;
        InstancesPath = Path.Combine(AppPath, "Instances");
        string appdataPath = $"{AppPath}appdata.toml";
        if (File.Exists(appdataPath))
        {
            try
            {
                using (StreamReader reader = new StreamReader(appdataPath))
                {
                    LogWriter.WriteInfo($"Reading app data from {appdataPath}");
                    string file = reader.ReadToEnd();
                    var options = new TomlModelOptions()
                    {
                        IgnoreMissingProperties = true,
                        
                    };
                    AppData = Toml.ToModel<AppData>(file, options: options);
                }
            }
            catch (Exception ex)
            {
                LogWriter.WriteError($"Caught {ex.GetType().Name} when trying to read from \"{appdataPath}\" file: {ex.Message}\n" +
                                     "Will initiate as a new unconfigured app");
                InitiateAsNew();
            }
        }
        else InitiateAsNew();
    }

    private static void InitiateAsNew()
    {
        AppData = new();
        try
        {
            if (Directory.Exists($"{AppPath}Instances"))
                LogWriter.WriteWarning($"Directory \"{AppPath}Instances\" already exists.");
            else Directory.CreateDirectory($"{AppPath}Instances");
        }
        catch (Exception ex)
        {
            LogWriter.WriteError($"Caught {ex.GetType().Name} when trying to create a directory: \"{AppPath}Instances\": {ex.Message}");
        }
    }

    public static string GetAppDataToml()
    {
        return Toml.FromModel(AppData);
    }

    public static void SaveAppDataToml()
    {
        try
        {
            if (File.Exists($"{AppPath}/appdata.toml"))
            {
                File.Delete($"{AppPath}/appdata.toml");
                File.Create($"{AppPath}/appdata.toml").Close();
            }
            using (StreamWriter writer = new StreamWriter($"{AppPath}/appdata.toml"))
            {
                string toml = GetAppDataToml();
                writer.Write(toml);
                writer.Flush();
            }
        }
        catch (Exception ex)
        {
            LogWriter.WriteError($"Caught {ex.GetType().Name} when trying to write to \"{AppPath}/appdata.toml\": {ex.Message}");
        }
    }

    public static async Task<bool> CheckUpdates(string branch = "master")
    {
        LogWriter.WriteInfo($"Fetching updates for {branch} branch...");
        try
        {
            using (HttpClient client = new())
            {
                client.BaseAddress = new Uri($"{UpdatePath}{AppData.UpdateBranch}/version");
                string get = await client.GetStringAsync(client.BaseAddress);
                if (get != CurrentVersion) return true;
            }
        }
        catch (Exception ex)
        {
            LogWriter.WriteInfo($"Caught {ex.GetType().Name} when trying to fetch updates for {branch} branch: {ex.Message}");
        }
        return false;
    }
}