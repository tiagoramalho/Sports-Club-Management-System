using System;

public class EvaluationSession
{
    public int EvalId { get; set; }
    public int ID { get; set; }
    public DateTime dateSession { get; set; }
    

    public EvaluationSession()
	{
	}

    private void submitEvaluationSession(EvaluationSession ES)
    {
        if (!verifySGBDConnection())
            return;
        SqlCommand cmd = new SqlCommand();
        cmd.ComandText = "INSERT INTO EvaluationSession (EvalId, ID, dateSession) " + "VALUES (@EvalId, @ID, @dateSession)";
        cmd.Parameters.AddWithValue("@EvalId", ES.EvalId);
        cmd.Parameters.AddWithValue("@ID", ES.ID);
        cmd.Parameters.AddWithValue("@dateSession", ES.dateSession);
        cmd.Connection = cn;

        try
        {
            cmd.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
            throw new Exception("Failed to Insert Evaluation Session in database. \n ERROR MESSAGE: \n" + ex.Message);
        }
        finally
        {
            cn.Close();
        }

    }

    private void selectEvaluationSession()
    {
        if (!verifySGBDConnection())
            return;

        SqlCommand cmd = new SqlCommand("SELECT * FROM EvaluationSession", cn);
        SqlDataReader reader = cmd.ExecuteReader();
        listBox1.Items.Clear();
        while (reader.Read())
        {
            EvaluationSession ES = new EvaluationSession();
            ES.EvalId = reader["EvalId"].ToString();
            ES.ID = reader["ID"].ToString();
            ES.dateSession = reader["dateSession"].ToString();
            listBox1.Items.Add(ES);

        }
        cn.Close();



    }
}
