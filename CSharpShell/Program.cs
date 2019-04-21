using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpShell
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            Shell shell = new Shell();
            shell.Run();

#if DEBUG
            Console.WriteLine();
            Console.Write("Press any key to exit...");
            Console.ReadKey();
#endif
        }
    }
}
