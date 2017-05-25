using System;
using System.Collections.ObjectModel;
using System.Data.SqlClient;

namespace CluSys.lib
{
    [Serializable]
    internal class EvaluationSession
    {
        public int Id { get; set; } = -1;
        public int EvalId { get; set; }
        public DateTime Date { get; set; }

        public int NumberOfProblems
        {
            get
            {
                using (var conn = ClusysUtils.GetConnection())
                {
                    conn.Open();

                    var cmd = new SqlCommand($"SELECT dbo.GetNumberOfProblems ({EvalId}, {Id}) AS NumberOfProblems;", conn);
                    var reader = cmd.ExecuteReader();
                    return reader.Read() ? int.Parse(reader["NumberOfProblems"].ToString()) : 0;
                }
            }
        }

        public int NumberOfTreatments
        {
            get
            {
                using (var conn = ClusysUtils.GetConnection())
                {
                    conn.Open();

                    var cmd = new SqlCommand($"SELECT dbo.GetNumberOfTreatments ({EvalId}, {Id}) AS NumberOfTreatments;", conn);
                    var reader = cmd.ExecuteReader();
                    return reader.Read() ? int.Parse(reader["NumberOfTreatments"].ToString()) : 0;
                }
            }
        }

        private bool Equals(EvaluationSession other) { return Id == other.Id && EvalId == other.EvalId; }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((EvaluationSession) obj);
        }

        public override int GetHashCode() { unchecked { return (Id * 397) ^ EvalId; } }

        public MedicalEvaluation GetMedicalEvaluation()
        {
            using (var cn = ClusysUtils.GetConnection())
            {
                cn.Open();

                var cmd = new SqlCommand($"SELECT * FROM MedicalEvaluation WHERE Id={EvalId}", cn);
                var reader = cmd.ExecuteReader();

                if (!reader.Read())
                    return null;

                var evaluation = new MedicalEvaluation
                {
                    Id = int.Parse(reader["Id"].ToString()),
                    Weight = double.Parse(reader["Weight"].ToString()),
                    Height = double.Parse(reader["Height"].ToString()),
                    Story = reader["Story"].ToString(),
                    OpeningDate = DateTime.Parse(reader["OpeningDate"].ToString()),
                    ClosingDate = reader["ClosingDate"].ToString() == "" ? null : (DateTime?) DateTime.Parse(reader["ClosingDATE"].ToString()),
                    ExpectedRecovery = reader["ExpectedRecovery"].ToString() == "" ? null : (DateTime?) DateTime.Parse(reader["ExpectedRecovery"].ToString()),
                    AthleteCC = reader["AthleteCC"].ToString(),
                    PhysiotherapistCC = reader["PhysiotherapistCC"].ToString(),
                };

                return evaluation;
            }
        }

        public ObservableCollection<BodyChartMark> GetMarks()
        {
            using (var cn = ClusysUtils.GetConnection())
            {
                cn.Open();

                var bodyMarks = new ObservableCollection<BodyChartMark>();
                var cmd = new SqlCommand($"SELECT * FROM BodyChartMark WHERE EvalId={EvalId} AND SessionId={Id}", cn);
                var reader = cmd.ExecuteReader();

                while (reader.Read())
                    bodyMarks.Add(new BodyChartMark
                    {
                        Id = int.Parse(reader["Id"].ToString()),
                        X = double.Parse(reader["x"].ToString()),
                        Y = double.Parse(reader["y"].ToString()),
                        PainLevel = int.Parse(reader["PainLevel"].ToString()),
                        Description = reader["Description"].ToString(),
                        EvalId = int.Parse(reader["EvalId"].ToString()),
                        SessionId = int.Parse(reader["SessionId"].ToString()),
                        ViewId = int.Parse(reader["ViewId"].ToString()),
                    });

                return bodyMarks;
            }
        }

        public ObservableCollection<MajorProblem> GetProblems()
        {
            using (var cn = ClusysUtils.GetConnection())
            {
                cn.Open();

                var problems = new ObservableCollection<MajorProblem>();
                var cmd = new SqlCommand($"SELECT * FROM MajorProblem WHERE EvalId={EvalId} AND SessionId={Id}", cn);
                var reader = cmd.ExecuteReader();

                while (reader.Read())
                    problems.Add(new MajorProblem
                    {
                        Id = int.Parse(reader["Id"].ToString()),
                        Description = reader["Description"].ToString(),
                        EvalId = int.Parse(reader["EvalId"].ToString()),
                        SessionId = int.Parse(reader["SessionId"].ToString()),
                    });

                return problems;
            }
        }

        public ObservableCollection<TreatmentPlan> GetTreatments()
        {
            using (var cn = ClusysUtils.GetConnection())
            {
                cn.Open();

                var treatments = new ObservableCollection<TreatmentPlan>();
                var cmd = new SqlCommand($"SELECT * FROM TreatmentPlan WHERE EvalId={EvalId} AND SessionId={Id}", cn);
                var reader = cmd.ExecuteReader();

                while (reader.Read())
                    treatments.Add(new TreatmentPlan
                    {
                        Id = int.Parse(reader["Id"].ToString()),
                        Description = reader["Description"].ToString(),
                        Objective = reader["Objective"].ToString(),
                        EvalId = int.Parse(reader["EvalId"].ToString()),
                        SessionId = int.Parse(reader["SessionId"].ToString()),
                        ProbId = int.Parse(reader["ProbId"].ToString()),
                    });

                cn.Close();
                return treatments;
            }
        }

        public ObservableCollection<SessionObservation> GetObservations()
        {
            using (var cn = ClusysUtils.GetConnection())
            {
                cn.Open();

                var observations = new ObservableCollection<SessionObservation>();
                var cmd = new SqlCommand($"SELECT * FROM SessionObs WHERE EvalId={EvalId} AND SessionId={Id}", cn);
                var reader = cmd.ExecuteReader();

                while (reader.Read())
                    observations.Add(new SessionObservation
                    {
                        Id = int.Parse(reader["Id"].ToString()),
                        Obs = reader["Description"].ToString(),
                        DateClosed = reader["DateClosed"].ToString() == "" ? null : (DateTime?) DateTime.Parse(reader["DateClosed"].ToString()),
                        EvalId = int.Parse(reader["EvalId"].ToString()),
                        SessionId = int.Parse(reader["SessionId"].ToString()),
                    });

                return observations;
            }
        }
    }
}
