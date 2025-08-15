using System;
using System.IO;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Interactivity;
using PmlUi.Models;
using PmlUi.ViewModels;
using Tomlyn;

namespace PmlUi.Views;

public partial class MainWindow : Window
{
    public static MainWindow Current { get; private set; }

    public string? CurrentInstance { get; set; }
    
    public MainWindow()
    {
        InitializeComponent();
        DataContext = new MainWindowViewModel();
        Current = this;
        CurrentInstance = null;
        NicknameBox.Text = Models.App.AppData.Nickname;
        LoadInstances();
        if (Design.IsDesignMode) return;
        Loaded += OnLoaded;
    }

    private async void OnLoaded(object? sender, RoutedEventArgs e)
    {
        await ProcessUpdates();
    }

    private async Task ProcessUpdates()
    {
        var context = (MainWindowViewModel)DataContext!;
        if (Models.App.AppData.UpdateBranch is not "master" and not "dev")
        {
            BranchMessageBox bmb = new(context.LocalText.GlobalText.PleaseSelectABranch, context.LocalText.GlobalText.BranchSelection);
            Models.App.AppData.UpdateBranch = await bmb.ShowDialog<string>(this);
            return;
        }
        bool update = await Models.App.CheckUpdates();
        if (update)
        {
            MessageBox mb = new(context.LocalText.GlobalText.UpdateDetectedMessage, context.LocalText.GlobalText.Update);
            await mb.ShowDialog(this);
        }
    }

    private void LoadInstances()
    {
        try
        {
            foreach ( string dir in Directory.GetDirectories(Models.App.InstancesPath) )
            {
                if (!File.Exists(Path.Combine(dir, "metadata.toml"))) continue;
                LogWriter.WriteInfo("Loading metadata.toml");
                try
                {
                    var vm = (DataContext as MainWindowViewModel)!;
                    string file = File.ReadAllText(Path.Combine(dir, "metadata.toml"));
                    var instance = Toml.ToModel<PhantomInstance>(file);
                        
                    InstanceDisplayer displayer = new()
                    {
                        DisplayText = instance.Name,
                        OnPressCommand = vm.InstanceDisplayerPressCommand,
                        Instance = instance
                    };
                    InstancesDisplayPanel.Children.Add(displayer);
                }
                catch (Exception ex)
                {
                    LogWriter.WriteError($"Caught {ex.GetType().Name} when trying to load an instance: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            LogWriter.WriteError($"Caught {ex.GetType().Name} when trying to enumerate instances: {ex.Message}");
        }
    }

    private void MainWindowClosing(object? sender, WindowClosingEventArgs e)
    {
        Models.App.SaveAppDataToml();
    }
}