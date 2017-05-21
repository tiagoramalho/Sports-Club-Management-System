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
    internal class SessionObservation
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public DateTime? DateClosed { get; set; }
        public int EvalId { get; set; }
        public int SessionId { get; set; }

        /*
        private void InsertSessionObs(SqlConnection cn)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "INSERT INTO SessionObs (ID, Obs, DateClosed, EvalId, SessionId) " +
                              "VALUES (@ID, @Obs, @DateClosed, @EvalId, @SessionId)";
            cmd.Parameters.AddWithValue("@ID", Id);
            cmd.Parameters.AddWithValue("@Obs", Description);
            cmd.Parameters.AddWithValue("@DateClosed", DateClosed);
            cmd.Parameters.AddWithValue("@EvalId", EvalId);
            cmd.Parameters.AddWithValue("@SessionId", SessionId);
            cmd.Connection = cn;

            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to Insert SessionObs in database. \n ERROR MESSAGE: \n" + ex.Message);
            }
        }
        */
    }
}
