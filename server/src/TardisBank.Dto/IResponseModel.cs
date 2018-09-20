using System;
using System.Collections.Generic;
using System.Linq;

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
            if(links.Any(x => x.Rel == rel)) return;
            links.Add(new LinkModel
            {
                Rel = rel,
                Href = href
            });
        }

        public LinkModel Link(string rel) => 
            links
                .Where(x => x.Rel == rel)
                .SingleOrDefault() ?? throw new ApplicationException($"Rel '{rel}' not found.");

        public LinkModel[] Links
        {
            get 
            {
                return links.ToArray();
            }
            set
            {
                links = new List<LinkModel>(value);
            }
        } 
    }

    public class LinkModel
    {
        public string Rel { get; set; }
        public string Href { get; set; }
    }
}