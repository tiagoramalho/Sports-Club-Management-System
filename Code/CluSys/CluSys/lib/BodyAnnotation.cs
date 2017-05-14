using System;
using System.Data.SqlClient;

public class BodyAnnotation
{
    public int BodyId { get; set; }
    public char AnnotSym { get; set; }

    private void submitBodyAnnotation(SqlConnection cn, BodyAnnotation BA)
    {
        
        SqlCommand cmd = new SqlCommand();
        cmd.CommandText = "INSERT INTO BodyAnnotation (BodyId, AnnotSym) " + "VALUES (@BodyId, @AnnotSym)";
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

   

}
