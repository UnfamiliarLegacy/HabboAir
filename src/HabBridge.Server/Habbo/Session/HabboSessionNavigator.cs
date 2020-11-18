using System.Collections.Generic;
using HabBridge.Server.Habbo.Session.Navigator;

namespace HabBridge.Server.Habbo.Session
{
    public class HabboSessionNavigator
    {
        public HabboSessionNavigator()
        {
            UserFlatCatMapping = new Dictionary<int, string>();
        }

        public string LastCategorySearch { get; private set; }

        public string LastCategorySearchId { get; private set; }

        public NavigatorSearchType LastCategorySearchType { get; private set; }

        public Dictionary<int, string> UserFlatCatMapping { get; }

        public void SetLastSearch(string category, string categoryId, NavigatorSearchType type)
        {
            LastCategorySearch = category;
            LastCategorySearchId = categoryId;
            LastCategorySearchType = type;
        }
    }
}
