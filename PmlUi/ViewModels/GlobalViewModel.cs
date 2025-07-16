using PmlUi.Models;

namespace PmlUi.ViewModels;

public class GlobalViewModel : ViewModelBase
{
    public Palette Palette { get; } = new();
}