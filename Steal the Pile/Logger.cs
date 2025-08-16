using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StealThePile
{
    class Logger
    {
        static public string folderPath = @"C:\Users\Administrator\source\repos\Steal the Pile\logs";

        static public void RegisterLog(string message)
        {
            Directory.CreateDirectory(folderPath);

            string logFile = Path.Combine(folderPath, "stealthepile.txt");

            try
            {
                using (StreamWriter sw = new StreamWriter(logFile, true))
                {
                    sw.WriteLine(message);
                }
            }

            catch (Exception e)
            {
                Console.WriteLine("Error: " + e.Message);
            }
        }
    }
}