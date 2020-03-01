using System;
using System.IO;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Cw1
{
    class Program
    {
        

        public static async Task Main(string[] args)
        {
            var httpClient = new HttpClient();
            HttpResponseMessage response = await httpClient.GetAsync(args[0]);
            if (response.IsSuccessStatusCode)
            {
                var htmlContent = await response.Content.ReadAsStringAsync();
                Regex regex = new Regex("[a-z]+[a-z0-9]*@[a-z0-9]+\\.[a-z]+\\.[a-z]+", RegexOptions.IgnoreCase);
                MatchCollection matches = regex.Matches(htmlContent);
                foreach (var match in matches)
                {
                    string path = @"emails.txt";
                    // This text is added only once to the file.
                    if (!File.Exists(path))
                    {
                        // Create a file to write to.
                        using (StreamWriter sw = File.CreateText(path))
                        {
                            sw.WriteLine(match.ToString());
                        }
                    }

                    // This text is always added, making the file longer over time
                    // if it is not deleted.
                    using (StreamWriter sw = File.AppendText(path))
                    {
                        sw.WriteLine(match.ToString());
                    }

                    // Open the file to read from.
                    using (StreamReader sr = File.OpenText(path))
                    {
                        string s = "";
                        while ((s = sr.ReadLine()) != null)
                        {
                            Console.WriteLine(s);
                        }
                    }
                } 
            } else
            {
                Console.WriteLine($"{response}");
                Console.WriteLine("Something went wrong");
            }
        }
    }
}
