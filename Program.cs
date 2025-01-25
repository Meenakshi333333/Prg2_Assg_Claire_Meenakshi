using PRG2_Assignment;
using System;
//Basic Feature 1
// Load files and initialize dictionaries
Dictionary<string, Airline> airlineDictionary = LoadAirlines("airlines.csv");
Dictionary<string, BoardingGate> boardingGateDictionary = LoadBoardingGates("boardinggates.csv");

Console.WriteLine("Airlines loaded: " + airlineDictionary.Count);
Console.WriteLine("Boarding gates loaded: " + boardingGateDictionary.Count);

//Menu Loop
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
            ListAllFlights();
            break;
        case "2":
            ListBoardingGates();
            break;
        case "3":
            AssignBoardingGate();
            break;
        case "4":
            CreateFlight();
            break;
        case "5":
            DisplayAirlineFlights();
            break;
        case "6":
            ModifyFlightDetails();
            break;
        case "7":
            DisplayFlightSchedule();
            break;
        case "0":
            Console.WriteLine("Goodbye!");
            return;
        default:
            Console.WriteLine("Invalid option. Please try again.");
            break;
    }
}
}

static Dictionary<string, Airline> LoadAirlines(string filePath)
{
    Dictionary<string, Airline> airlines = new Dictionary<string, Airline>();

    try
    {
        var lines = File.ReadLines(filePath).Skip(1); // Skip header
        foreach (var line in lines)
        {
            string[] parts = line.Split(',');
            if (parts.Length == 2)
            {
                string code = parts[0].Trim();
                string name = parts[1].Trim();
                airlines[code] = new Airline(code, name);
            }
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine("Error loading airlines: " + ex.Message);
    }

    return airlines;
}

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
                bool supportsDDJB = bool.Parse(parts[1].Trim());
                bool supportsCFFT = bool.Parse(parts[2].Trim());
                bool supportsLWTT = bool.Parse(parts[3].Trim());
                gates[name] = new BoardingGate(name, supportsDDJB, supportsCFFT, supportsLWTT);
            }
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine("Error loading boarding gates: " + ex.Message);
    }

    return gates;
}

//Basic feature 4
static void ListAllBoardingGates(Dictionary<string, BoardingGate> gates)
{
    Console.Clear();
    Console.WriteLine("=============================================");
    Console.WriteLine("List of Boarding Gates for Terminal 5");
    Console.WriteLine("=============================================");
    Console.WriteLine("Gate Name       Supports DDJB   Supports CFFT   Supports LWTT   Assigned Flight");

    foreach (var gate in gates.Values)
    {
        string flight = gate.AssignedFlight ?? "None";
        Console.WriteLine($"{gate.Name.PadRight(15)}{gate.SupportsDDJB.ToString().PadRight(15)}{gate.SupportsCFFT.ToString().PadRight(15)}{gate.SupportsLWTT.ToString().PadRight(15)}{flight}");
    }

    Console.WriteLine("\nPress any key to return to the menu...");
    Console.ReadKey();
}    
