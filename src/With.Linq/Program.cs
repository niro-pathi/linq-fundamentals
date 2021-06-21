using System;
using System.IO;
using System.Linq;

namespace With.Linq
{
    class Program
    {
        static void Main(string[] args)
        {
            string path = @"/Users/np351100/";
            ShowLargeFilesWithLinq(path);

        }

        private static void ShowLargeFilesWithLinq(string path)
        {
            var query = new DirectoryInfo(path).GetFiles()
                        .OrderByDescending(f => f.Length)
                        .Take(5);

            foreach (var file in query.Take(5))
            {
                Console.WriteLine($"{file.Name,-20} : {file.Length,10:N0}");
            }
        }
    }
}
