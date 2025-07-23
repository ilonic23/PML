using System.IO;
using CommunityToolkit.Mvvm.Input;
using PmlUi.Models;
using PmlUi.Views;

namespace PmlUi.ViewModels;

public partial class AddInstanceWindowViewModel : ViewModelBase
{
    public Palette Palette { get; } = new();
    
    public GlobalText GlobalText { get; } = new();
    public AddInstanceWindowText LocalText { get; } = new();

    [RelayCommand]
    private void Ok()
    {
        string? name = AddInstanceWindow.CurrentWindow.NameBox.Text;
        object? version = AddInstanceWindow.CurrentWindow.VersionBox.SelectedItem;
        if (name == null || version == null)
        {
            MessageBox mb = new();
            return;
        }
        PhantomInstance result = new(name, version.ToString()!, Path.Combine(Models.App.InstancesPath, name));
        AddInstanceWindow.CurrentWindow.Close(result);
    }

    [RelayCommand]
    private void Cancel()
    {
        AddInstanceWindow.CurrentWindow.Close(null);
    }
}