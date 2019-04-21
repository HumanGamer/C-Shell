using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using Microsoft.CSharp;

namespace CSharpShell
{
    public class Shell
    {
        protected List<string> Lines;

        private string Script
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                foreach (var line in Lines)
                {
                    sb.AppendLine(line);
                }

                return sb.ToString();
            }
        }

        public bool Running { get; private set; }

        public Shell()
        {
            Lines = new List<string>();
        }

        public void Run()
        {
            Running = true;
            while (Running)
            {
                Console.Write("> ");

                string line = Console.ReadLine();
                ProcessLine(line);
            }
        }

        protected void ProcessLine(string line)
        {
            if (line.StartsWith("?"))
            {
                switch (line.Substring(1))
                {
                    case "":
                    case "h":
                        Console.WriteLine("All commands are prefixed by a '?' symbol.");
                        Console.WriteLine();

                        Console.WriteLine("Command List:");
                        Console.WriteLine("h - Show This Help");
                        Console.WriteLine("q - Quit");
                        Console.WriteLine("e - Execute Code");
                        Console.WriteLine("x - Clear Buffer");
                        Console.WriteLine("c - Clear Console");
                        Console.WriteLine("v - View Buffer");
                        Console.WriteLine("s - Save Script");
                        Console.WriteLine("l - Load Script");
                        break;
                    case "q":
                        Console.WriteLine("Exiting...");
                        Running = false;
                        break;
                    case "e":
                        RunScript(Script);
                        break;
                    case "x":
                        Console.WriteLine("Clearing Buffer");
                        Lines.Clear();
                        break;
                    case "c":
                        Console.Clear();
                        break;
                    case "v":
                        Console.WriteLine("Viewing Buffer");
                        Console.WriteLine(Script);
                        break;
                    case "s":
                        Console.WriteLine("Saving Not Yet Implemented");
                        // TODO: Saving
                        break;
                    case "l":
                        Console.WriteLine("Loading Not Yet Implemented");
                        // TODO: Loading
                        break;
                    default:
                        Console.WriteLine("Command not found: " + line);
                        break;
                }

                return;
            }

            if (!string.IsNullOrWhiteSpace(line) || (Lines.Count > 0 && !string.IsNullOrWhiteSpace(Lines[Lines.Count - 1])))
                Lines.Add(line);
        }

        protected void RunScript(string script)
        {
            try
            {
                ScriptOptions options = ScriptOptions.Default;

                var result = CSharpScript.EvaluateAsync(script, options);
                if (result?.Result != null)
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
