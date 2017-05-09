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
    class Athlete
    {
        public string CC { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public DateTime Birthdate { get; set; }
        public string Photo { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Job { get; set; }
        public string DominantSide { get; set; }
        public string ModalityId { get; set; }

        public ObservableCollection<MedicalEvaluation> evaluations = null;

        public ObservableCollection<MedicalEvaluation> Evaluations(SqlConnection conn)
        {
            if(evaluations == null)
                evaluations = new ObservableCollection<MedicalEvaluation>();
            else
                evaluations.Clear();

            SqlCommand cmd = new SqlCommand("SELECT * FROM MedicalEvaluation WHERE AthleteCC=" + CC + ";", conn);
            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
                evaluations.Add(new MedicalEvaluation()
                {
                    ID = int.Parse(reader["ID"].ToString()),
                    Weightt = double.Parse(reader["Weightt"].ToString()),
                    Height = double.Parse(reader["Height"].ToString()),
                    Story = reader["Story"].ToString(),
                    OpeningDate = DateTime.Parse(reader["OpeningDate"].ToString()),
                    ClosingDATE = reader["ClosingDATE"].ToString() != "" ? (DateTime?)DateTime.Parse(reader["ClosingDATE"].ToString()) : null,
                    ExpectedRecovery = reader["ExpectedRecovery"].ToString() != "" ? (DateTime?)DateTime.Parse(reader["ExpectedRecovery"].ToString()) : null,
                    AthleteCC = reader["AthleteCC"].ToString(),
                    PhysiotherapistCC = reader["PhysiotherapistCC"].ToString(),
                });

            reader.Close();

            return evaluations;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            return CC == (obj as Athlete).CC;
        }

        public override int GetHashCode()
        {
            return int.Parse(CC);
        }
    }

    [Serializable()]
    class AthleteWithBody : Athlete
    {
        public double Height { get; set; }
        public double Weight { get; set; }
        public bool activeEvaluation { get; set; }

        public AthleteWithBody() { }

        public AthleteWithBody(SqlConnection conn, Athlete ath)
        {
            CC = ath.CC;
            FirstName = ath.FirstName;
            MiddleName = ath.MiddleName;
            LastName = ath.LastName;
            Birthdate = ath.Birthdate;
            Photo = ath.Photo;
            Phone = ath.Phone;
            Email = ath.Email;
            Password = ath.Password;
            Job = ath.Job;
            DominantSide = ath.DominantSide;
            ModalityId = ath.ModalityId;

            SqlCommand cmd = new SqlCommand("SELECT Weightt, Height, ClosingDATE FROM (SELECT Weightt, Height, OpeningDate, ClosingDATE FROM MedicalEvaluation WHERE AthleteCC = " + CC + ")  AS T WHERE OpeningDate >= all(SELECT OpeningDate FROM MedicalEvaluation WHERE AthleteCC = " + CC + ");", conn);
            SqlDataReader reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                Weight = Double.Parse(reader["Weightt"].ToString());
                Height = Double.Parse(reader["Height"].ToString());
                activeEvaluation = reader["ClosingDATE"].ToString() == "";
            }
            else
            {
                Weight = Double.NaN;
                Height = Double.NaN;
                activeEvaluation = false;
            }
            reader.Close();
        }
    }

    [Serializable()]
    class Athletes
    {
        public static ObservableCollection<Athlete> OpenEvaluations(SqlConnection conn)
        {
            var athletes = new ObservableCollection<Athlete>();

            SqlCommand cmd = new SqlCommand("SELECT * FROM Athlete WHERE CC in (SELECT AthleteCC FROM MedicalEvaluation WHERE ClosingDate is NULL);", conn);
            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                athletes.Add(new Athlete()
                {
                    CC = reader["CC"].ToString(),
                    FirstName = reader["FirstName"].ToString(),
                    MiddleName = reader["MiddleName"].ToString(),
                    LastName = reader["LastName"].ToString(),
                    Birthdate = DateTime.Parse(reader["Birthdate"].ToString()),
                    Photo = reader["Photo"].ToString(),
                    Phone = reader["Phone"].ToString(),
                    Email = reader["Email"].ToString(),
                    Password = reader["PWD"].ToString(),
                    Job = reader["Job"].ToString(),
                    DominantSide = reader["DominantSide"].ToString(),
                    ModalityId = reader["ModalityId"].ToString()
                });
            }

            reader.Close();

            return athletes;
        }
    }
}
