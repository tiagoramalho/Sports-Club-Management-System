using System;

namespace CluSys.lib
{
    [Serializable()]
    class MedicalEvaluation
    {
        public int ID { get; set; }
        public Double Weightt { get; set; }
        public Double Height { get; set; }
        public String Story { get; set; }
        public DateTime OpeningDate { get; set; }
        public DateTime? ClosingDATE { get; set; }
        public DateTime? ExpectedRecovery { get; set; }
        public String AthleteCC { get; set; }
        public String PhysiotherapistCC { get; set; }
    }
}
