using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CluSys.lib
{
    [Serializable()]
    class EvaluationSession
    {
        public int Id { get; set; }
        public int EvalId { get; set; }
        public DateTime Date { get; set; }

        private bool Equals(EvaluationSession other) => Id == other.Id && EvalId == other.EvalId;

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((EvaluationSession) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (Id * 397) ^ EvalId;
            }
        }
    }
}
