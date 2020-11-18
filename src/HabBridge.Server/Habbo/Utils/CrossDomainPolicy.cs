using System.Text;

namespace HabBridge.Server.Habbo.Utils
{
    public static class CrossDomainPolicy
    {
        private static readonly byte[] policyBytes;

        static CrossDomainPolicy()
        {
            policyBytes = Encoding.ASCII.GetBytes(GetPolicy());
        }

        public static string GetPolicy()
        {
            return "<?xml version=\"1.0\"?>\r\n<!DOCTYPE cross-domain-policy SYSTEM \"/xml/dtds/cross-domain-policy.dtd\">\r\n<cross-domain-policy>\r\n<allow-access-from domain=\"*\" to-ports=\"1-31111\" />\r\n</cross-domain-policy>\0";
        }

        public static byte[] GetPolicyBytes()
        {
            return policyBytes;
        }
    }
}
