using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace CluSys.lib
{
    public static class ClusysUtils
    {
        public const double DrawRadius = 2;

        public static SqlConnection GetConnection()
        {
            var conn = new SqlConnection("data source= RJ-JESUS\\SQLEXPRESS2014;integrated security=true;initial catalog=CluSys");

            conn.Open();
            return conn;
        }

        public static T FindByType<T>(Visual v)
        {
            while (v != null)
            {
                for(var i = 0; i < VisualTreeHelper.GetChildrenCount(v); i++)
                {
                    var c = VisualTreeHelper.GetChild(v, i);

                    if (c is T)
                        return (T)Convert.ChangeType(c, typeof(T));
                }

                v = VisualTreeHelper.GetParent(v) as Visual;
            }

            return default(T);
        }

        public static void DrawPoint(Point point, Canvas canvas, SolidColorBrush brush, MouseButtonEventHandler handler = null)
        {
            var mark = new Ellipse {Fill = brush};

            mark.SetValue(Canvas.LeftProperty, point.X);
            mark.SetValue(Canvas.TopProperty, point.Y);

            mark.StrokeThickness = 2;
            mark.Stroke = Brushes.Black;
            mark.Width = mark.Height = 2 * DrawRadius;

            if(handler != null)
                mark.MouseLeftButtonUp += handler;

            canvas.Children.Add(mark);
        }
    }

    public sealed class StringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var format = parameter as string;

            if (value == null || format == null)
                return value;
            else if (value is double && double.IsNaN((double)value))
                return "n/a";

            return string.Format(format, value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) { throw new NotSupportedException(); }
    }

    public sealed class InvertBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return !(bool?) value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return !(bool?) value;
        }
    }

    public sealed class GreaterThanZeroValudationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            double v;

            if(value == null || value.ToString() == "")
                return ValidationResult.ValidResult;

            try
            {
                v = double.Parse(value.ToString());
            }
            catch (Exception)
            {
                return new ValidationResult(false, "Imposível interpretar.");
            }

            return v <= 0 ? new ValidationResult(false, "Valor deve positivo.") : ValidationResult.ValidResult;
        }
    }
}
