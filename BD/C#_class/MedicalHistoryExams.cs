using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;

public class MedicalHistoryExams
{
    public int MHId { get; set; }
    public String Exam { get; set; }



    public MedicalHistoryExams()
	{

	}

    private void submitMedicalHistoryExams(MedicalHistoryExams ME)
    {
        if (!verifySGBDConnection())
            return;
        SqlCommand cmd = new SqlCommand();
        cmd.ComandText = "INSERT INTO MedicalHistoryExams (MHId, Exam)" + "VALUES (@MHId, @Exam)";
        cmd.Parameters.AddWithValue("@MHId", ME.MHId);
        cmd.Parameters.AddWithValue("@Exam", ME.Exam);
        cmd.Connection = cn;

        try
        {
            cmd.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
            throw new Exception("Failed to Insert Medical History Exam in database. \n ERROR MESSAGE: \n" + ex.Message);
        }
        finally
        {
            cn.Close();
        }

    }

    private void selectMedicaHistoryExam()
    {
        if (!verifySGBDConnection())
            return;

        SqlCommand cmd = new SqlCommand("SELECT * FROM MedicalHistoryExams", cn);
        SqlDataReader reader = cmd.ExecuteReader();
        listBox1.Items.Clear();
        while (reader.Read())
        {
            MedicalHistoryExams ME = new MedicalHistoryExams();
            ME.MHId = reader["MHId"].ToString();
            ME.Exam = reader["Exam"].ToString();
            listBox1.Items.Add(ME);

        }
        cn.Close();

    }


}
