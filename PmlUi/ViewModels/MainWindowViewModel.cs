using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Avalonia.Controls;
using Avalonia.Media;
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
    private async void AddInstance()
    {
        if (Design.IsDesignMode) return;
        try
        {
            LogWriter.WriteInfo($"Create new instance of {nameof(AddInstanceWindow)}");
            AddInstanceWindow window = new();
            PhantomInstance? instance = await window.ShowDialog<PhantomInstance?>(MainWindow.Current);
            if (instance == null) return;
            InstanceDisplayer? d =
                MainWindow.Current.InstancesDisplayPanel.Children.OfType<InstanceDisplayer>().
                    FirstOrDefault(find => find.DisplayText == instance.Name);
            if (d != null)
            {
                MessageBox mb = new(LocalText.ThatInstanceAlreadyExists, LocalText.GlobalText.Error, MbButtons.Ok);
                await mb.ShowDialog(mb);
                return;
            }
            try
            {
                LogWriter.WriteInfo($"Writing metadata.toml to {instance.Path}/metadata.toml");
                await using (StreamWriter writer = File.CreateText(Path.Combine(instance.Path, "metadata.toml")))
                {
                    await writer.WriteAsync(Toml.FromModel(instance));
                }
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
    private void InstanceDisplayerPress(object? sender)
    {
        var btn = sender as Button;
        if (sender != null && btn != null)
        {
            InstanceDisplayer d;
            Color color;
            if (MainWindow.Current.CurrentInstance == null)
            {
                MainWindow.Current.CurrentInstance = btn.Content!.ToString()!;
                d = MainWindow.Current.InstancesDisplayPanel.Children.OfType<InstanceDisplayer>().
                    First(find => find.DisplayText == MainWindow.Current.CurrentInstance);
                color = Color.FromArgb(50, 255,255,255);
                d.Background = new SolidColorBrush(color);
                return;
            }
            d = MainWindow.Current.InstancesDisplayPanel.Children.OfType<InstanceDisplayer>().
                    First(find => find.DisplayText == MainWindow.Current.CurrentInstance);
            d.Background = Brushes.Transparent;
            
            MainWindow.Current.CurrentInstance = btn.Content!.ToString()!;
            d = MainWindow.Current.InstancesDisplayPanel.Children.OfType<InstanceDisplayer>().
                    First(find => find.DisplayText == MainWindow.Current.CurrentInstance);
            color = Color.FromArgb(50, 255,255,255);
            d.Background = new SolidColorBrush(color);
        }
    }

    [RelayCommand]
    private async Task PlayButton()
    {
        InstanceDisplayer d = MainWindow.Current.InstancesDisplayPanel.Children.OfType<InstanceDisplayer>().
            First(find => find.DisplayText == MainWindow.Current.CurrentInstance);
        try
        {
            LogWriter.WriteInfo("Installing instance...");
            d.Instance.Launcher.ByteProgressChanged += (sender, progress) =>
            {
                MainWindow.Current.InstallationProgress.Maximum = progress.TotalBytes / (1024.0 * 1024);
                MainWindow.Current.InstallationProgress.Value = progress.ProgressedBytes / (1024.0 * 1024);
            };
            await d.Instance.InstallInstance();
            try
            {
                LogWriter.WriteInfo($"Starting instance...");
                if (!Launcher.ValidateNickname(Models.App.AppData.Nickname)) return;
                Process process = await d.Instance.BuildInstanceProcess(MSession.CreateOfflineSession(Models.App.AppData.Nickname));
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
    private async void RemoveInstance()
    {
        InstanceDisplayer d = MainWindow.Current.InstancesDisplayPanel.Children.OfType<InstanceDisplayer>().
            First(find => find.DisplayText == MainWindow.Current.CurrentInstance);

        MessageBox mb = new(LocalText.InstanceDeletionConfirmation, LocalText.GlobalText.Warning, MbButtons.YesNo);
        var result = await mb.ShowDialog<MbResult>(MainWindow.Current);
        if (result == MbResult.No) return;
        try
        {
            LogWriter.WriteWarning("Removing instance...");
            Directory.Delete(d.Instance.Path, true);
            MainWindow.Current.InstancesDisplayPanel.Children.Remove(d);
        }
        catch (Exception ex)
        {
            LogWriter.WriteError($"Caught {ex.GetType().Name} when trying to remove instance \"{d.Instance.Name}\": {ex.Message}");
        }
    }

    [RelayCommand]
    private void SetInstancesPanel()
    {
        MainWindow.Current.InstancesPanel.IsVisible = true;
        MainWindow.Current.AccountsPanel.IsVisible = false;
    }
    
    [RelayCommand]
    private void SetAccountsPanel()
    {
        MainWindow.Current.InstancesPanel.IsVisible = false;
        MainWindow.Current.AccountsPanel.IsVisible = true;
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