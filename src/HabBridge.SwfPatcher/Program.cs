using System;
using System.IO;
using HabBridge.SwfPatcher.Air;
using HabBridge.SwfPatcher.Config;

namespace HabBridge.SwfPatcher
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.Title = "HabBridge.SwfPatcher";
            Console.ForegroundColor = ConsoleColor.White;

            var swfs = new[]
            {
                "HabboTablet.orig.swf"
            };

            foreach (var swf in swfs)
            {
                using (var game = new AirGame(swf))
                {
                    game.Initialize();

                    Console.WriteLine("Loaded HabboTablet.swf");
                    Console.WriteLine($"  {"Revision:",-17} {game.Revision}");
                    Console.WriteLine($"  {"RevisionProtocol:",-17} {game.RevisionProtocol}");

                    game.ReplaceBinaryData("common_configuration_txt", File.ReadAllBytes("Data/common_configuration.txt"));
                    Console.WriteLine("  > Replace binary data \"common_configuration.txt\".");
                    
                    game.ReplaceLocalizationDefaults(new[]
                    {
                        new RetroLocalizationConfig
                        {
                            HotelCodeBase = "nl",
                            HotelCode = "local",
                            Code = "nl_NL.iso-8859-1",
                            Name = "Dutch",
                            Url = "${url.prefix}/gamedata/external_flash_texts/1"
                        }, 
                        new RetroLocalizationConfig
                        {
                            HotelCodeBase = "en",
                            HotelCode = "en",
                            Code = "en_US.iso-8859-1",
                            Name = "English",
                            Url = "${url.prefix}/gamedata/external_flash_texts/1"
                        }, 
                        new RetroLocalizationConfig
                        {
                            HotelCodeBase = "nl",
                            HotelCode = "leet",
                            Code = "nl_NL.iso-8859-1",
                            Name = "Dutch",
                            Url = "${url.prefix}/gamedata/external_flash_texts/1"
                        }, 
                    });
                    Console.WriteLine("  > Replace localization.");
                    
                    game.PatchHostCheck();
                    Console.WriteLine("  > Patched host check.");
                    
                    game.PatchCrypto("86851dd364d5c5cece3c883171cc6ddc5760779b992482bd1e20dd296888df91b33b936a7b93f06d29e8870f703a216257dec7c81de0058fea4cc5116f75e6efc4e9113513e45357dc3fd43d4efab5963ef178b78bd61e81a14c603b24c8bcce0a12230b320045498edc29282ff0603bc7b7dae8fc1b05b52b2f301a9dc783b7", "3");
                    Console.WriteLine("  > Patched crypto.");
                    
                    game.PatchHabboTracking();
                    Console.WriteLine("  > Patched HabboTracking.");
                    
                    game.PatchClientHelloMessageComposer();
                    Console.WriteLine("  > Patched ClientHelloMessageComposer.");

                    game.PatchLogger();
                    Console.WriteLine("  > Patched Logger.");

                    game.Reassemble("HabboTablet.mod.swf");
                    Console.WriteLine("  > Reassembled SWF file.");
                }
            }
        }
    }
}
