using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Navigation;

namespace CluSys.lib
{
    class ModalState
    {
        public Athlete Patient { get; set; }

        // Basic
        public double? Weight { get; set; }
        public double? Height { get; set; }
        public DateTime? Date { get; set; } = DateTime.Now;
        public string Story { get; set; }

        // Body chart
        public BodyChartView ActiveView
        {
            get { return Views[ActiveViewIdx]; }
            set { ActiveViewIdx = Views.IndexOf(value); }
        }
        public int ActiveViewIdx { get; set; } = 0;
        public readonly ObservableCollection<BodyChartView> Views = BodyChartViews.GetViews();
        public readonly ObservableCollection<BodyChartMark> Marks = new ObservableCollection<BodyChartMark>();
        public ObservableCollection<Annotation> Annotations { get; set; } = lib.Annotations.GetAnnotations();

        // Problems
        public ObservableCollection<MajorProblem> Problems { get; set; } = new ObservableCollection<MajorProblem>();

        // Treatments
        public ObservableCollection<TreatmentPlan> Treatments { get; set; } = new ObservableCollection<TreatmentPlan>();

        // Observations
        public ObservableCollection<SessionObservation> Observations { get; set; } = new ObservableCollection<SessionObservation>();

        // Others
        public bool MedicalDischarge { get; set; } = false;
        public DateTime? ExpectedRecoveryDate { get; set; }

        private const int PhysiotherapistCC = 12123;

        public void Save(SqlConnection cn)
        {
        }

        private int GetEvaluationId(SqlConnection cn)
        {
            var evalId = -1;

            var cmd = new SqlCommand
            {
                Connection = cn,
                CommandText = $"SELECT Id FROM MedicalEvaluation WHERE AthleteCC={Patient.CC} AND ClosingDate IS NULL;",
            };
            var reader = cmd.ExecuteReader();
            try
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    evalId = int.Parse(reader[0].ToString());
                    UpdateEvaluation(cn, evalId);
                }
            }
            finally
            {
                reader.Close();
            }

            if (evalId == -1)
            {
                var eval = new MedicalEvaluation
                {
                    AthleteCC = Patient.CC,
                    ClosingDate = null,
                    ExpectedRecovery = ExpectedRecoveryDate,
                    Height = Height ?? 0,  // this should be the last value recorded
                    OpeningDate = DateTime.Now,
                    PhysiotherapistCC = PhysiotherapistCC.ToString(),
                    Story = Story,
                    Weightt = Weight ?? 0,  // same
                };
                eval.InsertMedicalEvaluation(cn);
                cmd = new SqlCommand
                {
                    Connection = cn,
                    CommandText = $"SELECT Id FROM MedicalEvaluation WHERE AthleteCC={Patient.CC} AND ClosingDate IS NULL;",
                };
                reader = cmd.ExecuteReader();
                try
                {
                    reader.Read();
                    evalId = int.Parse(reader[0].ToString());
                }
                finally
                {
                    reader.Close();
                }
            }

            return evalId;
        }

        private void UpdateEvaluation(SqlConnection cn, int evalId)
        {

            var cmd = new SqlCommand
            {
                Connection = cn,
                CommandText = $"UPDATE MedicalEvaluation SET Height={Height ?? 0}, Weightt={Weight ?? 0}, ExpectedRecovery={ExpectedRecoveryDate} WHERE ID={evalId}",
            };

            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to update evaluation's value in the database. \n ERROR MESSAGE: \n" + ex.Message);
            }
        }
    }
}
