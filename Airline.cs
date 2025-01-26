using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRG2_Assignment
{
    public class Airline
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public Dictionary<string, Flight> Flights { get; set; }

        //constructors
        public Airline()
        {
            Flights = new Dictionary<string, Flight>();
        }
        public Airline(string code, string name)
        {
            Code = code;
            Name = name;
        }

        //methods
        public bool AddFlight(Flight flight)
        {
            if (Flights.ContainsKey(flight.FlightNumber))
            {
                return false;
            }
            else
            {
                Flights[flight.FlightNumber] = flight;
                return true;
            }
        }

        public bool RemoveFlight(Flight flight)
        {
            return Flights.Remove(flight.FlightNumber);
        }

        public double CalculateFees()
        {
            double totalFees = 0;
            foreach (var flight in Flights.Values)
            {
                totalFees += flight.CalculateFees();
            }
            return totalFees;
        }

        public override string ToString()
        {
            return $"Airline: {Name}, Code: {Code}, Flights: {Flights.Count}";
        }
    }
}
