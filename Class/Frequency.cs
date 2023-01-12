using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deafen.Class
{
    public class Frequency
    {
        public double milliseconds;
        public double frequency;
        public double volume;

        public Frequency(double m, double f, double v)
        {
            milliseconds = m;
            frequency = f;
            volume = v;
        }
    }
}
