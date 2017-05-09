using System;

public class BodyAnnotation
{
    public int BodyId { get; set; }
    public char AnnotSym { get; set; }

 

    public BodyAnnotation()
	{
	}

    private void submitBodyAnnotation(BodyAnnotation BA)
    {
        if (!verifySGBDConnection())
            return;
        SqlCommand cmd = new SqlCommand();
        cmd.ComandText = "INSERT INTO BodyAnnotation (BodyId, AnnotSym) " + "VALUES (@BodyId, @AnnotSym)";
        cmd.Parameters.AddWithValue("@BodyId", BA.BodyId);
        cmd.Parameters.AddWithValue("@AnnotSym", BA.AnnotSym);
        cmd.Connection = cn;

        try
        {
            cmd.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
            throw new Exception("Failed to Insert BodyAnnotation in database. \n ERROR MESSAGE: \n" + ex.Message);
        }
        finally
        {
            cn.Close();
        }

    }

    private void selectBodyAnnotation()
    {
        if (!verifySGBDConnection())
            return;

        SqlCommand cmd = new SqlCommand("SELECT * FROM BodyAnnotation", cn);
        SqlDataReader reader = cmd.ExecuteReader();
        listBox1.Items.Clear();
        while (reader.Read())
        {
            BodyAnnotation BA = new BodyAnnotation();
            BA.BodyId = reader["BodyId"].ToString();
            BA.AnnotSym = reader["AnnotSym"].ToString();
            listBox1.Items.Add(BA);

        }
        cn.Close();



    }

}
