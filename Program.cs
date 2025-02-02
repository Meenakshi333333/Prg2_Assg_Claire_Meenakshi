//==========================================================
// Student Number	: S10267951B
// Student Name	: Claire Chan
// Partner Name	: Senthilnathan Meeankshi
//==========================================================


using PRG2_Assignment;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

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
            string name = columns[0].Trim(); 
            string code = columns[1].Trim(); 

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
                // meenakshi this part wrong, look at csv file and see the order of special request codes
                string name = parts[0].Trim();
                bool supportsCFFT = parts[1].Trim().ToLower() == "true"; 
                bool supportsDDJB = parts[2].Trim().ToLower() == "true";
                bool supportsLWTT = parts[3].Trim().ToLower() == "true";
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
        while ((s = sr.ReadLine()) != null)
        {
            string[] parts = s.Split(",");
            string flightNumber = parts[0].Trim();
            string origin = parts[1].Trim();
            string destination = parts[2].Trim();
            DateTime expectedTime = Convert.ToDateTime(parts[3].Trim());
            string request = parts[4].Trim();
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


// basic feature 3 -- Claire
void ListAllFlights(Dictionary<string, Airline> airlineDictionary, Dictionary<string, Flight> flightDictionary)
{
    Console.WriteLine("{0,-16}{1,-23}{2,-23}{3,-23}{4,-22}", "Flight Number", "Airline Name", "Origin", "Destination", "Expected Departure/Arrival Time");
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

//Basic feature 4 -- Meenakshi
static void ListBoardingGates(Dictionary<string, BoardingGate> gates)
{
    Console.Clear();
    Console.WriteLine("=============================================");
    Console.WriteLine("List of Boarding Gates for Terminal 5");
    Console.WriteLine("=============================================");
    Console.WriteLine($"{"Gate Name",-10} {"DDJB",-6} {"CFFT",-6} {"LWTT",-6} {"Assigned Flight",-15}");

    foreach (var gate in gates.Values)
    {
        string flight = "None";  // Default to "None" if no flight is assigned

        if (gate.Flight != null)
        {
            flight = gate.Flight.FlightNumber;
        }
        Console.WriteLine($"{gate.GateName,-10} {gate.SupportsDDJB,-6} {gate.SupportsCFFT,-6} {gate.SupportsLWTT,-6} {flight,-15}");
    }

    Console.WriteLine("\nPress any key to return to the menu...");
    Console.ReadKey();
}

//basic feature 5 -- Claire
static void AssignBoardingGate(Dictionary<string, Flight> flightDictionary, Dictionary<string, BoardingGate> boardingGateDictionary)
{
    Console.WriteLine("=============================================");
    Console.WriteLine("Assign a Boarding Gate to a Flight");
    Console.WriteLine("=============================================");
    Console.WriteLine("Enter Flight Number:");
    string flightNumber = Console.ReadLine();

    // input validation for flightnum
    while (flightDictionary.ContainsKey(flightNumber) == false || flightNumber == null)
    {
        Console.WriteLine("Flight number entered does not exist. Try Again.");
        flightNumber = Console.ReadLine();
    }


    //loop
    bool assigned = false;
    do
    {
        Console.WriteLine("Enter Boarding Gate Name:");
        string boardingGate = Console.ReadLine();

        // input validation for boardinggate
        while (boardingGateDictionary.ContainsKey(boardingGate) == false || boardingGate == null)
        {
            Console.WriteLine("Boarding gate entered does not exist. Try Again");
            boardingGate = Console.ReadLine();
        }

        // find flight object
        foreach (KeyValuePair<string, Flight> flight in flightDictionary)
        {
            string flightnum = flight.Key;
            Flight Fobject = flight.Value;
            if (flightNumber == flightnum)
            {
                Console.WriteLine(Fobject.ToString()); // print basic info of flights
                foreach (KeyValuePair<string, BoardingGate> gate in boardingGateDictionary)
                {
                    string bgate = gate.Key;
                    BoardingGate Bobject = gate.Value;
                    if (boardingGate == bgate)
                    {
                        if (Bobject.Flight == null) // no assigned flight
                        {
                            Console.WriteLine(Bobject.ToString()); // print info of entered boarding gate
                            Bobject.Flight = Fobject;
                            Console.WriteLine("Would you like to update the status of the flight? (Y/N)");
                            string changestatus = Console.ReadLine().ToUpper();
                            if (changestatus == "Y")
                            {
                                Console.WriteLine("1. Delayed");
                                Console.WriteLine("2. Boarding");
                                Console.WriteLine("3. On Time");
                                Console.WriteLine("Please select the new status of the flight:");
                                string newstatus = Console.ReadLine();

                                // input validation for user selection
                                while (newstatus != "1" && newstatus != "2" && newstatus != "3")
                                {
                                    Console.WriteLine("Input entered not in options. Try Again.");
                                    newstatus = Console.ReadLine();
                                }

                                if (newstatus == "1")
                                {
                                    Bobject.Flight.Status = "Delayed";
                                }
                                else if (newstatus == "2")
                                {
                                    Bobject.Flight.Status = "Boarding";
                                }
                                else // newstatus = 3
                                {
                                    Bobject.Flight.Status = "On Time";
                                }
                            }
                            Console.WriteLine("Flight {0} has been assigned to Boarding Gate {1}!", flightnum, bgate);
                            assigned = false; // get out of loop
                        }
                        else
                        {
                            assigned = true; // repeat loop
                            Console.WriteLine("Boarding gate has already been assigned to a flight. Try Again.");
                        }
                    }
                }
            }
        }
    } while (assigned == true);
    
}

// basic feature 6 -- Claire

static void CreateFlight(Dictionary<string, Flight> flightDictionary, Dictionary<string, Airline> airlineDictionary)
{
    bool repeat = true;
    while (repeat == true) // to add another flight loop
    {
        while (true)
        {
            // flight number + validation
            Console.Write("Enter Flight Number: ");
            string flightNumber = Console.ReadLine();

            // flightnum validation
            // check airline code

            bool exists = false;
            while (flightNumber == null)
            {
                Console.WriteLine("Invalid input. Try Again.");
                Console.Write("Enter Flight Number: ");
                flightNumber = Console.ReadLine();
            }

            List<string> airlinecodes = new List<string>(airlineDictionary.Keys);
            foreach (string airlinecode in airlinecodes)
            {
                if (flightNumber.StartsWith(airlinecode))
                {
                    exists = true;
                    break;
                }

            }
            while (exists == false)
            {
                Console.WriteLine("Airline code is not in current database. Try Again.");

                //repeat input + validation
                Console.Write("Enter Flight Number: ");
                flightNumber = Console.ReadLine();
                while (flightNumber == null)
                {
                    Console.WriteLine("Invalid input. Try Again.");
                    Console.Write("Enter Flight Number: ");
                    flightNumber = Console.ReadLine();
                }
                foreach (string airlinecode in airlinecodes)
                {
                    if (flightNumber.StartsWith(airlinecode))
                    {
                        exists = true;
                        break;
                    }

                }

            }

            // other inputs
            Console.Write("Enter Origin: ");
            string origin = Console.ReadLine();
            while (origin == null)
            {
                Console.WriteLine("Invalid input. Try Again.");
                Console.Write("Enter Origin: ");
                origin = Console.ReadLine();
            }
            Console.Write("Enter Destination: ");
            string destination = Console.ReadLine();
            while (destination == null)
            {
                Console.WriteLine("Invalid input. Try Again.");
                Console.Write("Enter Destination: ");
                destination = Console.ReadLine();
            }

            bool format = false;

            //type check
            DateTime expectedTime = DateTime.Today;
            while (format == false)
            {
                try
                {
                    Console.Write("Enter Expected Departure/Arrival Time (dd/mm/yyyy hh:mm): ");
                    expectedTime = Convert.ToDateTime(Console.ReadLine());
                    format = true;
                    //check if got flight with same num on same day
                    foreach (KeyValuePair<string, Flight> flight in flightDictionary)
                    {
                        string fnum = flight.Key;
                        Flight Fobject = flight.Value;
                        if (flightNumber == fnum)
                        {
                            if (expectedTime.Day == Fobject.ExpectedTime.Day)
                            {
                                Console.WriteLine("Flight with same number has already been assigned to this day. Try another day.");
                                Console.Write("Enter Expected Departure/Arrival Time (dd/mm/yyyy hh:mm): ");
                                format = false;
                                break;

                            }
                        }
                    }


                }
                catch (FormatException)
                {
                    Console.WriteLine("Invalid format entered. Try Again.");
                }
            }

            //special request?
            Console.Write("Enter Special Request Code (CFFT/DDJB/LWTT/None): ");
            string request = Console.ReadLine();
            while ((request != "CFFT" && request != "DDJB" && request != "LWTT" && request != "None") || request == null)
            {
                Console.WriteLine("Invalid input. Try Again.");
                Console.Write("Enter Special Request Code (CFFT/DDJB/LWTT/None): ");
                request = Console.ReadLine();
            }
            string code = "";
            if (request == "None")
            {
                Flight flight = new NORMFlight(flightNumber, origin, destination, expectedTime);
                flightDictionary[flightNumber] = flight;
            }
            else if (request == "LWTT")
            {
                Flight flight = new LWTTFlight(flightNumber, origin, destination, expectedTime);
                flightDictionary[flightNumber] = flight;
                code = "LWTT";
            }
            else if (request == "DDJB")
            {
                Flight flight = new DDJBFlight(flightNumber, origin, destination, expectedTime);
                flightDictionary[flightNumber] = flight;
                code = "DDJB";
            }
            else if (request == "CFFT")
            {
                Flight flight = new CFFTFlight(flightNumber, origin, destination, expectedTime);
                flightDictionary[flightNumber] = flight;
                code = "CFFT";
            }

            //write into csv file
            using (StreamWriter sw = new StreamWriter("flights.csv", true))
            {
                sw.WriteLine("{0},{1},{2},{3},{4}", flightNumber, origin, destination, expectedTime, code);
            }

            Console.WriteLine($"Flight {flightNumber} has been added!");
            Console.WriteLine("Would you like to add another flight? (Y/N): ");
            string add = Console.ReadLine();
            while ((add.ToUpper() != "Y" && add.ToUpper() != "N") || add == null)
            {
                Console.WriteLine("Invalid option. Try Again.");
                add = Console.ReadLine();
            }
            if (add == "N")
            {
                repeat = false; // make it go back into the loop
                break;
            }
        }

    }
}


//basic feature 7 -- Meenakshi
static void DisplayAirlineFlights(Dictionary<string, Airline> airlineDictionary, Dictionary<string, Flight> flightDictionary)
{
    Console.WriteLine("=============================================");
    Console.WriteLine("List of Airlines for Changi Airport Terminal 5");
    Console.WriteLine("=============================================");
    Console.WriteLine($"{"Airline Code",-15} {"Airline Name",-30}");

    foreach (var airline in airlineDictionary.Values)
    {
        Console.WriteLine($"{airline.Code,-15} {airline.Name,-30}");
    }

    // Get user input for airline code
    Console.Write("\nEnter Airline Code: ");
    string airlineCode = Console.ReadLine().ToUpper();

    // Check if the airline code exists in the dictionary
    if (airlineDictionary.TryGetValue(airlineCode, out Airline selectedAirline))
    {
        // Display flights for the selected airline
        Console.WriteLine("\n=============================================");
        Console.WriteLine($"List of Flights for {selectedAirline.Name}");
        Console.WriteLine("=============================================");
        Console.WriteLine($"{"Flight Number",-15} {"Airline Name",-20} {"Origin",-20} {"Destination",-20} {"Expected Departure/Arrival Time",-30}");

        // Get the list of flights for the selected airline (convert AirlineCode to uppercase to avoid case mismatch)
        List<Flight> airlineFlights = new List<Flight>();
        foreach (KeyValuePair <string,Flight> flights in flightDictionary)
        {
            string Fobject = flights.Key;
            if (Fobject.StartsWith(airlineCode))
            {
                airlineFlights.Add(flights.Value);
            }
        }

        // Check if there are any flights for the selected airline
        if (airlineFlights.Count == 0)
        {
            Console.WriteLine("No flights found for this airline.");
        }
        else
        {
            // Display each flight's details
            foreach (var flight in airlineFlights)
            {
                Console.WriteLine($"{flight.FlightNumber,-15} {selectedAirline.Name,-20} {flight.Origin,-20} {flight.Destination,-20} {flight.ExpectedTime.ToString("dd/MM/yyyy hh:mm tt"),-30}");
            }
        }
    }
    else
    {
        Console.WriteLine("Invalid Airline Code.");
    }

    // Wait for user input to return to the menu
    Console.WriteLine("\nPress any key to return to the menu...");
    Console.ReadKey();
}


//basic feature 8 -- Meenakshi

static void ModifyFlightDetails(Dictionary<string, Airline> airlineDictionary, Dictionary<string, Flight> flightDictionary)
{
    Console.WriteLine("=============================================");
    Console.WriteLine("List of Airlines for Changi Airport Terminal 5");
    Console.WriteLine("=============================================");
    Console.WriteLine($"{"Airline Code",-15} {"Airline Name",-30}");

    foreach (var airline in airlineDictionary.Values)
    {
        Console.WriteLine($"{airline.Code,-15} {airline.Name,-30}");
    }

    // Get user input for airline code
    Console.Write("\nEnter Airline Code: ");
    string airlineCode = Console.ReadLine().ToUpper();

    if (airlineDictionary.TryGetValue(airlineCode, out Airline selectedAirline))
    {
        Console.WriteLine($"\nList of Flights for {selectedAirline.Name}");
        Console.WriteLine("Flight Number  Airline Name  Origin          Destination      Expected ");

        // Get the list of flights for the selected airline (convert AirlineCode to uppercase to avoid case mismatch)
        List<Flight> airlineFlights = new List<Flight>();
        foreach (KeyValuePair<string, Flight> flights in flightDictionary)
        {
            string Fobject = flights.Key;
            if (Fobject.StartsWith(airlineCode))
            {
                airlineFlights.Add(flights.Value);
            }
        }


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

// basic feature 9 -- Claire
static void DisplayFlightSchedule(Dictionary<string, Flight> flightDictionary, Dictionary<string, Airline> airlineDictionary)
{
    Console.WriteLine("=============================================");
    Console.WriteLine("Flight Schedule for Changi Airport Terminal 5");
    Console.WriteLine("=============================================");
    List<Flight> sortedflights = new List<Flight>(flightDictionary.Values);

    sortedflights.Sort();

    Console.WriteLine("{0,-16}{1,-23}{2,-23}{3,-23}{4,-22}", "Flight Number", "Airline Name", "Origin", "Destination", "Expected Departure/Arrival Time");
    foreach (Flight flight in sortedflights)
    {
        if (flight.FlightNumber.StartsWith("SQ"))
        {
            string airlineName = (airlineDictionary["SQ"]).Name;
            Console.WriteLine("{0,-16}{1,-23}{2,-23}{3,-23}{4,-22}",flight.FlightNumber, airlineName, flight.Origin, flight.Destination, flight.ExpectedTime);
        }
        else if (flight.FlightNumber.StartsWith("MH"))
        {
            string airlineName = airlineDictionary["MH"].Name;
            Console.WriteLine("{0,-16}{1,-23}{2,-23}{3,-23}{4,-22}", flight.FlightNumber, airlineName, flight.Origin, flight.Destination, flight.ExpectedTime);
        }
        else if (flight.FlightNumber.StartsWith("JL"))
        {
            string airlineName = airlineDictionary["JL"].Name;
            Console.WriteLine("{0,-16}{1,-23}{2,-23}{3,-23}{4,-22}", flight.FlightNumber, airlineName, flight.Origin, flight.Destination, flight.ExpectedTime);
        }
        else if (flight.FlightNumber.StartsWith("CX"))
        {
            string airlineName = airlineDictionary["CX"].Name;
            Console.WriteLine("{0,-16}{1,-23}{2,-23}{3,-23}{4,-22}", flight.FlightNumber, airlineName, flight.Origin, flight.Destination, flight.ExpectedTime);
        }
        else if (flight.FlightNumber.StartsWith("QF"))
        {
            string airlineName = airlineDictionary["QF"].Name;
            Console.WriteLine("{0,-16}{1,-23}{2,-23}{3,-23}{4,-22}", flight.FlightNumber, airlineName, flight.Origin, flight.Destination, flight.ExpectedTime);
        }
        else if (flight.FlightNumber.StartsWith("TR"))
        {
            string airlineName = airlineDictionary["TR"].Name;
            Console.WriteLine("{0,-16}{1,-23}{2,-23}{3,-23}{4,-22}", flight.FlightNumber, airlineName, flight.Origin, flight.Destination, flight.ExpectedTime);
        }
        else if (flight.FlightNumber.StartsWith("EK"))
        {
            string airlineName = airlineDictionary["EK"].Name;
            Console.WriteLine("{0,-16}{1,-23}{2,-23}{3,-23}{4,-22}", flight.FlightNumber, airlineName, flight.Origin, flight.Destination, flight.ExpectedTime);
        }
        else if (flight.FlightNumber.StartsWith("BA"))
        {
            string airlineName = airlineDictionary["BA"].Name;
            Console.WriteLine("{0,-16}{1,-23}{2,-23}{3,-23}{4,-22}", flight.FlightNumber, airlineName, flight.Origin, flight.Destination, flight.ExpectedTime);
        }
    }

}


//PROGRAM CODE

//Basic Feature 1 -- Meenakshi
// Load files and initialize dictionaries
Console.WriteLine("Loading Airlines. . .");
Dictionary<string, Airline> airlineDictionary = LoadAirlines("airlines.csv");
Console.WriteLine("{0} Airlines Loaded!", airlineDictionary.Count);

Console.WriteLine("Loading Boarding Gates. . .");
Dictionary<string, BoardingGate> boardingGateDictionary = LoadBoardingGates("boardinggates.csv");
Console.WriteLine("{0} Boarding Gates Loaded!", boardingGateDictionary.Count);

//basic feature 2 -- Claire
Console.WriteLine("Loading Flights. . .");
Dictionary<string, Flight> flightDictionary = LoadFlights("flights.csv");
Console.WriteLine("{0} Flights Loaded!", flightDictionary.Count);




// Menu Loop
bool flag = true;
do
{
    Console.WriteLine();
    Console.WriteLine();
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

    while (option != "1" && option != "2" && option != "3" && option != "4" && option != "5" && option != "6" && option != "7" && option != "0")
    {
        Console.WriteLine("\nInput invalid. Try Again.");
        Console.Write("Please select your option: ");
        option = Console.ReadLine();
    }

    if (option == "1")
        ListAllFlights(airlineDictionary, flightDictionary);
    else if (option == "2")
        ListBoardingGates(boardingGateDictionary);
    else if (option == "3")
        AssignBoardingGate(flightDictionary, boardingGateDictionary);
    else if (option == "4")
        CreateFlight(flightDictionary,airlineDictionary);
    else if (option == "5")
        DisplayAirlineFlights(airlineDictionary, flightDictionary);
    else if (option == "6")
        ModifyFlightDetails(airlineDictionary, flightDictionary);
    else if (option == "7")
        DisplayFlightSchedule(flightDictionary, airlineDictionary);
    else
    {
        Console.WriteLine("Goodbye!");
        flag = false;
    }
        
} while (flag == true);
