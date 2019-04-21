using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using Microsoft.CSharp;

namespace LiveCode
{
    public class Interface
    {
        protected List<string> Lines;

        public bool Running { get; private set; }

        public Interface()
        {
            Lines = new List<string>();
            Running = true;
        }

        public void ProcessLine(string line)
        {
            if (line == "?q")
            {
                Console.WriteLine("Exiting...");
                Running = false;
                return;
            }

            try
            {
                ScriptOptions options = ScriptOptions.Default;

                var result = CSharpScript.EvaluateAsync(line, options);
                if (result != null)
                    Console.WriteLine(result.Result.ToString());

            }
            catch (CompilationErrorException ex)
            {
                Console.BackgroundColor = ConsoleColor.Red;
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine(ex.Message);
                Console.ResetColor();
            }
            catch (Exception ex)
            {
                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine(ex.Message + ": " + ex.StackTrace);
                Console.ResetColor();
            }
        }
    }
}
