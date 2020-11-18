using System;
using System.Collections.Generic;
using HabBridge.Habbo.Shared;

namespace HabBridge.Hotels
{
    public static class HotelManager
    {
        private static readonly Dictionary<HotelType, HotelConfig> Hotels;

        static HotelManager()
        {
            Hotels = new Dictionary<HotelType, HotelConfig>
            {
                {
                    HotelType.Local, new HotelConfig
                    {
                        ServerIp = "127.0.0.1",
                        ServerPort = 30001,
                        FlashClientUrl = "http://localhost/habbo/gordon/PRODUCTION-201807031205-696881661/",
                        ExternalVariables = "http://localhost/habbo/gamedata/external_variables.txt",
                        ExternalFlashTexts = null,
                        Encryption = true,
                        Exponent = "3",
                        Modulus = "86851dd364d5c5cece3c883171cc6ddc5760779b992482bd1e20dd296888df91b33b936a7b93f06d29e8870f703a216257dec7c81de0058fea4cc5116f75e6efc4e9113513e45357dc3fd43d4efab5963ef178b78bd61e81a14c603b24c8bcce0a12230b320045498edc29282ff0603bc7b7dae8fc1b05b52b2f301a9dc783b7",
                        Release = Release.PRODUCTION_201607262204_86871104
                    }
                },
                {
                    HotelType.Space, new HotelConfig
                    {
                        ServerIp = "127.0.0.1",
                        ServerPort = 30001,
                        FlashClientUrl = "https://game.spacehotell.nl/r63_new/",
                        ExternalVariables = "https://game.spacehotell.nl/r63_new/external_variables.txt?242",
                        ExternalFlashTexts = null,
                        Encryption = true,
                        Exponent = "3",
                        Modulus = "86851dd364d5c5cece3c883171cc6ddc5760779b992482bd1e20dd296888df91b33b936a7b93f06d29e8870f703a216257dec7c81de0058fea4cc5116f75e6efc4e9113513e45357dc3fd43d4efab5963ef178b78bd61e81a14c603b24c8bcce0a12230b320045498edc29282ff0603bc7b7dae8fc1b05b52b2f301a9dc783b7",
                        Release = Release.PRODUCTION_201607262204_86871104
                    }
                },
                {
                    HotelType.Leet, new HotelConfig
                    {
                        ServerIp = "127.0.0.1",
                        ServerPort = 30001,
                        FlashClientUrl = "https://images.leet.ws/library/",
                        ExternalVariables = "https://images.leet.ws/library/gamedata/leet_vars.txt?v=33",
                        ExternalFlashTexts = "https://images.leet.ws/library/gamedata/leet_texts.txt?v=33",
                        Encryption = false,
                        Exponent = "10001",
                        Modulus = "0edffe6d4b27495bbe3f3a978fb1c18f112785d7fb183136b029842c52b40b19722cfc2a7a78995013da0cc3145dd39609e00cea89746947f3f17a6dda386f0f5c0d51c275a161cc42668b309b48fa8207daf36deb8ca5cb77cf34f89988cb0503e58331b52f46595324ab424f6f627ced4918f42b39b440c06d18da2fc8a9b0d",
                        Release = Release.PRODUCTION_201701242205_837386174
                    }
                }
            };
        }

        public static HotelConfig GetHotel(HotelType hotelType)
        {
            if (!Hotels.ContainsKey(hotelType))
            {
                throw new ApplicationException($"Hotel {hotelType} does not exist in the HotelManager.");
            }

            return Hotels[hotelType];
        }
    }
}
