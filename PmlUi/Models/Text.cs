namespace PmlUi.Models;

public class GlobalText
{
    public string OK { get; set; } = "OK";
    public string Cancel { get; set; } = "Cancel";
    public string Apply { get; set; } = "Apply";
    public string Yes { get; set; } = "Yes";
    public string No { get; set; } = "No";
    public string Error { get; set; } = "Error";
    public string Warning { get; set; } = "Warning";
    public string UpdateDetectedMessage { get; set; } = "There is an update for your selected branch";
    public string Update { get; set; } = "Update";
    public string PleaseSelectABranch { get; set; } = "Please select an update branch.\nmaster - releases for everyone\ndev - newest but buggy";
    public string BranchSelection { get; set; } = "Branch selection";
}

public class MainWindowText
{
    public GlobalText GlobalText { get; set; } = new();
    public string Nickname { get; set; } = "Nickname";
    public string NicknameWatermark { get; set; } = "Enter nickname";
    public string ThatInstanceAlreadyExists { get; set; } = "Instance already exists";
    public string IncorrectNickname { get; set; } = "Incorrect nickname";
    public string InstanceDeletionConfirmation { get; set; } = "Are you sure you want to delete this instance?";
}

public class AddInstanceWindowText
{
    public GlobalText GlobalText { get; set; } = new();
    public string Version { get; set; } = "Version";
    public string Name { get; set; } = "Name";
    public string NameWatermark { get; set; } = "Enter name";
    public string VersionWatermark { get; set; } = "Select a version";
}