using System;
using System.Collections.Generic;
using System.IO;
using Avalonia.Controls;
using Avalonia.Logging;
using CommunityToolkit.Mvvm.Input;
using PmlUi.Models;
using PmlUi.ViewModels;
using Tomlyn;

namespace PmlUi.Views;

public partial class MainWindow : Window
{
    public static MainWindow Current { get; private set; }
    private string _currentInstance;
    
    public string? CurrentInstance
    {
        get
        {
            return _currentInstance;
        }
        set
        {
            _currentInstance = value;
        }
    }
    
    public MainWindow()
    {
        InitializeComponent();
        DataContext = new MainWindowViewModel();
        Current = this;
        CurrentInstance = null;
        NicknameBox.Text = Models.App.AppData.Nickname;
        LoadInstances();
    }

    private void LoadInstances()
    {
        try
        {
            foreach ( string dir in Directory.GetDirectories(Models.App.InstancesPath) )
            {
                if ( File.Exists( Path.Combine(dir, "metadata.toml") ) )
                {
                    LogWriter.WriteInfo("Loading metadata.toml");
                    try
                    {
                        MainWindowViewModel vm = (DataContext as MainWindowViewModel)!;
                        string file = File.ReadAllText(Path.Combine(dir, "metadata.toml"));
                        PhantomInstance instance = Toml.ToModel<PhantomInstance>(file);
                        
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