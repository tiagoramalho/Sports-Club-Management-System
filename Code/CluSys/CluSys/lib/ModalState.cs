﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace CluSys.lib
{
    class ModalState
    {
        // Basic
        public int? ID { get; set; }

        public double? Weight { get; set; }

        public double? Height { get; set; }
        public DateTime? Date { get; set; } = DateTime.Now;
        public string Story { get; set; }

        // Body chart
        public BodyChartView ActiveView
        {
            get { return Views[ActiveViewIdx]; }
            set { ActiveViewIdx = Views.IndexOf(value); }
        }
        public int ActiveViewIdx { get; set; } = 0;
        public readonly ObservableCollection<BodyChartView> Views = BodyChartViews.GetViews();
        public readonly ObservableCollection<BodyChartMark> Marks = new ObservableCollection<BodyChartMark>();
        public ObservableCollection<Annotation> Annotations { get; set; } = lib.Annotations.GetAnnotations();

        // Problems
        public ObservableCollection<MajorProblem> Problems { get; set; } = new ObservableCollection<MajorProblem>();

        // Treatments
        public ObservableCollection<TreatmentPlan> Treatments { get; set; } = new ObservableCollection<TreatmentPlan>();

        // Observations
        public ObservableCollection<SessionObservation> Observations { get; set; } = new ObservableCollection<SessionObservation>();

        // Others
        public bool MedicalDischarge { get; set; } = false;
        public DateTime? ExpectedRecoveryDate { get; set; }
        
        public void updateHeight(SqlConnection cn)
        {

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "UPDATE MedicalEvaluation SET Height = @Height WHERE ID =" + ID;
            cmd.Parameters.AddWithValue("@Height", Height);
            cmd.Connection = cn;

            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to Update Height in database. \n ERROR MESSAGE: \n" + ex.Message);
            }
            finally
            {
                cn.Close();
            }

        }
        public void updateWeight(SqlConnection cn)
        {

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "UPDATE MedicalEvaluation SET Weightt = @Weightt WHERE ID =" + ID;
            cmd.Parameters.AddWithValue("@Weightt", Weight);
            cmd.Connection = cn;

            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to Update Weightt in database. \n ERROR MESSAGE: \n" + ex.Message);
            }
            finally
            {
                cn.Close();
            }

        }
        public void updateExpectedRecoveryDate(SqlConnection cn)
        {

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "UPDATE MedicalEvaluation SET ExpectedRecovery = @ExpectedRecovery WHERE ID =" + ID;
            cmd.Parameters.AddWithValue("@ExpectedRecovery", ExpectedRecoveryDate);
            cmd.Connection = cn;

            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to Update ExpectedRecovery in database. \n ERROR MESSAGE: \n" + ex.Message);
            }
            finally
            {
                cn.Close();
            }

        }


    }
    
}
