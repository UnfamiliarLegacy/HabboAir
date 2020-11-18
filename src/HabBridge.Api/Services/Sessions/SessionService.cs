using System.Threading.Tasks;
using HabBridge.Api.V1.Data;

namespace HabBridge.Api.Services.Sessions
{
    /// <summary>
    ///     Combines the power of <see cref="SessionCookieService"/>
    ///     and <see cref="SessionManagerService"/>.
    ///     DI: Scoped.
    /// </summary>
    public class SessionService
    {
        private readonly RequestData _requestData;

        private readonly SessionCookieService _sessionCookieService;

        private readonly SessionManagerService _sessionManagerService;

        public SessionService(
            RequestData requestData,
            SessionCookieService sessionCookieService,
            SessionManagerService sessionManagerService)
        {
            _requestData = requestData;
            _sessionCookieService = sessionCookieService;
            _sessionManagerService = sessionManagerService;
        }

        public async Task LogOutAsync()
        {
            // Ask the client to remove our cookie.
            _sessionCookieService.ClearSession();

            // Remove our own references to the session.
            await _sessionManagerService.RemoveSessionAsync(_requestData.Session);

            // Make session null so we can not accidentally save it again.
            _requestData.Session = null;
        }
    }
}
