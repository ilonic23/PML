using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Animation;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.Styling;
using Avalonia.Threading;

namespace PmlUi.Views;

public partial class RippleButton : UserControl
{
    public RippleButton()
    {
        InitializeComponent();
        MainButton.AddHandler(Button.PointerPressedEvent, MainButton_OnPointerPressed, handledEventsToo: true);
        _ = MainButton.Classes.Concat(Classes);
        MainButton.Styles.Concat(Styles);
    }

    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        base.OnPropertyChanged(change);
        if (change.Property == WidthProperty)
            MainButton.Width = Width;
        if (change.Property == HeightProperty)
            MainButton.Height = Height;
    }

    private async void MainButton_OnPointerPressed(object? sender, PointerPressedEventArgs e)
    {
        // var ellipse = new Ellipse()
        // {
        //     Fill = new SolidColorBrush(new Color(10, 255, 255, 255))
        // };
        // ellipse.Classes.Add("animated");
        // Panel.Children.Add(ellipse);
        //
        // var position = e.GetCurrentPoint(Panel).Position;
        // ellipse.PropertyChanged += (s, e) =>
        // {
        //     Canvas.SetLeft(ellipse, position.X - ellipse.Width / 2);
        //     Canvas.SetTop(ellipse, position.Y - ellipse.Height / 2);
        // };
//         var ellipse = new Ellipse();
//         ellipse.Width = 0;
//         ellipse.Height = 0;
//         ellipse.Fill = new SolidColorBrush(new Color(10, 255, 255, 255));
//
//         Panel.Children.Add(ellipse);
//
// // Get initial mouse position
//         var position = e.GetPosition(Panel);
//
//         var animation = new Animation()
//         {
//             Duration = TimeSpan.FromMilliseconds(500),
//             Children =
//             {
//                 new KeyFrame()
//                 {
//                     Cue = new (0d),
//                     Setters =
//                     {
//                         new Setter(WidthProperty, 0), new Setter(HeightProperty, 0),
//                     }
//                 },
//                 new KeyFrame()
//                 {
//                     Cue = new(1.0),
//                     Setters =
//                     {
//                         new Setter(WidthProperty, 1000), new Setter(HeightProperty, 1000),
//                     }
//                 }
//             }
//         };
//         animation.PropertyChanged += (s, e) =>
//         {
//             Canvas.SetLeft(ellipse, position.X - ellipse.Width / 2);
//             Canvas.SetTop(ellipse, position.Y - ellipse.Height / 2);
//         };
//         
//         await animation.RunAsync(ellipse);
//
         // await Task.Run(async () =>
         // {
         //     await Dispatcher.UIThread.InvokeAsync(async () =>
         //     {
         //         // while (ellipse.Width < this.Width + this.Width/2)
         //         // {
         //         //     ellipse.Width++;
         //         //     ellipse.Height++;
         //         //
         //         //     // Update position so the ellipse stays centered on the click
         //         //     Canvas.SetLeft(ellipse, position.X - ellipse.Width / 2);
         //         //     Canvas.SetTop(ellipse, position.Y - ellipse.Height / 2);
         //         //
         //         //     await Task.Delay(1);
         //         // }
         //         // Panel.Children.Remove(ellipse);
         //         
         //     });
         // });
         
         var ellipse = new Ellipse();
         ellipse.Width = 0;
         ellipse.Height = 0;
         ellipse.Fill = new SolidColorBrush(new Color(10, 255, 255, 255));
         var position = e.GetPosition(Panel);

         double targetSize;
         
         var corners = new (double X, double Y)[]
         {
             (0, 0),
             (MainButton.Width, 0),
             (0, MainButton.Height),
             (MainButton.Width, MainButton.Height)
         };
         
         targetSize = corners
             .Select(c => Math.Sqrt(Math.Pow(c.X - position.X, 2) + Math.Pow(c.Y - position.Y, 2)))
             .Max() * 2;

         Panel.Children.Add(ellipse); // This is a Canvas too!!!
         
         var animation = new Animation()
         {
             Duration = TimeSpan.FromMilliseconds(500),
             Children =
             {
                 new KeyFrame()
                 {
                     Cue = new (0d),
                     Setters =
                     {
                         new Setter(Ellipse.WidthProperty, 0d), new Setter(Ellipse.HeightProperty, 0d),
                     }
                 },
                 new KeyFrame()
                 {
                     Cue = new(1.0),
                     Setters =
                     {
                         new Setter(Ellipse.WidthProperty, targetSize), new Setter(Ellipse.HeightProperty, targetSize),
                     }
                 }
             }
         };
         ellipse.PropertyChanged += (s, e) =>
         {
             if (e.Property == Ellipse.WidthProperty)
                 Canvas.SetLeft(ellipse, position.X - ellipse.Width / 2);
             if (e.Property == Ellipse.HeightProperty)
                 Canvas.SetTop(ellipse, position.Y - ellipse.Height / 2);
         };

         CancellationTokenSource tempSource = new();

         await animation.RunAsync(ellipse).WaitAsync(tempSource.Token);
         Panel.Children.Remove(ellipse);
    }
}