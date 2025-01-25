using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRG2_Assignment
{
    public class LWTTFlight : Flight
    {
        public double RequestFee { get; set; }
        public LWTTFlight() : base() { }
        public LWTTFlight(string flightNumber, string origin, string destination, DateTime expectedTime, string status = "On Time") : base(flightNumber, origin, destination, expectedTime, status) { }
        public override double CalculateFees()
        {
            return RequestFee;
        }
        public override string ToString()
        {
            return base.ToString();
        }
    }
}
