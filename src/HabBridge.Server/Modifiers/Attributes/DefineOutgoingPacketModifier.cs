using System;
using HabBridge.Habbo.Shared;
using HabBridge.Server.Habbo;

namespace HabBridge.Server.Modifiers.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class DefineOutgoingPacketModifier : Attribute
    {
        public DefineOutgoingPacketModifier(Release[] releases, Outgoing header)
        {
            Releases = releases;
            Header = header;
        }

        public Release[] Releases { get; }

        public Outgoing Header { get; }
    }
}
