using VehicleLibrary1;
using System;

namespace Task1
{
    internal class Program
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

        static IEnumerable<Vehicle> GetExpensiveCars(Queue<List<Vehicle>> factory)
        {
            var expensiveCars = from workshop in factory
                                from car in workshop
                                where car.Price > 400000
                                select car;

            if (expensiveCars.Any())
            {
                return expensiveCars;
            }
            else
            {
                Console.WriteLine("Таких машин нет");
                return Enumerable.Empty<Vehicle>();
            }
        }

        static void PrintAveragePrice(Queue<List<Vehicle>> factory)
        {
            var averagePrice = (from workshop in factory
                                from car in workshop
                                select car.Price).Average();
            Console.WriteLine($"Средняя цена автомобилей: {averagePrice}");
        }

        static void PrintCarsByBrand(Queue<List<Vehicle>> factory)
        {
            var carsByBrand = from workshop in factory
                              from car in workshop
                              group car by car.Brand into brandGroup
                              select new { Brand = brandGroup.Key, Count = brandGroup.Count() };
            foreach (var group in carsByBrand)
            {
                Console.WriteLine($"Марка: {group.Brand}, Количество: {group.Count}");
            }
        }

        static void PrintExpensiveCars(Queue<List<Vehicle>> factory)
        {
            var expensiveSedans = from workshop in factory
                                  from car in workshop
                                  let isExpensiveSedan = car is Car && car.Price > 300000
                                  where isExpensiveSedan
                                  select car;
            foreach (var car in expensiveSedans)
            {
                Console.WriteLine($"Марка: {car.Brand}, Цена: {car.Price}");
            }
        }

        static void JoinLinq(Queue<List<Vehicle>> factory, List<Driver> drivers)
        {
            var union = from driver in drivers
                      join car in factory.SelectMany(t => t) on driver.BrandCar equals car.Brand
                      select new { Name = driver.Name, Brand = driver.BrandCar, Price = car.Price };

            foreach (var item in union)
            {
                Console.WriteLine($"Имя: {item.Name}, Модель: {item.Brand}, Цена: {item.Price}");
            }
            Console.WriteLine();
        }

        public static IEnumerable<Vehicle> GetExpensiveCarsExtension(Queue<List<Vehicle>> factory)
        {
            return factory.SelectMany(workshop => workshop).Where(car => car.Price > 400000);
        }

        static void PrintAveragePriceExtension(Queue<List<Vehicle>> factory)
        {
            var averagePrice = factory.SelectMany(workshop => workshop).Average(car => car.Price);
            Console.WriteLine($"Средняя цена автомобилей: {averagePrice}");
        }

        static void PrintCarsByBrandExtension(Queue<List<Vehicle>> factory)
        {
            var carsByBrand = factory.SelectMany(workshop => workshop).GroupBy(car => car.Brand).Select(brandGroup => new { Brand = brandGroup.Key, Count = brandGroup.Count() });
            foreach (var group in carsByBrand)
            {
                Console.WriteLine($"Марка: {group.Brand}, Количество: {group.Count}");
            }
        }

        static void PrintExpensiveCarsExtension(Queue<List<Vehicle>> factory)
        {
            var expensiveSedans = factory.SelectMany(workshop => workshop).Where(car => car is Car && car.Price > 300000).Select(car => car);
            foreach (var car in expensiveSedans)
            {
                Console.WriteLine($"Марка: {car.Brand}, Цена: {car.Price}");
            }
        }

        static void JoinExtension(Queue<List<Vehicle>> facrory, List<Driver> drivers)
        {
            var union = drivers.Join(facrory.SelectMany(c => c), p => p.BrandCar, v => v.Brand, (p, v) => new { Name = p.Name, Brand = p.BrandCar, Price = v.Price });

            foreach (var item in union)
            {
                Console.WriteLine($"Имя: {item.Name}, Модель: {item.Brand}, Цена: {item.Price}");
            }
            Console.WriteLine();
        }

        static void MakeListDrivers(List<Driver> drivers)
        {
            Random random = new Random();
            int count = random.Next(4);
            for (int i = 0; i < count; i++)
            {
                Driver driver = new Driver();
                driver.RandomInit();

                drivers.Add(driver);
            }
        }

        static void ShowDrivers(List<Driver> drivers)
        {
            foreach (var driver in drivers)
            {
                Console.WriteLine(drivers.ToString());
            }
        }

        static void Main(string[] args)
        {
            Queue<List<Vehicle>> factory = new Queue<List<Vehicle>>();

            Console.WriteLine("Введите количество машин, которые хотите создать:");
            int count = int.Parse(Console.ReadLine());

            List<Vehicle> workshop1 = new List<Vehicle>();
            for (int i = 0; i < count; i++)
            {
                workshop1.Add(CreateObject());
            }

            List<Vehicle> workshop2 = new List<Vehicle>();
            for (int i = 0; i < count; i++)
            {
                workshop2.Add(CreateObject());
            }
            factory.Enqueue(workshop1);
            factory.Enqueue(workshop2);

            foreach (var car in factory)
            {
                PrintVehicles(car);
            }

            while (true)
            {
                Console.WriteLine("1. Запрос Where (LINQ)");
                Console.WriteLine("2. Запрос Agregation (LINQ)");
                Console.WriteLine("3. Запрос Group By (LINQ)");
                Console.WriteLine("4. Запрос Let (LINQ)");
                Console.WriteLine("5. Запрос Join (LINQ)");
                Console.WriteLine("6. Запрос Where (Методы расширения)");
                Console.WriteLine("7. Запрос Agregation (Методы расширения)");
                Console.WriteLine("8. Запрос Group By (Методы расширения)");
                Console.WriteLine("9. Запрос Let (Методы расширения)");
                Console.WriteLine("10. Запрос Join (Методы расширения)");
                Console.WriteLine("11. Выход");
                Console.Write("Введите номер действия: ");

                int choice = int.Parse(Console.ReadLine());

                switch (choice)
                {
                    case 1:
                        Console.WriteLine("Запрос Where (LINQ)");
                        PrintVehicles(GetExpensiveCars(factory));
                        break;
                    case 2:
                        Console.WriteLine("Запрос Agregation (LINQ)");
                        PrintAveragePrice(factory);
                        break;
                    case 3:
                        Console.WriteLine("Запрос Group By (LINQ)");
                        PrintCarsByBrand(factory);
                        break;
                    case 4:
                        Console.WriteLine("Запрос Let (LINQ)");
                        PrintExpensiveCars(factory);
                        break;
                    case 5:
                        Console.WriteLine("Запрос Join (LINQ)");
                        List<Driver> drivers = new List<Driver>();
                        MakeListDrivers(drivers);
                        ShowDrivers(drivers);
                        JoinLinq(factory, drivers);
                        break;
                    case 6:
                        Console.WriteLine("Запрос Where (Методы расширения)");
                        PrintVehicles(GetExpensiveCarsExtension(factory));
                        break;
                    case 7:
                        Console.WriteLine("Запрос Agregation (Методы расширения)");
                        PrintAveragePriceExtension(factory);
                        break;
                    case 8:
                        Console.WriteLine("Запрос Group By (Методы расширения)");
                        PrintCarsByBrandExtension(factory);
                        break;
                    case 9:
                        Console.WriteLine("Запрос Let (Методы расширения)");
                        PrintExpensiveCarsExtension(factory);
                        break;
                    case 10:
                        Console.WriteLine("Запрос Join (Методы расширения)");
                        List<Driver> drivers2 = new List<Driver>();
                        MakeListDrivers(drivers2);
                        ShowDrivers(drivers2);
                        JoinExtension(factory, drivers2);
                        break;
                    case 11:
                        Console.WriteLine("Выход из программы.");
                        return;
                    default:
                        Console.WriteLine("Неверный выбор.");
                        break;
                }
            }
        }
    }
}