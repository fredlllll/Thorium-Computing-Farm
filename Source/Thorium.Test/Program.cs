using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using Thorium.Shared.DTOs;

namespace Thorium.Test
{
    public class Program
    {
        static HttpClient http;

        static void AddJob(JobDTO job)
        {
            http = new HttpClient();
            HttpRequestMessage req = new HttpRequestMessage(HttpMethod.Post, "http://127.0.0.1:8080/addjob");
            var content = JsonSerializer.Serialize(job);
            req.Content = new StringContent(content);
            var res = http.Send(req);
        }


        public static void Main()
        {

            JobDTO job = new()
            {
                Name = "this is a test",
                Description = "Description",
                TaskCount = 3,
                Operations =
                [
                    new OperationDTO()
                    {
                        OperationType = "exe",
                        OperationData = new Dictionary<string, string>() {
                            { "fileName" , "notepad.exe" },
                            { "workingDir","C:\\Users\\Freddy\\Desktop"},
                            {"arguments", "[\"message.txt\"]" }
                        },
                    }
                ]
            };

            AddJob(job);

            Console.ReadKey();
        }
    }
}
