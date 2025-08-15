using System.Threading.Tasks;
using Avalonia.Controls;
using PmlUi.Models;

namespace PmlUi.Views;

public partial class AddInstanceWindow : Window
{
    public static AddInstanceWindow CurrentWindow { get; private set; }

        
    public AddInstanceWindow()
    {
        InitializeComponent();
        if (Design.IsDesignMode) return;
        CurrentWindow = this;
        _ = AddVersionsAsync();
    }
    

    private async Task AddVersionsAsync()
    {
        foreach (var version in await new PML.Launcher().GetVersionsAsync())
            VersionBox.Items.Add(version);
    }
    
}