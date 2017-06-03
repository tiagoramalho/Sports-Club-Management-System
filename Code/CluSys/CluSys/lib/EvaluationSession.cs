using System;
using System.Collections.ObjectModel;
using System.Data.SqlClient;

namespace CluSys.lib
{
    [Serializable]
    public class EvaluationSession
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

                    using (var cmd = new SqlCommand($"SELECT CluSys.F_GetNumberOfProblems ({EvalId}, {Id}) AS NumberOfProblems;", conn))
                    {
                        using (var reader = cmd.ExecuteReader())
                        {
                            return reader.Read() ? int.Parse(reader["NumberOfProblems"].ToString()) : 0;
                        }
                    }
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

                    using (var cmd = new SqlCommand($"SELECT CluSys.F_GetNumberOfTreatments ({EvalId}, {Id}) AS NumberOfTreatments;", conn))
                    {
                        using (var reader = cmd.ExecuteReader())
                        {
                            return reader.Read() ? int.Parse(reader["NumberOfTreatments"].ToString()) : 0;
                        }
                    }
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

                MedicalEvaluation evaluation;
                using (var cmd = new SqlCommand($"SELECT * FROM CluSys.F_GetEvaluation ({EvalId});", cn))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (!reader.Read())
                            return null;

                        evaluation = new MedicalEvaluation
                        {
                            Id = int.Parse(reader["Id"].ToString()),
                            Weight = double.TryParse(reader["Weight"].ToString(), out double weight) ? (double?) weight : null,
                            Height = double.TryParse(reader["Height"].ToString(), out double height) ? (double?) height : null,
                            Story = reader["Story"].ToString(),
                            OpeningDate = DateTime.Parse(reader["OpeningDate"].ToString()),
                            ClosingDate = DateTime.TryParse(reader["ClosingDate"].ToString(), out DateTime closingDate) ? (DateTime?) closingDate : null,
                            ExpectedRecovery = DateTime.TryParse(reader["ExpectedRecovery"].ToString(), out DateTime expectedRecovery) ? (DateTime?) expectedRecovery : null,
                            AthleteCC = reader["AthleteCC"].ToString(),
                            PhysiotherapistCC = reader["PhysiotherapistCC"].ToString()
                        };
                    }
                }

                return evaluation;
            }
        }

        public ObservableCollection<BodyChartMark> GetMarks()
        {
            using (var cn = ClusysUtils.GetConnection())
            {
                cn.Open();

                var bodyMarks = new ObservableCollection<BodyChartMark>();
                using (var cmd = new SqlCommand($"SELECT * FROM CluSys.F_GetBodyChartMarks ({EvalId}, {Id})", cn))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                            bodyMarks.Add(new BodyChartMark(int.Parse(reader["Id"].ToString()))
                            {
                                X = double.Parse(reader["X"].ToString()),
                                Y = double.Parse(reader["Y"].ToString()),
                                PainLevel = int.Parse(reader["PainLevel"].ToString()),
                                Description = reader["Description"].ToString(),
                                EvalId = int.Parse(reader["EvalId"].ToString()),
                                SessionId = int.Parse(reader["SessionId"].ToString()),
                                ViewId = int.Parse(reader["ViewId"].ToString()),
                            });
                    }
                }

                return bodyMarks;
            }
        }

        public ObservableCollection<MajorProblem> GetProblems()
        {
            using (var cn = ClusysUtils.GetConnection())
            {
                cn.Open();

                var problems = new ObservableCollection<MajorProblem>();
                using (var cmd = new SqlCommand($"SELECT * FROM CluSys.F_GetProblems ({EvalId}, {Id});", cn))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                            problems.Add(new MajorProblem
                            {
                                Id = int.Parse(reader["Id"].ToString()),
                                Description = reader["Description"].ToString(),
                                EvalId = int.Parse(reader["EvalId"].ToString()),
                                SessionId = int.Parse(reader["SessionId"].ToString()),
                            });
                    }
                }

                return problems;
            }
        }

        public ObservableCollection<TreatmentPlan> GetTreatments()
        {
            using (var cn = ClusysUtils.GetConnection())
            {
                cn.Open();

                var treatments = new ObservableCollection<TreatmentPlan>();
                using (var cmd = new SqlCommand($"SELECT * FROM CluSys.F_GetTreatments ({EvalId}, {Id});", cn))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                            treatments.Add(new TreatmentPlan
                            {
                                Id = int.Parse(reader["Id"].ToString()),
                                Description = reader["Description"].ToString(),
                                Objective = reader["Objective"].ToString(),
                                EvalId = int.Parse(reader["EvalId"].ToString()),
                                SessionId = int.Parse(reader["SessionId"].ToString()),
                                ProbId = int.TryParse(reader["ProbId"].ToString(), out int probId) ? (int?)probId : null,
                            });
                    }
                }

                return treatments;
            }
        }

        /*
        public ObservableCollection<SessionObservation> GetObservations()
        {
            using (var cn = ClusysUtils.GetConnection())
            {
                cn.Open();

                var observations = new ObservableCollection<SessionObservation>();
                using (var cmd = new SqlCommand($"SELECT * FROM SessionObservation WHERE EvalId={EvalId} AND SessionId={Id}", cn))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                            observations.Add(new SessionObservation
                            {
                                Id = int.Parse(reader["Id"].ToString()),
                                Obs = reader["Description"].ToString(),
                                DateClosed = reader["DateClosed"].ToString() == "" ? null : (DateTime?) DateTime.Parse(reader["DateClosed"].ToString()),
                                EvalId = int.Parse(reader["EvalId"].ToString()),
                                SessionId = int.Parse(reader["SessionId"].ToString()),
                            });
                    }
                }

                return observations;
            }           
        }
        */
    }
}
