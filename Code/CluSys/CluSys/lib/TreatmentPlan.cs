using System;
using System.Data.SqlClient;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace CluSys.lib
{
    internal class TreatmentPlan
    {
        public int Id { get; set; } = -1;
        public string Description { get; set; }
        public string Objective { get; set; }
        public int EvalId { get; set; }
        public int SessionId { get; set; }
        public int ProbId { get; set; }

        public MajorProblem RefProblem { get; set; }

        private void InsertTreatmentPlan(SqlConnection cn)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "INSERT INTO TreatmentPlan (Obs, Objective, EvalId, SessionId, ProbId) " + "VALUES (@Obs, @Objective, @EvalId, @SessionId, @ProbId)";
            cmd.Parameters.AddWithValue("@Obs", Description);
            cmd.Parameters.AddWithValue("@objective", Objective);
            cmd.Parameters.AddWithValue("@EvalId", EvalId);
            cmd.Parameters.AddWithValue("@SessionId", SessionId);
            cmd.Parameters.AddWithValue("@ProbId", ProbId);
            cmd.Connection = cn;

            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to Insert TreatmentPlan in database. \n ERROR MESSAGE: \n" + ex.Message);
            }
        }
    }

    public sealed class RefProblemConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return "Prob.: n/a";

            var id = ((MajorProblem) value).CountId;

            return id > 0 ? $"Prob.: {id}" : "Prob.: n/a";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
