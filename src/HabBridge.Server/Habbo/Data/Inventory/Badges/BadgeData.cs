using System.Collections.Generic;

namespace HabBridge.Server.Habbo.Data.Inventory.Badges
{
    public class BadgeData
    {
        public string GroupName { get; set; }

        public List<BadgePointLimitData> Levels { get; set; }
    }
}
