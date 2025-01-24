using PRG2_Assignment;

Console.WriteLine("Loading Airlines...");
Console.WriteLine("8 Airlines Loaded!");
Console.WriteLine("Loading Boarding Gates...");
Console.WriteLine("66 Boarding Gates Loaded!");
Console.WriteLine("Loading Flights...");
Console.WriteLine("30 Flights Loaded!");

Console.WriteLine("=============================================");
Console.WriteLine("Welcome to Changi Airport Terminal 5");
Console.WriteLine("=============================================");
while (true)
{
    Console.WriteLine("1. List All Flights");
    Console.WriteLine("2. List Boarding Gates");
    Console.WriteLine("3. Assign a Boarding Gate to a Flight");
    Console.WriteLine("4. Create Flight");
    Console.WriteLine("5. Display Airline Flights");
    Console.WriteLine("6. Modify Flight Details");
    Console.WriteLine("7. Display Flight Schedule");
    Console.WriteLine("0. Exit");
    Console.Write("Please select your option: ");

    int choice = Convert.ToInt32(Console.ReadLine());

    if (choice == 1)
    {
        ListFlights(flights);
    }
    else if (choice == 0)
    {
        break;
    }

}
        