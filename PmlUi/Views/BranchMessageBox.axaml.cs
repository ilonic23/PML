using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;

namespace PmlUi.Views;

public partial class BranchMessageBox : Window
{
    public BranchMessageBox()
    {
        InitializeComponent();
        BranchBox.Items.Add("master");
        BranchBox.Items.Add("dev");
    }
    
    public BranchMessageBox(string message, string title)
    {
        InitializeComponent();
        BranchBox.Items.Add("master");
        BranchBox.Items.Add("dev");
        Message.Text = message;
        Title = title;
    }

    private void OkButtonPress(object? sender, RoutedEventArgs e)
    {
        if (BranchBox.SelectedItem == null) return;
        Close(BranchBox.SelectedItem.ToString());
    }
}