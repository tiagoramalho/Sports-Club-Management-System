using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace CluSys.lib
{
    internal class ModalState
    {
        private const string PhysiotherapistCC = "12123";

        public Athlete Athlete { get; set; }
        public MedicalEvaluation Evaluation { get; set; }
        public EvaluationSession Session { get; set; }

        // Body chart
        public int ActiveViewIdx { get; set; }
        public readonly ObservableCollection<BodyChartView> Views;
        public BodyChartView ActiveView {
            get => Views[ActiveViewIdx];
            set => ActiveViewIdx = Views.IndexOf(value);
        }

        public readonly ObservableCollection<BodyChartMark> Marks;
        public readonly ObservableCollection<BodyChartMark> DeletedMarks;
        public ObservableCollection<Annotation> Annotations { get; }

        // Problems
        public ObservableCollection<MajorProblem> Problems { get; set; }
        public readonly ObservableCollection<MajorProblem> DeletedProblems;

        // Treatments
        public ObservableCollection<TreatmentPlan> Treatments { get; set; }
        public readonly ObservableCollection<TreatmentPlan> DeletedTreatments;

        // Observations
        public ObservableCollection<SessionObservation> Observations { get; set; }
        public readonly ObservableCollection<SessionObservation> DeletedObservations;

        // Others
        public bool MedicalDischarge { get; set; }

        public bool CanBeEdited { get; set; }

        public ModalState(Athlete athlete, MedicalEvaluation evaluation = null, EvaluationSession session = null)
        {
            Athlete = athlete;
            Evaluation = evaluation ?? new MedicalEvaluation();
            Session = session ?? new EvaluationSession { Date = DateTime.Now };

            ActiveViewIdx = 0;
            Views = BodyChartViews.GetViews();
            Annotations = lib.Annotations.GetAnnotations();

            DeletedMarks = new ObservableCollection<BodyChartMark>();
            DeletedProblems = new ObservableCollection<MajorProblem>();
            DeletedTreatments = new ObservableCollection<TreatmentPlan>();
            DeletedObservations = new ObservableCollection<SessionObservation>();

            if (evaluation != null && session != null)
            {
                Marks = session.GetMarks();
                Problems = session.GetProblems(); foreach (var prob in Problems) prob.Container = Problems;
                Treatments = session.GetTreatments(); foreach (var treatment in Treatments) treatment.RefProblem = Problems.FirstOrDefault(prob => prob.Id == treatment.ProbId);
                Observations = evaluation.GetObservations();
                MedicalDischarge = evaluation.ClosingDate == session.Date;
                CanBeEdited = false;
            }
            else
            {
                Marks = new ObservableCollection<BodyChartMark>();
                Problems = new ObservableCollection<MajorProblem>();
                Treatments = new ObservableCollection<TreatmentPlan>();
                Observations = new ObservableCollection<SessionObservation>();
                MedicalDischarge = false;
                CanBeEdited = true;
            }
        }

        public void Save()
        {
            Session.Date = Session.Date.Date.Add(DateTime.Now.TimeOfDay);

            using (var cn = ClusysUtils.GetConnection())
            {
                cn.Open();

                int saveIdx = -1;
                SqlTransaction transaction = null;

                try
                {
                    using (transaction = cn.BeginTransaction())
                    {
                        saveIdx = 0;
                        transaction.Save($"Save{saveIdx}"); saveIdx += 1;

                        GetEvalId(cn, transaction);
                        GetSessionId(cn, transaction);
                        UpdateEvaluation(cn, transaction);
                        transaction.Save($"Save{saveIdx}"); saveIdx += 1;

                        SaveMarks(cn, transaction);
                        transaction.Save($"Save{saveIdx}"); saveIdx += 1;

                        SaveProblems(cn, transaction);
                        transaction.Save($"Save{saveIdx}"); saveIdx += 1;

                        SaveTreatments(cn, transaction);
                        transaction.Save($"Save{saveIdx}"); saveIdx += 1;

                        SaveObservations(cn, transaction);
                        transaction.Save($"Save{saveIdx}");

                        transaction.Commit();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Commit Exception Type: {0}", ex.GetType());
                    Console.WriteLine("  Message: {0}", ex.Message);

                    // Attempt to roll back the transaction.
                    try
                    {
                        if (saveIdx <= 0) transaction?.Rollback();
                        else transaction?.Rollback($"Save{saveIdx-1}");
                    }
                    catch (Exception ex2)
                    {
                        // This catch block will handle any errors that may have occurred
                        // on the server that would cause the rollback to fail, such as
                        // a closed connection.
                        Console.WriteLine("Rollback Exception Type: {0}", ex2.GetType());
                        Console.WriteLine("  Message: {0}", ex2.Message);
                    }
                }
            }

            DeletedMarks.Clear();
            DeletedProblems.Clear();
            DeletedTreatments.Clear();
            DeletedObservations.Clear();
        }

        private void GetEvalId(SqlConnection cn, SqlTransaction transaction)
        {
            using (var cmd = new SqlCommand("CluSys.P_GetOrCreateEvaluation", cn, transaction) {CommandType = CommandType.StoredProcedure})
            {
                cmd.Parameters.Add(new SqlParameter("@AthleteCC", Athlete.CC));
                cmd.Parameters.Add(new SqlParameter("@PhysiotherapistCC", PhysiotherapistCC));
                cmd.Parameters.Add(new SqlParameter("@OpeningDate", Session.Date));
                cmd.Parameters.Add(new SqlParameter("@EvalId", SqlDbType.Int) { Direction = ParameterDirection.Output });
                cmd.ExecuteNonQuery();

                Evaluation.Id = Session.EvalId = Convert.ToInt32(cmd.Parameters["@EvalId"].Value);
            }
        }

        private void GetSessionId(SqlConnection cn, SqlTransaction transaction)
        {
            using (var cmd = new SqlCommand("CluSys.P_GetOrCreateSession", cn, transaction) {CommandType = CommandType.StoredProcedure})
            {
                cmd.Parameters.Add(new SqlParameter("@AthleteCC", Athlete.CC));
                cmd.Parameters.Add(new SqlParameter("@PhysiotherapistCC", PhysiotherapistCC));
                cmd.Parameters.Add(new SqlParameter("@Date", Session.Date));
                cmd.Parameters.Add(new SqlParameter("@EvalId", Session.EvalId));
                cmd.Parameters.Add(new SqlParameter("@SessionId", SqlDbType.Int) { Direction = ParameterDirection.Output });
                cmd.ExecuteNonQuery();

                Session.Id = Convert.ToInt32(cmd.Parameters["@SessionId"].Value);
            }
        }

        private void UpdateEvaluation(SqlConnection cn, SqlTransaction transaction)
        {
            using (var cmd = new SqlCommand("CluSys.P_UpdateEvaluation", cn, transaction) {CommandType = CommandType.StoredProcedure})
            {
                cmd.Parameters.Add(new SqlParameter("@EvalId", Evaluation.Id));
                if(Evaluation.Weight != null && Evaluation.Weight > 0) cmd.Parameters.Add(new SqlParameter("@Weight", Evaluation.Weight));
                if(Evaluation.Height != null && Evaluation.Height > 0) cmd.Parameters.Add(new SqlParameter("@Height", Evaluation.Height));
                if(Evaluation.Story != null) cmd.Parameters.Add(new SqlParameter("@Story", Evaluation.Story));
                if(MedicalDischarge) cmd.Parameters.Add(new SqlParameter("@ClosingDate", Session.Date));
                if(Evaluation.ExpectedRecovery != null) cmd.Parameters.Add(new SqlParameter("@ExpectedRecovery", Evaluation.ExpectedRecovery));
                cmd.ExecuteNonQuery();
            }
        }

        private void SaveMarks(SqlConnection cn, SqlTransaction transaction)
        {
            using (var cmd = new SqlCommand("CluSys.P_AddBodyChartMark", cn, transaction) {CommandType = CommandType.StoredProcedure} )
            {
                cmd.Parameters.Add(new SqlParameter("@X", SqlDbType.Float));
                cmd.Parameters.Add(new SqlParameter("@Y", SqlDbType.Float));
                cmd.Parameters.Add(new SqlParameter("@PainLevel", SqlDbType.TinyInt));
                cmd.Parameters.Add(new SqlParameter("@Description", SqlDbType.NVarChar));
                cmd.Parameters.Add(new SqlParameter("@EvalId", SqlDbType.Int));
                cmd.Parameters.Add(new SqlParameter("@SessionId", SqlDbType.Int));
                cmd.Parameters.Add(new SqlParameter("@ViewId", SqlDbType.Int));
                cmd.Parameters.Add(new SqlParameter("@MarkId", SqlDbType.Int) {Direction = ParameterDirection.Output});

                foreach (var mark in Marks)
                {
                    if (mark.Id == -1)
                    {
                        cmd.Parameters["@X"].Value = mark.X;
                        cmd.Parameters["@Y"].Value = mark.Y;
                        cmd.Parameters["@PainLevel"].Value = mark.PainLevel;
                        cmd.Parameters["@Description"].Value = (object) mark.Description ?? DBNull.Value;
                        cmd.Parameters["@EvalId"].Value = mark.EvalId = Evaluation.Id;
                        cmd.Parameters["@SessionId"].Value = mark.SessionId = Session.Id;
                        cmd.Parameters["@ViewId"].Value = mark.ViewId;
                        cmd.ExecuteNonQuery();

                        mark.Id = int.Parse(cmd.Parameters["@MarkId"].Value.ToString());

                        using (var cmd2 = new SqlCommand("CluSys.P_AddBodyAnnotation", cn, transaction) { CommandType = CommandType.StoredProcedure })
                        {
                            cmd2.Parameters.Add(new SqlParameter("@MarkId", SqlDbType.Int));
                            cmd2.Parameters.Add(new SqlParameter("@AnnotSym", SqlDbType.NVarChar));

                            foreach (var annot in mark.Annotations)
                            {
                                cmd2.Parameters["@MarkId"].Value = mark.Id;
                                cmd2.Parameters["@AnnotSym"].Value = annot.Symbol;
                                cmd2.ExecuteNonQuery();
                            }
                        }
                    }
                }
            }
            using (var cmd = new SqlCommand("CluSys.P_RmBodyChartMark", cn, transaction) { CommandType = CommandType.StoredProcedure })
            {
                cmd.Parameters.Add(new SqlParameter("@MarkId", SqlDbType.Int));

                foreach (var mark in DeletedMarks)
                {
                    if (mark.Id != -1)
                    {
                        cmd.Parameters["@MarkId"].Value = mark.Id;
                        cmd.ExecuteNonQuery();
                    }
                }
            }
        }

        private void SaveProblems(SqlConnection cn, SqlTransaction transaction)
        {
            using (var cmd = new SqlCommand("CluSys.P_AddProblem", cn, transaction) { CommandType = CommandType.StoredProcedure })
            {
                cmd.Parameters.Add(new SqlParameter("@Description", SqlDbType.NVarChar));
                cmd.Parameters.Add(new SqlParameter("@EvalId", SqlDbType.Int));
                cmd.Parameters.Add(new SqlParameter("@SessionId", SqlDbType.Int));
                cmd.Parameters.Add(new SqlParameter("@ProbId", SqlDbType.Int) { Direction = ParameterDirection.Output });

                foreach (var prob in Problems)
                {
                    if (prob.Id == -1)
                    {
                        cmd.Parameters["@Description"].Value = prob.Description;
                        cmd.Parameters["@EvalId"].Value = prob.EvalId = Evaluation.Id;
                        cmd.Parameters["@SessionId"].Value = prob.SessionId = Session.Id;
                        cmd.ExecuteNonQuery();
                        prob.Id = int.Parse(cmd.Parameters["@ProbId"].Value.ToString());
                    }
                }
            }
            using (var cmd = new SqlCommand("CluSys.P_RmProblem", cn, transaction) { CommandType = CommandType.StoredProcedure })
            {
                cmd.Parameters.Add(new SqlParameter("@ProbId", SqlDbType.Int));

                foreach (var prob in DeletedProblems)
                {
                    if (prob.Id != -1)
                    {
                        cmd.Parameters["@ProbId"].Value = prob.Id;
                        cmd.ExecuteNonQuery();
                    }
                }
            }
        }

        private void SaveTreatments(SqlConnection cn, SqlTransaction transaction)
        {
            using (var cmd = new SqlCommand("CluSys.P_AddTreatment", cn, transaction) { CommandType = CommandType.StoredProcedure })
            {
                cmd.Parameters.Add(new SqlParameter("@Description", SqlDbType.NVarChar));
                cmd.Parameters.Add(new SqlParameter("@Objective", SqlDbType.NVarChar));
                cmd.Parameters.Add(new SqlParameter("@EvalId", SqlDbType.Int));
                cmd.Parameters.Add(new SqlParameter("@SessionId", SqlDbType.Int));
                cmd.Parameters.Add(new SqlParameter("@ProbId", SqlDbType.Int));
                cmd.Parameters.Add(new SqlParameter("@TreatmentId", SqlDbType.Int) { Direction = ParameterDirection.Output });

                foreach (var treatment in Treatments)
                {
                    if (treatment.Id == -1)
                    {
                        cmd.Parameters["@Description"].Value = (object) treatment.Description ?? DBNull.Value;
                        cmd.Parameters["@Objective"].Value = (object) treatment.Objective ?? DBNull.Value;
                        cmd.Parameters["@EvalId"].Value = treatment.EvalId = Evaluation.Id;
                        cmd.Parameters["@SessionId"].Value = treatment.SessionId = Session.Id;
                        cmd.Parameters["@ProbId"].Value = (object) treatment.RefProblem?.Id ?? DBNull.Value;
                        cmd.ExecuteNonQuery();
                        treatment.Id = int.Parse(cmd.Parameters["@TreatmentId"].Value.ToString());
                    }
                }
            }
            using (var cmd = new SqlCommand("CluSys.P_RmTreatment", cn, transaction) { CommandType = CommandType.StoredProcedure })
            {
                cmd.Parameters.Add(new SqlParameter("@TreatmentId", SqlDbType.Int));

                foreach (var treatment in DeletedTreatments)
                {
                    if (treatment.Id != -1)
                    {
                        cmd.Parameters["@TreatmentId"].Value = treatment.Id;
                        cmd.ExecuteNonQuery();
                    }
                }
            }
        }

        private void SaveObservations(SqlConnection cn, SqlTransaction transaction)
        {
            using (var cmd = new SqlCommand("CluSys.P_SetObservation", cn, transaction) { CommandType = CommandType.StoredProcedure })
            {
                cmd.Parameters.Add(new SqlParameter("@Obs", SqlDbType.NVarChar));
                cmd.Parameters.Add(new SqlParameter("@EvalId", SqlDbType.Int));
                cmd.Parameters.Add(new SqlParameter("@SessionId", SqlDbType.Int));
                cmd.Parameters.Add(new SqlParameter("@ObsId", SqlDbType.Int) { Direction = ParameterDirection.Output });

                foreach (var obs in Observations)
                {
                    if (obs.Id == -1)
                    {
                        cmd.Parameters["@Obs"].Value = obs.Obs;
                        cmd.Parameters["@EvalId"].Value = obs.EvalId = Evaluation.Id;
                        cmd.Parameters["@SessionId"].Value = obs.SessionId = Session.Id;
                        cmd.ExecuteNonQuery();
                        obs.Id = int.Parse(cmd.Parameters["@ObsId"].Value.ToString());
                    }
                }
            }
            using (var cmd = new SqlCommand("CluSys.P_SetObservation", cn, transaction) { CommandType = CommandType.StoredProcedure })
            {
                cmd.Parameters.Add(new SqlParameter("@DateClosed", SqlDbType.DateTime));
                cmd.Parameters.Add(new SqlParameter("@ObsId", SqlDbType.Int));

                foreach (var obs in DeletedObservations)
                {
                    if (obs.Id != -1)
                    {
                        cmd.Parameters["@DateClosed"].Value = Session.Date;
                        cmd.Parameters["@ObsId"].Value = obs.Id;
                        cmd.ExecuteNonQuery();
                    }
                }
            }
        }
    }
}
