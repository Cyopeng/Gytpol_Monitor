using System;
using System.IO;
using System.ServiceProcess;
using System.Threading;

class Program
{
    static void Main()
    {
        string logPath = @"C:\gytpol\log.txt";

        while (true)
        {
            try
            {
                Log("Checking services...");

                CheckAndRestartService("Gytpol Analyzer Service");
                CheckAndRestartService("Gytpol Validator Service");
                CheckAndRestartService("Gytpol Web UI Service");

                Log("Services checked. Waiting for 30 minutes...");
                Thread.Sleep(30 * 60 * 1000); // Sleep for 30 minutes
            }
            catch (Exception ex)
            {
                Log($"Error: {ex.Message}");
            }
        }
    }

    static void CheckAndRestartService(string serviceName)
    {
        using (ServiceController serviceController = new ServiceController(serviceName))
        {
            string status = serviceController.Status.ToString();

            Log($"{serviceName} status: {status}");

            if (status != "Running")
            {
                Log($"Restarting {serviceName}...");

                serviceController.Stop();
                serviceController.WaitForStatus(ServiceControllerStatus.Stopped, TimeSpan.FromMinutes(1));
                serviceController.Start();
                serviceController.WaitForStatus(ServiceControllerStatus.Running, TimeSpan.FromMinutes(1));

                Log($"{serviceName} restarted.");
            }
        }
    }

    static void Log(string message)
    {
        string logPath = @"C:\gytpol\log.txt";

        using (StreamWriter sw = File.AppendText(logPath))
        {
            sw.WriteLine($"{DateTime.Now}: {message}");
        }

        Console.WriteLine($"{DateTime.Now}: {message}");
    }
}
