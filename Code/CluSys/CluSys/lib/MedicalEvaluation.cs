using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;

namespace CluSys.lib
{
    [Serializable()]
    internal class MedicalEvaluation
    {
        public int Id { private get; set; } = -1;
        public double Weightt { get; set; }
        public double Height { get; set; }
        public string Story { get; set; }
        public DateTime OpeningDate { get; set; }
        public DateTime? ClosingDate { get; set; }
        public DateTime? ExpectedRecovery { get; set; }
        public string AthleteCC { get; set; }
        public string PhysiotherapistCC { get; set; }

        public int CountId
        {
            get { return _container?.IndexOf(this) + 1 ?? Id; }
        }

        private readonly ObservableCollection<MedicalEvaluation> _container;

        public MedicalEvaluation(ObservableCollection<MedicalEvaluation> container = null)
        {
            _container = container;
        }

        public ObservableCollection<EvaluationSession> Sessions(SqlConnection conn)
        {
            var sessions = new ObservableCollection<EvaluationSession>();

            SqlCommand cmd = new SqlCommand($"SELECT * FROM EvaluationSession WHERE EvalId={Id};", conn);
            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
                sessions.Add(new EvaluationSession()
                {
                    Id = int.Parse(reader["ID"].ToString()),
                    EvalId= int.Parse(reader["EvalId"].ToString()),
                    Date = DateTime.Parse(reader["DateSession"].ToString()),
                });

            reader.Close();

            return sessions;
        }

        private bool Equals(MedicalEvaluation other)
        {
            return Id == other.Id;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((MedicalEvaluation) obj);
        }

        public override int GetHashCode()
        {
            return Id;
        }

        private void submitMedicalEvaluation(SqlConnection cn, MedicalEvaluation ME)
        {
           
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "INSERT INTO MedicalEvaluation (Weightt, Height, Story, OpeningDate, ClosingDate, ExpectedRecovery, AthleteCC, PhysiotherapistCC) " + "VALUES (@Weightt, @Height, @Story, @OpeningDate, @ClosingDate, @ExpectedRecovery, @AthleteCC, @PhysiotherapistCC)";
            cmd.Parameters.AddWithValue("@Weightt", ME.Weightt);
            cmd.Parameters.AddWithValue("@Height", ME.Height);
            cmd.Parameters.AddWithValue("@Story", ME.Story);
            cmd.Parameters.AddWithValue("@OpeningDate", ME.OpeningDate);
            cmd.Parameters.AddWithValue("@ClosingDATE", ME.ClosingDate);
            cmd.Parameters.AddWithValue("@ExpectedRecovery", ME.ExpectedRecovery);
            cmd.Parameters.AddWithValue("@AthleteCC", ME.AthleteCC);
            cmd.Parameters.AddWithValue("@PhysiotherapistCC", ME.PhysiotherapistCC);
            cmd.Connection = cn;

            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to Insert Medical Evaluation in database. \n ERROR MESSAGE: \n" + ex.Message);
            }
            finally
            {
                cn.Close();
            }

        }
        
    }
}
