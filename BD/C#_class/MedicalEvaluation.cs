ausing System;

public class MedicalEvaluation
{
   
    public Double Weightt { get; set; }
    public Double Height { get; set; }
    public String Story { get; set; }
    public DateTime OpeningDate { get; set; }
    public DateTime ClosingDATE { get; set; }
    public DateTime ExpectedRecovery { get; set; }
    public String AthleteCC { get; set; }
    public String PhysiotherapistCC { get; set; }



    public MedicalEvaluation()
	{
	}
    private List<EvaluationSession> Sessions(SqlConnection conn, int EvalId)
    {
        List<EvaluationSession> list = new List<EvaluationSession>();
        SqlCommand cmd = new SqlCommand("SELECT * FROM EvaluationSession WHERE EvalId =" + EvalId + ";", conn);
        SqlDataReader reader = cmd.ExecuteReader();
        while (reader.Read())
            list.Add(new EvaluationSession()
            {
                ID = int.Parse(reader["ID"].ToString()),
                EvalId = int.Parse(reader["EvalId"].ToString()),
                dateSession = DateTime.Parse(reader["dateSession"].ToString()),


            });
        reader.Close();
        return list;

    }
    /*private void submitMedicalEvaluation(MedicalEvaluation ME)
    {
        if (!verifySGBDConnection())
            return;
        SqlCommand cmd = new SqlCommand();
        cmd.ComandText = "INSERT INTO MedicalEvaluation (ID, Weightt, Height, Story, OpeningDate, ClosingDATE, ExpectedRecovery, AthleteCC, PhysiotherapistCC) " + "VALUES (@ID, @Weightt, @Height, @Story, @OpeningDate, @ClosingDATE, @ExpectedRecovery, @AthleteCC, @PhysiotherapistCC)";
        cmd.Parameters.AddWithValue("@ID", ME.ID);
        cmd.Parameters.AddWithValue("@Weightt", ME.Weightt);
        cmd.Parameters.AddWithValue("@Height", ME.Height);
        cmd.Parameters.AddWithValue("@Story", ME.Story);
        cmd.Parameters.AddWithValue("@OpeningDate", ME.OpeningDate);
        cmd.Parameters.AddWithValue("@ClosingDATE", ME.ClosingDATE);
        cmd.Parameters.AddWithValue("@ExpectedRecovery", ME.ExpectedRecovery);
        cmd.Parameters.AddWithValue("@AthleteCC", ME.AthleteCC);
        cmd.Parameters.AddWithValue("@PhysiotherapistCC", ME.PhysiotherapistCC);
        cmd.Connection = cn;

        try
        {
            cmd.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
            throw new Exception("Failed to Insert Medical Evaluation in database. \n ERROR MESSAGE: \n" + ex.Message);
        }
        finally
        {
            cn.Close();
        }

    }


    private void selectMedicalEvaluation()
    {
        if (!verifySGBDConnection())
            return;

        SqlCommand cmd = new SqlCommand("SELECT * FROM MedicalEvaluation", cn);
        SqlDataReader reader = cmd.ExecuteReader();
        listBox1.Items.Clear();
        while (reader.Read())
        {
            MedicalEvaluation ME = new MedicalEvaluation();
            ME.ID = reader["ID"].ToString();
            ME.Weightt = reader["Weightt"].ToString();
            ME.Height = reader["Height"].ToString();
            ME.Story = reader["Story"].ToString();
            ME.OpeningDate = reader["OpeningDate"].ToString();
            ME.ClosingDATEs = reader["ClosingDATE"].ToString();
            ME.ExpectedRecovery = reader["ExpectedRecovery"].ToString();
            ME.AthleteCC = reader["AthleteCC"].ToString();
            ME.PhysiotherapistCC = reader["PhysiotherapistCC"].ToString();
            listBox1.Items.Add(ME);

        }
        cn.Close();



    }*/


}
