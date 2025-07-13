using System;
using Avalonia.Controls;

namespace PmlUi.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private void MainWindowClosing(object? sender, WindowClosingEventArgs e)
    {
        throw new AggregateException("Test closing exception!");
    }
}