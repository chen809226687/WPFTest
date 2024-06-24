using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using System.Reactive.Linq;

namespace SUKIUITest
{
    public class CustomControl : ComboBox
    {
        public static readonly StyledProperty<bool> IsEnabledProperty =
            AvaloniaProperty.Register<CustomControl, bool>(nameof(IsEnabled), defaultValue: true);

        public bool IsEnabled
        {
            get => GetValue(IsEnabledProperty);
            set => SetValue(IsEnabledProperty, value);
        }

        public CustomControl()
        {
            Bind(BackgroundProperty, this.GetObservable(IsEnabledProperty)
                .Select(isEnabled => isEnabled ? Brushes.LightGreen : Brushes.Gray));

            Width = 25;
            Height = 25;
            CornerRadius = new CornerRadius(50);
            HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center;
            VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center;
        }
    }
}