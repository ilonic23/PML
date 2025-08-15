using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Animation;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Input;
using Avalonia.Media;
using Avalonia.Styling;

namespace PmlUi.Views;

public partial class RippleButton : UserControl
{
    public RippleButton()
    {
        InitializeComponent();
        MainButton.AddHandler(PointerPressedEvent, MainButton_OnPointerPressed, handledEventsToo: true);
        _ = MainButton.Classes.Concat(Classes);
        _ = MainButton.Styles.Concat(Styles);
    }

    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        base.OnPropertyChanged(change);
        if (change.Property == WidthProperty)
            MainButton.Width = Width;
        if (change.Property == HeightProperty)
            MainButton.Height = Height;
    }

    private async Task MainButton_OnPointerPressed(object? sender, PointerPressedEventArgs e)
    {
         var ellipse = new Ellipse
         {
             Width = 0,
             Height = 0,
             Fill = new SolidColorBrush(new Color(10, 255, 255, 255))
         };
         var position = e.GetPosition(Panel);

         var corners = new (double X, double Y)[]
         {
             (0, 0),
             (MainButton.Width, 0),
             (0, MainButton.Height),
             (MainButton.Width, MainButton.Height)
         };
         
         double targetSize = corners
             .Select(c => Math.Sqrt(Math.Pow(c.X - position.X, 2) + Math.Pow(c.Y - position.Y, 2)))
             .Max() * 2;

         Panel.Children.Add(ellipse); // This is a Canvas too!!!
         
         var animation = new Animation()
         {
             Duration = TimeSpan.FromMilliseconds(500),
             Children =
             {
                 new KeyFrame
                 {
                     Cue = new Cue(0d),
                     Setters =
                     {
                         new Setter(WidthProperty, 0d), new Setter(HeightProperty, 0d)
                     }
                 },
                 new KeyFrame
                 {
                     Cue = new Cue(1.0),
                     Setters =
                     {
                         new Setter(WidthProperty, targetSize), new Setter(HeightProperty, targetSize)
                     }
                 }
             }
         };
         ellipse.PropertyChanged += (_, args) =>
         {
             if (args.Property == WidthProperty)
                 Canvas.SetLeft(ellipse, position.X - ellipse.Width / 2);
             if (args.Property == HeightProperty)
                 Canvas.SetTop(ellipse, position.Y - ellipse.Height / 2);
         };

         CancellationTokenSource tempSource = new();

         await animation.RunAsync(ellipse, tempSource.Token).WaitAsync(tempSource.Token);
         Panel.Children.Remove(ellipse);
    }
}