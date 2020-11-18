namespace HabBridge.Server.Habbo.Data.Navigator
{
    public class FlatCategory
    {
        public int Id { get; set; }

        public string NodeName { get; set; }

        public bool Visible { get; set; }

        public bool Automatic { get; set; }

        public string Unknown0 { get; set; }

        /// <summary>
        ///     Name related.
        ///     i.e.: 'navigator.flatcategory.global.{Unknown1}'
        /// </summary>
        public string Unknown1 { get; set; }

        public bool Unknown2;
    }
}
