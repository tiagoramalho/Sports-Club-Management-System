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
        public DateTime? ExpectedRecoveryDate { get; set; }
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

        public ObservableCollection<EvaluationSession> Sessions
        {
            get
            {
                var sessions = new ObservableCollection<EvaluationSession>();

                SqlConnection conn = ClusysUtils.GetConnection();
                SqlCommand cmd = new SqlCommand($"SELECT * FROM EvaluationSession WHERE EvalId={Id};", conn);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                    sessions.Add(new EvaluationSession()
                    {
                        Id = int.Parse(reader["ID"].ToString()),
                        EvalId = int.Parse(reader["EvalId"].ToString()),
                        Date = DateTime.Parse(reader["DateSession"].ToString()),
                    });

                reader.Close();

                return sessions;
            }
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

        /*
        public void InsertMedicalEvaluation(SqlConnection cn)
        {
           
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "INSERT INTO MedicalEvaluation (Weightt, Height, Story, OpeningDate, ClosingDate, ExpectedRecovery, AthleteCC, PhysiotherapistCC) " + "VALUES (@Weightt, @Height, @Story, @OpeningDate, @ClosingDate, @ExpectedRecovery, @AthleteCC, @PhysiotherapistCC)";
            cmd.Parameters.AddWithValue("@Weightt", Weightt);
            cmd.Parameters.AddWithValue("@Height", Height);
            cmd.Parameters.AddWithValue("@Story", Story);
            cmd.Parameters.AddWithValue("@OpeningDate", OpeningDate);
            cmd.Parameters.AddWithValue("@ClosingDATE", ClosingDate);
            cmd.Parameters.AddWithValue("@ExpectedRecovery", ExpectedRecoveryDate);
            cmd.Parameters.AddWithValue("@AthleteCC", AthleteCC);
            cmd.Parameters.AddWithValue("@PhysiotherapistCC", PhysiotherapistCC);
            cmd.Connection = cn;

            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to Insert Medical Evaluation in database. \n ERROR MESSAGE: \n" + ex.Message);
            }

        }
        */
        
    }
}
