using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRG2_Assignment
{
    public abstract class Flight
    {
        public string FlightNumber { get; set; }
        public string Origin { get; set; }
        public string Destination { get; set; }
        public DateTime ExpectedTime { get; set; }
        public string Status { get; set; }

        public abstract double CalculateFees();

        public override string ToString()
        {
            return $"Flight: {FlightNumber}, From: {Origin}, To: {Destination}, Status: {Status}";
        }
    }
}
