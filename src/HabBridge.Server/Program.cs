using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Serilog;

namespace HabBridge.Server
{
    internal class Program
    {
        private static readonly ILogger Logger = Log.ForContext<Program>();
        private static readonly ManualResetEvent QuitEvent = new ManualResetEvent(false);

        private static async Task<int> Main()
        {
            Console.Title = "HabBridge.Server";
            Console.CancelKeyPress += (sender, args) =>
            {
                QuitEvent.Set();
                args.Cancel = true; // We will handle the cancel.
            };

            // Apply configurations.
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Console()
                .CreateLogger();

            // Start application.
            var app = new Application();

            try
            {
                // Start in a try-catch to avoid deadlocks.
                await app.StartAsync();

                QuitEvent.WaitOne();
            }
            catch (Exception e)
            {
                Logger.Error(e, "Something went wrong while starting the application");

                if (Debugger.IsAttached)
                {
                    Console.ReadKey();
                }

                // Exit with non-zero to let other process know that something went wrong.
                return 1; 
            }

            try
            {
                // Stop in a try-catch.
                app.Stop();
            }
            catch (Exception e)
            {
                Logger.Error(e, "Something went wrong while stopping the application");

                if (Debugger.IsAttached)
                {
                    Console.ReadKey();
                }

                // Exit with non-zero to let other process know that something went wrong.
                return 1;
            }

            // Clean exit.
            return 0;
        }
    }
}
