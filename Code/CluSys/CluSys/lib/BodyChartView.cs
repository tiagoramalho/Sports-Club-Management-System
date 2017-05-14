using System;
using System.Collections.ObjectModel;
using System.Data.SqlClient;

namespace CluSys.lib
{
    [Serializable]
    internal class BodyChartView
    {
        public int Id { get; set; }
        public string ImageBody { get; set; }
        public int OrderImage { get; set; }

        private bool Equals(BodyChartView other)
        {
            return Id == other.Id;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((BodyChartView) obj);
        }

        public override int GetHashCode()
        {
            return Id;
        }
    }

    static class BodyChartViews
    {
        public static ObservableCollection<BodyChartView> GetViews(SqlConnection cn = null)
        {
            if (cn == null) cn = ClusysUtils.GetConnection();

            var bodyViews = new ObservableCollection<BodyChartView>();
            SqlCommand cmd = new SqlCommand("SELECT * FROM BodyChartView", cn);
            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
                bodyViews.Add(new BodyChartView
                {
                    Id = int.Parse(reader["ID"].ToString()),
                    ImageBody = reader["ImageBody"].ToString(),
                    OrderImage = int.Parse(reader["OrderImage"].ToString()),
                });

            cn.Close();
            return bodyViews;
        }
    }
}
