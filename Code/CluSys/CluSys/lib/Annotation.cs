using System;
using System.Collections.ObjectModel;
using System.Data.SqlClient;

namespace CluSys.lib
{
    [Serializable]
    public class Annotation
    {
        public string Symbol { get; set; }
        public string Meaning { get; set; }

        private bool Equals(Annotation other) { return string.Equals(Symbol, other.Symbol, StringComparison.OrdinalIgnoreCase); }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((Annotation) obj);
        }

        public override int GetHashCode() { return Symbol == null ? 0 : StringComparer.OrdinalIgnoreCase.GetHashCode(Symbol); }
    }

    internal static class Annotations
    {
        public static ObservableCollection<Annotation> GetAnnotations()
        {
            using (var cn = ClusysUtils.GetConnection())
            {
                cn.Open();

                var annotations = new ObservableCollection<Annotation>();
                using (var cmd = new SqlCommand("SELECT * FROM CluSys.F_GetAnnotations();", cn))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                            annotations.Add(new Annotation
                            {
                                Symbol = reader["Symbol"].ToString(),
                                Meaning = reader["Meaning"].ToString(),
                            });
                    }
                }

                return annotations;
            }
        }
    }
}
