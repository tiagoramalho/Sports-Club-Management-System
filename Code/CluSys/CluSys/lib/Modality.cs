﻿using System;
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

        private readonly ObservableCollection<Athlete> _athletes = new ObservableCollection<Athlete>();

        public ObservableCollection<Athlete> GetAthletes(SqlConnection cn)
        {
            SqlCommand cmd = new SqlCommand($"SELECT * FROM Athlete WHERE ModalityId='{name}'", cn);
            SqlDataReader reader = cmd.ExecuteReader();

            _athletes.Clear();
            while (reader.Read())
                _athletes.Add(new Athlete()
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
                    ModalityId = reader["ModalityId"].ToString(),
                });

            return _athletes;
        }
    }

    static class Modalities
    {
        public static ObservableCollection<Modality> LoadSQL(SqlConnection cn)
        {
            var modalities = new ObservableCollection<Modality>();

            SqlCommand cmd = new SqlCommand("SELECT * FROM Modality", cn);
            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
                modalities.Add(new Modality()
                {
                    name = reader["Name"].ToString(),
                    recognitionYear = int.Parse(reader["RecognitionYear"].ToString()),
                });

            reader.Close();

            return modalities;
        }
    }
}