using System;

public class FunctionalTestSet
{
    public String Name { get; set; }
    public String Obs { get; set; }

    

    public FunctionalTestSet()
	{
	}

    private void submitFunctionalTestSet(FunctionalTestSet FT)
    {
        if (!verifySGBDConnection())
            return;
        SqlCommand cmd = new SqlCommand();
        cmd.ComandText = "INSERT INTO FunctionalTestSet (Name, Obs) " + "VALUES (@Name, @Obs)";
        cmd.Parameters.AddWithValue("@Name", FT.Name);
        cmd.Parameters.AddWithValue("@Obs", FT.Obs);
        cmd.Connection = cn;

        try
        {
            cmd.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
            throw new Exception("Failed to Insert FunctionalTestSet in database. \n ERROR MESSAGE: \n" + ex.Message);
        }
        finally
        {
            cn.Close();
        }

    }

    private void selectFunctionalTestSet()
    {
        if (!verifySGBDConnection())
            return;

        SqlCommand cmd = new SqlCommand("SELECT * FROM FunctionalTestSet", cn);
        SqlDataReader reader = cmd.ExecuteReader();
        listBox1.Items.Clear();
        while (reader.Read())
        {
            FunctionalTestSet FT = new FunctionalTestSet();
            FT.Name = reader["Name"].ToString();
            FT.Obs = reader["Obs"].ToString();
            listBox1.Items.Add(FT);

        }
        cn.Close();



    }
}
