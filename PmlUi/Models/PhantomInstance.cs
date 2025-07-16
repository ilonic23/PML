using System.Diagnostics;
using System.Threading.Tasks;
using CmlLib.Core.Auth;
using PML;

namespace PmlUi.Models;

public class PhantomInstance
{
    private string _path = "";
    private Launcher _launcher;
    public string Name { get; set; }
    public string Version { get; set; }
    public string CategoryName { get; set; }

    public string Path
    {
        get => _path;
        set
        {
            _path = value;
            Launcher = new Launcher(_path);
        }
    }

    public ModLoader ModLoader { get; set; }

    public Launcher Launcher
    {
        get => _launcher;
        set
        {
            if (_launcher != null)
            {
                if (!_launcher.IsEmptyConstructorUsed())
                    _launcher = value;
            }
            else
            {
                _launcher = value;
            }
        }
    }

    public PhantomInstance()
    {
        
    }

    public PhantomInstance(string name, string version, string path, string categoryName = "", ModLoader modLoader = ModLoader.None)
    {
        Name = name;
        Version = version;
        CategoryName = categoryName;
        Path = path;
        ModLoader = modLoader;
        Launcher = new(path);
    }

    public async Task InstallInstance()
    {
        await Launcher.InstallVersionAsync(Version);
    }

    public async Task<Process> BuildInstanceProcess(MSession session)
    {
        return await Launcher.BuildProcessAsync(Version, session);
    }
}

public enum ModLoader
{
    None,
    Forge,
    Fabric,
    NeoForge,
    Quilt
}