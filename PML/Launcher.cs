using System.Diagnostics;
using CmlLib.Core;
using CmlLib.Core.Auth;
using CmlLib.Core.ProcessBuilder;

namespace PML;

public class Launcher
{
    private MinecraftLauncher _launcher;
    private MinecraftPath _path;

    public Launcher(string path)
    {
        _path = new PMLPath(path);
        _launcher = new (_path);
    }

    public async Task<List<string>> GetVersionsAsync(bool release = true, bool snapshot = true, bool old = true)
    {
        List<string> versions = new();
        foreach (var version in await _launcher.GetAllVersionsAsync())
        {
            if (release && version.Type == "release")
                versions.Add(version.Name);
            else if (snapshot && version.Type == "snapshot")
                versions.Add(version.Name);
            else if (old && version.Type == "old_alpha" || old && version.Type == "old_beta")
                versions.Add(version.Name);
        }

        return versions;
    }
    public async Task InstallVersionAsync(string version)
    {
        await _launcher.InstallAsync(version);
    }
    public async Task<Process> BuildProcessAsync(string version, MSession session)
    {
        return await _launcher.BuildProcessAsync(version, new MLaunchOption()
        {
            Session = session
        });
    }
}