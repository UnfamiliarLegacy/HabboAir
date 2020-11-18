using System.Threading.Tasks;
using HabBridge.Server.Net;
using HabBridge.Server.Registers;
using Serilog;

namespace HabBridge.Server
{
    internal class Application
    {
        private static readonly ILogger Logger = Log.ForContext<Application>();
        private readonly ClientHandler _clientHandler;

        public Application()
        {
            _clientHandler = new ClientHandler();
        }

        /// <summary>
        ///     Responsible for starting everything.
        /// </summary>
        /// <returns></returns>
        public async Task StartAsync()
        {
            Logger.Information("Starting application");

            await Registar.InitializeAsync();
            await _clientHandler.StartAsync();

            Logger.Information("Started application, press CTRL+C to exit");
        }

        /// <summary>
        ///     Responsible for stopping everything.
        /// </summary>
        /// <returns></returns>
        public void Stop()
        {
            Logger.Information("Stopping application");
            
            _clientHandler.Stop();

            Logger.Information("Stopped application");
        }
    }
}
