using System;
using System.Collections.ObjectModel;
using System.Data.SqlClient;

namespace CluSys.lib
{
    [Serializable]
    internal class Athlete
    {
        public string CC { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public DateTime Birthdate { get; set; }
        public string Photo { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public byte[] Password { get; set; }
        public string Job { get; set; }
        public string DominantSide { get; set; }
        public string ModalityId { get; set; }

        public bool ActiveEvaluation
        {
            get
            {
                using (var cn = ClusysUtils.GetConnection())
                {
                    cn.Open();

                    var cmd = new SqlCommand($"SELECT dbo.F_HasActiveEvaluation ('{CC}') AS Result;", cn);
                    var reader = cmd.ExecuteReader();

                    return reader.Read() && bool.Parse(reader["Result"].ToString());
                }
            }
        }

        public double? Height
        {
            get
            {
                using (var cn = ClusysUtils.GetConnection())
                {
                    cn.Open();

                    var cmd = new SqlCommand($"SELECT * FROM F_GetHeight ('{CC}');", cn);
                    var reader = cmd.ExecuteReader();

                    if (reader.Read()) return double.TryParse(reader["Height"].ToString(), out double height) ? (double?) height : null;
                }

                return null;
            }
        }

        public double? Weight
        {
            get
            {
                using (var cn = ClusysUtils.GetConnection())
                {
                    cn.Open();

                    var cmd = new SqlCommand($"SELECT * FROM F_GetWeight ('{CC}');", cn);
                    var reader = cmd.ExecuteReader();

                    if (reader.Read()) return double.TryParse(reader["Weight"].ToString(), out double weight) ? (double?)weight : null;
                }

                return null;
            }
        }

        public int Age
        {
            get
            {
                var today = DateTime.Today;
                var age = today.Year - Birthdate.Year;
                age -= Birthdate > today.AddYears(-age) ? 1 : 0;
                return age;
            }
        }

        public ObservableCollection<MedicalEvaluation> GetEvaluations()
        {
            using (var cn = ClusysUtils.GetConnection())
            {
                cn.Open();

                var evaluations = new ObservableCollection<MedicalEvaluation>();
                var cmd = new SqlCommand($"SELECT * FROM MedicalEvaluation WHERE AthleteCC='{CC}';", cn);
                var reader = cmd.ExecuteReader();

                while (reader.Read())
                    evaluations.Add(new MedicalEvaluation(evaluations)
                    {
                        Id = int.Parse(reader["Id"].ToString()),
                        Weight = double.TryParse(reader["Weight"].ToString(), out double weight) ? (double?)weight : null,
                        Height = double.TryParse(reader["Height"].ToString(), out double height) ? (double?)height : null,
                        Story = reader["Story"].ToString(),
                        OpeningDate = DateTime.Parse(reader["OpeningDate"].ToString()),
                        ClosingDate = DateTime.TryParse(reader["ClosingDate"].ToString(), out DateTime closingDate) ? (DateTime?)closingDate : null,
                        ExpectedRecovery = DateTime.TryParse(reader["ExpectedRecovery"].ToString(), out DateTime expectedRecovery) ? (DateTime?)expectedRecovery : null,
                        AthleteCC = reader["AthleteCC"].ToString(),
                        PhysiotherapistCC = reader["PhysiotherapistCC"].ToString(),
                    });

                return evaluations;
            }
        }

        private bool Equals(Athlete other) { return string.Equals(CC, other.CC, StringComparison.OrdinalIgnoreCase); }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((Athlete) obj);
        }

        public override int GetHashCode() { return CC == null ? 0 : StringComparer.OrdinalIgnoreCase.GetHashCode(CC); }
    }

    [Serializable]
    internal class Athletes
    {
        public static ObservableCollection<Athlete> AthletesWithOpenEvaluations()
        {
            using (var cn = ClusysUtils.GetConnection())
            {
                cn.Open();

                var athletes = new ObservableCollection<Athlete>();

                var cmd = new SqlCommand("SELECT * FROM F_GetAthletesWithOpenEvaluations ();", cn);
                var reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    athletes.Add(new Athlete
                    {
                        CC = reader["CC"].ToString(),
                        FirstName = reader["FirstName"].ToString(),
                        MiddleName = reader["MiddleName"].ToString(),
                        LastName = reader["LastName"].ToString(),
                        Birthdate = DateTime.Parse(reader["Birthdate"].ToString()),
                        Photo = reader["Photo"].ToString(),
                        Phone = reader["Phone"].ToString(),
                        Email = reader["Email"].ToString(),
                        Password = reader["Password"] as byte[],
                        Job = reader["Job"].ToString(),
                        DominantSide = reader["DominantSide"].ToString(),
                        ModalityId = reader["ModalityId"].ToString()
                    });
                }

                return athletes;
            }
        }
    }
}
