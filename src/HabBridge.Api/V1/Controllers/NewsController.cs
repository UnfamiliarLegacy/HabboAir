using System.Collections.Generic;
using HabBridge.Api.V1.Data;
using Microsoft.AspNetCore.Mvc;

namespace HabBridge.Api.V1.Controllers
{
    [Route("v1/{hotel}/news")]
    public class NewsController : ControllerBase
    {
        [HttpGet()]
        public List<NewsItem> GetNews()
        {
            return new List<NewsItem>
            {
                new NewsItem
                {
                    Id = "1",
                    Guid = "26977-easter-2020-alpine-heights",
                    Path = "/community/article/26977/easter-2020-alpine-heights",
                    Name = "easter-2020-alpine-heights",
                    Title = "Easter 2020: Alpine Heights!",
                    Published = 1585737490000,
                    Content = "<h2>Easter 2020: Alpine Heights!</h2>\r\n<p>Habbos! We hope you are as thrilled as we are to welcome you to our Easter event: Alpine Heights! This month we&#x2019;re bringing you closer to nature with our leap into the Alps, complete with all the survival gear you could ever need to reach the top.&#xA0;</p>\r\n<p>Look the part with our brand new clothing items! These include yoga pants, a camping rucksack, and a very fashionable life jacket with extra poof to keep you afloat in those wild rapids! Since Easter is a time for birth, Habbo also sees the return of some of our less conventional pets! Ever wanted to get a Leprechaun or a baby chick at the same time? Well, now is the time.</p>\r\n<h3>Games and activities:<a href=\"https://images.habbo.com/web_images/habbo-web-articles/spromo_easter20_gen.png\" habbo-lightbox class=\"remove-link\" target=\"_blank\" rel=\"noopener noreferrer\"><img class=\"align-right\" src=\"https://images.habbo.com/web_images/habbo-web-articles/spromo_easter20_gen.png\" alt=\"spromo_easter20_gen\" width=\"150\" height=\"150\"></a></h3>\r\n<p>This month, get ready to have your survival abilities put to the test while we take you on a treasure hunting adventure through the rocky alpine mountains. Your mission will be to find five ancient relics that are believed to hold an incredibly powerful energy before they fall into the wrong hands.</p>\r\n<p>Find all relics before it&apos;s too late and collect the exclusive badges we&apos;ve prepared for you. Remember to keep your eye on the Navigator throughout the month to make sure you don&apos;t miss out!</p>\r\n<h3>Siren and Mermaid Rocks:</h3>\r\n<p>Reap the rewards of our special Siren and Mermaid rocks! Items gained from cracking them vary in rarity, therefore some will be harder to obtain than others. Siren and Mermaid Rocks will be made available during several 24-hour periods over the course of the event. These rocks are classed as rares and will not be sold again!</p>\r\n<p>NOTE: the hammer effect is required to crack the rocks and the magic wand effect is required for cleaning the items inside.</p>\r\n<p><a href=\"https://images.habbo.com/web_images/habbo-web-articles/easter20_illuall-except-FR-and-NL.png\" habbo-lightbox class=\"remove-link\" target=\"_blank\" rel=\"noopener noreferrer\"><img src=\"https://images.habbo.com/web_images/habbo-web-articles/easter20_illuall-except-FR-and-NL.png\" alt=\"easter20_illu(all except FR and NL)\" width=\"700\" height=\"700\"></a></p>\r\n<h3>Rares:</h3>\r\n<ul>\r\n\t<li>Nature Cap</li>\r\n\t<li>Hop Costume</li>\r\n\t<li>Mountain Goat</li>\r\n\t<li>Mystic Tree</li>\r\n\t<li>Fortune Duck (LTD)</li>\r\n</ul>\r\n<h3>Bundles:</h3>\r\n<ul>\r\n\t<li>Camping Trip Bundle</li>\r\n\t<li>Fishing Day Bundle</li>\r\n\t<li>Great Farm Bake Bundle</li>\r\n\t<li>Easter Treehouse Bundle</li>\r\n</ul>\r\n<p>We look forward to continuing our adventures with you in Alpine Heights this Easter!</p>\r\n<p>-Habbo Staff</p>",
                    Summary = "New adventures await in Habbo for Easter 2020!",
                    Categories = new List<string>
                    {
                        "campaigns_activities",
                        "d63",
                        "live",
                        "visible",
                        "publish",
                        "staging",
                        "visible-2"
                    },
                    Featured = "https://images.habbo.com/web_images/habbo-web-articles/lpromo_easter20_gen.png",
                    Link = "http://articles.varoke.net/blog/2020/04/01/easter-2020-alpine-heights/",
                    Format = "standard",
                    Sticky = true,
                    Status = "publish",
                    Thumbnail = "https://images.habbo.com/web_images/habbo-web-articles/lpromo_easter20_gen_thumb.png",
                    ArticleImage = null,
                    Env = "live",
                    Future = false,
                    VisibleCategories = new List<NewsCategory>
                    {
                        new NewsCategory
                        {
                            Slug = "campaigns_activities",
                            Title = "NEWS_CATEGORY_CAMPAIGNS_ACTIVITIES",
                            Url = "/community/category/campaigns-activities"
                        }
                    }
                }
            };
        }
    }
}