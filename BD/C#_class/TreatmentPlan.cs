using System;

public class TreatmentPlan
{

    public String Obs { get; set; }
    public String Obs { get; set; }
    public int EvalId { get; set; }
    public int SessionId { get; set; }
    public int ProbId { get; set; }



   


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
