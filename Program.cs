using PRG2_Assignment;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

/*
 BASIC INFO OF CASE SCENARIO:
- flight plan (Flight num, Origin, Destination, Expected Departure/Arrival)
- cannot have flights with same flight number on same day
- flights are coordinated in sequence as of when they are received
- when assigned. status becomes "On Time"
- can only assign 1 boarding gate to 1 flight per day !!!!!!!!

SPECIAL REQUEST CODES:
1. DDJB (double-decker jet bridge requested)
- A10, A11, A12, A13, A20, A21, A22, B10, B11, B12
2. CFFT (connecting flight fast transfer requested)
- B1, B2, B3 + All C Gates (C1 to C22)
3. LWTT (Longer waiting time requested)
- A1, A2, A20, A21, A22, C14, C15, C16 + All B Gates (B1 to B22)

CALCULATING FEES:
* depends on (arriving/departing), (got request code?), base fee = $300
A1. Arriving flight = $500
A2. Departing flight = $800
B. Base fee = $300
C1. DDJB = $300
C2. CFFT = $150
C3. LWTT = $500

INCENTIVES TO AIRLINES:
* promotions are stackable
1. for every 3 arriving/departing flights = $350 discount
2. more than 5 arriving/departing flights = 3% off total bill before any deductions
3. flights that arrive/depart before 11am or after 9pm = $110 discount
4. airlines with origin of DXB, BKK ot NRT = $25 discount
5. no request codes = $50 discount
 */

//METHODS

//LoadAirlines() - Meenakshi
static Dictionary<string, Airline> LoadAirlines(string filePath)
{
    var airlineDictionary = new Dictionary<string, Airline>();

    try
    {
        var lines = File.ReadAllLines(filePath);
        foreach (var line in lines.Skip(1))  // Skip header
        {
            var columns = line.Split(',');
            string code = columns[0].Trim();
            string name = columns[1].Trim();

            airlineDictionary[code] = new Airline { Code = code, Name = name };
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error reading the CSV file: {ex.Message}");
    }

    return airlineDictionary;
}

//LoadBoardingGates() - Meenakshi
static Dictionary<string, BoardingGate> LoadBoardingGates(string filePath)
{
    Dictionary<string, BoardingGate> gates = new Dictionary<string, BoardingGate>();

    try
    {
        var lines = File.ReadLines(filePath).Skip(1); // Skip header
        foreach (var line in lines)
        {
            string[] parts = line.Split(',');
            if (parts.Length == 4)
            {
                string name = parts[0].Trim();
                bool supportsCFFT = parts[1].Trim() == "true";
                bool supportsDDJB = parts[2].Trim() == "true";
                bool supportsLWTT = parts[3].Trim() == "true";
                gates[name] = new BoardingGate(name, supportsCFFT, supportsDDJB, supportsLWTT);
            }
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine("Error loading boarding gates: " + ex.Message);
    }
    return gates;
}


//basic feature2 LoadFlights() - Claire
static Dictionary<string, Flight> LoadFlights(string filename)
{
    Dictionary<string, Flight> flights = new Dictionary<string, Flight>();
    using (StreamReader sr = new StreamReader(filename))
    {
        string? s = sr.ReadLine(); //heading
        while (( s = sr.ReadLine())!= null)
        {
            string[] parts = s.Split(",");
            string flightNumber = parts[0];
            string origin = parts[1];
            string destination = parts[2];
            DateTime expectedTime = Convert.ToDateTime(parts[3]);
            string request = parts[4];
            if (request == "")
            {
                Flight flight = new NORMFlight(flightNumber, origin, destination, expectedTime);
                flights[flightNumber] = flight;
            }
            else if (request == "LWTT")
            {
                Flight flight = new LWTTFlight(flightNumber, origin, destination, expectedTime);
                flights[flightNumber] = flight;
            }
            else if (request == "DDJB")
            {
                Flight flight = new DDJBFlight(flightNumber, origin, destination, expectedTime);
                flights[flightNumber] = flight;
            }
            else if (request == "CFFT")
            {
                Flight flight = new CFFTFlight(flightNumber, origin, destination, expectedTime);
                flights[flightNumber] = flight;
            }
        }
    }
    return flights;
}

//Listing flights for basic feature 3 - Claire
void ListAllFlights(Dictionary<string, Airline> airlineDictionary, Dictionary<string,Flight> flightDictionary)
{
    Console.WriteLine("{0,-16}{1,-23}{2,-23}{3,-23}{4,-22}","Flight Number", "Airline Name", "Origin", "Destination", "Expected Departure/Arrival Time");
    foreach (KeyValuePair<string, Flight> flight in flightDictionary)
    {
        string flightnum = flight.Key;
        Flight flightdets = flight.Value;
        if (flightdets.FlightNumber.StartsWith("SQ"))
        {
            string airlineName = (airlineDictionary["SQ"]).Name;
            Console.WriteLine("{0,-16}{1,-23}{2,-23}{3,-23}{4,-22}", flightnum, airlineName, flightdets.Origin, flightdets.Destination, flightdets.ExpectedTime);
        }
        else if (flightdets.FlightNumber.StartsWith("MH"))
        {
            string airlineName = airlineDictionary["MH"].Name;
            Console.WriteLine("{0,-16}{1,-23}{2,-23}{3,-23}{4,-22}", flightnum, airlineName, flightdets.Origin, flightdets.Destination, flightdets.ExpectedTime);
        }
        else if (flightdets.FlightNumber.StartsWith("JL"))
        {
            string airlineName = airlineDictionary["JL"].Name;
            Console.WriteLine("{0,-16}{1,-23}{2,-23}{3,-23}{4,-22}", flightnum, airlineName, flightdets.Origin, flightdets.Destination, flightdets.ExpectedTime);
        }
        else if (flightdets.FlightNumber.StartsWith("CX"))
        {
            string airlineName = airlineDictionary["CX"].Name;
            Console.WriteLine("{0,-16}{1,-23}{2,-23}{3,-23}{4,-22}", flightnum, airlineName, flightdets.Origin, flightdets.Destination, flightdets.ExpectedTime);
        }
        else if (flightdets.FlightNumber.StartsWith("QF"))
        {
            string airlineName = airlineDictionary["QF"].Name;
            Console.WriteLine("{0,-16}{1,-23}{2,-23}{3,-23}{4,-22}", flightnum, airlineName, flightdets.Origin, flightdets.Destination, flightdets.ExpectedTime);
        }
        else if (flightdets.FlightNumber.StartsWith("TR"))
        {
            string airlineName = airlineDictionary["TR"].Name;
            Console.WriteLine("{0,-16}{1,-23}{2,-23}{3,-23}{4,-22}", flightnum, airlineName, flightdets.Origin, flightdets.Destination, flightdets.ExpectedTime);
        }
        else if (flightdets.FlightNumber.StartsWith("EK"))
        {
            string airlineName = airlineDictionary["EK"].Name;
            Console.WriteLine("{0,-16}{1,-23}{2,-23}{3,-23}{4,-22}", flightnum, airlineName, flightdets.Origin, flightdets.Destination, flightdets.ExpectedTime);
        }
        else if (flightdets.FlightNumber.StartsWith("BA"))
        {
            string airlineName = airlineDictionary["BA"].Name;
            Console.WriteLine("{0,-16}{1,-23}{2,-23}{3,-23}{4,-22}", flightnum, airlineName, flightdets.Origin, flightdets.Destination, flightdets.ExpectedTime);
        }
        else
            Console.WriteLine("unknown flight found");
    }
}

//PROGRAM CODE

//Basic Feature 1
// Load files and initialize dictionaries
Console.WriteLine("Loading Airlines. . .");
Dictionary<string, Airline> airlineDictionary = LoadAirlines("airlines.csv");
Console.WriteLine("{0} Airlines Loaded!", airlineDictionary.Count);

Console.WriteLine("Loading Boarding Gates. . .");
Dictionary<string, BoardingGate> boardingGateDictionary = LoadBoardingGates("boardinggates.csv");
Console.WriteLine("{0} Boarding Gates Loaded!", boardingGateDictionary.Count);

//basic feature 2
Console.WriteLine("Loading Flights. . .");
Dictionary<string, Flight> flightDictionary = LoadFlights("flights.csv");
Console.WriteLine("{0} Flights Loaded!", flightDictionary.Count);

//basic feature 3
ListAllFlights(airlineDictionary, flightDictionary);

//Basic feature 4
static void ListBoardingGates(Dictionary<string, BoardingGate> gates)
{
    Console.Clear();
    Console.WriteLine("=============================================");
    Console.WriteLine("List of Boarding Gates for Terminal 5");
    Console.WriteLine("=============================================");
    Console.WriteLine("Gate Name       DDJB   CFFT    LWTT    Assigned Flight");

    foreach (var gate in gates.Values)
    {
        string flight = "None";  // Default to "None" if no flight is assigned

        if (gate.Flight != null)
        {
            flight = gate.Flight.FlightNumber;  
        }
        Console.WriteLine(gate.GateName + "       " + gate.SupportsDDJB.ToString() + "       "
            + gate.SupportsCFFT.ToString() + "       " + gate.SupportsLWTT.ToString() + "       " + flight);
    }
    Console.WriteLine("\nPress any key to return to the menu...");
    Console.ReadKey();
}


//basic feature 7
static void DisplayAirlineFlights(Dictionary<string, Airline> airlineDictionary, Dictionary<string, Flight> flightDictionary)
{
    Console.Clear();
    Console.WriteLine("=============================================");
    Console.WriteLine("List of Airlines for Changi Airport Terminal 5");
    Console.WriteLine("=============================================");
    Console.WriteLine("Airline Code   Airline Name");

    foreach (var airline in airlineDictionary.Values)
    {
        Console.WriteLine(airline.Code + "       " + airline.Name);
    }

    Console.Write("\nEnter Airline Code: ");
    string airlineCode = Console.ReadLine().ToUpper();

    if (airlineDictionary.TryGetValue(airlineCode, out Airline selectedAirline))
    {
        Console.WriteLine("\n=============================================");
        Console.WriteLine($"List of Flights for {selectedAirline.Name}");
        Console.WriteLine("=============================================");
        Console.WriteLine("Flight Number   Airline Name     Origin            Destination        Expected ");

        var airlineFlights = flightDictionary.Values.Where(f => f.AirlineCode == airlineCode).ToList();

        if (airlineFlights.Count == 0)
        {
            Console.WriteLine("No flights found for this airline.");
        }
        else
        {
            foreach (var flight in airlineFlights)
            {
                Console.WriteLine(flight.FlightNumber + "       " + selectedAirline.Name + "       "
                    + flight.Origin + "       " + flight.Destination + "       "
                    + flight.ExpectedTime.ToString("dd/MM/yyyy hh:mm tt"));
            }
        }
    }
    else
    {
        Console.WriteLine("Invalid Airline Code.");
    }

    Console.WriteLine("\nPress any key to return to the menu...");
    Console.ReadKey();
}


//basic feature 8

static void ModifyFlightDetails(Dictionary<string, Airline> airlineDictionary)
{
    // List all airlines
    Console.WriteLine("=============================================");
    Console.WriteLine("List of Airlines for Changi Airport Terminal 5");
    Console.WriteLine("=============================================");
    Console.WriteLine("Airline Code  Airline Name");

    foreach (var airline in airlineDictionary.Values)
    {
        Console.WriteLine(airline.Code + "  " + airline.Name);
    }

    Console.Write("\nEnter Airline Code: ");
    string airlineCode = Console.ReadLine().ToUpper();

    if (airlineDictionary.TryGetValue(airlineCode, out Airline selectedAirline))
    {
        Console.WriteLine($"\nList of Flights for {selectedAirline.Name}");
        Console.WriteLine("Flight Number  Airline Name  Origin          Destination      Expected ");

        var airlineFlights = selectedAirline.Flights.Values.ToList(); // Get flights from the selected airline

        if (airlineFlights.Count == 0)
        {
            Console.WriteLine("No flights found for this airline.");
        }
        else
        {
            foreach (var flight in airlineFlights)
            {
                Console.WriteLine(flight.FlightNumber + "  " + selectedAirline.Name + "  "
                    + flight.Origin + "  " + flight.Destination + "  "
                    + flight.ExpectedTime.ToString("dd/MM/yyyy hh:mm tt"));
            }

            Console.WriteLine("\nChoose an existing Flight to modify or delete:");
            string flightNumber = Console.ReadLine().ToUpper();

            var selectedFlight = airlineFlights.FirstOrDefault(f => f.FlightNumber == flightNumber);

            if (selectedFlight != null)
            {
                Console.WriteLine("\n1. Modify Flight");
                Console.WriteLine("2. Delete Flight");
                Console.Write("Choose an option: ");
                string modifyOption = Console.ReadLine();

                if (modifyOption == "1")
                {
                    Console.WriteLine("\n1. Modify Basic Information");
                    Console.WriteLine("2. Modify Status");
                    Console.WriteLine("3. Modify Special Request Code");
                    Console.WriteLine("4. Modify Boarding Gate");
                    Console.Write("Choose an option: ");
                    string modificationOption = Console.ReadLine();

                    switch (modificationOption)
                    {
                        case "1":
                            Console.Write("\nEnter new Origin: ");
                            selectedFlight.Origin = Console.ReadLine();
                            Console.Write("Enter new Destination: ");
                            selectedFlight.Destination = Console.ReadLine();
                            Console.Write("Enter new Expected Departure/Arrival Time (dd/MM/yyyy hh:mm tt): ");
                            selectedFlight.ExpectedTime = DateTime.ParseExact(Console.ReadLine(), "dd/MM/yyyy hh:mm tt", null);
                            break;
                        case "2":
                            Console.Write("\nEnter new Status: ");
                            selectedFlight.Status = Console.ReadLine();
                            break;
                        case "3":
                            Console.Write("\nEnter new Special Request Code: ");
                            selectedFlight.SpecialRequestCode = Console.ReadLine();
                            break;
                        case "4":
                            Console.Write("\nEnter new Boarding Gate: ");
                            string gateName = Console.ReadLine();

                            // Create a new BoardingGate object
                            selectedFlight.BoardingGate = new BoardingGate
                            {
                                GateName = gateName,
                                SupportsCFFT = true,  
                                SupportsDDJB = true, 
                                SupportsLWTT = true   
                            };

                            break;
                        default:
                            Console.WriteLine("Invalid option. Returning to menu...");
                            break;
                    }

                    // Display updated flight details
                    Console.WriteLine("\nFlight updated!");
                    Console.WriteLine("=============================================");
                    Console.WriteLine($"Flight Number: {selectedFlight.FlightNumber}");
                    Console.WriteLine($"Airline Name: {selectedAirline.Name}");
                    Console.WriteLine($"Origin: {selectedFlight.Origin}");
                    Console.WriteLine($"Destination: {selectedFlight.Destination}");
                    Console.WriteLine($"Expected Departure/Arrival Time: {selectedFlight.ExpectedTime:dd/MM/yyyy hh:mm tt}");
                    Console.WriteLine($"Status: {selectedFlight.Status}");
                    Console.WriteLine($"Special Request Code: {selectedFlight.SpecialRequestCode ?? "None"}");
                    Console.WriteLine($"Boarding Gate: {selectedFlight.BoardingGate?.GateName ?? "Unassigned"}");
                }
                else if (modifyOption == "2")
                {
                    Console.WriteLine("\nAre you sure you want to delete this flight? [Y/N]");
                    string confirmation = Console.ReadLine().ToUpper();

                    if (confirmation == "Y")
                    {
                        // Remove the flight from the airline's flight dictionary
                        selectedAirline.RemoveFlight(selectedFlight);
                        Console.WriteLine("Flight has been deleted.");
                    }
                    else
                    {
                        Console.WriteLine("Flight deletion canceled.");
                    }
                }
                else
                {
                    Console.WriteLine("Invalid option. Returning to menu...");
                }
            }
            else
            {
                Console.WriteLine("Flight not found.");
            }
        }
    }
    else
    {
        Console.WriteLine("Invalid Airline Code.");
    }

    Console.WriteLine("\nPress any key to return to the menu...");
    Console.ReadKey();
}

static void AssignBoardingGate()
{

}
static void CreateFlight()
{

}
static void DisplayFlightSchedule()
{

}
// Menu Loop
while (true)
{
    Console.Clear();
    Console.WriteLine("=============================================");
    Console.WriteLine("Welcome to Changi Airport Terminal 5");
    Console.WriteLine("=============================================");
    Console.WriteLine("1. List All Flights");
    Console.WriteLine("2. List Boarding Gates");
    Console.WriteLine("3. Assign a Boarding Gate to a Flight");
    Console.WriteLine("4. Create Flight");
    Console.WriteLine("5. Display Airline Flights");
    Console.WriteLine("6. Modify Flight Details");
    Console.WriteLine("7. Display Flight Schedule");
    Console.WriteLine("0. Exit");
    Console.Write("\nPlease select your option: ");

    string option = Console.ReadLine();

    switch (option)
    {
        case "1":
            ListAllFlights(airlineDictionary, flightDictionary);
            break;
        case "2":
            ListBoardingGates(boardingGateDictionary);
            break;
        case "3":
            AssignBoardingGate();
            break;
        case "4":
            CreateFlight();
            break;
        case "5":
            DisplayAirlineFlights(airlineDictionary, flightDictionary);
            break;
        case "6":
            ModifyFlightDetails(airlineDictionary);
            break;
        case "7":
            DisplayFlightSchedule();
            break;
        case "0":
            Console.WriteLine("Exiting...");
            return;
        default:
            Console.WriteLine("Invalid option, please try again.");
            break;
    }

    // Wait for user input to return to the menu
    Console.WriteLine("\nPress any key to return to the menu...");
    Console.ReadKey();
}

