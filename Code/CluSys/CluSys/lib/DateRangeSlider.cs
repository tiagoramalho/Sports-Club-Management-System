using System;
using System.Globalization;
using System.Windows.Controls;
using System.Windows.Data;
using MahApps.Metro.Controls;

namespace CluSys.lib
{
    public class DateRangeSlider : RangeSlider
    {
        public DateTime MinimumDate { get; set; } = new DateTime(1980, 9, 1);
        public DateTime MaximumDate { get; set; } = new DateTime(2020, 9, 1);
        public DateTime LowerDate { get; set; } = new DateTime(1990, 1, 1);
        public DateTime UpperDate { get; set; } = new DateTime(2050, 1, 1);

        public RangeValidationRule MinDateValidator { get; }
        public RangeValidationRule MaxDateValidator { get; }

        public DateRangeConverter DateRangeToStringConverter { get; set; }

        public double LowerDateAsDouble
        {
            get => DateRangeConverter.ProportionalDate2Double(LowerDate, MinimumDate, MaximumDate, Minimum, Maximum);
            set => LowerDate = DateRangeConverter.ProportionalDouble2Date(value, Minimum, Maximum, MinimumDate, MaximumDate);
        }

        public double UpperDateAsDouble
        {
            get => DateRangeConverter.ProportionalDate2Double(UpperDate, MinimumDate, MaximumDate, Minimum, Maximum);
            set => UpperDate = DateRangeConverter.ProportionalDouble2Date(value, Minimum, Maximum, MinimumDate, MaximumDate);
        }

        public DateRangeSlider()
        {
            DateRangeToStringConverter = new DateRangeConverter(this);
            MinDateValidator = new RangeValidationRule(this, checkMax:true);
            MaxDateValidator = new RangeValidationRule(this, checkMin:true);
        }
    }

    public sealed class DateRangeConverter : IValueConverter
    {
        private readonly DateRangeSlider _drs;

        public DateRangeConverter(DateRangeSlider drs) => _drs = drs;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) => value == null ? null : ProportionalDouble2Date((double)value, _drs.Minimum, _drs.Maximum, _drs.MinimumDate, _drs.MaximumDate).ToString("D", new CultureInfo("pt-PT"));

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotSupportedException();

        public static double ProportionalDate2Double(DateTime d, DateTime minDate, DateTime maxDate, double minDouble, double maxDouble)
        {
            var minTicks = minDate.Ticks;
            var maxTicks = maxDate.Ticks;
            var ticks = d.Ticks;

            var res = minDouble + (double)(ticks - minTicks) / (maxTicks - minTicks) * (maxDouble - minDouble);

            return res < minDouble ? minDouble : (res > maxDouble ? maxDouble : res);
        }

        public static DateTime ProportionalDouble2Date(double d, double minDouble, double maxDouble, DateTime minDate, DateTime maxDate)
        {
            var minTicks = minDate.Ticks;
            var maxTicks = maxDate.Ticks;
            var ticks = (long) (minTicks + (d - minDouble) / (maxDouble - minDouble) * (maxTicks - minTicks));

            var res = new DateTime(ticks);

            return res < minDate ? minDate : (res > maxDate ? maxDate : res);
        }
    }

    public sealed class RangeValidationRule : ValidationRule
    {
        private readonly bool _checkMin;
        private readonly bool _checkMax;
        private readonly DateRangeSlider _dr;

        public RangeValidationRule(DateRangeSlider dr, bool checkMin=false, bool checkMax=false)
        {
            _dr = dr;
            _checkMin = checkMin;
            _checkMax = checkMax;
        }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            DateTime time;

            if (!DateTime.TryParse((value ?? "").ToString(), CultureInfo.CurrentCulture, DateTimeStyles.AssumeLocal | DateTimeStyles.AllowWhiteSpaces, out time))
                return new ValidationResult(false, "Formato inválido");

            if(_checkMin && time.Date < _dr.MinimumDate)
                return new ValidationResult(false, "Intervalo inválido");
            else if(_checkMax && time.Date > _dr.MaximumDate)
                return new ValidationResult(false, "Intervalo inválido");
            return ValidationResult.ValidResult;
        }
    }
}