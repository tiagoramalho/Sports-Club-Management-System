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

        private double? _height;
        public double Height
        {
            get
            {
                if (_height == null)
                    using (var cn = ClusysUtils.GetConnection())
                    {
                        cn.Open();

                        var cmd = new SqlCommand($"SELECT Height FROM F_GetWeightAndHeight ('{CC}');", cn);
                        var reader = cmd.ExecuteReader();

                        _height = reader.Read() ? double.Parse(reader["Height"].ToString()) : double.NaN;
                    }

                return (double) _height;
            }
            set { _height = value; }
        }

        private double? _weight;
        public double Weight
        {
            get
            {
                if (_weight == null)
                    using (var cn = ClusysUtils.GetConnection())
                    {
                        cn.Open();

                        var cmd = new SqlCommand($"SELECT Weight FROM F_GetWeightAndHeight ('{CC}');", cn);
                        var reader = cmd.ExecuteReader();

                        _weight = reader.Read() ? double.Parse(reader["Weight"].ToString()) : double.NaN;
                    }

                return (double) _weight;
            }

            set { _weight = value; }
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
                        Weight = double.Parse(reader["Weight"].ToString()),
                        Height = double.Parse(reader["Height"].ToString()),
                        Story = reader["Story"].ToString(),
                        OpeningDate = DateTime.Parse(reader["OpeningDate"].ToString()),
                        ClosingDate = reader["ClosingDate"].ToString() == "" ? null : (DateTime?) DateTime.Parse(reader["ClosingDATE"].ToString()),
                        ExpectedRecovery = reader["ExpectedRecovery"].ToString() == "" ? null : (DateTime?) DateTime.Parse(reader["ExpectedRecovery"].ToString()),
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
