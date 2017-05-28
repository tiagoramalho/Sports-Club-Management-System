using System;
using System.Collections.ObjectModel;
using System.Data.SqlClient;

namespace CluSys.lib
{
    [Serializable]
    internal class MedicalEvaluation
    {
        public int Id { get; set; } = -1;
        public double? Weight { get; set; }
        public double? Height { get; set; }
        public string Story { get; set; }
        public DateTime OpeningDate { get; set; }
        public DateTime? ClosingDate { get; set; }
        public DateTime? ExpectedRecovery { get; set; }
        public string AthleteCC { get; set; }
        public string PhysiotherapistCC { get; set; }

        private readonly ObservableCollection<MedicalEvaluation> _container;
        public int CountId { get { return _container?.IndexOf(this) + 1 ?? Id; } }


        public MedicalEvaluation(ObservableCollection<MedicalEvaluation> container = null) { _container = container; }

        public ObservableCollection<EvaluationSession> Sessions
        {
            get
            {
                using (var conn = ClusysUtils.GetConnection())
                {
                    conn.Open();

                    var sessions = new ObservableCollection<EvaluationSession>();
                    var cmd = new SqlCommand($"SELECT * FROM EvaluationSession WHERE EvalId={Id};", conn);
                    var reader = cmd.ExecuteReader();

                    while (reader.Read())
                        sessions.Add(new EvaluationSession
                        {
                            EvalId = int.Parse(reader["EvalId"].ToString()),
                            Id = int.Parse(reader["Id"].ToString()),
                            Date = DateTime.Parse(reader["Date"].ToString()),
                        });

                    return sessions;
                }
            }
        }

        public ObservableCollection<SessionObservation> GetObservations()
        {
            var obs = new ObservableCollection<SessionObservation>();

            using (var conn = ClusysUtils.GetConnection())
            {
                conn.Open();

                using (var cmd = new SqlCommand($"SELECT * FROM dbo.F_GetOpenObservations ({Id});", conn))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                            obs.Add(new SessionObservation
                            {
                                Id = int.Parse(reader["Id"].ToString()),
                                Obs = reader["Obs"].ToString(),
                                DateClosed = string.IsNullOrEmpty(reader["DateClosed"].ToString()) ? null : (DateTime?) DateTime.Parse(reader["DateClosed"].ToString()),
                                EvalId = int.Parse(reader["EvalId"].ToString()),
                                SessionId = int.Parse(reader["SessionId"].ToString())
                            });
                    }
                }
            }

            return obs;
        }

        private bool Equals(MedicalEvaluation other) { return Id == other.Id; }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((MedicalEvaluation) obj);
        }

        public override int GetHashCode() { return Id; }
    }
}
