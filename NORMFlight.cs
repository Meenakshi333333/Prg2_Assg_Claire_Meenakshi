using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRG2_Assignment
{
    public class NORMFlight : Flight
    {
        //constructors
        public NORMFlight():base() { }
        public NORMFlight(string flightNumber, string origin, string destination, DateTime expectedTime, string status = "On Time") : base(flightNumber, origin, destination, expectedTime, status) { }
        public override double CalculateFees()
        {
            double fees = 0;
            return fees;
        }
        public override string ToString()
        {
            return base.ToString();
        }
    }
}
