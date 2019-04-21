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

            if (args.Length > 0)
                shell.RunScriptFile(args[0]);
            else
                shell.Run();

#if DEBUG
            Console.WriteLine();
            Console.Write("Press any key to exit...");
            Console.ReadKey();
#endif
        }
    }
}
