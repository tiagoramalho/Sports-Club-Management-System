using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CluSys.lib
{
    [Serializable()]
    class EvaluationSession
    {
        public int Id { get; set; }
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

        public static ObservableCollection<BocyChartMark> GetMarks(SqlConnection cn)
        {
            var bodyMarks = new ObservableCollection<BocyChartMark>();
            SqlCommand cmd = new SqlCommand("SELECT * FROM BocyChartMark", cn);
            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
                bodyMarks.Add(new BocyChartMark()
                {
                    ID = int.Parse(reader["ID"].ToString()),
                    x = double.Parse(reader["x"].ToString()),
                    y = double.Parse(reader["y"].ToString()),
                    PainLevel = int.Parse(reader["PainLevel"].ToString()),
                    Obs = reader["Obs"].ToString(),
                    EvalId = int.Parse(reader["EvalId"].ToString()),
                    SessionId = int.Parse(reader["SessionId"].ToString()),
                    ViewId = int.Parse(reader["ViewId"].ToString()),
                });
            
            cn.Close();
            return bodyMarks;




        }
    }
}
