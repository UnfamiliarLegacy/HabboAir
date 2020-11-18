using System.Collections.Generic;

namespace HabBridge.Server.Habbo.Data.CallForHelp
{
    public class CallForHelpCategoryData
    {
        public string Name { get; set; }

        public List<CallForHelpTopicData> Topics { get; set; }
    }
}
