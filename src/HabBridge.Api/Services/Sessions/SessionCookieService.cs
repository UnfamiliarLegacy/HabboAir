using Microsoft.AspNetCore.Http;

namespace HabBridge.Api.Services.Sessions
{
    /// <summary>
    ///     Manages cookies on the client.
    ///     DI: Scoped (required for HttpContext)
    /// </summary>
    public class SessionCookieService
    {
        private const string SessionKey = "session.id";

        private readonly HttpContext _context;

        public SessionCookieService(IHttpContextAccessor contextAccessor)
        {
            _context = contextAccessor.HttpContext;
        }

        /// <summary>
        ///     Grabs the session string value.
        /// </summary>
        /// <param name="sessionHash">The value of the session cookie, if any.</param>
        /// <returns>True if found.</returns>
        public bool TryGetSessionHash(out string sessionHash)
        {
            if (_context.Request.Cookies.ContainsKey(SessionKey) &&
                _context.Request.Cookies.TryGetValue(SessionKey, out var sessionHashTemp))
            {
                sessionHash = sessionHashTemp;
                return true;
            }

            sessionHash = string.Empty;
            return false;
        }

        /// <summary>
        ///     Sets a cookie for the client to store the given <see cref="sessionHash"/>.
        /// </summary>
        /// <param name="sessionHash"></param>
        public void SetSessionHash(string sessionHash)
        {
            _context.Response.Cookies.Append(SessionKey, sessionHash, new CookieOptions
            {
                Path = "/",
                // Secure = true
            });
        }

        /// <summary>
        ///     Asks the client to remove our cookie.
        /// </summary>
        public void ClearSession()
        {
            if (_context.Request.Cookies.ContainsKey(SessionKey))
            {
                _context.Response.Cookies.Delete(SessionKey);
            }
        }
    }
}
