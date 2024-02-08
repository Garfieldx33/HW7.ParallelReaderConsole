using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace HW7.ParallelReaderConsole
{
    public static class WhiteSpaceCounter
    {
        private static int CountSpacesInString(string inputString)
        {
            int count = 0;
            for (int i = 0; i < inputString.Length; i++)
            {
                if (inputString[i] == ' ')
                {
                    count++;
                }
            }
            return count;
        }
        private static string ReadStringsFromFile(string filePath)
        {
            string outputString = string.Empty;
            if (File.Exists(filePath))
            {
                using (StreamReader streamReader = new StreamReader(filePath, Encoding.UTF8))
                {
                    outputString = streamReader.ReadToEnd();
                }
            }
            return outputString;
        }

        private static Task<int> CountSpacesInStringAsync(string inputString)
        {
            return Task.Run(() => 
            {
                int count = 0;
                for (int i = 0; i < inputString.Length; i++)
                {
                    if (inputString[i] == ' ')
                    {
                        count++;
                    }
                }
                return count;
            });
        }
        private static async Task<string> ReadStringsFromFileAsync(string filePath)
        {
            string outputString = string.Empty;
            using (StreamReader streamReader = new StreamReader(filePath, Encoding.UTF8))
            {
                outputString = await streamReader.ReadToEndAsync();
            }
            return outputString;
        }



        public static double GetTimeOfCounting(List<string> filesPath)
        {
            var timer = new Stopwatch();
            timer.Start();

            foreach (string file in filesPath)
            {
                string fileStrings = ReadStringsFromFile(file);
                int spacesCount = CountSpacesInString(fileStrings);
                Console.WriteLine($"В файле {file} содержится {spacesCount} пробелов");
            }
            timer.Stop();
            return timer.Elapsed.TotalSeconds;
        }
        
        public static async Task<double> GetTimeOfCountingAsync(List<string> filesPath)
        {
            var timer = new Stopwatch();
            timer.Start();

            foreach (string file in filesPath)
            {
                string fileStrings = ReadStringsFromFile(file);
                int spacesCount = await CountSpacesInStringAsync(fileStrings);
                Console.WriteLine($"В файле {file} содержится {spacesCount} пробелов");
            }
            timer.Stop();
            return timer.Elapsed.TotalSeconds;
        }

        public static async Task<double> GetTimeOfCountingParallel(List<string> filesPath)
        {
            var timer = new Stopwatch();
            timer.Start();
            List<Task> readTasks = new List<Task>();

            foreach (string file in filesPath)
            {
                Task t = ReadFromFileAndCountSpaces(file);
                readTasks.Add(t);
            }
            await Task.WhenAll(readTasks);

            timer.Stop();
            return timer.Elapsed.TotalSeconds;
        }

        private static Task ReadFromFileAndCountSpaces(string filePath)
        {
            return Task.Factory.StartNew(() =>
            {
                string fileStrings = ReadStringsFromFile(filePath);
                int spacesCount = CountSpacesInString(fileStrings);
                Console.WriteLine($"В файле {filePath} содержится {spacesCount} пробелов");
            });
        }
    }
}
