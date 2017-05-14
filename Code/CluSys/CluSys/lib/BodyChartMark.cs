using System;
using System.Data.SqlClient;

namespace CluSys.lib
{
    [Serializable]
    public class BodyChartMark
    {
        public int Id { get; set; } = -1;
        public int EvalId { get; set; }
        public int SessionId { get; set; }
        public int ViewId { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
        public int PainLevel { get; set; }
        public string Obs { get; set; }

        private bool Equals(BodyChartMark other)
        {
            if(Id != -1 && other.Id != -1)
                return Id == other.Id;
            return Math.Abs(X - other.X) < double.Epsilon && Math.Abs(Y - other.Y) < double.Epsilon;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((BodyChartMark) obj);
        }

        public override int GetHashCode()
        {
            if(Id != -1)
                return Id;
            unchecked
            {
                var hash = 17;
                hash = hash * 23 + X.GetHashCode();
                hash = hash * 23 + Y.GetHashCode();
                return hash;
            }
        }

        private void submitBodyChartMark(SqlConnection cn, BodyChartMark BM)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "INSERT INTO BodyChartMark (x, y, PainLevel, Obs, EvalId, SessionId, ViewId) " + "VALUES (@x, @y, @PainLevel, @Obs, @EvalId, @SessionId, @ViewId)";
            cmd.Parameters.AddWithValue("@x", BM.X);
            cmd.Parameters.AddWithValue("@y", BM.Y);
            cmd.Parameters.AddWithValue("@PainLevel", BM.PainLevel);
            cmd.Parameters.AddWithValue("@Obs", BM.Obs);
            cmd.Parameters.AddWithValue("@EvalId", BM.EvalId);
            cmd.Parameters.AddWithValue("@SessionId", BM.SessionId);
            cmd.Parameters.AddWithValue("@ViewId", BM.ViewId);
            cmd.Connection = cn;

            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to Insert BocyChartMark in database. \n ERROR MESSAGE: \n" + ex.Message);
            }
            finally
            {
                cn.Close();
            }

        }


    }
}
