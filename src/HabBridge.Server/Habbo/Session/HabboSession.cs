namespace HabBridge.Server.Habbo.Session
{
    public class HabboSession
    {
        public HabboSession()
        {
            Navigator = new HabboSessionNavigator();
            Room = new HabboSessionRoom();
        }

        public HabboSessionNavigator Navigator { get; }

        public HabboSessionRoom Room { get; }
    }
}
