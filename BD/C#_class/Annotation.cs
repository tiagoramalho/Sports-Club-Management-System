using System;

public class Annotation
{
    public Char Symbol { get; set; }
    public String Meaning { get; set; }

    

    public Annotation()
	{
	}

    private void submitAnnotation(Annotation A)
    {
        if (!verifySGBDConnection())
            return;
        SqlCommand cmd = new SqlCommand();
        cmd.ComandText = "INSERT INTO Annotation (Symbol, Meaning) " + "VALUES (@Symbol, @Meaning)";
        cmd.Parameters.AddWithValue("@Symbol", A.Symbol);
        cmd.Parameters.AddWithValue("@Meaning", A.Meaning);
        cmd.Connection = cn;

        try
        {
            cmd.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
            throw new Exception("Failed to Insert Annotation in database. \n ERROR MESSAGE: \n" + ex.Message);
        }
        finally
        {
            cn.Close();
        }

    }

    private void selectAnnotation()
    {
        if (!verifySGBDConnection())
            return;

        SqlCommand cmd = new SqlCommand("SELECT * FROM Annotation", cn);
        SqlDataReader reader = cmd.ExecuteReader();
        listBox1.Items.Clear();
        while (reader.Read())
        {
            Annotation A = new Annotation();
            A.Symbol = reader["Symbol"].ToString();
            A.Meaning = reader["Meaning"].ToString();
            listBox1.Items.Add(A);

        }
        cn.Close();



    }
}
