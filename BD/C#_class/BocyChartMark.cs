using System;

public class BocyChartMark
{
    public double x { get; set; }
    public double y { get; set; }
    public int PainLevel { get; set; }
    public String Obs { get; set; }
    public int EvalId { get; set; }
    public int SessionId { get; set; }
    public int ViewId { get; set; }

    
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
