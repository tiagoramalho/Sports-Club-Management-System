using System;
using System.Collections.ObjectModel;
using System.Data.SqlClient;

namespace CluSys.lib
{
    [Serializable()]
    class BodyChartView
    {
        public int ID { get; set; }
        public String ImageBody { get; set; }
        public int OrderImage { get; set; }



    }

    static class BodyChartViews
    {
        public static ObservableCollection<BodyChartView> GetViews(SqlConnection cn)
        {
            var bodyViews = new ObservableCollection<BodyChartView>();
            SqlCommand cmd = new SqlCommand("SELECT * FROM BodyChartView", cn);
            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
                bodyViews.Add(new BodyChartView()
                {
                    ID = int.Parse(reader["ID"].ToString()),
                    ImageBody = reader["ImageBody"].ToString(),
                    OrderImage = int.Parse(reader["OrderImage"].ToString()),
                });
            cn.Close();
            return bodyViews;
        }

    }
}

