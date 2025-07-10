using CmlLib.Core;

namespace PML;

public class PMLPath : MinecraftPath
{
    public PMLPath(string path)
    {
        BasePath = NormalizePath(path);

        Library = NormalizePath(BasePath + "/lib");
        Versions = NormalizePath(BasePath + "/ver");
        Resource = NormalizePath(BasePath + "/res");

        Runtime = NormalizePath(BasePath + "/java");
        Assets = NormalizePath(BasePath + "/assets");

        CreateDirs();
    }
}