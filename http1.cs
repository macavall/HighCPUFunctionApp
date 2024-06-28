using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace HighCPU
{
    public static class CancelClass
    {
        public static CancellationTokenSource _cancellationTokenSource;
        public static bool _isRunning = false;
    }

    public class http1
    {
        //private CancellationTokenSource _cancellationTokenSource;

        private readonly ILogger<http1> _logger;

        public http1(ILogger<http1> logger)
        {
            _logger = logger;
        }

        [Function("http1")]
        public IActionResult Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            _logger.LogInformation("TESTING:  " + req.Query["input"]);

            if(req.Query["input"] == "start")
            {
                if (CancelClass._isRunning == false)
                {
                    CancelClass._isRunning = true;
                    StartHighCPU();
                }
            }
            else if(req.Query["input"] == "stop")
            {
                CancelClass._isRunning = false;
                StopHighCPU();
            }

            //StartHighCPU();

            return new OkObjectResult("Welcome to Azure Functions!");
        }

        private void StartHighCPU()
        {
            // Get the number of processor cores
            int coreCount = Environment.ProcessorCount;
            Console.WriteLine($"Using {coreCount} cores.");

            // Create a CancellationTokenSource
            CancelClass._cancellationTokenSource = new CancellationTokenSource();
            var token = CancelClass._cancellationTokenSource.Token;

            // Create a task for each core
            Task[] tasks = new Task[coreCount];

            for (int i = 0; i < coreCount; i++)
            {
                tasks[i] = Task.Run(() => ConsumeCpu(token), token);
            }

            // Wait for all tasks to complete (this will never happen in this example)
            Task.WaitAll(tasks);
        }

        public static void ConsumeCpu(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                // Perform a CPU-intensive task
                double result = 0;
                for (int i = 0; i < int.MaxValue; i++)
                {
                    result += Math.Sqrt(i);
                }
            }

            Console.WriteLine("Task was cancelled.");
        }

        public void StopHighCPU()
        {
            if (CancelClass._cancellationTokenSource != null)
            {
                CancelClass._cancellationTokenSource.Cancel();
                Console.WriteLine("Cancellation requested.");
            }
        }
    }
}
