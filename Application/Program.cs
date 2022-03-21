using System;
using System.Collections.Generic;
using System.IO;

namespace Application
{
    public class Program
    {
        public static DateTime CURRENT_TIME = DateTime.MinValue;

        static void Main(string[] args)
        {
            DispatcherEngine.ClearTables();
            bool showMenu = true;
            while (showMenu)
            {
                showMenu = MainMenu();
            }
        }

        private static bool MainMenu()
        {
            Console.Clear();
            Console.WriteLine("Choose an option:");
            Console.WriteLine("1) Read From Scenario File");
            Console.WriteLine("2) Read From Command Line");
            Console.Write("\r\nSelect an option: ");

            switch (Console.ReadLine())
            {
                case "1":
                    ReadFromScenarioFile();
                    return false;
                case "2":
                    ReadFromConsole();
                    return false;
                default:
                    return true;
            }
        }

        private static void ReadFromScenarioFile()
        {
            Console.Clear();
            do
            {
                Console.WriteLine();
                Console.WriteLine(@"Please give the full path. For Example: C:\Users\Evren\Desktop\VS\ECommerceConsoleApp\scenerioFile.txt");
                Console.Write("\r\nPath: ");
                string path = Console.ReadLine();
                if (File.Exists(path))
                {
                    Console.WriteLine("\r\nCommands are processed, please wait...");
                    using (StreamReader sr = File.OpenText(path))
                    {
                        string line = "";
                        while ((line = sr.ReadLine()) != null)
                        {
                            Console.WriteLine(line);
                            ProduceCommand(line);
                        }
                    }
                }
                else
                {
                    DispatcherEngine.ShowErrorMessage("File does not exist.");
                }
                DispatcherEngine.ClearTables();
            } while (true);
        }

        private static void ReadFromConsole()
        {
            Console.Clear();
            do
            {
                Console.Write("\r\nPlease enter command: ");
                string commandLine = Console.ReadLine();
                ProduceCommand(commandLine);
            } while (true);
        }

        private static void ProduceCommand(string commandLine)
        {
            string command = string.Empty;
            var commandArray = commandLine.Trim().Split(" ");
            bool hasCommand = false;

            command = commandArray[0];
            Delegate @delegate;
            hasCommand = DispatcherEngine.CommandSet.TryGetValue(command, out @delegate);

            if (hasCommand)
            {
                var parameters = new List<object>();

                for (int i = 1; i < commandArray.Length; i++)
                    parameters.Add(commandArray[i]);

                try
                {
                    @delegate.DynamicInvoke(parameters.ToArray());
                }
                catch
                {
                    DispatcherEngine.ShowErrorMessage("Incorrect command usage.");
                }
            }
            else
                DispatcherEngine.ShowErrorMessage("Command not found.");
        }
    }
}
