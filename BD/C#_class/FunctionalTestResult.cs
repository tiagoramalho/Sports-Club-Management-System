using System;

public class FunctionalTestResult
{
    private int ID;
    private String Result;
    private int EvalId;
    private int SessionId;
    private String TestName;

    public int ID
    {
        get { return ID; }
        set { ID = value; }

    }

    public String Result
    {
        get { return Result; }
        set { Result = value; }

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

    public String TestName
    {
        get { return TestName; }
        set { TestName = value; }

    }

    public FunctionalTestResult()
	{
	}

    private void submitFunctionalTestResult(FunctionalTestResult FR)
    {
        if (!verifySGBDConnection())
            return;
        SqlCommand cmd = new SqlCommand();
        cmd.ComandText = "INSERT INTO FunctionalTestResult (ID, Result, EvalId, SessionId, TestName) " + "VALUES (@ID, @Result, @EvalId, @SessionId, @TestName)";
        cmd.Parameters.AddWithValue("@ID", FR.ID);
        cmd.Parameters.AddWithValue("@Result", FR.Result);
        cmd.Parameters.AddWithValue("@EvalId", FR.EvalId);
        cmd.Parameters.AddWithValue("@SessionId", FR.SessionId);
        cmd.Parameters.AddWithValue("@TestName", FR.TestName);
        cmd.Connection = cn;

        try
        {
            cmd.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
            throw new Exception("Failed to Insert FunctionalTestResult in database. \n ERROR MESSAGE: \n" + ex.Message);
        }
        finally
        {
            cn.Close();
        }

    }

    private void selectFunctionalTestResult()
    {
        if (!verifySGBDConnection())
            return;

        SqlCommand cmd = new SqlCommand("SELECT * FROM FunctionalTestResult", cn);
        SqlDataReader reader = cmd.ExecuteReader();
        listBox1.Items.Clear();
        while (reader.Read())
        {
            FunctionalTestResult FR = new FunctionalTestResult();
            FR.ID = reader["ID"].ToString();
            FR.Result = reader["Result"].ToString();
            FR.EvalId = reader["EvalId"].ToString();
            FR.SessionId = reader["SessionId"].ToString();
            FR.TestName = reader["TestName"].ToString();
            listBox1.Items.Add(FR);

        }
        cn.Close();



    }







}
