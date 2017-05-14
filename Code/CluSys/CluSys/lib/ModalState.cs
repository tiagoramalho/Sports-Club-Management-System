using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace CluSys.lib
{
    class ModalState
    {
        // Basic
        public int? Weight { get; set; }

        public int? Height { get; set; }
        public DateTime? Date { get; set; } = DateTime.Now;
        public string Story { get; set; }

        // Body chart
        public BodyChartView ActiveView
        {
            get { return Views[ActiveViewIdx]; }
            set { ActiveViewIdx = Views.IndexOf(value); }
        }
        public int ActiveViewIdx { get; set; } = 0;
        public readonly ObservableCollection<BodyChartView> Views = BodyChartViews.GetViews();
        public readonly ObservableCollection<BodyChartMark> Marks = new ObservableCollection<BodyChartMark>();
        public ObservableCollection<Annotation> Annotations { get; set; } = lib.Annotations.GetAnnotations();

        // Problems
        public ObservableCollection<string> Problems { get; set; } = new ObservableCollection<string>();
    }

    public sealed class ProblemToIndexConverter : IMultiValueConverter
    {
        object IMultiValueConverter.Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var text = values[0] as string;
            var se = ClusysUtils.FindByType<ScrollViewer>((Visual) values[1]).DataContext as ModalState;

            return se?.Problems.IndexOf(text).ToString() ?? "";
        }

        object[] IMultiValueConverter.ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
