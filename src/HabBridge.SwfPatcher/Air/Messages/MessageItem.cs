using Flazzy.ABC;

namespace HabBridge.SwfPatcher.Air.Messages
{
    public class MessageItem
    {
        public MessageItem(ushort id, MessageType type, ASClass clazz)
        {
            Id = id;
            Type = type;
            Clazz = clazz;
        }

        public ushort Id { get; }

        public MessageType Type { get; }

        public ASClass Clazz { get; }
    }
}
