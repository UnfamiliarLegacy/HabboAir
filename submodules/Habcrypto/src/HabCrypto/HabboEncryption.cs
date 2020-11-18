namespace HabCrypto
{
    public class HabboEncryption
    {
        public HabboEncryption(string e, string n)
        {
            Crypto = new HabboRSACrypto(e, n);
            Diffie = new HabboDiffieHellman(Crypto, HabboConnectionType.Client);
        }

        public HabboEncryption(string e, string n, string d)
        {
            Crypto = new HabboRSACrypto(e, n, d);
            Diffie = new HabboDiffieHellman(Crypto, HabboConnectionType.Server);
        }

        public HabboRSACrypto Crypto { get; }

        public HabboDiffieHellman Diffie { get; }
        
        public HabboRC4 ClientRC4 { get; set; }

        public HabboRC4 ServerRC4 { get; set; }
    }
}
