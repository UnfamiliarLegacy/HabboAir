namespace HabBridge.Server.Habbo.Data.HabboInventory
{
    public class AvatarEffect
    {
        public int SpriteId { get; set; }

        public AvatarEffectType Type { get; set; }

        public int Duration { get; set; }

        public int Quantity { get; set; }

        public int TimeLeft { get; set; }

        public bool Permanent { get; set; }
    }
}
