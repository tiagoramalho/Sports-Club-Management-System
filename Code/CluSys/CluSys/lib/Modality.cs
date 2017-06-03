using System;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Globalization;
using System.Windows.Data;

namespace CluSys.lib
{
    [Serializable]
    public class Modality
    {
        public string Name { get; set; }
        public int RecognitionYear { get; set; }

        /* This function is used! It is called using reflection. */
        public ObservableCollection<Athlete> GetAthletes()
        {
            using (var cn = ClusysUtils.GetConnection())
            {
                cn.Open();

                var athletes = new ObservableCollection<Athlete>();
                using (var cmd = new SqlCommand($"SELECT * FROM CluSys.F_GetAthletes ('{Name}');", cn))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
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
                    }
                }

                return athletes;
            }
        }

        public override string ToString() => $"{Name}, {RecognitionYear}";
    }

    internal static class Modalities
    {
        public static ObservableCollection<Modality> GetModalities()
        {
            using (var cn = ClusysUtils.GetConnection())
            {
                cn.Open();

                var modalities = new ObservableCollection<Modality>();
                using (var cmd = new SqlCommand("SELECT * FROM CluSys.F_GetModalities ()", cn))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                            modalities.Add(new Modality
                            {
                                Name = reader["Name"].ToString(),
                                RecognitionYear = int.Parse(reader["RecognitionYear"].ToString()),
                            });
                    }
                }

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

            var methodInfo = value.GetType().GetMethod(methodName);
            return methodInfo == null ? value : methodInfo.Invoke(value, new object[0]);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) { throw new NotSupportedException(); }
    }
}
