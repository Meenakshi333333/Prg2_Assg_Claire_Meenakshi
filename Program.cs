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
    Console.WriteLine("Welcome to Changi" +
        "Airport Terminal 5");
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
static void ListBoardingGates(Dictionary<string, BoardingGate> gates)
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

//basic feature 7
static void DisplayAirlineFlights()
{
    Console.Clear();
    Console.WriteLine("=============================================");
    Console.WriteLine("List of Airlines for Changi Airport Terminal 5");
    Console.WriteLine("=============================================");
    Console.WriteLine("Airline Code   Airline Name");

    foreach (var airline in airlineDictionary.Values)
    {
        Console.WriteLine($"{airline.Code.PadRight(15)} {airline.Name}");
    }

    Console.Write("\nEnter Airline Code: ");
    string airlineCode = Console.ReadLine().ToUpper();

    if (airlineDictionary.TryGetValue(airlineCode, out Airline selectedAirline))
    {
        Console.WriteLine("\n=============================================");
        Console.WriteLine($"List of Flights for {selectedAirline.Name}");
        Console.WriteLine("=============================================");
        Console.WriteLine("Flight Number   Airline Name   Origin            Destination        Expected Departure/Arrival Time");

        var airlineFlights = flights.Where(f => f.AirlineCode == airlineCode).ToList();

        if (airlineFlights.Count == 0)
        {
            Console.WriteLine("No flights found for this airline.");
        }
        else
        {
            foreach (var flight in airlineFlights)
            {
                Console.WriteLine($"{flight.FlightNumber.PadRight(15)} {selectedAirline.Name.PadRight(15)} {flight.Origin.PadRight(16)} {flight.Destination.PadRight(16)} {flight.ExpectedTime:dd/MM/yyyy hh:mm tt}");
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
static void ModifyFlightDetails()
{

    // List all airlines
    Console.WriteLine("List of Airlines for Changi Airport Terminal 5");
    Console.WriteLine("=============================================");
    Console.WriteLine("Airline Code  Airline Name");

    foreach (var airline in airlineDictionary.Values)
    {
        Console.WriteLine($"{airline.Code.PadRight(12)} {airline.Name}");
    }

    Console.Write("\nEnter Airline Code: ");
    string airlineCode = Console.ReadLine().ToUpper();

    if (airlineDictionary.TryGetValue(airlineCode, out Airline selectedAirline))
    {
        Console.WriteLine($"\nList of Flights for {selectedAirline.Name}");
        Console.WriteLine("Flight Number  Airline Name  Origin          Destination      Expected Departure/Arrival Time");

        var airlineFlights = flights.Where(f => f.AirlineCode == airlineCode).ToList();
        if (airlineFlights.Count == 0)
        {
            Console.WriteLine("No flights found for this airline.");
        }
        else
        {
            foreach (var flight in airlineFlights)
            {
                Console.WriteLine($"{flight.FlightNumber.PadRight(15)} {selectedAirline.Name.PadRight(15)} {flight.Origin.PadRight(16)} {flight.Destination.PadRight(15)} {flight.ExpectedTime:dd/MM/yyyy hh:mm tt}");
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
                            selectedFlight.BoardingGate = Console.ReadLine();
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
                    Console.WriteLine($"Boarding Gate: {selectedFlight.BoardingGate ?? "Unassigned"}");
                }
                else if (modifyOption == "2")
                {
                    Console.WriteLine("\nAre you sure you want to delete this flight? [Y/N]");
                    string confirmation = Console.ReadLine().ToUpper();

                    if (confirmation == "Y")
                    {
                        airlineFlights.Remove(selectedFlight);
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
