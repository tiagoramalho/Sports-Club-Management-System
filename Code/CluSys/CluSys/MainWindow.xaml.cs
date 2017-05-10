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

namespace CluSys
{
    public partial class MainWindow : Window
    {
        private SqlConnection cn;
        private ObservableCollection<Athlete> openAthletes;

        public MainWindow()
        {
            // Init
            OpenSGBDConnection();
            openAthletes = new ObservableCollection<Athlete>();

            InitializeComponent();

            ModalityList.ItemsSource = Modalities.LoadSQL(cn);
            OpenAthletesList.ItemsSource = openAthletes;  // empty on start
            AthletesWithOpenEvaluations.ItemsSource = Athletes.OpenEvaluations(cn);
        }

        private bool OpenSGBDConnection()
        {
            if (cn == null)
                cn = GetSGBDConnection();

            if (cn.State != ConnectionState.Open)
                cn.Open();

            return cn.State == ConnectionState.Open;
        }

        public static SqlConnection GetSGBDConnection()
        {
            SqlConnection conn = new SqlConnection("data source= RJ-JESUS\\SQLEXPRESS2014;integrated security=true;initial catalog=CluSys");
            conn.Open();
            return conn;
        }

        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ListBox lb = null;
            Visual v = (Visual)e.Source;
            var filterText = ((TextBox)sender).Text;

            while (v != null)
            {
                for(int i = 0; i < VisualTreeHelper.GetChildrenCount(v); i++)
                {
                    var c = VisualTreeHelper.GetChild(v, i);
                    if (c is ListBox)
                    {
                        lb = (ListBox)c;
                        goto done;
                    }
                }
                v = VisualTreeHelper.GetParent(v) as Visual;
            }

            done: if (lb == null)
                return;

            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(lb.ItemsSource);
            view.Filter = (a) => {
                var athlete = a as Athlete;

                return (athlete.FirstName + " " + athlete.LastName).Contains(filterText);
            };
        }

        private void OpenAthlete(object sender, MouseButtonEventArgs e)
        {
            Athlete athlete;
            var item = ItemsControl.ContainerFromElement(sender as ListBox, e.OriginalSource as DependencyObject) as ListBoxItem;

            if (item == null)
                return;

            athlete = new AthleteWithBody(cn, item.Content as Athlete);
            AthleteContent.DataContext = athlete;

            if (!openAthletes.Contains(athlete))
                openAthletes.Insert(0, athlete);

            EvaluationsList.ItemsSource = athlete.Evaluations(cn);

            HomeContent.Visibility = Visibility.Hidden;
            AthleteContent.Visibility = Visibility.Visible;
        }

        private void GoHome(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("Opening the 'Home' view...");

            AthleteContent.Visibility = Visibility.Hidden;
            HomeContent.Visibility = Visibility.Visible;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Expander expander = findByType<Expander>((Visual)e.Source);

            if (expander == null)
                return;

            expander.IsExpanded = true;
        }

        private T findByType<T>(Visual v)
        {
            while (v != null)
            {
                for(int i = 0; i < VisualTreeHelper.GetChildrenCount(v); i++)
                {
                    var c = VisualTreeHelper.GetChild(v, i);
                    if (c is T)
                    {
                        return (T)Convert.ChangeType(c, typeof(T));
                    }
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
            else if (value is Double && Double.IsNaN((Double)value))
                return "n/a";

            return String.Format(format, value);
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

            var methodInfo = value.GetType().GetMethod(methodName, new Type[1] { typeof(SqlConnection) });

            if (methodInfo == null)
                return value;

            return methodInfo.Invoke(value, new object[1] { MainWindow.GetSGBDConnection() } );
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) { throw new NotSupportedException(); }
    }
}
