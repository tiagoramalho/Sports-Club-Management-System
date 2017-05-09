using System;

public class MajorProblem
{

   
    public String Obs { get; set; }
    public int EvalId { get; set; }
    public int SessionId { get; set; }


    


    public MajorProblem()
	{
	}

    private void submitMajorProblem(MajorProblem MP)
    {
        if (!verifySGBDConnection())
            return;
        SqlCommand cmd = new SqlCommand();
        cmd.ComandText = "INSERT INTO MajorProblem (ID, Obs, EvalId, SessionId) " + "VALUES (@ID, @Obs, @EvalId, @SessionId)";
        cmd.Parameters.AddWithValue("@ID", MP.ID);
        cmd.Parameters.AddWithValue("@Obs", MP.Obs);
        cmd.Parameters.AddWithValue("@EvalId", MP.EvalId);
        cmd.Parameters.AddWithValue("@SessionId", MP.SessionId);
        cmd.Connection = cn;

        try
        {
            cmd.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
            throw new Exception("Failed to Insert MajorProblem in database. \n ERROR MESSAGE: \n" + ex.Message);
        }
        finally
        {
            cn.Close();
        }

    }

    private void selectMajorProblem()
    {
        if (!verifySGBDConnection())
            return;

        SqlCommand cmd = new SqlCommand("SELECT * FROM MajorProblem", cn);
        SqlDataReader reader = cmd.ExecuteReader();
        listBox1.Items.Clear();
        while (reader.Read())
        {
            MajorProblem MP = new MajorProblem();
            MP.ID = reader["ID"].ToString();
            MP.Obs = reader["Obs"].ToString();
            MP.EvalId = reader["EvalId"].ToString();
            MP.SessionId = reader["SessionId"].ToString();
            listBox1.Items.Add(MP);

        }
        cn.Close();



    }
}
