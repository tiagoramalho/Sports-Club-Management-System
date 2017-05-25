using System;
using System.Collections.ObjectModel;
using System.Data.SqlClient;

namespace CluSys.lib
{
    [Serializable]
    internal class BodyChartView
    {
        public int Id { get; set; }
        public string Image { get; set; }
        public int Order { get; set; }

        private bool Equals(BodyChartView other) { return Id == other.Id; }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((BodyChartView) obj);
        }

        public override int GetHashCode() { return Id; }
    }

    internal static class BodyChartViews
    {
        public static ObservableCollection<BodyChartView> GetViews()
        {
            using (var cn = ClusysUtils.GetConnection())
            {
                cn.Open();

                var bodyViews = new ObservableCollection<BodyChartView>();
                var cmd = new SqlCommand("SELECT * FROM BodyChartView", cn);
                var reader = cmd.ExecuteReader();

                while (reader.Read())
                    bodyViews.Add(new BodyChartView
                    {
                        Id = int.Parse(reader["Id"].ToString()),
                        Image = reader["Image"].ToString(),
                        Order = int.Parse(reader["Order"].ToString()),
                    });

                return bodyViews;
            }
        }
    }
}
