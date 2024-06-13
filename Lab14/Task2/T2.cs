using System;
using T4;
using VehicleLibrary1;

class Program
{
    static Vehicle CreateObject()
    {
        Random rand = new Random();
        int answ = rand.Next(1, 4);
        Vehicle car;
        if (answ == 1)
            car = new Car();
        else if (answ == 2)
            car = new SUV();
        else
            car = new Truck();
        car.RandomInit();
        return car;
    }

    static void PrintVehicles(IEnumerable<Vehicle> vehicles)
    {
        foreach (var vehicle in vehicles)
        {
            Console.WriteLine(vehicle.ToString());
        }
    }

    static void LinqQueries(MyCollection<Vehicle> vehicles)
    {
        Console.WriteLine("Запросы LINQ");
        PrintNewerVehiclesLinq(vehicles);
        PrintTotalVehicleCountLinq(vehicles);
        PrintAggregationResultsLinq(vehicles);
        PrintVehiclesGroupedByYearLinq(vehicles);
    }

    static void PrintNewerVehiclesLinq(MyCollection<Vehicle> vehicles)
    {
        Console.WriteLine("Машины, выпущенные после 2015 года:");
        var newerVehiclesLinq = from v in vehicles
                                where v.Year > 2015
                                select v;
        PrintVehicles(newerVehiclesLinq);
    }

    static void PrintTotalVehicleCountLinq(MyCollection<Vehicle> vehicles)
    {
        Console.WriteLine("\nОбщее количество машин: " + (from v in vehicles select v).Count());
    }

    static void PrintAggregationResultsLinq(MyCollection<Vehicle> vehicles)
    {
        Console.WriteLine("\nСредняя цена машин: " + (from v in vehicles select v.Price).Average());
        Console.WriteLine("Самая дорогая машина: " + (from v in vehicles select v.Price).Max());
        Console.WriteLine("Самая дешевая машина: " + (from v in vehicles select v.Price).Min());
        Console.WriteLine("Суммарная цена всех машин: " + (from v in vehicles select v.Price).Sum());
    }

    static void PrintVehiclesGroupedByYearLinq(MyCollection<Vehicle> vehicles)
    {
        Console.WriteLine("\nМашины, сгруппированные по году выпуска:");
        var groupedByYearLinq = from v in vehicles
                                group v by v.Year into yearGroup
                                select new { Year = yearGroup.Key, Vehicles = yearGroup };
        foreach (var group in groupedByYearLinq)
        {
            Console.WriteLine("Год: " + group.Year);
            foreach (var vehicle in group.Vehicles)
            {
                Console.WriteLine(vehicle.ToString());
            }
        }
    }

    static void ExtensionMethods(MyCollection<Vehicle> vehicles)
    {
        Console.WriteLine("\nМетоды расширения");
        PrintNewerVehiclesExtension(vehicles);
        PrintTotalVehicleCountExtension(vehicles);
        PrintAggregationResultsExtension(vehicles);
        PrintVehiclesGroupedByYearExtension(vehicles);
    }

    static void PrintNewerVehiclesExtension(MyCollection<Vehicle> vehicles)
    {
        Console.WriteLine("\nМашины, выпущенные после 2015 года:");
        var newerVehiclesExtension = vehicles.Where(v => v.Year > 2015);
        PrintVehicles(newerVehiclesExtension);
    }

    static void PrintTotalVehicleCountExtension(MyCollection<Vehicle> vehicles)
    {
        Console.WriteLine("\nОбщее количество машин: " + vehicles.Count());
    }

    static void PrintAggregationResultsExtension(MyCollection<Vehicle> vehicles)
    {
        Console.WriteLine("\nСредняя цена машин: " + vehicles.Average(v => v.Price));
        Console.WriteLine("Самая дорогая машина: " + vehicles.Max(v => v.Price));
        Console.WriteLine("Самая дешевая машина: " + vehicles.Min(v => v.Price));
        Console.WriteLine("Суммарная цена всех машин: " + vehicles.Sum(v => v.Price));
    }

    static void PrintVehiclesGroupedByYearExtension(MyCollection<Vehicle> vehicles)
    {
        Console.WriteLine("\nМашины, сгруппированные по году выпуска:");
        var groupedByYearExtension = vehicles.GroupBy(v => v.Year);
        foreach (var group in groupedByYearExtension)
        {
            Console.WriteLine("Год: " + group.Key);
            foreach (var vehicle in group)
            {
                Console.WriteLine(vehicle.ToString());
            }
        }
    }

    static void CreateCollection(MyCollection<Vehicle> vehicles)
    {
        Console.WriteLine("Введите количество машин, которые хотите создать (максимум 11):");
        int count = int.Parse(Console.ReadLine());

        List<Vehicle> list = new List<Vehicle>();
        for (int i = 0; i < count; i++)
        {
            list.Add(CreateObject());
        }

        vehicles.AddRange(list);
        vehicles.Show();
    }

    static void Main(string[] args)
    {
        MyCollection<Vehicle> vehicles = new MyCollection<Vehicle>();

        while (true)
        {
            Console.WriteLine("1. Создать коллекцию машин");
            Console.WriteLine("2. Выполнить запросы LINQ");
            Console.WriteLine("3. Выполнить запросы с использованием методов расширения");
            Console.WriteLine("4. Выход");
            Console.Write("Введите номер действия: ");

            int choice = int.Parse(Console.ReadLine());

            switch (choice)
            {
                case 1:
                    CreateCollection(vehicles);
                    break;
                case 2:
                    LinqQueries(vehicles);
                    break;
                case 3:
                    ExtensionMethods(vehicles);
                    break;
                case 4:
                    Console.WriteLine("Выход из программы.");
                    return;
                default:
                    Console.WriteLine("Неверный выбор.");
                    break;
            }
        }
    }
}