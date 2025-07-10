using System.Diagnostics;
using CmlLib.Core;
using CmlLib.Core.Auth;
using CmlLib.Core.ProcessBuilder;
using CmlLib.Core.Version;

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

    public async Task<List<IVersion>> GetVersionsAsync(bool release = true, bool snapshot = true, bool old = true)
    {
        List<IVersion> versions = new();
        foreach (var version in await _launcher.GetAllVersionsAsync())
        {
            if (release && version.Type == "release")
                versions.Add(await _launcher.GetVersionAsync(version.Name));
            else if (snapshot && version.Type == "snapshot")
                versions.Add(await _launcher.GetVersionAsync(version.Name));
            else if (old && version.Type == "old_alpha" || old && version.Type == "old_beta")
                versions.Add(await _launcher.GetVersionAsync(version.Name));
        }

        return versions;
    }
    public async Task InstallVersionAsync(IVersion version)
    {
        await _launcher.InstallAsync(version);
    }
    public Process BuildProcess(IVersion version, MSession session)
    {
        return _launcher.BuildProcess(version, new MLaunchOption()
        {
            Session = session
        });
    }
}