using System;

namespace CluSys.lib
{
    [Serializable()]
    public class BocyChartMark
    {
        public int ID { get; set; }
        public double x { get; set; }
        public double y { get; set; }
        public int PainLevel { get; set; }
        public String Obs { get; set; }
        public int EvalId { get; set; }
        public int SessionId { get; set; }
        public int ViewId { get; set; }

    }
}
