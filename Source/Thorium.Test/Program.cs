using System;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Text.Json;
using Thorium.Shared.DTOs;
using Thorium.Shared.DTOs.OperationData;

namespace Thorium.Test
{
    public class Program
    {
        static HttpClient http;

        public static void Main()
        {
            
            http = new HttpClient();
            HttpRequestMessage req = new HttpRequestMessage(HttpMethod.Post, "http://127.0.0.1:8080/addjob");
            JobDTO job = new ()
            {
                Id = "1234abcd",
                Name = "this is a test",
                TaskCount = 10,
                Operations =
                [
                    new OperationDTO()
                    {
                        OperationType = "exe",
                        OperationData = new ExeDTO() { FilePath="notepad.exe"},
                    }
                ]
            };
            var content = JsonSerializer.Serialize(job);
            req.Content = new StringContent(content);

            var res = http.Send(req);

            Console.ReadKey();
        }
    }
}
