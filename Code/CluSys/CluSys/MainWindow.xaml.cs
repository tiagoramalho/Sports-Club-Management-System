using CluSys.lib;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Globalization;
using System.ComponentModel;
using System.Diagnostics;
using System.Collections.Specialized;
using System.Reflection;
using System.Windows.Markup;
using MahApps.Metro.Controls;

namespace CluSys
{
    public partial class MainWindow
    {
        private SqlConnection _cn;
        private readonly ObservableCollection<Athlete> _openAthletes;

        public MainWindow()
        {
            // Init
            OpenConnection();
            _openAthletes = new ObservableCollection<Athlete>();

            InitializeComponent();

            // Set culture
            CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("pt-PT");
            CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo("pt-PT");

            BindingOperations.GetBinding(MinDateRange, DatePicker.SelectedDateProperty)?.ValidationRules.Add(SliderRange.MinDateValidator);
            BindingOperations.GetBinding(MaxDateRange, DatePicker.SelectedDateProperty)?.ValidationRules.Add(SliderRange.MaxDateValidator);

            OpenAthletesList.ItemsSource = _openAthletes;  // empty on start
            ModalityList.ItemsSource = Modalities.LoadSQL(_cn);
            AthletesWithOpenEvaluations.ItemsSource = Athletes.AthletesWithOpenEvaluations(_cn);
        }

        private bool OpenConnection()
        {
            if (_cn == null)
                _cn = GetConnection();

            if (_cn.State != ConnectionState.Open)
                _cn.Open();

            return _cn.State == ConnectionState.Open;
        }

        public static SqlConnection GetConnection()
        {
            var conn = new SqlConnection("data source= RJ-JESUS\\SQLEXPRESS2014;integrated security=true;initial catalog=CluSys");
            conn.Open();
            return conn;
        }

        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var filterText = ((TextBox)sender).Text;
            ListBox lb = FindByType<ListBox>((Visual)e.Source);

            if (lb == null)
                return;

            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(lb.ItemsSource);
            view.Filter = (a) => {
                var athlete = a as Athlete;

                return (athlete?.FirstName + " " + athlete?.LastName).Contains(filterText);
            };
        }

        private void GoHome(object sender, RoutedEventArgs e)
        {
            AthleteContent.Visibility = Visibility.Hidden;
            HomeContent.Visibility = Visibility.Visible;
        }

        private void OpenAthlete(object sender, MouseButtonEventArgs e)
        {
            var item = ItemsControl.ContainerFromElement(sender as ListBox, (DependencyObject) e.OriginalSource) as ListBoxItem;

            if (item == null)
                return;

            var athlete = new AthleteWithBody(_cn, item.Content as Athlete);
            AthleteContent.DataContext = athlete;

            if (!_openAthletes.Contains(athlete))
                _openAthletes.Insert(0, athlete);

            // Get this athlete's evaluations
            EvaluationsList.ItemsSource = athlete.Evaluations(_cn);
            // Filter them based on date
            FilterEvaluations(SliderRange, null);

            HomeContent.Visibility = Visibility.Hidden;
            AthleteContent.Visibility = Visibility.Visible;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var me = (sender as Control)?.DataContext as MedicalEvaluation;
            SessionsList.ItemsSource = me?.Sessions(_cn);
        }

        private void FilterEvaluations(object sender, RangeParameterChangedEventArgs rangeParameterChangedEventArgs)
        {
            var dr = sender as DateRangeSlider;

            if (dr == null || EvaluationsList.ItemsSource == null)
                return;

            var view = (CollectionView)CollectionViewSource.GetDefaultView(EvaluationsList.ItemsSource);
            view.Filter = (medicalEvaluation) =>
            {
                var me = medicalEvaluation as MedicalEvaluation;

                return me != null &&  (dr.LowerDate <= me.OpeningDate && me.OpeningDate <= dr.UpperDate
                                       || dr.LowerDate <= me.ClosingDate && me.ClosingDate <= dr.UpperDate);
            };
        }

        private static T FindByType<T>(Visual v)
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
                return value;
            else if (value is double && double.IsNaN((double)value))
                return "n/a";

            return string.Format(format, value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) { throw new NotSupportedException(); }
    }
}
