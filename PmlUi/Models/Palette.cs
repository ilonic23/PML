using Avalonia.Media;

namespace PmlUi.Models;

public class Palette
{
    public SolidColorBrush Background { get; set; }
    public SolidColorBrush Foreground { get; set; }
    public SolidColorBrush SecondaryForeground { get; set; }
    
    public SolidColorBrush ButtonEnabled  { get; set; }
    public SolidColorBrush ButtonDisabled { get; set; }
    public SolidColorBrush ButtonPressed { get; set; }
    public SolidColorBrush ButtonHovered { get; set; }
    
    public SolidColorBrush PrimaryTextColor { get; set; }
    public SolidColorBrush SecondaryTextColor { get; set; }

    
    // Generates a palette with standard colors used in Phantom Launhcer
    public Palette()
    {
        Background = new SolidColorBrush(Color.FromRgb(46, 40, 45));
        Foreground = new SolidColorBrush(Color.FromRgb(70, 60, 63));
        SecondaryForeground = new SolidColorBrush(Color.FromRgb(55, 50, 53));
        ButtonEnabled = new SolidColorBrush(Color.FromArgb(255,79, 97, 165));
        ButtonDisabled = new SolidColorBrush(Color.FromRgb(46, 40, 45));
        ButtonPressed = new SolidColorBrush(Color.FromRgb(40, 78, 131));
        ButtonHovered = new SolidColorBrush(Color.FromRgb(75, 92, 155));
        PrimaryTextColor = new SolidColorBrush(Color.FromRgb(255,255,255));
        SecondaryTextColor = new SolidColorBrush(Color.FromRgb(0,0,0));
    }
}
