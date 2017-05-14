using System;
using System.Data.SqlClient;

public class TreatmentPlan
{
    public int ID { get; set; } = -1;
    public String Obs { get; set; }
    public String Objective { get; set; }
    public int EvalId { get; set; }
    public int SessionId { get; set; }
    public int ProbId { get; set; }


    

    private void submitTreatmentPlan(SqlConnection cn, TreatmentPlan TP)
    {
        
        SqlCommand cmd = new SqlCommand();
        cmd.CommandText = "INSERT INTO TreatmentPlan (Obs, Objective, EvalId, SessionId, ProbId) " + "VALUES (@Obs, @Objective, @EvalId, @SessionId, @ProbId)";
        cmd.Parameters.AddWithValue("@Obs", TP.Obs);
        cmd.Parameters.AddWithValue("@objective", TP.Objective);
        cmd.Parameters.AddWithValue("@EvalId", TP.EvalId);
        cmd.Parameters.AddWithValue("@SessionId", TP.SessionId);
        cmd.Parameters.AddWithValue("@ProbId", TP.ProbId);
        cmd.Connection = cn;

        try
        {
            cmd.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
            throw new Exception("Failed to Insert TreatmentPlan in database. \n ERROR MESSAGE: \n" + ex.Message);
        }
        finally
        {
            cn.Close();
        }

    }

    
}
