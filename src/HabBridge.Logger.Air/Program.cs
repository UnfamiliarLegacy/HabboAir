using System;
using System.Net;
using System.Threading.Tasks;

namespace HabBridge.Logger.Air
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            HLogger.FAKE_EXPONENT = 3;
            HLogger.FAKE_MODULUS = "86851dd364d5c5cece3c883171cc6ddc5760779b992482bd1e20dd296888df91b33b936a7b93f06d29e8870f703a216257dec7c81de0058fea4cc5116f75e6efc4e9113513e45357dc3fd43d4efab5963ef178b78bd61e81a14c603b24c8bcce0a12230b320045498edc29282ff0603bc7b7dae8fc1b05b52b2f301a9dc783b7";
            HLogger.FAKE_PRIVATE_EXPONENT = "59ae13e243392e89ded305764bdd9e92e4eafa67bb6dac7e1415e8c645b0950bccd26246fd0d4af37145af5fa026c0ec3a94853013eaae5ff1888360f4f9449ee023762ec195dff3f30ca0b08b8c947e3859877b5d7dced5c8715c58b53740b84e11fbc71349a27c31745fcefeeea57cff291099205e230e0c7c27e8e1c0512b";

            HLogger.REAL_EXPONENT = 0x10001;
            HLogger.REAL_MODULUS = "e052808c1abef69a1a62c396396b85955e2ff522f5157639fa6a19a98b54e0e4d6e44f44c4c0390fee8ccf642a22b6d46d7228b10e34ae6fffb61a35c11333780af6dd1aaafa7388fa6c65b51e8225c6b57cf5fbac30856e896229512e1f9af034895937b2cb6637eb6edf768c10189df30c10d8a3ec20488a198063599ca6ad";

            var entry = await Dns.GetHostEntryAsync("game-us.habbo.com");

            Console.Title = "HabBridge.Logger.Air";
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($"Intercepting habbo packets on port 30000, redirecting to game-us.habbo.com.");

            while (true)
            {
                var logger = new HLogger(entry.AddressList[0].ToString(), 30000, new int[]
                {
                    4000,
                    1887,
                    2125,
                    1706
                });
                await logger.Start();
            }
        }
    }
}
