using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace lab11
{
    class Program
    {
        static async Task Main(string[] args)
        {
            List<string> categories = new List<string> { "IT", "Marketing" };

            List<Task> tasks = new List<Task>();

            foreach (var category in categories)
            {
                tasks.Add(SearchJobs(category));
            }

            await Task.WhenAll(tasks);

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }

        static async Task SearchJobs(string category)
        {
            string baseUrl = "https://www.work.ua/en/jobs-by-category/";
            string url = $"{baseUrl}{category}/";



            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.GetAsync(url);
                var content = await response.Content.ReadAsStringAsync();

                var doc = new HtmlDocument();
                doc.LoadHtml(content);

                var jobNodes = doc.DocumentNode.SelectNodes("//div[@class='job-link']");

                if (jobNodes != null)
                {
                    foreach (var jobNode in jobNodes)
                    {
                        string jobTitle = jobNode.SelectSingleNode(".//h2")?.InnerText.Trim();
                        string company = jobNode.SelectSingleNode(".//b")?.InnerText.Trim();
                        string location = jobNode.SelectSingleNode(".//span[@class='add-top-xs']")?.InnerText.Trim();
                        string datePosted = jobNode.SelectSingleNode(".//span[@class='text-muted']")?.InnerText.Trim();

                        Console.WriteLine($"Category: {category}");
                        Console.WriteLine($"Title: {jobTitle}");
                        Console.WriteLine($"Company: {company}");
                        Console.WriteLine($"Location: {location}");
                        Console.WriteLine($"Date Posted: {datePosted}");
                        Console.WriteLine();
                    }
                }
                else
                {
                    Console.WriteLine($"No jobs found for category: {category}");
                }
            }

            Console.ReadLine();
        }

    }
}
