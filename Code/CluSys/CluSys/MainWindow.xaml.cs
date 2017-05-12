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
using MaterialDesignThemes.Wpf;

namespace CluSys.lib
{
}

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
                _cn = ClusysUtils.GetConnection();

            if (_cn.State != ConnectionState.Open)
                _cn.Open();

            return _cn.State == ConnectionState.Open;
        }

        private void FilterAthletes(object sender, TextChangedEventArgs e)
        {
            var filterText = ((TextBox)sender).Text;
            var lb = ClusysUtils.FindByType<ListBox>((Visual)e.Source);

            if (lb == null)
                return;

            var view = (CollectionView)CollectionViewSource.GetDefaultView(lb.ItemsSource);
            view.Filter = (a) => {
                var athlete = a as Athlete;

                return (athlete?.FirstName + " " + athlete?.LastName).IndexOf(filterText, StringComparison.OrdinalIgnoreCase) >= 0;
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

        private void OpenSessions(object sender, RoutedEventArgs e)
        {
            var me = (sender as Control)?.DataContext as MedicalEvaluation;

            SessionsList.ItemsSource = me?.Sessions(_cn);
            SessionsExpander.IsExpanded = true;
        }

        private void ResetAthleteContent()
        {
            EvaluationsList.ItemsSource = null;
            SessionsList.ItemsSource = null;
            SessionsExpander.IsExpanded = false;
        }

        private void FilterEvaluations(object sender, RangeParameterChangedEventArgs rangeParameterChangedEventArgs)
        {
            var dr = sender as DateRangeSlider;

            if (dr == null)
                return;

            var meView = (CollectionView) CollectionViewSource.GetDefaultView(EvaluationsList.ItemsSource);
            if(meView != null)
                meView.Filter = evaluation =>
                {
                    var me = evaluation as MedicalEvaluation;

                    return me != null && (dr.LowerDate <= me.OpeningDate && me.OpeningDate <= dr.UpperDate
                                          || dr.LowerDate <= me.ClosingDate && me.ClosingDate <= dr.UpperDate);
                };

            var seView = (CollectionView) CollectionViewSource.GetDefaultView(SessionsList.ItemsSource);
            if(seView != null)
                seView.Filter = session =>
                {
                    var se = session as EvaluationSession;

                    return se != null && dr.LowerDate <= se.Date && se.Date <= dr.UpperDate;
                };
        }
    }
}
