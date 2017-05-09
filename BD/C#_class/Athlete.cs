using System;

public class Athlete
{
    public string CC { get; set; }
    public string FirstName { get; set; }
    public string MiddleName { get; set; }
    public string LastName { get; set; }
    public DateTime Birthdate { get; set; }
    public string Photo { get; set; }
    public string Phone { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string Job { get; set; }
    public string DominantSide { get; set; }
    public string ModalityId { get; set; }
}
private Tuple<double, double, bool> BodyInformation(SqlConnection conn, string CC)
{


    SqlCommand cmd = new SqlCommand("SELECT Weightt, Height, ClosingDATE FROM (SELECT Weightt, Height, OpeningDate, ClosingDATE FROM MedicalEvaluation WHERE AthleteCC = " + CC + ")  AS T WHERE OpeningDate >= all(SELECT OpeningDate FROM MedicalEvaluation WHERE AthleteCC = " + CC + ");", conn);
    SqlDataReader reader = cmd.ExecuteReader();
    MedicalEvaluation ME = new MedicalEvaluation();
    reader.Read();
    ME.Weightt = Double.Parse(reader["Weightt"].ToString());
    ME.Height = Double.Parse(reader["Height"].ToString());
    if (reader["ClosingDATE"].ToString() == "")
    {
        return Tuple.Create(ME.Weightt, ME.Height, false);
    }
    return Tuple.Create(ME.Weightt, ME.Height, true);
}

private List<Athlete> Fovourites(SqlConnection conn)
{
    List<Athlete> list = new List<Athlete>();
    SqlCommand cmd = new SqlCommand("SELECT * FROM Athlete WHERE CC in (SELECT AthleteCC FROM MedicalEvaluation WHERE ClosingDate is NULL);", conn);
    SqlDataReader reader = cmd.ExecuteReader();
    while (reader.Read())
    {
        Athlete A = new Athlete();
        A.CC = reader["CC"].ToString();
        A.FirstName = reader["FirstName"].ToString();
        A.MiddleName = reader["MiddleName"].ToString();
        A.LastName = reader["LastName"].ToString();
        A.Birthdate = DateTime.Parse(reader["Birthdate"].ToString());
        A.Photo = reader["Photo"].ToString();
        A.Phone = reader["Phone"].ToString();
        A.Email = reader["Email"].ToString();
        A.Password = reader["PWD"].ToString();
        A.Job = reader["Job"].ToString();
        A.DominantSide = reader["DominantSide"].ToString();
        A.ModalityId = reader["ModalityId"].ToString();

        list.Add(A);

    }
    return list;
}
private List<MedicalEvaluation> Evaluations(SqlConnection conn, string CC)
{
    List<MedicalEvaluation> list = new List<MedicalEvaluation>();
    SqlCommand cmd = new SqlCommand("SELECT * FROM MedicalEvaluation WHERE AthleteCC=" + CC + ";", conn);
    SqlDataReader reader = cmd.ExecuteReader();
    while (reader.Read())
        list.Add(new MedicalEvaluation()
        {
            ID = int.Parse(reader["ID"].ToString()),
            Weightt = double.Parse(reader["Weightt"].ToString()),
            Height = double.Parse(reader["Height"].ToString()),
            Story = reader["Story"].ToString(),
            OpeningDate = DateTime.Parse(reader["OpeningDate"].ToString()),
            ClosingDATE = reader["ClosingDATE"].ToString() != "" ? (DateTime?)DateTime.Parse(reader["ClosingDATE"].ToString()) : null,
            ExpectedRecovery = reader["ExpectedRecovery"].ToString() != "" ? (DateTime?)DateTime.Parse(reader["ExpectedRecovery"].ToString()) : null,
            AthleteCC = reader["AthleteCC"].ToString(),
            PhysiotherapistCC = reader["PhysiotherapistCC"].ToString(),

        });
    return list;


}
