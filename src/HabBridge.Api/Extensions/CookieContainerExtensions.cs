using System;
using System.Net;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace HabBridge.Api.Extensions
{
    public static class CookieContainerExtensions
    {
        /// <summary>
        ///     Searches for a specific cookie in the <see cref="CookieContainer"/>.
        /// </summary>
        /// <param name="container">The container holding all cookies.</param>
        /// <param name="name">The name of the cookie.</param>
        /// <param name="domain">Require a specific domain match.</param>
        /// <param name="path">Require a specific path match.</param>
        /// <returns>A cookie if found, otherwise null.</returns>
        public static Cookie GetCookie(this CookieContainer container, string name, string domain, string path = null)
        {
            Cookie foundCookie = null;

            foreach (Cookie cookie in container.GetCookies(new Uri($"https://{domain}")))
            {
                if (cookie.Name.Equals(name) &&
                    cookie.Domain.Equals(domain) &&
                    (string.IsNullOrEmpty(path) || cookie.Path.Equals(path)))
                {
                    foundCookie = cookie;
                    break;
                }
            }

            return foundCookie;
        }

        public static void ClearAll(this CookieContainer container)
        {
            var hashTable = typeof(CookieContainer)
                .GetField("m_domainTable", BindingFlags.NonPublic | BindingFlags.Instance)
                ?.GetValue(container) as Hashtable;

            if (hashTable == null) {
                throw new ApplicationException("Could not find the cookie hashtable");
            }

            lock (hashTable.SyncRoot)
            {
                hashTable.Clear();
            }
        }
    }
}
