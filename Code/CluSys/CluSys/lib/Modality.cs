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
    class Modality
    {
        public String name { get; set; }
        public int recognitionYear { get; set; }

        public ObservableCollection<Athlete> GetAthletes(SqlConnection cn)
        {
            ObservableCollection<Athlete> athletes = new ObservableCollection<Athlete>();

            SqlCommand cmd = new SqlCommand(String.Format("SELECT * FROM Athlete WHERE ModalityId='{0}'", name), cn);
            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
                athletes.Add(new Athlete()
                {
                    CC = reader["CC"].ToString(),
                    Name = reader["Name"].ToString(),
                    Birthdate = DateTime.Parse(reader["Birthdate"].ToString()),
                    Photo = reader["Photo"].ToString(),
                    Phone = reader["Phone"].ToString(),
                    Email = reader["Email"].ToString(),
                    Password = reader["PWD"].ToString(),
                    Job = reader["Job"].ToString(),
                    DominantSide = reader["DominantSide"].ToString(),
                    ModalityId = reader["ModalityId"].ToString(),
                });

            return athletes;
        }
    }

    class Modalities : ObservableCollection<Modality>
    {
        public static Modalities LoadSQL(SqlConnection cn)
        {
            Modalities modalities = new Modalities();

            SqlCommand cmd = new SqlCommand("SELECT * FROM Modality", cn);
            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
                modalities.Add(new Modality()
                {
                    name = reader["Name"].ToString(),
                    recognitionYear = int.Parse(reader["RecognitionYear"].ToString()),
                });

            return modalities;
        }
    }
}
