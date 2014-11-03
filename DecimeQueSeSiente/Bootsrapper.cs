using DecimeQueSeSiente.Infrastructure;
using DecimeQueSeSiente.Model;
using HelterScraper.Infrastructure;
using System;
using System.Collections.Generic;

namespace DecimeQueSeSiente
{
    public class Bootsrapper
    {
        WorldCupScraper worlCupScraper;

        public Bootsrapper()
        {
            Func<ScraperService<Match>> matchScraper = () => new FifaMatchScraper();

            var scraperEngine = new ScraperEngine();
            scraperEngine.RegisterService(matchScraper);

            worlCupScraper = new WorldCupScraper(scraperEngine);
        }

        public IObservable<Match> ScrapeMatchesFromUrls(List<string> matchesUrls)
        {
            return worlCupScraper.ScrapeMatchesFromUrls(matchesUrls);
        }

    }
}
