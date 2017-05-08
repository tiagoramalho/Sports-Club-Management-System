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

            var modalities = Modalities.LoadSQL(cn);

            InitializeComponent();

            ModalityList.ItemsSource = modalities;
            OpenAthletesList.ItemsSource = openAthletes;
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
            view.Filter = (a) => { return (a as Athlete).Name.Contains(filterText); };
        }

        private void OpenAthlete(object sender, MouseButtonEventArgs e)
        {
            Athlete athlete;
            var item = ItemsControl.ContainerFromElement(sender as ListBox, e.OriginalSource as DependencyObject) as ListBoxItem;

            if (item == null)
                return;

            athlete = item.Content as Athlete;

            // Do more things

            if(!openAthletes.Contains(athlete))
                openAthletes.Add(athlete);
        }

        private void GoHome(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("Opening the 'Home' view...");
            this.Show();
        }
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
