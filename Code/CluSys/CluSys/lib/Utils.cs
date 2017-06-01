using System;
using System.Data.SqlClient;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace CluSys.lib
{
    internal static class ClusysUtils
    {
        //public static SqlConnection GetConnection() { return new SqlConnection("data source= RJ-JESUS\\SQLEXPRESS2014;integrated security=true;initial catalog=CluSys"); }
        public static SqlConnection GetConnection() { return new SqlConnection(@"Data Source=193.136.175.33\SQLEXPRESS2012,8293;Initial Catalog=p1g2;User ID=p1g2;Password=sqluaricardotiago;"); }

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
    }

    public sealed class StringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var format = parameter as string;

            if (value == null || format == null)
                return "n/a";

            return string.Format(format, value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) { throw new NotSupportedException(); }
    }

    public sealed class FirstUpperStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var str = (string) new StringConverter().Convert(value, targetType, parameter, culture);
            if(!string.IsNullOrEmpty(str))
                str = char.ToUpper(str[0]) + (str.Length > 1 ? str.Substring(1) : "");

            return str;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) { throw new NotSupportedException(); }
    }

    public sealed class ExpectedRecoveryPickerEnabledConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values == null || values.Length != 2 || values[0] == DependencyProperty.UnsetValue || values[1] == DependencyProperty.UnsetValue)
                return false;

            return (bool) values[0] && !(bool) values[1];
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) { throw new NotImplementedException(); }
    }

    public sealed class NewSessionVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is MedicalEvaluation))
                return Visibility.Hidden;

            return ((MedicalEvaluation) value).ClosingDate == null ? Visibility.Visible : Visibility.Hidden;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public sealed class DoubleToStringConverter : IValueConverter
    {
        private bool _comma;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return _comma ? value?.ToString() + ',' : value?.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return null;

            double? res;
            var doubleText = value.ToString().Replace(".", ",");

            try
            {
                res = double.Parse(doubleText);
                _comma = (doubleText.Length > 0 ? doubleText[doubleText.Length - 1] : '\0') == ',';
            }
            catch (Exception)
            {
                _comma = false;
                return null;
            }

            return res;
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
                v = double.Parse(value.ToString().Replace(".", ","));
            }
            catch (Exception)
            {
                return new ValidationResult(false, "Imposível interpretar.");
            }

            return v <= 0 ? new ValidationResult(false, "Valor deve ser positivo.") : ValidationResult.ValidResult;
        }
    }
}
