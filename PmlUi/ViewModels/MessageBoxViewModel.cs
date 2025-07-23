using PmlUi.Models;

namespace PmlUi.ViewModels;

public class MessageBoxViewModel : ViewModelBase
{
    public GlobalText GlobalText { get; } = new();
    public Palette Palette { get; } = new();
}