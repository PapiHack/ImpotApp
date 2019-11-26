using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImpotsLST.Utils
{
    public class IRPPInterval
    {
        public int Min { get; set; }
        public int Max { get; set; }
        public int Pourcentage { get; set; }

        public IRPPInterval(int min, int max, int pourcentage)
        {
            Min = min;
            Max = max;
            Pourcentage = pourcentage;
        }
    }
}
