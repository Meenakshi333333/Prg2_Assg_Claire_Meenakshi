//==========================================================
// Student Number	: S10267951B
// Student Name	: Claire Chan
// Partner Name	: Senthilnathan Meeankshi
//==========================================================

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
        public CFFTFlight() : base() { }
        public CFFTFlight(string flightNumber, string origin, string destination, DateTime expectedTime, string status = "On Time") : base(flightNumber, origin, destination, expectedTime, status) { }
        public override double CalculateFees()
        {
            return RequestFee;
        }
        public override string ToString()
        {
            return base.ToString() + "\nSpecial Request Code: CFFT";
        }
    }
}
