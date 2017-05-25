using System;
using System.Collections.ObjectModel;
using System.Data.SqlClient;

namespace CluSys.lib
{
    internal class ModalState
    {
        public Athlete Athlete { get; set; }
        public MedicalEvaluation Evaluation { get; set; }
        public EvaluationSession Session { get; set; }

        // Body chart
        public int ActiveViewIdx { get; set; }
        public readonly ObservableCollection<BodyChartView> Views;
        public BodyChartView ActiveView { get { return Views[ActiveViewIdx]; } set { ActiveViewIdx = Views.IndexOf(value); } }

        public ObservableCollection<BodyChartMark> Marks;
        public ObservableCollection<Annotation> Annotations { get; }

        // Problems
        public ObservableCollection<MajorProblem> Problems { get; set; }

        // Treatments
        public ObservableCollection<TreatmentPlan> Treatments { get; set; }

        // Observations
        public ObservableCollection<SessionObservation> Observations { get; set; }

        // Others
        public bool MedicalDischarge { get; set; }

        public bool CanBeEdited { get; set; }
        public bool ExpectedRecoveryPickerEnabled { get { return CanBeEdited && !MedicalDischarge; } }

        public ModalState(Athlete athlete, MedicalEvaluation evaluation = null, EvaluationSession session = null)
        {
            Athlete = athlete;
            Evaluation = evaluation ?? new MedicalEvaluation();
            Session = session ?? new EvaluationSession { Date = DateTime.Today };

            ActiveViewIdx = 0;
            Views = BodyChartViews.GetViews();
            Annotations = lib.Annotations.GetAnnotations();

            if (session != null)
            {
                Marks = session.GetMarks();
                Problems = session.GetProblems();
                Treatments = session.GetTreatments();
                Observations = session.GetObservations();
                MedicalDischarge = evaluation?.ClosingDate == session.Date;
                CanBeEdited = false;
            }
            else
            {
                Marks = new ObservableCollection<BodyChartMark>();
                Problems = new ObservableCollection<MajorProblem>();
                Treatments = new ObservableCollection<TreatmentPlan>();
                Observations = new ObservableCollection<SessionObservation>();
                MedicalDischarge = false;
                CanBeEdited = true;
            }
        }

        public void Save(SqlConnection cn)
        {
        }

        private int GetEvaluationId(SqlConnection cn)
        {
            return -1;
        }

        private void UpdateEvaluation(SqlConnection cn, int evalId)
        {
        }
    }
}
