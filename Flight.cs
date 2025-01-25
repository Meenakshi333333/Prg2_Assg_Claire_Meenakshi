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

        //constructors
        public Flight() {}
        public Flight(string flightNumber, string origin, string destination, DateTime expectedTime, string status = "On Time")
        {
            FlightNumber = flightNumber;
            Origin = origin;
            Destination = destination;
            ExpectedTime = expectedTime;
            Status = status;
        }

        //abstract method    
        public abstract double CalculateFees();

        public override string ToString()
        {
            return $"Flight: {FlightNumber}, From: {Origin}, To: {Destination}, Expected Departure/Arrival: {ExpectedTime}, Status: {Status}";
        }
    }
}
