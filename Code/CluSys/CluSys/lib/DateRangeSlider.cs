using System;
using System.Globalization;
using System.Windows.Data;
using MahApps.Metro.Controls;

namespace CluSys.lib
{
    public class DateRangeSlider : RangeSlider
    {
        public DateTime MinimumDate { get; set; } = new DateTime(1980, 9, 1);
        public DateTime MaximumDate { get; set; } = new DateTime(3000, 9, 1);
        public DateTime LowerDate = new DateTime(1990, 1, 1);
        public DateTime UpperDate = new DateTime(2050, 1, 1);

        public double LowerDateAsDouble
        {
            get => ProportionalDate2Double(LowerDate, MinimumDate, MaximumDate, Minimum, Maximum);
            set => LowerDate = ProportionalDouble2Date(value, Minimum, Maximum, MinimumDate, MaximumDate);
        }

        public double UpperDateAsDouble
        {
            get => ProportionalDate2Double(UpperDate, MinimumDate, MaximumDate, Minimum, Maximum);
            set => UpperDate = ProportionalDouble2Date(value, Minimum, Maximum, MinimumDate, MaximumDate);
        }

        public TooltipConverter TooltipToStringConverter { get; set; }

        public DateRangeSlider() => TooltipToStringConverter = new TooltipConverter(this);

        private static double ProportionalDate2Double(DateTime d, DateTime minDate, DateTime maxDate, double minDouble, double maxDouble)
        {
            var minTicks = minDate.Ticks;
            var maxTicks = maxDate.Ticks;
            var ticks = d.Ticks;

            var res = minDouble + (double)(ticks - minTicks) / (maxTicks - minTicks) * (maxDouble - minDouble);

            return res < minDouble ? minDouble : (res > maxDouble ? maxDouble : res);
        }

        private static DateTime ProportionalDouble2Date(double d, double minDouble, double maxDouble, DateTime minDate, DateTime maxDate)
        {
            var minTicks = minDate.Ticks;
            var maxTicks = maxDate.Ticks;
            var ticks = (long) (minTicks + (d - minDouble) / (maxDouble - minDouble) * (maxTicks - minTicks));

            var res = new DateTime(ticks);

            return res < minDate ? minDate : (res > maxDate ? maxDate : res);
        }

        public sealed class TooltipConverter : IValueConverter
        {
            private readonly DateRangeSlider _drs;

            public TooltipConverter(DateRangeSlider drs) => _drs = drs;

            public object Convert(object value, Type targetType, object parameter, CultureInfo culture) => value == null ? null : ProportionalDouble2Date((double)value, _drs.Minimum, _drs.Maximum, _drs.MinimumDate, _drs.MaximumDate).ToString("D", new CultureInfo("pt-PT"));

            public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotSupportedException();
        }
    }
}