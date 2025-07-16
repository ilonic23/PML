using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using PmlUi.Models;
using PmlUi.ViewModels;

namespace PmlUi.Views;

public partial class AddInstanceWindow : Window
{
    public bool ButtonPressed = false;
    public static AddInstanceWindow CurrentWindow { get; private set; }
    public PhantomInstance Result { get; private set; }

        
    public AddInstanceWindow()
    {
        InitializeComponent();
        if (Design.IsDesignMode) return;
        CurrentWindow = this;
        AddVersionsAsync();
    }
    

    private async Task AddVersionsAsync()
    {
        foreach (var version in await new PML.Launcher().GetVersionsAsync())
            VersionBox.Items.Add(version);
    }
    
}