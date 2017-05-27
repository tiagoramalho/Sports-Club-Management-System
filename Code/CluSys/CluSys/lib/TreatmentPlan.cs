using System;
using System.Globalization;
using System.Windows.Data;

namespace CluSys.lib
{
    internal class TreatmentPlan
    {
        public int Id { get; set; } = -1;
        public string Description { get; set; }
        public string Objective { get; set; }
        public int EvalId { get; set; }
        public int SessionId { get; set; }
        public int? ProbId { get; set; }

        public MajorProblem RefProblem { get; set; }
    }

    public sealed class RefProblemConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return "Prob.: n/a";

            var id = ((MajorProblem) value).CountId;

            return id > 0 ? $"Prob.: {id}" : "Prob.: n/a";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) { throw new NotImplementedException(); }
    }
}
