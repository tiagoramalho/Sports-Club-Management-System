using System;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Globalization;
using System.Windows.Data;

namespace CluSys.lib
{
    [Serializable]
    internal class Modality
    {
        public string Name { get; set; }
        public int RecognitionYear { get; set; }

        public ObservableCollection<Athlete> GetAthletes()
        {
            using (var cn = ClusysUtils.GetConnection())
            {
                cn.Open();

                var athletes = new ObservableCollection<Athlete>();
                var cmd = new SqlCommand($"SELECT * FROM Athlete WHERE ModalityId='{Name}'", cn);
                var reader = cmd.ExecuteReader();

                while (reader.Read())
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
                        ModalityId = reader["ModalityId"].ToString(),
                    });

                return athletes;
            }
        }
    }

    internal static class Modalities
    {
        public static ObservableCollection<Modality> GetModalities()
        {
            using (var cn = ClusysUtils.GetConnection())
            {
                cn.Open();

                var modalities = new ObservableCollection<Modality>();
                var cmd = new SqlCommand("SELECT * FROM Modality", cn);
                var reader = cmd.ExecuteReader();

                while (reader.Read())
                    modalities.Add(new Modality
                    {
                        Name = reader["Name"].ToString(),
                        RecognitionYear = int.Parse(reader["RecognitionYear"].ToString()),
                    });

                return modalities;
            }
        }
    }

    public sealed class ModalityDotGetAthletesConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var methodName = parameter as string;

            if (value == null || methodName == null)
                return value;

            var methodInfo = value.GetType().GetMethod(methodName, new Type[1] { typeof(SqlConnection) });

            return methodInfo == null ? value : methodInfo.Invoke(value, new object[1] { ClusysUtils.GetConnection() } );
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) { throw new NotSupportedException(); }
    }
}
