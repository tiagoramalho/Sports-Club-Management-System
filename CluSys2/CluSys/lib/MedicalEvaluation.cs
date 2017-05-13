using System;

namespace CluSys.lib
{
    [Serializable()]
    class MedicalEvaluation
    {
        public int ID { get; set; }
        public double Weightt { get; set; }
        public double Height { get; set; }
        public string Story { get; set; }
        public DateTime OpeningDate { get; set; }
        public DateTime? ClosingDate { get; set; }
        public DateTime? ExpectedRecovery { get; set; }
        public string AthleteCC { get; set; }
        public string PhysiotherapistCC { get; set; }

        private bool Equals(MedicalEvaluation other) => ID == other.ID;

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((MedicalEvaluation) obj);
        }

        public override int GetHashCode() => ID;
    }
}
