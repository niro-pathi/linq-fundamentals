using System;
using System.Collections.Generic;
using System.Linq;

namespace Linq.Sample01
{
    class Program
    {
        static void Main(string[] args)
        {

            IEnumerable<Employee> developers = new Employee[]
            {
                new Employee {id = 1, Name= "Scott"},
                new Employee {id = 2, Name= "Chris"},
                new Employee {id = 2, Name= "Tom"}
            };

            IEnumerable<Employee> sales = new List<Employee>()
            {
                new Employee { id = 3, Name = "Alex" }
            };

            IEnumerator<Employee> enumerator = developers.GetEnumerator();
            while (enumerator.MoveNext())
            {
                Console.WriteLine(enumerator.Current.Name);
            }

            // Lambda expression
            foreach (var employee in developers.Where(
                            e => e.Name.StartsWith("S")))
            {
                Console.WriteLine(employee.Name);
            }

            //Display only developers with 5 letters name with method syntex
            foreach (var employee in developers.Where(
                            e => e.Name.Length == 5)
                            .OrderBy(e => e.Name))
            {
                Console.WriteLine(employee.Name);
            }

            //Display only developers with 3 letters name with query syntex
            var query = from developer in developers
                        where developer.Name.Length == 3
                        orderby developer.Name
                        select developer;


            foreach (var employee in query)
            {
                Console.WriteLine(employee.Name);
            }


        }
    }
}
