using System.Net;
using Newtonsoft.Json;

namespace HabBridge.Api.V1.Requests
{
    public class LogCrashRequest
    {
        public string Message { get; set; }

        [JsonIgnore]
        public string MessageDecoded => WebUtility.UrlDecode(Message);
    }
}
