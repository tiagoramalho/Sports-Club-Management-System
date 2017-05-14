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

        public static ObservableCollection<BodyChartMark> GetMarks(SqlConnection cn)
        {
            var bodyMarks = new ObservableCollection<BodyChartMark>();
            SqlCommand cmd = new SqlCommand("SELECT * FROM BocyChartMark", cn);
            SqlDataReader reader = cmd.ExecuteReader();

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
    }
}
