using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRG2_Assignment
{
    public class CFFTFlight : Flight
    {
        public double RequestFee { get; set; }

        public override double CalculateFees()
        {
            return RequestFee;
        }
    }
}
