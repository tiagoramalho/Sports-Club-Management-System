using System;

public class BocyChartMark
{
    private int ID;
    private double x;
    private double y;
    private int PainLevel;
    private String Obs;
    private int EvalId;
    private int SessionId;
    private int ViewId;

    public int ID
    {
        get { return ID; }
        set { ID = value; }

    }

    public double x
    {
        get { return x; }
        set { x = value; }

    }

    public double y
    {
        get { return y; }
        set { y = value; }

    }

    public int PainLevel
    {
        get { return PainLevel; }
        set { PainLevel = value; }

    }

    public String Obs
    {
        get { return Obs; }
        set { Obs = value; }

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

    public int ViewId
    {
        get { return ViewId; }
        set { ViewId = value; }

    }

  
    public BocyChartMark()
	{
	}

    private void submitBocyChartMark(BocyChartMark BM)
    {
        if (!verifySGBDConnection())
            return;
        SqlCommand cmd = new SqlCommand();
        cmd.ComandText = "INSERT INTO BocyChartMark (ID, x, y, PainLevel, Obs, EvalId, SessionId, ViewId) " + "VALUES (@ID, @x, @y, @PainLevel, @Obs, @EvalId, @SessionId, @ViewId)";
        cmd.Parameters.AddWithValue("@ID", BM.ID);
        cmd.Parameters.AddWithValue("@x", BM.x);
        cmd.Parameters.AddWithValue("@y", BM.y);
        cmd.Parameters.AddWithValue("@PainLevel", BM.PainLevel);
        cmd.Parameters.AddWithValue("@Obs", BM.Obs);
        cmd.Parameters.AddWithValue("@EvalId", BM.EvalId);
        cmd.Parameters.AddWithValue("@SessionId", BM.SessionId);
        cmd.Parameters.AddWithValue("@ViewId", BM.ViewId);
        cmd.Connection = cn;

        try
        {
            cmd.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
            throw new Exception("Failed to Insert BocyChartMark in database. \n ERROR MESSAGE: \n" + ex.Message);
        }
        finally
        {
            cn.Close();
        }

    }

    private void selectBocyChartMark()
    {
        if (!verifySGBDConnection())
            return;

        SqlCommand cmd = new SqlCommand("SELECT * FROM BocyChartMark", cn);
        SqlDataReader reader = cmd.ExecuteReader();
        listBox1.Items.Clear();
        while (reader.Read())
        {
            BocyChartMark BM = new BocyChartMark();
            BM.ID = reader["ID"].ToString();
            BM.x = reader["x"].ToString();
            BM.y = reader["y"].ToString();
            BM.PainLevel = reader["PainLevel"].ToString();
            BM.Obs = reader["Obs"].ToString();
            BM.EvalId = reader["EvalId"].ToString();
            BM.SessionId = reader["SessionId"].ToString();
            BM.ViewId = reader["ViewId"].ToString();
            listBox1.Items.Add(BM);

        }
        cn.Close();



    }
}
