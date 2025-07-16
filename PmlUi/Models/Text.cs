namespace PmlUi.Models;

public class GlobalText
{
    public string OK { get; set; } = "OK";
    public string Cancel { get; set; } = "Cancel";
    public string Apply { get; set; } = "Apply";
}

public class MainWindowText
{
    public GlobalText GlobalText { get; set; } = new();
    
    public string Nickname { get; set; } = "Nickname";
    public string NicknameWatermark { get; set; } = "Enter nickname";
}

public class AddInstanceWindowText
{
    public GlobalText GlobalText { get; set; } = new();
    public string Version { get; set; } = "Version";
    public string Name { get; set; } = "Name";
    public string NameWatermark { get; set; } = "Enter name";
    public string VersionWatermark { get; set; } = "Select a version";
}