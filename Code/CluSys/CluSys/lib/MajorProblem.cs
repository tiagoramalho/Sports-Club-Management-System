using System;
using System.Collections.ObjectModel;
using System.Data.SqlClient;

namespace CluSys.lib
{
    class MajorProblem
    {

        public int Id { get; set; } = -1;
        public string Obs { get; set; }
        public int EvalId { get; set; }
        public int SessionId { get; set; }

        public int CountId
        {
            get { return _container?.IndexOf(this) + 1 ?? Id; }
        }

        private readonly ObservableCollection<MajorProblem> _container;

        public MajorProblem(ObservableCollection<MajorProblem> container = null)
        {
            _container = container;
        }

        private void InsertMajorProblem(SqlConnection cn)
        {

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "INSERT INTO MajorProblem (Obs, EvalId, SessionId) " + "VALUES (@Obs, @EvalId, @SessionId)";
            cmd.Parameters.AddWithValue("@Obs", Obs);
            cmd.Parameters.AddWithValue("@EvalId", EvalId);
            cmd.Parameters.AddWithValue("@SessionId", SessionId);
            cmd.Connection = cn;

            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to Insert MajorProblem in database. \n ERROR MESSAGE: \n" + ex.Message);
            }
        }

        private bool Equals(MajorProblem other)
        {
            if (Id != -1 && other.Id != -1)
                return Id == other.Id;
            return string.Equals(Obs, other.Obs, StringComparison.OrdinalIgnoreCase);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((MajorProblem) obj);
        }

        public override int GetHashCode()
        {
            if (Id != -1)
                return Id.GetHashCode();
            return Obs != null ? StringComparer.OrdinalIgnoreCase.GetHashCode(Obs) : 0;
        }
    }
}
