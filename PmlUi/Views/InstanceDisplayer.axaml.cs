using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using CommunityToolkit.Mvvm.Input;
using PmlUi.Models;

namespace PmlUi.Views;

public class InstanceDisplayer : TemplatedControl
{
    public static readonly StyledProperty<string> DisplayTextProperty =
        AvaloniaProperty.Register<InstanceDisplayer, string>(nameof(DisplayText));
    
    public static readonly StyledProperty<IRelayCommand> OnPressCommandProperty =
        AvaloniaProperty.Register<InstanceDisplayer, IRelayCommand>(nameof(OnPressCommand));
    
    public static readonly StyledProperty<PhantomInstance> InstanceProperty =
        AvaloniaProperty.Register<InstanceDisplayer, PhantomInstance>(nameof(Instance));

    public string DisplayText
    {
        get => GetValue(DisplayTextProperty);
        set => SetValue(DisplayTextProperty, value);
    }

    public IRelayCommand OnPressCommand
    {
        get => GetValue(OnPressCommandProperty);
        set => SetValue(OnPressCommandProperty, value);
    }

    public PhantomInstance Instance
    {
        get => GetValue(InstanceProperty);
        set => SetValue(InstanceProperty, value);
    }
}