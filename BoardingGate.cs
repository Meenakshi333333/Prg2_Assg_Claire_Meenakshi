using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRG2_Assignment
{
    public class BoardingGate
    {
        public string GateName { get; set; }
        public bool SupportsCFFT { get; set; }
        public bool SupportsDDJB { get; set; }
        public bool SupportsLWTT { get; set; }
        public Flight Flight { get; set; }


        //constructors
        public BoardingGate() { }
        public BoardingGate(string gateName, bool supportsCFFT, bool supportsDDJB, bool supportsLWTT)
        {
            GateName = gateName;
            SupportsCFFT = supportsCFFT;
            SupportsDDJB = supportsDDJB;
            SupportsLWTT = supportsLWTT;
        }

        public double CalculateFees()
        {
            return Flight?.CalculateFees() ?? 0;
        }

        public override string ToString()
        {
            return $"BoardingGate: {GateName}, Flight: {Flight?.FlightNumber ?? "None"}";
        }
    }
}
