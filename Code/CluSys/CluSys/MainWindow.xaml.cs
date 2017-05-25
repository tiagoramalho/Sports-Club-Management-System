using CluSys.lib;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Globalization;
using MahApps.Metro.Controls;

namespace CluSys
{
    public partial class MainWindow
    {
        private bool _withinMarkPopup = false;
        private readonly ObservableCollection<Athlete> _openAthletes;

        public MainWindow()
        {
            // Init
            _openAthletes = new ObservableCollection<Athlete>();

            InitializeComponent();

            // Set culture
            CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("pt-PT");
            CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo("pt-PT");

            BindingOperations.GetBinding(MinDateRange, DatePicker.SelectedDateProperty)?.ValidationRules.Add(SliderRange.MinDateValidator);
            BindingOperations.GetBinding(MaxDateRange, DatePicker.SelectedDateProperty)?.ValidationRules.Add(SliderRange.MaxDateValidator);

            OpenAthletesList.ItemsSource = _openAthletes;  // empty on start
            ModalityList.ItemsSource = Modalities.GetModalities();
            AthletesWithOpenEvaluations.ItemsSource = Athletes.AthletesWithOpenEvaluations();

            // Modal
            EvaluationModal.DataContext = new ModalState((Athlete)AthleteContent.DataContext);
            EvaluationModal.PreviewMouseUp += (sender, args) => { if (!_withinMarkPopup) BodyChartMarkPopup.IsPopupOpen = false; };
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
            AthleteContent.Visibility = Visibility.Hidden; AthleteContent.DataContext = null;
            HomeContent.Visibility = Visibility.Visible;
        }

        private void OpenAthlete(object sender, MouseButtonEventArgs e)
        {
            var item = ItemsControl.ContainerFromElement(sender as ListBox, (DependencyObject) e.OriginalSource) as ListBoxItem;

            if (item == null)
                return;

            var athlete = new Athlete();

            // Nothing to do if athlete already open
            if(Equals(AthleteContent.DataContext, athlete))
                return;

            AthleteContent.DataContext = athlete;

            if (!_openAthletes.Contains(athlete))
                _openAthletes.Insert(0, athlete);

            // Reset the athlete's content
            ResetAthleteContent();
            // Get this athlete's evaluations
            EvaluationsList.ItemsSource = athlete.GetEvaluations();
            // Filter them based on date
            FilterEvaluations(SliderRange);

            // Toogle visibility
            HomeContent.Visibility = Visibility.Hidden;
            AthleteContent.Visibility = Visibility.Visible;
        }

        private void OpenSessions(object sender, RoutedEventArgs e)
        {
            var me = (sender as Control)?.DataContext as MedicalEvaluation;

            SessionsExpander.DataContext = me;
            SessionsList.ItemsSource = me?.Sessions;
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
            if (meView != null)
                meView.Filter = evaluation =>
                {
                    var me = evaluation as MedicalEvaluation;

                    return me != null && (dr.LowerDate <= me.OpeningDate && me.OpeningDate <= dr.UpperDate
                                          && (me.ClosingDate == null ||
                                              dr.LowerDate <= me.ClosingDate && me.ClosingDate <= dr.UpperDate));
                };

            var seView = (CollectionView) CollectionViewSource.GetDefaultView(SessionsList.ItemsSource);
            if (seView != null)
                seView.Filter = session =>
                {
                    var se = session as EvaluationSession;

                    return se != null && dr.LowerDate <= se.Date && se.Date <= dr.UpperDate;
                };
        }

        /******************************************************************************************************
         * Body Chart
         *****************************************************************************************************/

        private void BodyChartClick(object sender, MouseButtonEventArgs e)
        {
            if (_withinMarkPopup)
                return;

            var bc = BodyChartCanvas;
            var lb = AnnotationsList;
            var ms = (ModalState) EvaluationModal.DataContext;
            var activeView = ms.ActiveView;
            var point = new Point(e.GetPosition(BodyChart).X - BodyChartMark.DrawRadius, e.GetPosition(BodyChart).Y - BodyChartMark.DrawRadius);

            var mark = ms.Marks.FirstOrDefault(m => Point.Subtract(point, new Point(m.X, m.Y)).Length < 4 * BodyChartMark.DrawRadius);

            if (mark == null)
            {
                if (ms.CanBeEdited == false)
                    return;

                mark = new BodyChartMark { ViewId = activeView.Id, X = point.X, Y = point.Y, PainLevel = 2 };

                ms.Marks.Add(mark);
                BodyChartMark.DrawMark(bc, mark, BodyChartClick);
            }

            BodyChartMarkPopup.IsPopupOpen = false;  // close the popup
            BodyChartMarkPopup.DataContext = null;

            lb.SelectedItems.Clear();
            foreach (var annotation in mark.Annotations)
                lb.SelectedItems.Add(annotation);

            BodyChartMarkPopup.DataContext = mark;
            BodyChartMarkPopup.IsPopupOpen = true;  // open the popup
        }

        private void BodyChartMarkPopup_OnMouseEnter(object sender, MouseEventArgs e) { _withinMarkPopup = true; }

        private void BodyChartMarkPopup_OnMouseLeave(object sender, MouseEventArgs e) { _withinMarkPopup = false; }

        private void AnnotationSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var lb = AnnotationsList;
            var mark = BodyChartMarkPopup.DataContext as BodyChartMark;

            if(lb == null || mark == null)
                return;

            mark.Annotations.Clear();
            foreach (var selectedItem in lb.SelectedItems)
                mark.Annotations.Add(selectedItem as Annotation);
        }

        private void DeleteMark(object sender, RoutedEventArgs e)
        {
            var bc = BodyChartCanvas;
            var ms = (ModalState) EvaluationModal.DataContext;
            var mark = BodyChartMarkPopup.DataContext as BodyChartMark;

            if (ms == null || mark == null)
                return;

            BodyChartMarkPopup.IsPopupOpen = false;
            BodyChartMarkPopup.DataContext = null;

            var ellipse = bc.Children.OfType<Ellipse>().FirstOrDefault(point => Math.Abs((double)point.GetValue(Canvas.LeftProperty) - mark.X) < double.Epsilon && Math.Abs((double)point.GetValue(Canvas.TopProperty) - mark.Y) < double.Epsilon);
            bc.Children.Remove(ellipse);
            ms.Marks.Remove(mark);
        }

        private void SaveMark(object sender, RoutedEventArgs e) { BodyChartMarkPopup.IsPopupOpen = false;  /* That's it. */ }

        private void LeftView(object sender, RoutedEventArgs e)
        {
            var ms = (ModalState) EvaluationModal.DataContext;
            var newIdx = ms.ActiveViewIdx + 1;

            RotateView(ms, newIdx >= ms.Views.Count ? 0 : newIdx);
        }

        private void RightView(object sender, RoutedEventArgs e)
        {
            var ms = (ModalState) EvaluationModal.DataContext;
            var newIdx = ms.ActiveViewIdx - 1;

            RotateView(ms, newIdx < 0 ? ms.Views.Count - 1 : newIdx);
        }

        private void RotateView(ModalState ms, int newIdx)
        {
            var bc = BodyChartCanvas;

            ms.ActiveViewIdx = newIdx;
            var newActiveView = ms.ActiveView;

            BodyChartMarkPopup.IsPopupOpen = false;

            // Clear the canvas
            bc.Children.RemoveRange(1, int.MaxValue);
            // Add values (if they exist)
            foreach (var m in ms.Marks)
                if (m.ViewId == newActiveView.Id)
                    BodyChartMark.DrawMark(bc, m, BodyChartClick);

            // Make it visible now
            BodyChart.DataContext = newActiveView;
        }

        /******************************************************************************************************
         * Problems
         *****************************************************************************************************/

        private void AddProblem(object sender, RoutedEventArgs e)
        {
            var ms = (ModalState) EvaluationModal.DataContext;
            var prob = new MajorProblem(ms.Problems) { Description = NewProblemText.Text };

            if (ms.Problems.Contains(prob))
                return;

            ms.Problems.Add(prob); NewProblemText.Text = string.Empty;
        }

        private void DeleteProblem(object sender, RoutedEventArgs e)
        {
            var prob = (sender as Control)?.DataContext as MajorProblem;

            if (prob == null)
                return;

            var ms = (ModalState) EvaluationModal.DataContext;

            ms.Problems.Remove(prob);

            // Forcefully reload the ProblemsList
            ProblemsList.ItemsSource = null;
            ProblemsList.ItemsSource = ms.Problems;

            // Forcefully reload the TreatmentsList
            TreatmentsList.ItemsSource = null;
            TreatmentsList.ItemsSource = ms.Treatments;
        }

        /******************************************************************************************************
         * Treatments
         *****************************************************************************************************/

        private void AddTreatment(object sender, RoutedEventArgs e)
        {
            var ms = (ModalState) EvaluationModal.DataContext;
            var problem = NewTreatmentRefProblem.SelectionBoxItem as MajorProblem;

            var treatment = new TreatmentPlan { Description = NewTreatmentDescription.Text, Objective = NewTreatmentObjective.Text, RefProblem = problem};

            // Call the value checker...

            if (ms.Treatments.Contains(treatment))
                return;

            ms.Treatments.Add(treatment);
            NewTreatmentDescription.Text = string.Empty;
            NewTreatmentObjective.Text = string.Empty;
            NewTreatmentRefProblem.SelectedIndex = -1;
        }

        private void DeleteTreatment(object sender, RoutedEventArgs e)
        {
            var treatment = (sender as Control)?.DataContext as TreatmentPlan;

            if(treatment == null)
                return;

            var ms = (ModalState) EvaluationModal.DataContext;

            ms.Treatments.Remove(treatment);
        }

        /******************************************************************************************************
         * Observations
         *****************************************************************************************************/

        private void AddObservation(object sender, RoutedEventArgs e)
        {
            var ms = (ModalState) EvaluationModal.DataContext;
            var obs = new SessionObservation { Obs = NewObservationText.Text };

            if (ms.Observations.Contains(obs))
                return;

            ms.Observations.Add(obs); NewObservationText.Text = string.Empty;
        }

        private void DeleteObservation(object sender, RoutedEventArgs e)
        {
            var obs = (sender as Control)?.DataContext as SessionObservation;

            if(obs == null)
                return;

            var ms = (ModalState) EvaluationModal.DataContext;

            ms.Observations.Remove(obs);
        }

        private void SaveSession(object sender, RoutedEventArgs e) {
            SessionModal.IsOpen = false;  // close the modal
        }

        private void OpenSession(object sender, RoutedEventArgs e)
        {
            var session = (EvaluationSession)((Control)sender).DataContext;
            var evaluation = session.GetMedicalEvaluation();

            EvaluationModal.DataContext = new ModalState((Athlete)AthleteContent.DataContext, evaluation, session);
            SessionModal.IsOpen = true;
        }

        private void NewSession(object sender, RoutedEventArgs e)
        {
            EvaluationModal.DataContext = new ModalState((Athlete)AthleteContent.DataContext);
            SessionModal.IsOpen = true;
        }

        private void TryClickOutOfModal(object sender, MouseButtonEventArgs e)
        {
            if (_withinMarkPopup)
                return;
            else
                BodyChartMarkPopup.IsPopupOpen = false;

            if (SessionModal.IsOpen && !EvaluationModal.IsMouseOver)
            {
                if (((ModalState) EvaluationModal.DataContext).CanBeEdited)
                    EvaluationModal.IsTopDrawerOpen = true;
                else
                    SessionModal.IsOpen = false;
            }
        }

        private void CloseDrawerAndModal(object sender, RoutedEventArgs e) { EvaluationModal.IsTopDrawerOpen = SessionModal.IsOpen = false; }
    }
}
