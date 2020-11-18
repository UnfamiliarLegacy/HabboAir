using HabBridge.Habbo.Shared;

namespace HabBridge.Hotels
{
    public class HotelConfig
    {
        internal HotelConfig() { }

        public string ServerIp { get; set; }
        public int ServerPort { get; set; }
        public string FlashClientUrl { get; set; }
        public string ExternalVariables { get; set; }
        public string ExternalFlashTexts { get; set; }
        public bool Encryption { get; set; }
        
        /// <summary>
        ///     The exponent extracted from the target 'habbo.swf'.
        /// </summary>
        public string Exponent { get; set; }

        /// <summary>
        ///     The modulus extracted from the target 'habbo.swf'.
        /// </summary>
        public string Modulus { get; set; }

        /// <summary>
        ///     The release used by the target.
        /// </summary>
        public Release Release { get; set; }
    }
}
