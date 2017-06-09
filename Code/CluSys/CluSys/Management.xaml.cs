using CluSys.lib;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using CluSys.Annotations;
using System.Security.Cryptography;
using System.Runtime.InteropServices;

namespace CluSys
{
    /// <summary>
    /// Interaction logic for Management.xaml
    /// </summary>
    public sealed partial class Management : INotifyPropertyChanged
    {
        public ObservableCollection<Modality> CollectionOfModalities => Modalities.GetModalities();
        public ObservableCollection<AtheleteInfo> CollectionOfAthletes => AtheleteInfo.GetAthleteInfo();
        public Management()
        {
            InitializeComponent();
        }

        [ComVisible(true)]
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
            ModalityName.Text = string.Empty;
            RecognitionYear.Text = string.Empty;
        }

        private void InsertClass(object sender, RoutedEventArgs e)
        {
            using (var cn = ClusysUtils.GetConnection())
            {
                cn.Open();
                using (var cmd = new SqlCommand("CluSys.P_AddClass", cn) { CommandType = CommandType.StoredProcedure })
                {
                    cmd.Parameters.Add(new SqlParameter("@ModalityId", SqlDbType.NVarChar));
                    cmd.Parameters.Add(new SqlParameter("@Name", SqlDbType.NVarChar));
                    cmd.Parameters.Add(new SqlParameter("@InitialAge", SqlDbType.TinyInt));
                    cmd.Parameters.Add(new SqlParameter("@FinalAge", SqlDbType.TinyInt));
                    cmd.Parameters["@ModalityId"].Value = ((Modality)ModalityID.SelectedItem).Name;
                    cmd.Parameters["@Name"].Value = ClassName.Text;
                    cmd.Parameters["@InitialAge"].Value = int.Parse(InitialAge.Text);
                    cmd.Parameters["@FinalAge"].Value = int.Parse(FinalAge.Text);
                    cmd.ExecuteNonQuery();
                }
            }
            ModalityID.SelectedIndex = -1;
            ClassName.Text = string.Empty;
            InitialAge.Text = string.Empty;
            FinalAge.Text = string.Empty;
        }

        private void InsertAthlete(object sender, RoutedEventArgs e)
        {
            Console.WriteLine(DominantSide.SelectedValue);
            using (var cn = ClusysUtils.GetConnection())
            {
                cn.Open();
                using (var cmd = new SqlCommand("CluSys.P_AddAthlete", cn) { CommandType = CommandType.StoredProcedure })
                {
                    cmd.Parameters.Add(new SqlParameter("@CC", SqlDbType.NChar));
                    cmd.Parameters.Add(new SqlParameter("@FirstName", SqlDbType.NVarChar));
                    cmd.Parameters.Add(new SqlParameter("@MiddleName", SqlDbType.NVarChar));
                    cmd.Parameters.Add(new SqlParameter("@LastName", SqlDbType.NVarChar));
                    cmd.Parameters.Add(new SqlParameter("@Birthdate", SqlDbType.Date));
                    cmd.Parameters.Add(new SqlParameter("@Photo", SqlDbType.NVarChar));
                    cmd.Parameters.Add(new SqlParameter("@Phone", SqlDbType.NChar));
                    cmd.Parameters.Add(new SqlParameter("@Email", SqlDbType.NVarChar));
                    cmd.Parameters.Add(new SqlParameter("@Password", SqlDbType.Binary));
                    cmd.Parameters.Add(new SqlParameter("@Job", SqlDbType.NVarChar));
                    cmd.Parameters.Add(new SqlParameter("@DominantSide", SqlDbType.NVarChar));
                    cmd.Parameters.Add(new SqlParameter("@ModalityId", SqlDbType.NVarChar));
                    cmd.Parameters["@CC"].Value = CC.Text;
                    cmd.Parameters["@FirstName"].Value = F_Name.Text;
                    cmd.Parameters["@MiddleName"].Value = M_Name.Text;
                    cmd.Parameters["@LastName"].Value = L_Name.Text;
                    cmd.Parameters["@Birthdate"].Value = BirthDate.SelectedDate;
                    cmd.Parameters["@Photo"].Value = Photo.Text;
                    cmd.Parameters["@Phone"].Value = Phone.Text;
                    cmd.Parameters["@Email"].Value = Mail.Text;
                    cmd.Parameters["@Job"].Value = Job.Text;
                    cmd.Parameters["@DominantSide"].Value = (DominantSide.SelectedItem as ComboBoxItem)?.Content;
                    cmd.Parameters["@ModalityId"].Value = (Modality.SelectedItem as Modality)?.Name;
                    using (var shaM = new SHA512Managed())
                        cmd.Parameters["@Password"].Value = shaM.ComputeHash(Encoding.UTF8.GetBytes(Password.Password));
                    cmd.ExecuteNonQuery();

                }
            }
            Console.WriteLine("passou aqui");
            OnPropertyChanged(nameof(CollectionOfAthletes));
            CC.Text = string.Empty;
            F_Name.Text = string.Empty;
            L_Name.Text = string.Empty;
            M_Name.Text = string.Empty;
            BirthDate.SelectedDate = null;
            Photo.Text = string.Empty;
            Phone.Text = string.Empty;
            Mail.Text = string.Empty;
            Job.Text= string.Empty;
            DominantSide.SelectedIndex = -1;
            Password.Password = string.Empty;
            Modality.SelectedIndex = -1;
        }


        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


    }

    public class AtheleteInfo
    {
        public string CC { get; set; }
        public string Name { get; set; }
        public DateTime Birthdate { get; set; }
        public int Age { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string DominantSide { get; set; }
        public string ModalityName { get; set; }
        public string ClassName { get; set; }
        public int? EvalId { get; set; }
        public DateTime? ExpectedRecovery { get; set; }
        public string PhysiotherapistName { get; set; }



        public static ObservableCollection<AtheleteInfo> GetAthleteInfo()
        {

            using (var cn = ClusysUtils.GetConnection())
            {
                cn.Open();
                var listInfo = new ObservableCollection<AtheleteInfo>();
                using (var cmd = new SqlCommand("SELECT * FROM CluSys.F_GetAthletesInfo();", cn))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                            listInfo.Add(new AtheleteInfo
                            {
                                CC = reader["CC"].ToString(),
                                Name = reader["Name"].ToString(),
                                Birthdate = DateTime.Parse(reader["Birthdate"].ToString()),
                                Age = int.Parse(reader["Age"].ToString()),
                                Phone = reader["Phone"].ToString(),
                                Email = reader["Email"].ToString(),
                                DominantSide = reader["DominantSide"].ToString(),
                                ModalityName = reader["ModalityName"].ToString(),
                                ClassName = reader["ClassName"].ToString(),
                                EvalId = reader["EvaluationId"].ToString() != "" ? (int?)int.Parse(reader["EvaluationId"].ToString()) : null,
                                ExpectedRecovery = reader["ExpectedRecovery"].ToString() != "" ? (DateTime?)DateTime.Parse(reader["ExpectedRecovery"].ToString()) : null,
                                PhysiotherapistName = reader["PhysiotherapistName"].ToString()
                            });
                    }
                }
            return listInfo;
            }
        }
    }
}


