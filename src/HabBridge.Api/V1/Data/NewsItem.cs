using System.Collections.Generic;

namespace HabBridge.Api.V1.Data
{
    public class NewsItem
    {
        public string Id { get; set; }
        public string Guid { get; set; }
        public string Path { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public ulong Published { get; set; }
        public string Content { get; set; }
        public string Summary { get; set; }
        public List<string> Categories { get; set; }
        public string Featured { get; set; }
        public string Link { get; set; }
        public string Format { get; set; }
        public bool Sticky { get; set; }
        public string Status { get; set; }
        public string Thumbnail { get; set; }
        public string ArticleImage { get; set; }
        public string Env { get; set; }
        public bool Future { get; set; }
        public List<NewsCategory> VisibleCategories { get; set; }
    }

    public class NewsCategory
    {
        public string Slug { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
    }
}