using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CluSys.lib
{
    [Serializable]
    internal class Annotation
    {
        public string Symbol { get; set; }
        public string Meaning { get; set; }

        public bool IsSelected { get; set; } = false;
    }

    internal static class Annotations
    {
        public static ObservableCollection<Annotation> GetAnnotations(SqlConnection cn = null)
        {
            if (cn == null) cn = ClusysUtils.GetConnection();

            var annotations = new ObservableCollection<Annotation>();
            SqlCommand cmd = new SqlCommand("SELECT * FROM Annotation", cn);
            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
                annotations.Add(new Annotation
                {
                    Symbol = reader["Symbol"].ToString(),
                    Meaning = reader["Meaning"].ToString(),
                });

            cn.Close();

            return annotations;
        }
    }
}
