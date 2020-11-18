namespace HabBridge.Server.Habbo.Data.Users
{
    public class RelationshipStatusInfo
    {
        public RelationshipStatusType Type { get; set; }

        public int Amount { get; set; }

        public int TargetUserId { get; set; }

        public string TargetUsername { get; set; }

        public string TargetFigure { get; set; }
    }
}
