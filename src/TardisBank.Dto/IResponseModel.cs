using System.Collections.Generic;

namespace TardisBank.Dto
{
    public interface IResponseModel
    {
        LinkModel[] Links { get; }
        void AddLink(string rel, string href);
    }

    public class ResponseModelBase : IResponseModel
    {
        private List<LinkModel> links = new List<LinkModel>();

        public void AddLink(string rel, string href)
        {
            links.Add(new LinkModel
            {
                Rel = rel,
                Href = href
            });
        }

        public LinkModel[] Links => links.ToArray();
    }

    public class LinkModel
    {
        public string Rel { get; set; }
        public string Href { get; set; }
    }
}