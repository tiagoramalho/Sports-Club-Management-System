using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace CluSys.lib
{
    [Serializable]
    internal class BodyChartMark
    {
        public const double DrawRadius = 2;

        public int Id { get; set; } = -1;
        public int EvalId { get; set; }
        public int SessionId { get; set; }
        public int ViewId { get; set; }
        public int PainLevel { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
        public string Description { get; set; }

        public ObservableCollection<Annotation> Annotations { get; } = new ObservableCollection<Annotation>();

        public static Color ColorForIndex(int i)
        {
            Color[] colors =
            {
                Color.FromArgb(255, 255, 235, 59),
                Color.FromArgb(255, 255, 222, 51),
                Color.FromArgb(255, 255, 206, 44),
                Color.FromArgb(255, 255, 192, 38),
                Color.FromArgb(255, 255, 178, 32),
                Color.FromArgb(255, 255, 162, 29),
                Color.FromArgb(255, 255, 145, 27),
                Color.FromArgb(255, 255, 128, 28),
                Color.FromArgb(255, 255, 109, 30),
                Color.FromArgb(255, 255,  87, 34)
            };

            return i >= 0 && i < colors.Length ? colors[i] : Color.FromArgb(255, 0, 150, 136);
        }

        private bool Equals(BodyChartMark other)
        {
            if(Id != -1 && other.Id != -1)
                return Id == other.Id;
            return Math.Abs(X - other.X) < double.Epsilon && Math.Abs(Y - other.Y) < double.Epsilon;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((BodyChartMark) obj);
        }

        public override int GetHashCode()
        {
            if(Id != -1)
                return Id;
            unchecked
            {
                var hash = 17;
                hash = hash * 23 + X.GetHashCode();
                hash = hash * 23 + Y.GetHashCode();
                return hash;
            }
        }

        public static void DrawMark(Canvas canvas, BodyChartMark mark, MouseButtonEventHandler handler = null)
        {
            var ellipse = new Ellipse();

            ellipse.SetValue(Canvas.LeftProperty, mark.X);
            ellipse.SetValue(Canvas.TopProperty, mark.Y);

            ellipse.StrokeThickness = 2;
            ellipse.Width = ellipse.Height = 2 * DrawRadius;
            ellipse.SetBinding(Shape.StrokeProperty, new Binding("PainLevel") { Source = mark, Converter = new PainToColorConverter() });

            if (handler != null)
                ellipse.MouseLeftButtonUp += handler;

            canvas.Children.Add(ellipse);
        }
    }

    public sealed class PainToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return new SolidColorBrush(value == null ? BodyChartMark.ColorForIndex(-1) : BodyChartMark.ColorForIndex((int) value - 1));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) { throw new NotImplementedException(); }
    }
}
