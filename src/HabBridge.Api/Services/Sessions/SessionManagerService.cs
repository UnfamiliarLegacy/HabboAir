using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using HabBridge.Hotels;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace HabBridge.Api.Services.Sessions
{
    public class SessionManagerService
    {
        private readonly IDistributedCache _cache;

        private readonly ConcurrentDictionary<string, Session> _sessions;

        public SessionManagerService(IDistributedCache cache)
        {
            _cache = cache;
            _sessions = new ConcurrentDictionary<string, Session>();
        }

        /// <summary>
        ///     Creates a new unique session, which is stored into redis and a local cache.
        /// </summary>
        /// <param name="hotel"></param>
        /// <returns></returns>
        public async Task<Session> CreateSessionAsync(HotelType hotel)
        {
            Session session;

            // Create a new session until it is unique.
            do
            {
                session = new Session
                {
                    Id = new SessionId(),
                    Hotel = hotel
                };

                session.Init();

                // Store session in local cache.
            } while (!_sessions.TryAdd(session.Id.Hash, session));

            // Store session in redis.
            await SaveSession(session);

            return session;
        }

        /// <summary>
        ///     Attempts to retrieve a <see cref="Session"/> from a <see cref="SessionId.Hash"/>
        ///     which is grabbed from the client using <see cref="SessionCookieService"/>.
        /// </summary>
        /// <param name="sessionHash"></param>
        /// <param name="session"></param>
        /// <returns></returns>
        public bool TryGetSession(string sessionHash, out Session session)
        {
            // Hit our local cache.
            if (_sessions.TryGetValue(sessionHash, out session))
            {
                return true;
            }

            // Hit redis.
            var sessionData = _cache.GetString(sessionHash);
            if (!string.IsNullOrEmpty(sessionData))
            {
                session = JsonConvert.DeserializeObject<Session>(sessionData);

                // Store locally.
                _sessions.TryAdd(session.Id.Hash, session);
                return true;
            }

            // Nothing.
            return false;
        }

        /// <summary>
        ///     Removes everything we have of a <see cref="Session"/>
        ///     so it can not be re-used.
        /// </summary>
        /// <param name="session"></param>
        /// <returns></returns>
        public async Task RemoveSessionAsync(Session session)
        {
            // Remove locally.
            _sessions.TryRemove(session.Id.Hash, out _);

            // Remove from redis.
            await _cache.RemoveAsync(session.Id.Hash);
        }

        /// <summary>
        ///     Saves a session to the local and remote cache.
        /// </summary>
        /// <param name="session"></param>
        /// <returns></returns>
        public async Task SaveSession(Session session)
        {
            // Reset SaveChanges property.
            session.SaveChanges = false;
            
            // Store in redis.
            await _cache.SetStringAsync(session.Id.Hash, JsonConvert.SerializeObject(session, Formatting.None), new DistributedCacheEntryOptions
            {
                SlidingExpiration = TimeSpan.FromDays(7)
            });
            
            // Store locally.
            _sessions.TryAdd(session.Id.Hash, session);
        }
    }
}
