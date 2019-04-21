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
                for (int i = 0; i < Lines.Count; i++)
                {
                    string line = Lines[i];
                    if (string.IsNullOrWhiteSpace(line))
                        continue;

                    sb.Append(line);

                    if (i < Lines.Count - 1)
                        sb.AppendLine();
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
                switch (line.Substring(1, 1))
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
                        Console.WriteLine("Viewing Buffer: ");
                        Console.WriteLine("----------------");
                        Console.WriteLine(Script);
                        Console.WriteLine("----------------");
                        break;
                    case "s":
                        string filename = line.Substring(2);
                        if (!filename.StartsWith(" "))
                        {
                            Console.WriteLine("Usage: ?s <filename>");
                            break;
                        }

                        filename = filename.Substring(1).Trim();

                        try
                        {
                            if (File.Exists(filename))
                                File.Delete(filename);

                            File.WriteAllText(filename, Script);
                        }
                        catch (IOException ex)
                        {
                            Console.WriteLine("An error occurred while saving the file.");
                            break;
                        }

                        Console.WriteLine("Saved to " + filename);
                        break;
                    case "l":
                        string filename2 = line.Substring(2);
                        if (!filename2.StartsWith(" "))
                        {
                            Console.WriteLine("Usage: ?s <filename>");
                            break;
                        }

                        filename2 = filename2.Substring(1).Trim();

                        if (!File.Exists(filename2))
                        {
                            Console.WriteLine("File not found: '" + filename2 + "'");
                            break;
                        }

                        try
                        {
                            Lines.Clear();
                            Lines.AddRange(File.ReadAllText(filename2)
                                .Split(new string[] {"\r\n", "\n"}, StringSplitOptions.None));
                        }
                        catch (IOException ex)
                        {
                            Console.WriteLine("An error occurred while loading the file.");
                            break;
                        }

                        Console.WriteLine("Loaded from " + filename2);
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

        public void RunScriptFile(string file)
        {
            RunScript(File.ReadAllText(file));
        }

        protected void RunScript(string script)
        {
            RunScript(script, "System", "System.IO", "System.Linq", "System.Collections.Generic", "System.Text");
        }

        protected void RunScript(string script, params string[] imports)
        {
            try
            {
                ScriptOptions options = ScriptOptions.Default;
                options = options.WithReferences("System");
                options = options.AddImports(imports);

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
