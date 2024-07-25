using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace HighCPU
{
    public class http2
    {
        private readonly ILogger<http2> _logger;

        public http2(ILogger<http2> logger)
        {
            _logger = logger;
        }

        [Function("http2")]
        public IActionResult Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");



            return new OkObjectResult("Welcome to Azure Functions!");
        }
    }

    public class  MyClass
    {
        public MyClass()
        {
            System.Threading.Thread.Sleep(10000);
            MyClassMethod();
        }

        public void MyClassMethod()
        {
            System.Threading.Thread.Sleep(10000);
            Console.WriteLine("MyClassMethod2");
        }

        public void MyClassMethod2()
        {
            System.Threading.Thread.Sleep(10000);
            Console.WriteLine("MyClassMethod2");
        }

        public void MyClassMethod3()
        {
            System.Threading.Thread.Sleep(10000);
            Console.WriteLine("MyClassMethod3");
        }

        public void MyClassMethod4()
        {
            System.Threading.Thread.Sleep(10000);
            Console.WriteLine("MyClassMethod4");
        }
    }
}
