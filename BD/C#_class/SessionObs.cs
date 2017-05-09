using System;

public class SessionObs
{
    public String Obs { get; set; }
    public DateTime DateClosed { get; set; }
    public int EvalId { get; set; }
    public int SessionId { get; set; }

    

    public SessionObs()
	{
	}

    private void submitSessionObs(SessionObs SO)
    {
        if (!verifySGBDConnection())
            return;
        SqlCommand cmd = new SqlCommand();
        cmd.ComandText = "INSERT INTO SessionObs (ID, Obs, DateClosed, EvalId, SessionId) " + "VALUES (@ID, @Obs, @DateClosed, @EvalId, @SessionId)";
        cmd.Parameters.AddWithValue("@ID", SO.ID);
        cmd.Parameters.AddWithValue("@Obs", SO.Obs);
        cmd.Parameters.AddWithValue("@DateClosed", SO.DateClosed);
        cmd.Parameters.AddWithValue("@EvalId", SO.EvalId);
        cmd.Parameters.AddWithValue("@SessionId", SO.SessionId);
        cmd.Connection = cn;

        try
        {
            cmd.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
            throw new Exception("Failed to Insert SessionObs in database. \n ERROR MESSAGE: \n" + ex.Message);
        }
        finally
        {
            cn.Close();
        }

    }

    private void selectSessionObs()
    {
        if (!verifySGBDConnection())
            return;

        SqlCommand cmd = new SqlCommand("SELECT * FROM SessionObs", cn);
        SqlDataReader reader = cmd.ExecuteReader();
        listBox1.Items.Clear();
        while (reader.Read())
        {
            SessionObs SO = new SessionObs();
            SO.ID = reader["ID"].ToString();
            SO.Obs = reader["Obs"].ToString();
            SO.DateClosed = reader["DateClosed"].ToString();
            SO.EvalId = reader["EvalId"].ToString();
            SO.SessionId = reader["SessionId"].ToString();
            listBox1.Items.Add(SO);

        }
        cn.Close();



    }
}
