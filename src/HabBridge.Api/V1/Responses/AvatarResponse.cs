using System;

namespace HabBridge.Api.V1.Responses
{
    public class AvatarResponse
    {
        public string UniqueId { get; set; }

        public string Name { get; set; }

        public string FigureString { get; set; }

        public string Motto { get; set; }

        public bool BuildersClubMember { get; set; }

        public bool HabboClubMember { get; set; }

        public DateTimeOffset LastWebAccess { get; set; }

        public DateTimeOffset CreationTime { get; set; }

        public bool Banned { get; set; }
    }
}
