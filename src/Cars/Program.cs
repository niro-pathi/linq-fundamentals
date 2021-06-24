using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Cars
{
    class Program
    {
        static void Main(string[] args)
        {
            var manufacturers = ProcessManufactureers("src/Cars/manufacturers.csv");

            //Extension Method
            var cars = ProcessFileExtensionMethod("src/Cars/fuel.csv");


            // Find most fuel efficent cars
            //var query = cars.Join(manufacturers,
            //                        c => new { c.Manufacturer, c.Year },
            //                        m => new { Manufacturer = m.Name, m.Year },
            //                        (c, m) => new
            //                        {
            //                            m.Headquarters,
            //                            c.Name,
            //                            c.Combined
            //                        })

            //            .OrderByDescending(c => c.Combined)
            //            .ThenBy(c => c.Name);

            // Find first car
            //var topcar = fileOutput.Where(c => c.Manufacturer == "BMW" && c.Year >= 2016)
            //                     .OrderByDescending(c => c.Combined)
            //                     .ThenBy(c => c.Name)
            //                     .Select(c => c)
            //                     .First();

            //Console.WriteLine($"Top Car : {topcar.Name}" );

            ////Any cars with manifacturer BMW
            //var result = cars.Any(c => c.Manufacturer == "BMW");
            //Console.WriteLine($"Do we have cars manufactured by BMW?  {result}");

            //Filter by manufacturer
            //var query =
            //    cars.GroupBy(c => c.Manufacturer.ToUpper())
            //    .OrderBy(g => g.Key);

            //Filter by manufacturer and headquarters
            //var query=
            //    manufacturers.GroupJoin(cars, m => m.Name.ToUpper(), c => c.Manufacturer.ToUpper(),
            //            (m, g) =>
            //                new
            //                {
            //                    Manufacturer = m,
            //                    Cars = g
            //                })
            //    .GroupBy(m => m.Manufacturer.Headquarters);

            var query =
                cars.GroupBy(c => c.Manufacturer)
                .Select(g =>
                {
                    var results = g.Aggregate(new CarStatistics(),
                                    (acc, c) => acc.Accumulate(c),
                                    acc => acc.Compute());
                    return new
                    {
                        Name = g.Key,
                        Average = results.Average,
                        Min = results.Min,
                        Max = results.Max

                    };
                })
                .OrderByDescending(r => r.Max);

            // Query Syntex example
            //var cars = ProcessFileQuerySyntex("src/Cars/fuel.csv");


            // Find most fuel efficent cars
            //var query = from car in cars
            //            join manufacturer in manufacturers
            //                on new {car.Manufacturer, car.year}
            //                equals
            //                {manufacturer = manufacturer.Name, manufacturer.Year}
            //            orderby car.Combined descending, car.Name
            //            select new
            //            {
            //                manufacturer.Headquarters,
            //                car.Name,
            //                car.Combined
            //            };

            //Filter by manufacturer
            //var query =
            //        from car in cars
            //        group car by car.Manufacturer.ToUpper() into manufacturer
            //        orderby manufacturer.Key
            //        select manufacturer;

            //Filter by manufacturer and headquarters
            //var query =
            //    from manufacturer in manufacturers
            //    join car in cars on manufacturer.Name equals car.Manufacturer
            //        into carGroup
            //    select new
            //    {
            //        Manufacturer = manufacturer,
            //        Cars = carGroup
            //    } into result
            //    group result by result.Manufacturer.Headquarters;

            //var query2 =
            //        from car in cars
            //        group car by car.Manufacturer into carGroup
            //        select new
            //        {
            //            Name = carGroup.Key,
            //            Max = carGroup.Max(C => C.Combined),
            //            Min = carGroup.Min(C => C.Combined),
            //            Mvg = carGroup.Average(C => C.Combined),
            //        } into result
            //        orderby result.Max descending
            //        select result;


            // Display the top 10 result

            //foreach (var car in query.Take(10))
            //{
            //    Console.WriteLine($"{car.Headquarters} {car.Name} : {car.Combined}");
            //}

            //Display group by

            //foreach (var group in query)
            //{
            //    Console.WriteLine($"{group.Key}");

            //    foreach (var car in group.SelectMany(g => g.Cars)
            //                                .OrderByDescending(c => c.Combined).Take(3))
            //    {
            //        Console.WriteLine($"\t {car.Name} : {car.Combined}");
            //    }
            //}

            //Display manufacturer by fuel efficency
            foreach (var result in query)
            {
                Console.WriteLine($"{result.Name}");
                Console.WriteLine($"\t Max: {result.Max}");
                Console.WriteLine($"\t Min: {result.Min}");
                Console.WriteLine($"\t Avg: {result.Average}");
            }

        }

        public class CarStatistics
        {
            public CarStatistics()
            {
                Max = Int32.MinValue;
                Min = Int32.MaxValue;

            }

            public CarStatistics Accumulate (Car car)
            {

                Total += car.Combined;
                Count++;
                Max = Math.Max(Max, car.Combined);
                Min = Math.Min(Min, car.Combined);

                return this;
            }

            public CarStatistics Compute()
            {
                Average = Total / Count;
                return this;
            }

            public int Max { get; set; }
            public int Min { get; set; }
            public int Total { get; set; }
            public int Count { get; set; }
            public double Average { get; set; }

            
        }

        private static List<Car> ProcessFileQuerySyntex(string path)
        {
            var query =
                from line in File.ReadAllLines(path).Skip(1)
                where line.Length > 1
                select Car.ParseFormCSV(line);


            return query.ToList();
        }

        private static List<Car> ProcessFileExtensionMethod(string path)
        {
            return File.ReadAllLines(path)
                 .Skip(1)
                 .Where(l => l.Length > 1)
                 .Select(l => Car.ParseFormCSV(l))
                 .ToList();
        }

        private static List<Manufacturer> ProcessManufactureers(string path)
        {
            var query =
                File.ReadAllLines(path)
                    .Where(l => l.Length > 1)
                    .Select(l =>
                    {
                        var columns = l.Split(',');
                        return new Manufacturer
                        {
                            Name = columns[0],
                            Headquarters = columns[1],
                            Year = int.Parse(columns[2])
                        };
                    });

            return query.ToList();
        }
    }
}
