using System;
using System.Collections.Generic;
using System.Linq;

public class Program
{
    static int parkingLotCapacity;
    static Vehicle[] parkingSlots;

    public static void Main()
    {
        string command;
        do
        {
            command = Console.ReadLine();
            ExecuteCommand(command);
        } while (command != "exit");
    }

    static void ExecuteCommand(string command)
    {
        string[] splitCommand = command.Split(' ');

        switch (splitCommand[0])
        {
            case "create_parking_lot":
                parkingLotCapacity = Convert.ToInt32(splitCommand[1]);
                parkingSlots = new Vehicle[parkingLotCapacity];
                Console.WriteLine($"Created a parking lot with {parkingLotCapacity} slots");
                break;
            case "park":
                ParkVehicle(new Vehicle(splitCommand[1], splitCommand[2], splitCommand[3]));
                break;
            case "leave":
                LeaveSlot(Convert.ToInt32(splitCommand[1]));
                break;
            case "status":
                PrintParkingLotStatus();
                break;
            case "type_of_vehicles":
                PrintVehicleTypeCount(splitCommand[1]);
                break;
            case "registration_numbers_for_vehicles_with_colour":
                PrintRegistrationNumbersForColor(splitCommand[1]);
                break;
            case "slot_numbers_for_vehicles_with_colour":
                PrintSlotNumbersForColor(splitCommand[1]);
                break;
            case "slot_number_for_registration_number":
                PrintSlotNumberForRegistrationNumber(splitCommand[1]);
                break;
            case "exit":
                break;
            default:
                Console.WriteLine("Invalid command");
                break;
        }
    }

    static void ParkVehicle(Vehicle vehicle)
    {
        int slotNumber = GetNextAvailableSlot();
        if (slotNumber == -1)
        {
            Console.WriteLine("Sorry, parking lot is full");
            return;
        }

        parkingSlots[slotNumber] = vehicle;
        Console.WriteLine($"Allocated slot number: {slotNumber + 1}");
    }

    static void LeaveSlot(int slotNumber)
    {
        if (IsValidSlotNumber(slotNumber) && parkingSlots[slotNumber - 1] != null)
        {
            parkingSlots[slotNumber - 1] = null;
            Console.WriteLine($"Slot number {slotNumber} is free");
        }
        else
        {
            Console.WriteLine($"Slot number {slotNumber} not found");
        }
    }

    static void PrintParkingLotStatus()
    {
        Console.WriteLine("Slot\tNo.\tType\tRegistration No\tColour");
        for (int i = 0; i < parkingLotCapacity; i++)
        {
            if (parkingSlots[i] != null)
            {
                Console.WriteLine($"{i + 1}\t{parkingSlots[i].Type}\t{parkingSlots[i].RegistrationNumber}\t{parkingSlots[i].Color}");
            }
        }
    }

    static void PrintVehicleTypeCount(string type)
    {
        int count = parkingSlots.Count(vehicle => vehicle != null && vehicle.Type.Equals(type, StringComparison.OrdinalIgnoreCase));
        Console.WriteLine(count);
    }

    static void PrintRegistrationNumbersForColor(string color)
    {
        List<string> registrationNumbers = parkingSlots
            .Where(vehicle => vehicle != null && vehicle.Color.Equals(color, StringComparison.OrdinalIgnoreCase))
            .Select(vehicle => vehicle.RegistrationNumber)
            .ToList();

        Console.WriteLine(string.Join(", ", registrationNumbers));
    }

    static void PrintSlotNumbersForColor(string color)
    {
        List<int> slotNumbers = parkingSlots
            .Select((vehicle, index) => new { Vehicle = vehicle, Index = index + 1 })
            .Where(entry => entry.Vehicle != null && entry.Vehicle.Color.Equals(color, StringComparison.OrdinalIgnoreCase))
            .Select(entry => entry.Index)
            .ToList();

        Console.WriteLine(string.Join(", ", slotNumbers));
    }

    static void PrintSlotNumberForRegistrationNumber(string regNumber)
    {
        int slotNumber = parkingSlots
            .Select((vehicle, index) => new { Vehicle = vehicle, Index = index + 1 })
            .Where(entry => entry.Vehicle != null && entry.Vehicle.RegistrationNumber.Equals(regNumber, StringComparison.OrdinalIgnoreCase))
            .Select(entry => entry.Index)
            .FirstOrDefault();

        if (slotNumber != 0)
        {
            Console.WriteLine(slotNumber);
        }
        else
        {
            Console.WriteLine("Not found");
        }
    }

    static bool IsValidSlotNumber(int slotNumber)
    {
        return slotNumber >= 1 && slotNumber <= parkingLotCapacity;
    }

    static int GetNextAvailableSlot()
    {
        for (int i = 0; i < parkingLotCapacity; i++)
        {
            if (parkingSlots[i] == null)
            {
                return i;
            }
        }
        return -1;
    }
}

public class Vehicle
{
    public string RegistrationNumber { get; }
    public string Color { get; }
    public string Type { get; }

    public Vehicle(string registrationNumber, string color, string type)
    {
        RegistrationNumber = registrationNumber;
        Color = color;
        Type = type;
    }
}
