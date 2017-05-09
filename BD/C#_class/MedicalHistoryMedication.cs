using System;

public class MedicalHistoryMedication
{
    public int MHId { get; set; }
    public String Medication { get; set; }

   

    public MedicalHistoryMedication()
	{

	}

    private void submitMedicalHistoryMedication(MedicalHistoryMedication MM)
    {
        if (!verifySGBDConnection())
            return;
        SqlCommand cmd = new SqlCommand();
        cmd.ComandText = "INSERT INTO MedicalHistoryMedication (MHId, Medication)" + "VALUES (@MHId, @Medication)";
        cmd.Parameters.AddWithValue("@MHId", MM.MHId);
        cmd.Parameters.AddWithValue("@Medication", MM.Medication);
        cmd.Connection = cn;

        try
        {
            cmd.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
            throw new Exception("Failed to Insert Medical History Medication in database. \n ERROR MESSAGE: \n" + ex.Message);
        }
        finally
        {
            cn.Close();
        }

    }

    private void selectMedicaHistoryMedication()
    {
        if (!verifySGBDConnection())
            return;

        SqlCommand cmd = new SqlCommand("SELECT * FROM MedicalHistoryMedication", cn);
        SqlDataReader reader = cmd.ExecuteReader();
        listBox1.Items.Clear();
        while (reader.Read())
        {
            MedicalHistoryMedication MM = new MedicalHistoryMedication();
            MM.MHId = reader["MHId"].ToString();
            MM.Medication = reader["Medication"].ToString();
            listBox1.Items.Add(MM);

        }
        cn.Close();

    }
}
