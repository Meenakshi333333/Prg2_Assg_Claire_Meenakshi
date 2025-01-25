using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRG2_Assignment
{
    public class DDJBFlight : Flight
    {
        public double RequestFee { get; set; }
        public DDJBFlight() : base() { }
        public DDJBFlight(string flightNumber, string origin, string destination, DateTime expectedTime, string status = "On Time") : base(flightNumber, origin, destination, expectedTime, status) { }
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
