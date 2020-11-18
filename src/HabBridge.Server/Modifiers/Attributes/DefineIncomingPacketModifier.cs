using System;
using HabBridge.Habbo.Shared;
using HabBridge.Server.Habbo;

namespace HabBridge.Server.Modifiers.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class DefineIncomingPacketModifier : Attribute
    {
        public DefineIncomingPacketModifier(Release[] releases, Incoming header)
        {
            Releases = releases;
            Header = header;
        }

        public Release[] Releases { get; }

        public Incoming Header { get; }
    }
}
