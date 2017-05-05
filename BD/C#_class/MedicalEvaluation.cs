ausing System;

public class MedicalEvaluation
{
    private int ID;
    private Double Weightt;
    private Double Height;
    private String Story;
    private DateTime OpeningDate;
    private DateTime ClosingDATE;
    private DateTime ExpectedRecovery;
    private String AthleteCC;
    private String PhysiotherapistCC;

    public int ID
    {
        get { return ID; }
        set { ID = value; }
    }

    public double Weightt
    {
        get { return Weightt; }
        set { Weightt = value; }
    }

    public double Height
    {
        get { return Height; }
        set { Height = value; }
    }

    public string Story
    {
        get { return Story; }
        set { Story = value; }
    }

    public DateTime OpeningDate
    {
        get { return OpeningDate; }
        set { OpeningDate = value; }
    }

    public DateTime ClosingDATE
    {
        get { return ClosingDATE; }
        set { ClosingDATE = value; }
    }

    public DateTime ExpectedRecovery
    {
        get { return ExpectedRecovery; }
        set { ExpectedRecovery = value; }
    }

    public string AthleteCC
    {
        get { return AthleteCC; }
        set { AthleteCC = value; }
    }

    public string PhysiotherapistCC
    {
        get { return PhysiotherapistCC; }
        set { PhysiotherapistCC = value; }
    }


    public MedicalEvaluation()
	{
	}

    private void submitMedicalEvaluation(MedicalEvaluation ME)
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



    }

}
