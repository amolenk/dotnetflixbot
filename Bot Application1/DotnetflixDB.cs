using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Threading.Tasks;
using System.Web;
using System.Xml;

namespace Bot_Application1
{
    public class DotnetflixDB
    {
        public static Task<List<Episode>> FindEpisodesAsync(string subject)
        {
            using (var reader = XmlReader.Create("http://www.dotnetflix.com/rss"))
            {
                var feed = SyndicationFeed.Load(reader);

                return Task.FromResult(feed.Items
                    .Where(i => i.Title.Text.ToLowerInvariant().Contains(subject.ToLowerInvariant()))
                    .Select(i => new Episode
                    {
                        Title = i.Title.Text,
                        Link = i.Links.First().Uri
                    })
                    .ToList());
            }
        }
    }
}