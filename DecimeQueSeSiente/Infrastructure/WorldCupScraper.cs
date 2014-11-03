using DecimeQueSeSiente.Model;
using HelterScraper.Infrastructure;
using HelterScraper.Tools;
using System;
using System.Collections.Generic;
using System.Reactive.Linq;

namespace DecimeQueSeSiente.Infrastructure
{
    class WorldCupScraper
    {
        ScraperEngine scraperEngine;

        public WorldCupScraper(ScraperEngine engine)
        {
            scraperEngine = engine;
        }

        public IObservable<Match> ScrapeMatchesFromUrls(List<string> matchesUrls)
        {
            var validURLs = matchesUrls.ToObservable().Where(Functions.isValidURL);

            return validURLs
                        .Select(x => scraperEngine.Resolve<Match>(x)().Scrape(x))
                        .Merge();
        }
    }
}
