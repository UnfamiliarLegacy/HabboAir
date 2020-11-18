using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HabBridge.Api.V1.Responses
{
    public class LoginResponse
    {
        public string UniqueId { get; set; }

        public string Name { get; set; }

        public string FigureString { get; set; }

        public string Motto { get; set; }

        public bool BuildersClubMember { get; set; }

        public bool HabboClubMember { get; set; }

        public DateTimeOffset LastWebAccess { get; set; }

        public DateTimeOffset CreationTime { get; set; }

        public long SessionLogId { get; set; }

        public long LoginLogId { get; set; }

        public string Email { get; set; }

        public int IdentityId { get; set; }

        public bool EmailVerified { get; set; }

        public bool IdentityVerified { get; set; }

        public string IdentityType { get; set; }

        public bool Trusted { get; set; }

        public string[] Force { get; set; }

        public int AccountId { get; set; }

        public object Country { get; set; }

        public string[] Traits { get; set; }

        public string Partner { get; set; }
    }
}
