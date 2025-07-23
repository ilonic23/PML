using System.Collections.Generic;

namespace PmlUi.Models;

public class AppData
{
    // public string Global { get; set; } = "Ignore this, used for toml";
    public string Language { get; set; } = "en";
    public string Nickname { get; set; } = "";
    public string UpdateBranch  { get; set; } = "";

    public AppData()
    {
    }
    
    public AppData(string language)
    {
        Language = language;
    }
    public AppData(string language, string nicknames)
    {
        Language = language;
        Nickname = nicknames;
    }
}