using System;

namespace CluSys.lib
{
    [Serializable]
    public class SessionObservation
    {
        public int Id { get; set; }
        public string Obs { get; set; }
        public DateTime? DateClosed { get; set; }
        public int EvalId { get; set; }
        public int SessionId { get; set; }
    }
}
