using System;

public class TreatmentPlan
{

    private int ID;
    private String Obs;
    private String Obs;
    private int EvalId;
    private int SessionId;
    private int ProbId;



    public int ID
    {
        get { return ID; }
        set { ID = value; }

    }

    public String Obs
    {
        get { return Obs; }
        set { Obs = value; }

    }

    public String objective
    {
        get { return objective; }
        set { objective = value; }

    }

    public int EvalId
    {
        get { return EvalId; }
        set { EvalId = value; }

    }

    public int SessionId
    {
        get { return SessionId; }
        set { SessionId = value; }

    }

    public int ProbId
    {
        get { return ProbId; }
        set { ProbId = value; }

    }


    public TreatmentPlan()
	{
	}

    private void submitTreatmentPlan(TreatmentPlan TP)
    {
        if (!verifySGBDConnection())
            return;
        SqlCommand cmd = new SqlCommand();
        cmd.ComandText = "INSERT INTO TreatmentPlan (ID, Obs, objective, EvalId, SessionId, ProbId) " + "VALUES (@ID, @Obs, @objective, @EvalId, @SessionId, @ProbId)";
        cmd.Parameters.AddWithValue("@ID", TP.ID);
        cmd.Parameters.AddWithValue("@Obs", TP.Obs);
        cmd.Parameters.AddWithValue("@objective", TP.objective);
        cmd.Parameters.AddWithValue("@EvalId", TP.EvalId);
        cmd.Parameters.AddWithValue("@SessionId", TP.SessionId);
        cmd.Parameters.AddWithValue("@ProbId", TP.ProbId);
        cmd.Connection = cn;

        try
        {
            cmd.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
            throw new Exception("Failed to Insert TreatmentPlan in database. \n ERROR MESSAGE: \n" + ex.Message);
        }
        finally
        {
            cn.Close();
        }

    }

    private void selectTreatmentPlan()
    {
        if (!verifySGBDConnection())
            return;

        SqlCommand cmd = new SqlCommand("SELECT * FROM TreatmentPlan", cn);
        SqlDataReader reader = cmd.ExecuteReader();
        listBox1.Items.Clear();
        while (reader.Read())
        {
            TreatmentPlan TP = new TreatmentPlan();
            TP.ID = reader["ID"].ToString();
            TP.Obs = reader["Obs"].ToString();
            TP.objective = reader["objective"].ToString();
            TP.EvalId = reader["EvalId"].ToString();
            TP.SessionId = reader["SessionId"].ToString();
            TP.ProbId = reader["ProbId"].ToString();
            listBox1.Items.Add(FT);

        }
        cn.Close();



    }
}
