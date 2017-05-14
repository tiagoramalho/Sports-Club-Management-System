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

namespace CluSys
{
    public partial class MainWindow
    {
        private SqlConnection _cn;
        private ModalState _modelState;
        private bool _withinMarkPopup = false;
        private readonly ObservableCollection<Athlete> _openAthletes;

        public MainWindow()
        {
            // Init
            OpenConnection();
            _modelState = new ModalState();
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

            // Modal
            EvaluationModal.DataContext = _modelState;
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
            // Toogle visibility
            AthleteContent.Visibility = Visibility.Hidden;
            HomeContent.Visibility = Visibility.Visible;
        }

        private void OpenAthlete(object sender, MouseButtonEventArgs e)
        {
            var item = ItemsControl.ContainerFromElement(sender as ListBox, (DependencyObject) e.OriginalSource) as ListBoxItem;

            if (item == null)
                return;

            var athlete = new AthleteWithBody(_cn, item.Content as Athlete);

            // Nothing to do if athlete already open
            if(Equals(AthleteContent.DataContext, athlete))
                return;

            AthleteContent.DataContext = athlete;

            if (!_openAthletes.Contains(athlete))
                _openAthletes.Insert(0, athlete);

            // Reset the athlete's content
            ResetAthleteContent();
            // Get this athlete's evaluations
            EvaluationsList.ItemsSource = athlete.Evaluations(_cn);
            // Filter them based on date
            FilterEvaluations(SliderRange);

            // Toogle visibility
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

        private void FilterEvaluations(object sender, RangeParameterChangedEventArgs rangeParameterChangedEventArgs=null)
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
                                          && (me.ClosingDate == null || dr.LowerDate <= me.ClosingDate && me.ClosingDate <= dr.UpperDate));
                };

            var seView = (CollectionView) CollectionViewSource.GetDefaultView(SessionsList.ItemsSource);
            if(seView != null)
                seView.Filter = session =>
                {
                    var se = session as EvaluationSession;

                    return se != null && dr.LowerDate <= se.Date && se.Date <= dr.UpperDate;
                };
        }

        private void BodyChartClick(object sender, MouseButtonEventArgs e)
        {
            if (_withinMarkPopup)
                return;

            var createMark = true;
            var bc = BodyChartCanvas;
            var ms = (ModalState) EvaluationModal.DataContext;
            var activeView = ms.ActiveView;
            var point = new Point(e.GetPosition(BodyChart).X - ClusysUtils.DrawRadius, e.GetPosition(BodyChart).Y - ClusysUtils.DrawRadius);

            BodyChartMarkPopup.IsPopupOpen = false;  // close the popup

            foreach (var m in ms.Marks)
                if (Point.Subtract(point, new Point(m.X, m.Y)).Length < 4 * ClusysUtils.DrawRadius)
                {
                    BodyChartMarkPopup.DataContext = m;
                    createMark = false;
                    break;
                }

            if (createMark)
            {
                var mark = new BodyChartMark { ViewId = activeView.Id, X = point.X, Y = point.Y, PainLevel = 2 };

                ms.Marks.Add(mark);
                BodyChartMarkPopup.DataContext = mark;

                ClusysUtils.DrawPoint(point, bc, Brushes.Black, BodyChartClick);
            }

            BodyChartMarkPopup.IsPopupOpen = true;  // open the popup
        }

        private void LeftView(object sender, RoutedEventArgs e)
        {
            var ms = (ModalState) EvaluationModal.DataContext;
            var newIdx = ms.ActiveViewIdx + 1;  // or overflow

            RotateView(ms, newIdx >= ms.Views.Count ? 0 : newIdx);
        }

        private void RightView(object sender, RoutedEventArgs e)
        {
            var ms = (ModalState) EvaluationModal.DataContext;
            var newIdx = ms.ActiveViewIdx - 1;  // or "underflow"

            RotateView(ms, newIdx < 0 ? ms.Views.Count - 1 : newIdx);
        }

        private void RotateView(ModalState ms, int newIdx)
        {
            var bc = BodyChartCanvas;

            ms.ActiveViewIdx = newIdx;
            var newActiveView = ms.ActiveView;

            // Clear the canvas
            bc.Children.RemoveRange(1, int.MaxValue);
            // Add values (if they exist)
            foreach (var m in ms.Marks)
                if (m.ViewId == newActiveView.Id)
                    ClusysUtils.DrawPoint(new Point(m.X, m.Y), bc, Brushes.Black, BodyChartClick);

            // Make it visible now
            BodyChart.DataContext = newActiveView;
        }

        private void AddProblem(object sender, RoutedEventArgs e)
        {
            var ms = (ModalState) EvaluationModal.DataContext;
            ms.Problems.Add(NewProblemText.Text);
            NewProblemText.Text = string.Empty;
        }

        private void BodyChartMarkPopup_OnMouseEnter(object sender, MouseEventArgs e) { _withinMarkPopup = true; }

        private void BodyChartMarkPopup_OnMouseLeave(object sender, MouseEventArgs e) { _withinMarkPopup = false; }
    }
}
