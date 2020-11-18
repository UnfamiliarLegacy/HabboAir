namespace HabBridge.Server.Habbo.Data.Friendlist
{
    public class HabboSearchResultData
    {
        public int UserId { get; set; }

        public string Username { get; set; }

        public string Motto { get; set; }

        public bool IsOnline { get; set; }

        public bool Unknown0 { get; set; }

        /// <summary>
        ///     Unused field.
        /// </summary>
        public string Unknown1 { get; set; }

        public int Unknown2 { get; set; }

        public string Figure { get; set; }

        public string LastOnline { get; set; }
    }
}
