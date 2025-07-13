using Avalonia.Media;

namespace PmlUi.Models;

public struct Palette
{
    public SolidColorBrush Background { get; }
    public SolidColorBrush Foreground { get; }
    
    public SolidColorBrush ButtonEnabled  { get; }
    public SolidColorBrush ButtonDisabled { get; }
    public SolidColorBrush ButtonPressed { get; }
    public SolidColorBrush ButtonHovered { get; }

    public SolidColorBrush Info { get; }
    public SolidColorBrush Error { get; }
    public SolidColorBrush Success { get; }
    public SolidColorBrush Warning { get; }
    
    public SolidColorBrush PrimaryTextColor { get; }
    public SolidColorBrush SecondaryTextColor { get; }
}