using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CluSys.lib
{
    [Serializable]
    class EvaluationSession
    {
        public int Id { get; set; } = -1;
        public int EvalId { get; set; }
        public DateTime Date { get; set; }

        public int NumberOfProblems
        {
            get
            {
                var conn = ClusysUtils.GetConnection();
                var cmd = new SqlCommand($"SELECT COUNT(*) as NumberOfProblems FROM MajorProblem WHERE EvalId={EvalId} and SessionId={Id};", conn);
                using (var reader = cmd.ExecuteReader())
                    return reader.Read() ? int.Parse(reader["NumberOfProblems"].ToString()) : 0;
            }
        }

        public int NumberOfTreatments
        {
            get
            {
                var conn = ClusysUtils.GetConnection();
                var cmd = new SqlCommand($"SELECT COUNT(*) as NumberOfTreatments FROM TreatmentPlan WHERE EvalId={EvalId} and SessionId={Id};", conn);
                using (var reader = cmd.ExecuteReader())
                    return reader.Read() ? int.Parse(reader["NumberOfTreatments"].ToString()) : 0;
            }
        }

        private bool Equals(EvaluationSession other)
        {
            return Id == other.Id && EvalId == other.EvalId;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((EvaluationSession) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (Id * 397) ^ EvalId;
            }
        }

        /*
        private void submitEvaluationSession(SqlConnection cn, EvaluationSession ES)
        {
            
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "INSERT INTO EvaluationSession (EvalId, dateSession) " + "VALUES (@EvalId,, @dateSession)";
            cmd.Parameters.AddWithValue("@EvalId", ES.EvalId);
            cmd.Parameters.AddWithValue("@dateSession", ES.Date);
            cmd.Connection = cn;

            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to Insert Evaluation Session in database. \n ERROR MESSAGE: \n" + ex.Message);
            }
            finally
            {
                cn.Close();
            }

        }
        */

        public MedicalEvaluation GetEvaluation(SqlConnection cn = null)
        {
            cn = cn ?? ClusysUtils.GetConnection();
            SqlCommand cmd = new SqlCommand($"SELECT * FROM MedicalEvaluation WHERE Id={EvalId}", cn);
            SqlDataReader reader = cmd.ExecuteReader();

            if (!reader.Read())
                return null;
            var evaluation = new MedicalEvaluation
            {
                Id = int.Parse(reader["ID"].ToString()),
                Weightt = double.Parse(reader["Weightt"].ToString()),
                Height = double.Parse(reader["Height"].ToString()),
                Story = reader["Story"].ToString(),
                OpeningDate = DateTime.Parse(reader["OpeningDate"].ToString()),
                ClosingDate = reader["ClosingDATE"].ToString() != ""
                    ? (DateTime?)DateTime.Parse(reader["ClosingDATE"].ToString())
                    : null,
                ExpectedRecoveryDate = reader["ExpectedRecovery"].ToString() != ""
                    ? (DateTime?)DateTime.Parse(reader["ExpectedRecovery"].ToString())
                    : null,
                AthleteCC = reader["AthleteCC"].ToString(),
                PhysiotherapistCC = reader["PhysiotherapistCC"].ToString(),
            };

            reader.Close();
            return evaluation;
        }

        public ObservableCollection<BodyChartMark> GetMarks(SqlConnection cn = null)
        {
            var bodyMarks = new ObservableCollection<BodyChartMark>();

            cn = cn ?? ClusysUtils.GetConnection();
            var cmd = new SqlCommand($"SELECT * FROM BodyChartMark WHERE EvalId={EvalId} AND SessionId={Id}", cn);
            var reader = cmd.ExecuteReader();

            while (reader.Read())
                bodyMarks.Add(new BodyChartMark
                {
                    Id = int.Parse(reader["ID"].ToString()),
                    X = double.Parse(reader["x"].ToString()),
                    Y = double.Parse(reader["y"].ToString()),
                    PainLevel = int.Parse(reader["PainLevel"].ToString()),
                    Obs = reader["Obs"].ToString(),
                    EvalId = int.Parse(reader["EvalId"].ToString()),
                    SessionId = int.Parse(reader["SessionId"].ToString()),
                    ViewId = int.Parse(reader["ViewId"].ToString()),
                });
            
            cn.Close();
            return bodyMarks;
        }

        public ObservableCollection<MajorProblem> GetProblems(SqlConnection cn = null)
        {
            var problems = new ObservableCollection<MajorProblem>();

            cn = cn ?? ClusysUtils.GetConnection();
            var cmd = new SqlCommand($"SELECT * FROM MajorProblem WHERE EvalId={EvalId} AND SessionId={Id}", cn);
            var reader = cmd.ExecuteReader();

            while (reader.Read())
                problems.Add(new MajorProblem
                {
                    Id = int.Parse(reader["ID"].ToString()),
                    Obs = reader["Obs"].ToString(),
                    EvalId = int.Parse(reader["EvalId"].ToString()),
                    SessionId = int.Parse(reader["SessionId"].ToString()),
                });
            
            cn.Close();
            return problems;
        }

        public ObservableCollection<TreatmentPlan> GetTreatments(SqlConnection cn = null)
        {
            var treatments = new ObservableCollection<TreatmentPlan>();

            cn = cn ?? ClusysUtils.GetConnection();
            var cmd = new SqlCommand($"SELECT * FROM TreatmentPlan WHERE EvalId={EvalId} AND SessionId={Id}", cn);
            var reader = cmd.ExecuteReader();

            while (reader.Read())
                treatments.Add(new TreatmentPlan
                {
                    Id = int.Parse(reader["ID"].ToString()),
                    Description= reader["Obs"].ToString(),
                    Objective = reader["Objective"].ToString(),
                    EvalId = int.Parse(reader["EvalId"].ToString()),
                    SessionId = int.Parse(reader["SessionId"].ToString()),
                    ProbId = int.Parse(reader["ProbId"].ToString()),
                });
            
            cn.Close();
            return treatments;
        }

        public ObservableCollection<SessionObservation> GetObservations(SqlConnection cn = null)
        {
            var observations = new ObservableCollection<SessionObservation>();

            cn = cn ?? ClusysUtils.GetConnection();
            var cmd = new SqlCommand($"SELECT * FROM SessionObs WHERE EvalId={EvalId} AND SessionId={Id}", cn);
            var reader = cmd.ExecuteReader();

            while (reader.Read())
                observations.Add(new SessionObservation
                {
                    Id = int.Parse(reader["ID"].ToString()),
                    Description = reader["Obs"].ToString(),
                    DateClosed = reader["DateClosed"].ToString() == "" ? null : (DateTime?)DateTime.Parse(reader["DateClosed"].ToString()),
                    EvalId = int.Parse(reader["EvalId"].ToString()),
                    SessionId = int.Parse(reader["SessionId"].ToString()),
                });
            
            cn.Close();
            return observations;
        }
    }
}
