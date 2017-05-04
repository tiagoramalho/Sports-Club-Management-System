using System;




[Serializable()]
public class MedicalHistory
{
    private int ID;
    private String Obs;
    private DateTime dateM;
    private String AthleteCC;
    private String PhysiotherapistCC;

    public int ID {
        get { return ID; }
        set { ID = value; }

    }

    public String Obs{
        get { return Obs;}
        set { Obs = value; }

    }
    public DateTime dateM{
        get { return dateM; } 
        set { dateM = value; }

    }
    public String AthleteCC
    {
        get { return AthleteCC; }
        set { AthleteCC = value; }

    }
    public String PhysiotherapistCC
    {
        get { return PhysiotherapistCC; }
        set { PhysiotherapistCC = value; }

    }


    public MedicalHistory()
	{


	}

    //Query deve estar noutro documento, por agora aqui para estar tudo organizado

    private void submitMedicalHistory(MedicalHistory M)
    {
        if (!verifySGBDConnection())
            return;
        SqlCommand cmd = new SqlCommand();
        cmd.ComandText = "INSERT INTO MedicalHistory (ID, Obs, dateM, AthleteCC, PhysiotherapistCC) " + "VALUES (@ID, @Obs, @dateM, @AthleteCC, @PhysiotherapistCC)";
        cmd.Parameters.AddWithValue("@ID", M.ID);
        cmd.Parameters.AddWithValue("@Obs", M.Obs);
        cmd.Parameters.AddWithValue("@dateM", M.dateM);
        cmd.Parameters.AddWithValue("@AthleteCC", M.AthleteCC);
        cmd.Parameters.AddWithValue("@PhysiotherapistCC", M.PhysiotherapistCC);
        cmd.Connection = cn;

        try
        {
            cmd.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
            throw new Exception("Failed to Insert MedicalHistory in database. \n ERROR MESSAGE: \n" + ex.Message);
        }
        finally
        {
            cn.Close();
        }

    }

    private void selectMedicaHistory()
    {
        if (!verifySGBDConnection())
            return;

        SqlCommand cmd = new SqlCommand("SELECT * FROM MedicalHistory", cn);
        SqlDataReader reader = cmd.ExecuteReader();
        listBox1.Items.Clear();
        while (reader.Read())
        {
            MedicalHistory M = new MedicalHistory();
            M.ID = reader["ID"].ToString();
            M.Obs = reader["Obs"].ToString();
            M.dateM = reader["dateM"].ToString();
            M.AthleteCC = reader["AthleteCC"].ToString();
            M.PhysiotherapistCC = reader["PhysiotherapistCC"].ToString();
            listBox1.Items.Add(M);

        }
        cn.Close();



    }

}


