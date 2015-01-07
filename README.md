# YAWCS - Yet Another World Cup Scraper

It will scrape information from the [2014 Fifa World Cup Brazil](http://www.fifa.com/worldcup/matches/) using Reactive Extensions for .Net so you will feel the async experience&trade;

* You'll have to download and build the solution https://github.com/lucianodelucchi/HelterScraper
* Then you'll have to update the HelterScraper reference in DecimeQueSeSiente project to point to the dll you've just built in the previous point.

Sorry about that but for the moment I don't want to submit a nuget package and I've tried adding the HelterScraper solution as a git submodule but there is an issue with SQLitePCL.raw_basic.targets while compiling the solution.

Let me know if you find a solution.