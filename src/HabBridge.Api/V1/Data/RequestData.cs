using HabBridge.Api.Services.Sessions;

namespace HabBridge.Api.V1.Data
{
    /// <summary>
    ///     Used to store data passed around during a request.
    ///     DI: Scoped
    /// </summary>
    public class RequestData
    {
        public Session Session { get; set; }
    }
}
