using System;
using Avalonia.Controls;
using Avalonia.Interactivity;
using PmlUi.ViewModels;

namespace PmlUi.Views;

public partial class MessageBox : Window
{
    private readonly MbButtons _buttons;
    
    public MessageBox()
    {
        InitializeComponent();
    }

    public MessageBox(string message, string title, MbButtons buttons = MbButtons.Ok)
    {
        InitializeComponent();
        Title = title;
        Message.Text = message;
        var dataContext = (MessageBoxViewModel)DataContext!;
        _buttons = buttons;
        switch (_buttons)
        {
            case MbButtons.Ok:
                ButtonCenter.Content = dataContext.GlobalText.Ok;
                ButtonLeft.IsVisible = false;
                ButtonRight.IsVisible = false;
                break;
            case MbButtons.OkCancel:
                ButtonLeft.Content = dataContext.GlobalText.Ok;
                ButtonRight.Content = dataContext.GlobalText.Cancel;
                ButtonCenter.IsVisible = false;
                break;
            case MbButtons.YesNo:
                ButtonLeft.Content = dataContext.GlobalText.Yes;
                ButtonRight.Content = dataContext.GlobalText.No;
                ButtonCenter.IsVisible = false;
                break;
            case MbButtons.YesNoCancel:
                ButtonLeft.Content = dataContext.GlobalText.Yes;
                ButtonCenter.Content = dataContext.GlobalText.No;
                ButtonRight.Content = dataContext.GlobalText.Cancel;
                break;
        }
    }

    private void ButtonCenterPressed(object? sender, RoutedEventArgs e)
    {
        switch (_buttons)
        {
            case MbButtons.Ok:
                Close(MbResult.Ok);
                break;
            case MbButtons.YesNoCancel:
                Close(MbResult.No);
                break;
            default:
                throw new Exception("Unknown result");
        }
    }

    private void ButtonLeftPressed(object? sender, RoutedEventArgs e)
    {
        switch (_buttons)
        {
            case MbButtons.OkCancel:
                Close(MbResult.Ok);
                break;
            case MbButtons.YesNo:
            case MbButtons.YesNoCancel:
                Close(MbResult.Yes);
                break;
            default:
                throw new Exception("Unknown result");
        }
    }

    private void ButtonRightPressed(object? sender, RoutedEventArgs e)
    {
        switch (_buttons)
        {
            case MbButtons.OkCancel:
                Close(MbResult.Cancel);
                break;
            case MbButtons.YesNo:
                Close(MbResult.No);
                break;
            case MbButtons.YesNoCancel:
                Close(MbResult.Cancel);
                break;
            default:
                throw new Exception("Unknown result");
        }
    }
}

public enum MbButtons
{
    Ok,
    OkCancel,
    YesNo,
    YesNoCancel
}

public enum MbResult
{
    Ok,
    Cancel,
    Yes,
    No
}