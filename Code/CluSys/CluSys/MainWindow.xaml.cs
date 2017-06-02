using CluSys.lib;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
        private bool _withinMarkPopup;
        private Athlete _oldAthlete;
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
            EvaluationModal.PreviewMouseUp += delegate { if (!_withinMarkPopup) BodyChartMarkPopup.IsPopupOpen = false; };
        }

        private void FilterAthletes(object sender, TextChangedEventArgs e)
        {
            var filterText = ((TextBox)sender).Text;
            var lb = ClusysUtils.FindByType<ListBox>((Visual)e.Source);

            if (lb == null)
                return;

            var view = (CollectionView)CollectionViewSource.GetDefaultView(lb.ItemsSource);
            view.Filter = delegate(object a)
            {
                var athlete = a as Athlete;

                return (athlete?.FirstName + " " + athlete?.LastName).IndexOf(filterText, StringComparison.OrdinalIgnoreCase) >= 0;
            };
        }

        private void GoHome(object sender = null, RoutedEventArgs e = null)
        {
            _oldAthlete = null;
            // Toogle visibility
            AthleteContent.Visibility = Visibility.Hidden; AthleteContent.DataContext = null;
            HomeContent.Visibility = Visibility.Visible;
        }

        private void OpenAthlete(object sender = null, MouseButtonEventArgs e = null)
        {
            // Assume the sender is an athlete
            var athlete = sender as Athlete;

            // If it is not, get it from the sender's context
            if (athlete == null)
            {
                // No mouse to process
                if (e == null) return;

                athlete = (ItemsControl.ContainerFromElement(sender as ListBox, (DependencyObject) e.OriginalSource) as ListBoxItem)?.Content as Athlete;

                // Couldn't process the input as an athlete, thus, return
                if (athlete == null) return;
            }

            // Nothing to do if athlete already open
            if(Equals(AthleteContent.DataContext, athlete))
                return;

            // Save the current athlete
            _oldAthlete = AthleteContent.DataContext as Athlete;
            // Place the new one
            AthleteContent.DataContext = athlete;

            // Add it to the list if not already there
            if (!_openAthletes.Contains(athlete))
                _openAthletes.Insert(0, athlete);

            // Reset the athlete's content
            ResetAthleteContent();
            // Get this athlete's evaluations
            EvaluationsList.ItemsSource = athlete.GetEvaluations();
            // Filter them based on date
            FilterEvaluations();

            // Toogle visibility
            HomeContent.Visibility = Visibility.Hidden;
            AthleteContent.Visibility = Visibility.Visible;
        }

        private void CloseAthlete(object sender, RoutedEventArgs e)
        {
            var athlete = (Athlete)((Control) sender).DataContext;

            if (_oldAthlete == null || Equals(athlete, _oldAthlete))
                GoHome();
            else
                OpenAthlete(_oldAthlete);

            _oldAthlete = null;
            _openAthletes.Remove(athlete);
        }

        private void ResetAthleteContent()
        {
            EvaluationsList.ItemsSource = null;
            SessionsList.ItemsSource = null;
            SessionsExpander.IsExpanded = false;
        }

        private void FilterEvaluations(object sender = null, RangeParameterChangedEventArgs rangeParameterChangedEventArgs=null)
        {
            var dr = sender == null ? SliderRange : sender as DateRangeSlider;

            if (dr == null)
                return;

            if (CollectionViewSource.GetDefaultView(EvaluationsList.ItemsSource) is CollectionView meView)
            {
                meView.SortDescriptions.Add(new SortDescription("Id", ListSortDirection.Descending));
                meView.Filter = delegate (object evaluation)
                {
                    var me = evaluation as MedicalEvaluation;
                    var lowerDate = dr.LowerDate.Date;
                    var upperDate = dr.UpperDate.Date.AddDays(1).AddSeconds(-1);

                    return me != null && (lowerDate <= me.OpeningDate && me.OpeningDate <= upperDate
                                          && (me.ClosingDate == null || lowerDate <= me.ClosingDate && me.ClosingDate <= upperDate));
                };
                meView.Refresh();
            }

            if (CollectionViewSource.GetDefaultView(SessionsList.ItemsSource) is CollectionView seView)
            {
                seView.SortDescriptions.Add(new SortDescription("Id", ListSortDirection.Descending));
                seView.Filter = delegate (object session)
                {
                    var se = session as EvaluationSession;
                    var lowerDate = dr.LowerDate.Date;
                    var upperDate = dr.UpperDate.Date.AddDays(1).AddSeconds(-1);

                    return se != null && lowerDate <= se.Date && se.Date <= upperDate;
                };
                seView.Refresh();
            }
        }

        /******************************************************************************************************
         * Sessions
         *****************************************************************************************************/

        private void OpenSessions(object sender, RoutedEventArgs e)
        {
            MedicalEvaluation me;

            if (sender is MedicalEvaluation)
                me = (MedicalEvaluation) sender;
            else if (sender is Control)
                me = ((Control)sender).DataContext as MedicalEvaluation;
            else return;

            SessionsExpander.DataContext = me;
            SessionsList.ItemsSource = me?.Sessions;
            FilterEvaluations();

            SessionsExpander.IsExpanded = true;
        }

        private void NewSession(object sender, RoutedEventArgs e)
        {
            ModalState ms;
            EvaluationModal.DataContext = ms = new ModalState((Athlete)AthleteContent.DataContext);

            // Scroll the modal
            (EvaluationModal.Content as ScrollViewer)?.ScrollToTop();

            // clean the current body points
            UpdateBodyView(ms, 0);
            SessionModal.IsOpen = true;
        }

        private void OpenSession(object sender, RoutedEventArgs e)
        {
            var session = (EvaluationSession)((Control)sender).DataContext;
            var evaluation = session.GetMedicalEvaluation();

            ModalState ms;
            EvaluationModal.DataContext = ms = new ModalState((Athlete)AthleteContent.DataContext, evaluation, session);

            // Scroll the modal
            (EvaluationModal.Content as ScrollViewer)?.ScrollToTop();

            // make the initial body points visible
            UpdateBodyView(ms, 0);
            SessionModal.IsOpen = true;
        }

        private void SaveSession(object sender, RoutedEventArgs e)
        {
            var ms = EvaluationModal.DataContext as ModalState;

            if(ms == null)
                return;

            ms.Save();
            SessionModal.IsOpen = false;  // close the modal

            /* Update the athlete's home list */
            AthletesWithOpenEvaluations.ItemsSource = Athletes.AthletesWithOpenEvaluations();
            /* Update the athlete's content */
            AthleteContent.DataContext = null; AthleteContent.DataContext = ms.Athlete;
            /* Update the sessions' lists */
            EvaluationsList.ItemsSource = ms.Athlete.GetEvaluations();
            OpenSessions(ms.Evaluation, null);
            FilterEvaluations();
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

            var mark = ms.Marks.FirstOrDefault(delegate(BodyChartMark m)
            {
                return Point.Subtract(point, new Point(m.X, m.Y)).Length < 4 * BodyChartMark.DrawRadius;
            });

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

            var ellipse = bc.Children.OfType<Ellipse>().FirstOrDefault(delegate(Ellipse point)
            {
                return Math.Abs((double) point.GetValue(Canvas.LeftProperty) - mark.X) < double.Epsilon &&
                       Math.Abs((double) point.GetValue(Canvas.TopProperty) - mark.Y) < double.Epsilon;
            });
            bc.Children.Remove(ellipse);
            ms.Marks.Remove(mark);
        }

        private void SaveMark(object sender, RoutedEventArgs e) { BodyChartMarkPopup.IsPopupOpen = false;  /* That's it. */ }

        private void LeftView(object sender, RoutedEventArgs e)
        {
            var ms = (ModalState) EvaluationModal.DataContext;
            var newIdx = ms.ActiveViewIdx + 1;

            UpdateBodyView(ms, newIdx >= ms.Views.Count ? 0 : newIdx);
        }

        private void RightView(object sender, RoutedEventArgs e)
        {
            var ms = (ModalState) EvaluationModal.DataContext;
            var newIdx = ms.ActiveViewIdx - 1;

            UpdateBodyView(ms, newIdx < 0 ? ms.Views.Count - 1 : newIdx);
        }

        private void UpdateBodyView(ModalState ms, int newIdx)
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

        /******************************************************************************************************
         * Modals
         *****************************************************************************************************/

        private void TryClickOutOfModal(object sender, MouseButtonEventArgs e)
        {
            if (_withinMarkPopup)
                return;
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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            new Management().Show();
        }
    }
}
