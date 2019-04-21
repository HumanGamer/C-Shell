using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiveCode
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            Interface obj = new Interface();

            while (obj.Running)
            {
                Console.Write("> ");

                string line = Console.ReadLine();
                obj.ProcessLine(line);
            }

            Console.WriteLine();
            Console.Write("Press any key to exit...");
            Console.ReadKey();
        }
    }
}
