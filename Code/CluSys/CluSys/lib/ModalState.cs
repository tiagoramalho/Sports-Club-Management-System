using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;

namespace CluSys.lib
{
    internal class ModalState
    {
        private const string PhysiotherapistCC = "12123";

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

        public void Save()
        {
            GetEvalId(); GetSessionId();
        }

        private void GetEvalId()
        {
            using (var cn = ClusysUtils.GetConnection())
            {
                cn.Open();

                var cmd = new SqlCommand("P_GetOrCreateEvaluation", cn) {CommandType = CommandType.StoredProcedure};
                cmd.Parameters.Add(new SqlParameter("@AthleteCC", Athlete.CC));
                cmd.Parameters.Add(new SqlParameter("@PhysiotherapistCC", PhysiotherapistCC));
                cmd.Parameters.Add(new SqlParameter("@OpeningDate", Session.Date));
                cmd.Parameters.Add(new SqlParameter("@EvalId", SqlDbType.Int) { Direction = ParameterDirection.Output });
                cmd.ExecuteNonQuery();

                Evaluation.Id = Session.EvalId = Convert.ToInt32(cmd.Parameters["@EvalId"].Value);
            }
        }

        private void GetSessionId()
        {
            using (var cn = ClusysUtils.GetConnection())
            {
                cn.Open();

                var cmd = new SqlCommand("P_GetOrCreateSession", cn) {CommandType = CommandType.StoredProcedure};
                cmd.Parameters.Add(new SqlParameter("@AthleteCC", Athlete.CC));
                cmd.Parameters.Add(new SqlParameter("@PhysiotherapistCC", PhysiotherapistCC));
                cmd.Parameters.Add(new SqlParameter("@Date", Session.Date));
                cmd.Parameters.Add(new SqlParameter("@EvalId", Session.EvalId));
                cmd.Parameters.Add(new SqlParameter("@SessionId", SqlDbType.Int) { Direction = ParameterDirection.Output });
                cmd.ExecuteNonQuery();

                Session.Id = Convert.ToInt32(cmd.Parameters["@SessionId"].Value);
            }
        }

        private void UpdateEvaluation(int evalId)
        {
        }
    }
}
