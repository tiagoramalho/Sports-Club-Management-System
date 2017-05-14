using System;
using System.Data.SqlClient;

public class MajorProblem
{

    public int ID { get; set; } = -1;
    public String Obs { get; set; }
    public int EvalId { get; set; }
    public int SessionId { get; set; }

    
    private void submitMajorProblem(SqlConnection cn, MajorProblem MP)
    {
        
        SqlCommand cmd = new SqlCommand();
        cmd.CommandText = "INSERT INTO MajorProblem (Obs, EvalId, SessionId) " + "VALUES (@Obs, @EvalId, @SessionId)";
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

    
}
