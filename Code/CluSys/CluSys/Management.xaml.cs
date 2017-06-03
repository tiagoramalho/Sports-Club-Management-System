using CluSys.lib;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.CompilerServices;
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
using CluSys.Annotations;

namespace CluSys
{
    /// <summary>
    /// Interaction logic for Management.xaml
    /// </summary>
    public sealed partial class Management  : INotifyPropertyChanged
    {
        public ObservableCollection<Modality> CollectionOfModalities => Modalities.GetModalities();

        public Management()
        {
            InitializeComponent();
        }

        private void InsertModality(object sender, RoutedEventArgs e){
            using (var cn = ClusysUtils.GetConnection())
            {
                cn.Open();
                using (var cmd = new SqlCommand("CluSys.P_AddModality", cn) { CommandType = CommandType.StoredProcedure })
                {
                    cmd.Parameters.Add(new SqlParameter("@Name", SqlDbType.NVarChar));
                    cmd.Parameters.Add(new SqlParameter("@Year", SqlDbType.SmallInt));
                    cmd.Parameters["@Name"].Value = ModalityName.Text;
                    cmd.Parameters["@Year"].Value = RecognitionYear.Text;
                    cmd.ExecuteNonQuery();
                }
            }

            OnPropertyChanged(nameof(CollectionOfModalities));
            Console.WriteLine(ModalityName.Text);
        }

        private void InsertClass(object sender, RoutedEventArgs e){
            using (var cn = ClusysUtils.GetConnection())
            {
                cn.Open();
                using (var cmd = new SqlCommand("CluSys.P_AddClass", cn) { CommandType = CommandType.StoredProcedure })
                {
                    cmd.Parameters.Add(new SqlParameter("@ModalityId", SqlDbType.NVarChar));
                    cmd.Parameters.Add(new SqlParameter("@Name", SqlDbType.NVarChar));
                    cmd.Parameters.Add(new SqlParameter("@InitialAge", SqlDbType.TinyInt));
                    cmd.Parameters.Add(new SqlParameter("@FinalAge", SqlDbType.TinyInt));
                    cmd.Parameters["@ModalityId"].Value = ModalityID.Text;
                    cmd.Parameters["@Name"].Value = ClassName.Text;
                    cmd.Parameters["@InitialAge"].Value = InitialAge.Text;
                    cmd.Parameters["@FinalAge"].Value = FinalAge.Text;
                    cmd.ExecuteNonQuery();
                }
            }
        }

        private void InsertAthlete(object sender, RoutedEventArgs e){
            using (var cn = ClusysUtils.GetConnection())
            {
                cn.Open();
                using (var cmd = new SqlCommand("CluSys.P_AddClass", cn) { CommandType = CommandType.StoredProcedure })
                {
                    cmd.Parameters.Add(new SqlParameter("@ModalityId", SqlDbType.NVarChar));
                    cmd.Parameters.Add(new SqlParameter("@Name", SqlDbType.NVarChar));
                    cmd.Parameters.Add(new SqlParameter("@InitialAge", SqlDbType.TinyInt));
                    cmd.Parameters.Add(new SqlParameter("@FinalAge", SqlDbType.TinyInt));
                    cmd.Parameters["@ModalityId"].Value = ModalityID.Text;
                    cmd.Parameters["@Name"].Value = ClassName.Text;
                    cmd.Parameters["@InitialAge"].Value = InitialAge.Text;
                    cmd.Parameters["@FinalAge"].Value = FinalAge.Text;
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
