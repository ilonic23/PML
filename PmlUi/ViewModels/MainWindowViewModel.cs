using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Controls;
using CmlLib.Core;
using CmlLib.Core.Auth;
using CommunityToolkit.Mvvm.Input;
using PML;
using PmlUi.Models;
using PmlUi.Views;
using Tomlyn;

namespace PmlUi.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    public Palette Palette { get; } = new();
    public MainWindowText LocalText { get; } = new();
    
    [RelayCommand]
    private async Task AddInstance()
    {
        if (Design.IsDesignMode) return;
        try
        {
            LogWriter.WriteInfo($"Create new instance of {nameof(AddInstanceWindow)}");
            AddInstanceWindow window = new();
            var instance = await window.ShowDialog<PhantomInstance?>(MainWindow.Current);
            if (instance == null) return;
            var d =
                MainWindow.Current.InstancesDisplayPanel.Children.OfType<InstanceDisplayer>().
                    FirstOrDefault(find => find.DisplayText == instance.Name);
            if (d != null)
            {
                MessageBox mb = new(LocalText.ThatInstanceAlreadyExists, LocalText.GlobalText.Error);
                await mb.ShowDialog(mb);
                return;
            }
            try
            {
                LogWriter.WriteInfo($"Writing metadata.toml to {instance.Path}/metadata.toml");
                await using StreamWriter writer = File.CreateText(Path.Combine(instance.Path, "metadata.toml"));
                await writer.WriteAsync(Toml.FromModel(instance));
            }
            catch (Exception ex)
            {
                LogWriter.WriteError($"Caught {ex.GetType().Name} when trying to handle {nameof(PhantomInstance)}: {ex.Message}");
            }
            
            InstanceDisplayer displayer = new()
            {
                DisplayText = instance.Name,
                OnPressCommand = InstanceDisplayerPressCommand,
                Instance = instance
            };
            MainWindow.Current.InstancesDisplayPanel.Children.Add(displayer);
            
        }
        catch (Exception ex)
        {
            LogWriter.WriteError($"Caught {ex.GetType().Name} when trying to create a window: {nameof(AddInstanceWindow)}: {ex.Message}");
        }
    }

    [RelayCommand]
    private static void InstanceDisplayerPress(object? sender)
    {
        if (sender is not Button btn) return;
        InstanceDisplayer displayer;
        if (MainWindow.Current.CurrentInstance == null)
        {
            MainWindow.Current.CurrentInstance = btn.Content!.ToString()!;
            displayer = (btn.Parent as InstanceDisplayer)!;
            displayer.ToggleSelected();
            return;
        }
            
            
        displayer = (btn.Parent as InstanceDisplayer)!;
        displayer.ToggleSelected();
            
        MainWindow.Current.CurrentInstance = btn.Content!.ToString()!;
        displayer = MainWindow.Current.InstancesDisplayPanel.Children.OfType<InstanceDisplayer>().
            First(find => find.DisplayText == MainWindow.Current.CurrentInstance);
        displayer.ToggleSelected();
    }

    [RelayCommand]
    private async Task PlayButton()
    {
        InstanceDisplayer d = MainWindow.Current.InstancesDisplayPanel.Children.OfType<InstanceDisplayer>().
            First(find => find.DisplayText == MainWindow.Current.CurrentInstance);
        try
        {
            LogWriter.WriteInfo("Installing instance...");
            
            void ProcessByteProgress(object? sender, ByteProgress progress) =>
                MainWindow.Current.InstallationProgress.Value = progress.ProgressedBytes / (double)progress.TotalBytes * 100;
            
            d.Instance.Launcher.ByteProgressChanged += ProcessByteProgress;
            
            await d.Instance.InstallInstance();
            d.Instance.Launcher.ByteProgressChanged -= ProcessByteProgress;
            try
            {
                if (!Launcher.ValidateNickname(Models.App.AppData.Nickname))
                {
                    SetAccountsPanel();
                    MessageBox mb = new(LocalText.IncorrectNickname, LocalText.GlobalText.Error);
                    await mb.ShowDialog(MainWindow.Current);
                    return;
                }
                LogWriter.WriteInfo($"Starting instance...");
                var process = await d.Instance.BuildInstanceProcess(MSession.CreateOfflineSession(Models.App.AppData.Nickname));
                process.Start();
            }
            catch (Exception ex)
            {
                LogWriter.WriteError($"Caught {ex.GetType().Name} when trying to start instance \"{d.Instance.Name}\": {ex.Message}");
            }
        }
        catch (Exception ex)
        {
            LogWriter.WriteError($"Caught {ex.GetType().Name} when trying to install instance \"{d.Instance.Name}\": {ex.Message}");
        }
    }

    [RelayCommand]
    private async Task RemoveInstance()
    {
        var d = MainWindow.Current.InstancesDisplayPanel.Children.OfType<InstanceDisplayer>().
            First(find => find.DisplayText == MainWindow.Current.CurrentInstance);

        MessageBox mb = new(LocalText.InstanceDeletionConfirmation, LocalText.GlobalText.Warning, MbButtons.YesNo);
        var result = await mb.ShowDialog<MbResult>(MainWindow.Current);
        if (result == MbResult.No) return;
        try
        {
            LogWriter.WriteWarning("Removing instance...");
            Directory.Delete(d.Instance.Path, true);
            MainWindow.Current.InstancesDisplayPanel.Children.Remove(d);
            MainWindow.Current.CurrentInstance = null;
        }
        catch (Exception ex)
        {
            LogWriter.WriteError($"Caught {ex.GetType().Name} when trying to remove instance \"{d.Instance.Name}\": {ex.Message}");
        }
    }

    [RelayCommand]
    private static void SetInstancesPanel()
    {
        MainWindow.Current.InstancesPanel.IsVisible = true;
        MainWindow.Current.AccountsPanel.IsVisible = false;
        MainWindow.Current.SettingsPanel.IsVisible = false;
    }
    
    [RelayCommand]
    private static void SetAccountsPanel()
    {
        MainWindow.Current.InstancesPanel.IsVisible = false;
        MainWindow.Current.AccountsPanel.IsVisible = true;
        MainWindow.Current.SettingsPanel.IsVisible = false;
    }

    [RelayCommand]
    private static void SetSettingsPanel()
    {
        MainWindow.Current.InstancesPanel.IsVisible = false;
        MainWindow.Current.AccountsPanel.IsVisible = false;
        MainWindow.Current.SettingsPanel.IsVisible = true;
    }
    
    [RelayCommand]
    private async Task ChangeUpdateBranch()
    {
        var mb = new BranchMessageBox(LocalText.GlobalText.PleaseSelectABranch, LocalText.GlobalText.BranchSelection); 
        await mb.ShowDialog(MainWindow.Current);
    }

    [RelayCommand]
    private async Task ResetSettings()
    {
        MessageBox mb = new(LocalText.SettingsResetConfirmation, LocalText.GlobalText.Warning, MbButtons.YesNo);
        var result = await mb.ShowDialog<MbResult>(MainWindow.Current);
        if (result == MbResult.No) return;
        try
        {
            LogWriter.WriteWarning("Resetting settings...");
            File.Delete(Path.Combine(Models.App.AppPath, "appdata.toml"));
        }
        catch (Exception ex)
        {
            LogWriter.WriteError($"Caught {ex.GetType().Name} when trying to reset settings: {ex.Message}");
        }
    }

    [RelayCommand]
    private void NicknameApply()
    {
        string? nickname = MainWindow.Current.NicknameBox.Text;
        if (nickname == null || !Launcher.ValidateNickname(nickname))
        {
            MessageBox mb = new(LocalText.IncorrectNickname, LocalText.GlobalText.Error);
            mb.ShowDialog(MainWindow.Current);
            return;
        }
        Models.App.AppData.Nickname = nickname;
    }
}