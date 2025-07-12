using System.Diagnostics;
using System.Text.RegularExpressions;
using CmlLib.Core;
using CmlLib.Core.Auth;
using CmlLib.Core.ProcessBuilder;

namespace PML;

public class Launcher
{
    private readonly MinecraftLauncher _launcher;

    public string MinecraftNetherAuthFix { get; } =
        "-Dminecraft.api.auth.host=https://nope.invalid -Dminecraft.api.account.host=https://nope.invalid -Dminecraft.api.session.host=https://nope.invalid -Dminecraft.api.services.host=https://nope.invalid";

    public Launcher(string path)
    {
        _launcher = new (new PMLPath(path));
    }

    public bool ValidateNickname(string nickname)
    {
        return Regex.IsMatch(nickname, @"^[A-Za-z0-9_]{3,16}$");
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
        if (!ValidateNickname(session.Username!)) throw new ArgumentException("Invalid nickname.");
        return await _launcher.BuildProcessAsync(version, new MLaunchOption()
        {
            Session = session
        });
    }

    public async Task<Process> BuildProcessAsync(string version, MSession session, int maximumRamMb,
        bool fixNetherUpdateAuth)
    {
        if (!ValidateNickname(session.Username!)) throw new ArgumentException("Invalid nickname.");
        return await _launcher.BuildProcessAsync(version, new MLaunchOption()
        {
            Session = session,
            MaximumRamMb = maximumRamMb,
            ExtraJvmArguments = ( fixNetherUpdateAuth && version.Contains("1.16") ) ?
                new []
                {
                    MArgument.FromCommandLine(MinecraftNetherAuthFix),
                }
                : Array.Empty<MArgument>()
        });
    }

    public async Task<Process> BuildProcessAsync(string version, MSession session, int maximumRamMb)
    {
        if (!ValidateNickname(session.Username!)) throw new ArgumentException("Invalid nickname.");
        return await _launcher.BuildProcessAsync(version, new MLaunchOption()
        {
            Session = session,
            MaximumRamMb = maximumRamMb
        });
    }
    
    public async Task<Process> BuildProcessAsync(string version, MSession session, int maximumRamMb, MArgument[] extraJvmArguments, MArgument[] extraGameArguments)
    {
        if (!ValidateNickname(session.Username!)) throw new ArgumentException("Invalid nickname.");
        return await _launcher.BuildProcessAsync(version, new MLaunchOption()
        {
            Session = session,
            MaximumRamMb = maximumRamMb,
            ExtraJvmArguments = extraJvmArguments,
            ExtraGameArguments = extraGameArguments
        });
    }
}