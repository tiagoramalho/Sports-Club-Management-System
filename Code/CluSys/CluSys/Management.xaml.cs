using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace CluSys
{
    /// <summary>
    /// Interaction logic for Management.xaml
    /// </summary>
    public partial class Management 
    {
        public Management()
        {
            InitializeComponent();
        }
        public void InsertModality(object sender, RoutedEventArgs e){
            using (var cmd = new SqlCommand("CluSys.P_AddModality", cn) { CommandType = CommandType.StoredProcedure })
            {
                cmd.Parameters.Add(new SqlParameter("@Name", SqlDbType.NVarChar));
                cmd.Parameters.Add(new SqlParameter("@Year", SqlDbType.SMALLINT));
                foreach (var prob in Problems)
                {
                    cmd.Parameters["@Name"].Value = this.ModalityName.Text;
                    cmd.Parameters["@Year"].Value = this.RecognitionYear.Text;
                    cmd.ExecuteNonQuery();
                }
            }
        }
        public void InsertClass(object sender, RoutedEventArgs e){
            using (var cmd = new SqlCommand("CluSys.P_AddClass", cn) { CommandType = CommandType.StoredProcedure })
            {
                cmd.Parameters.Add(new SqlParameter("@ModalityId", SqlDbType.NVarChar));
                cmd.Parameters.Add(new SqlParameter("@Name", SqlDbType.NVarChar));
                 cmd.Parameters.Add(new SqlParameter("@InitialAge", SqlDbType.TINYINT));
                cmd.Parameters.Add(new SqlParameter("@FinalAge", SqlDbType.TINYINT));
                foreach (var prob in Problems)
                {
                    cmd.Parameters["@ModalityId"].Value = this.ModalityID.Text;
                    cmd.Parameters["@Name"].Value = this.ClassName.Text;
                    cmd.Parameters["@InitialAge"].Value =  this.InitialAge.Text;
                    cmd.Parameters["@FinalAge"].Value = this.FinalAge.Text;
                    cmd.ExecuteNonQuery();
                }
            }
        }

    }
}
