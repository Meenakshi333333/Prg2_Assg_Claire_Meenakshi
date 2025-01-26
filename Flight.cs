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
        public string SpecialRequestCode { get; set; }
        public string AirlineCode { get; set; } // New property to store the airline code

        public BoardingGate BoardingGate { get; set; }

        // Constructors
        public Flight() { }

        public Flight(string flightNumber, string origin, string destination, DateTime expectedTime, string status = "On Time", string airlineCode = "")
        {
            FlightNumber = flightNumber;
            Origin = origin;
            Destination = destination;
            ExpectedTime = expectedTime;
            Status = status;
            AirlineCode = airlineCode;
        }

        // Abstract method    
        public abstract double CalculateFees();

        public override string ToString()
        {
            return $"Flight: {FlightNumber}, From: {Origin}, To: {Destination}, Expected Departure/Arrival: {ExpectedTime}, Status: {Status}, Airline Code: {AirlineCode}";
        }
    }
}
