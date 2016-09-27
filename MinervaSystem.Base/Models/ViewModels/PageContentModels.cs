using System.Collections.Generic;

namespace MinervaSystem.Base.Models
{
    public class HeaderContentModel
    {
        public BreadcrumbModel Breadcrumb { get; set; }
        public string PageTitle { get; set; }
        public string SmallDescription { get; set; }

        public HeaderContentModel(string pageTitle, BreadcrumbModel breadcrumb)
        {
            PageTitle = pageTitle;
            Breadcrumb = breadcrumb;
        }
        public HeaderContentModel(string pageTitle, string smallDescription, BreadcrumbModel breadcrumb)
        {
            PageTitle = pageTitle;
            Breadcrumb = breadcrumb;
            SmallDescription = smallDescription;
        }
    }

    public class BreadcrumbModel
    {
        public LinkModel Root { get; private set; }
        public List<LinkModel> Links { get; set; }
        public string ActiveText { get; set; }

        public BreadcrumbModel(string activeText)
        {
            Root = new LinkModel("/", SiteConfig.SiteTitle);
            ActiveText = activeText;
        }

        public BreadcrumbModel(string activeText, List<LinkModel> links)
        {
            Root = new LinkModel("/", SiteConfig.SiteTitle);
            Links = links;
            ActiveText = activeText;
        }
    }

    public class LinkModel
    {
        public string Url { get; set; }
        public string Text { get; set; }

        public LinkModel(string url, string text)
        {
            Url = url;
            Text = text;
        }
    }
}
