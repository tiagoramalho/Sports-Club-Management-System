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
using System.Windows.Threading;

namespace CluSys
{
    public partial class MainWindow : Window
    {

       

        private SqlConnection _cn;
        private readonly ObservableCollection<Athlete> _openAthletes;

        public MainWindow()
        {
           
            OpenConnection();
            _openAthletes = new ObservableCollection<Athlete>();

            InitializeComponent();

            Problemas.ItemsSource = new List<String> { "Problema 1", "Problema 2", "Problema 3" };
            ModalityList.ItemsSource = Modalities.LoadSQL(_cn);
            OpenAthletesList.ItemsSource = _openAthletes;  // empty on start
            AthletesWithOpenEvaluations.ItemsSource = Athletes.OpenEvaluations(_cn);
        }
        private void Image_MouseDown(object sender, MouseButtonEventArgs e)
        {
            button.IsPopupOpen = true;
            Console.WriteLine(e.GetPosition(imagem).X + " " + e.GetPosition(imagem).Y);
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
            var conn = new SqlConnection("data source= DESKTOP-E9II57Q;integrated security=true;initial catalog=CluSys");
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
            Athlete athlete = null;
            var item = ItemsControl.ContainerFromElement(sender as ListBox, (DependencyObject) e.OriginalSource) as ListBoxItem;

            if (item == null)
                return;

            athlete = new AthleteWithBody(_cn, item.Content as Athlete);
            AthleteContent.DataContext = athlete;

            if (!_openAthletes.Contains(athlete))
                _openAthletes.Insert(0, athlete);

            // Get this athlete's evaluations
            EvaluationsList.ItemsSource = athlete.Evaluations(_cn);

            HomeContent.Visibility = Visibility.Hidden;
            AthleteContent.Visibility = Visibility.Visible;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //expander.IsExpanded = true;
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

        private void EvaluationsViewer_Scroll(object sender, MouseButtonEventArgs e)
        {
            //EvalViewer.ScrollToHorizontalOffset(EvalViewer.HorizontalOffset + 10);
            var itemsCount = EvaluationsList.Items.Count;
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

    public sealed class ModalityDotGetAthletesConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var methodName = parameter as string;

            if (value == null || methodName == null)
                return value;

            var methodInfo = value.GetType().GetMethod(methodName, new Type[] { typeof(SqlConnection) });

            if (methodInfo == null)
                return value;
            return methodInfo.Invoke(value, new object[1] { MainWindow.GetConnection() } );
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) { throw new NotSupportedException(); }
    }
}
