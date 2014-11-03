using DecimeQueSeSiente.Model;
using HelterScraper.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DecimeQueSeSiente.Infrastructure
{
    interface IMatchScraper
    {
        string HomeTeamSelector { get; }
        string AwayTeamSelector { get; }
        string HomeScoreSelector { get; }
        string AwayScoreSelector { get; }
        string DateSelector { get; }
        string HomeScorersSelector { get; }
        string HomeScorerNameSelector { get; }
        string HomeScorerTimeSelector { get; }
        string AwayScorersSelector { get; }
        string AwayScorerNameSelector { get; }
        string AwayScorerTimeSelector { get; }
        string PenaltiesResultSelector { get; }
    }

    class FifaMatchScraper : ObjectScraper<Match>, IMatchScraper
    {
        public string HomeTeamSelector { get { return "div.mh.result div.t.home span.t-nText"; } }
        public string AwayTeamSelector { get { return "div.mh.result div.t.away span.t-nText"; } }
        public string HomeScoreSelector { get { return "div.mh.result span.s-scoreText"; } }
        public string AwayScoreSelector { get { return HomeScoreSelector; } }
        public string DateSelector { get { return "div.mh-i-datetime"; } }
        public string HomeScorersSelector { get { return "div.t-scorer.home li.mh-scorer"; } }
        public string HomeScorerNameSelector { get { return "span.p-n-webname"; } }
        public string HomeScorerURLSelector { get { return "a"; } }
        public string HomeScorerTimeSelector { get { return "span.ml-scorer-evmin span"; } }
        public string AwayScorersSelector { get { return "div.t-scorer.away li.mh-scorer"; } }
        public string AwayScorerNameSelector { get { return HomeScorerNameSelector; } }
        public string AwayScorerURLSelector { get { return HomeScorerURLSelector; } }
        public string AwayScorerTimeSelector { get { return HomeScorerTimeSelector; } }
        public string PenaltiesResultSelector { get { return "div.mh-m div.mu-reasonwin span.text-reasonwin"; } }

        protected override string BaseURL { get { return "http://www.fifa.com/worldcup/matches/"; } }

        public FifaMatchScraper() : base() { }

        protected override Match ScrapeToObject(string html)
        {
            var matchBuilder = builderFactory();

            matchBuilder.ParseHtml(html);

            var homeTeam = matchBuilder.GetTextFromSelector(this.HomeTeamSelector);
            var awayTeam = matchBuilder.GetTextFromSelector(this.AwayTeamSelector);

            var scores = GetScore(matchBuilder.GetTextFromSelector(this.HomeScoreSelector));

            var date = GetDate(matchBuilder.GetTextFromSelector(this.DateSelector));

            var match = Match.CreateMatch(homeTeam, awayTeam, scores.Item1, scores.Item2, date, this.StartUrl);

            var homeGoals = GetGoals(matchBuilder.ScrapeTriplet(this.HomeScorersSelector, this.HomeScorerURLSelector, this.HomeScorerNameSelector, this.HomeScorerTimeSelector));
            var awayGoals = GetGoals(matchBuilder.ScrapeTriplet(this.AwayScorersSelector, this.AwayScorerURLSelector, this.AwayScorerNameSelector, this.AwayScorerTimeSelector));

            match.HomeGoals = homeGoals;
            match.AwayGoals = awayGoals;

            if (match.Result == Match.MatchResult.Draw)
            {
                var penaltiesResult = GetPenaltiesScore(matchBuilder.GetTextFromSelector(this.PenaltiesResultSelector));
                match.HomePenalties = penaltiesResult.Item1;
                match.AwayPenalties = penaltiesResult.Item2;
            }

            return match;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rawScore"></param>
        /// <returns></returns>
        Tuple<int,int> GetScore(string rawScore)
        {
            var scores = rawScore.Split('-').Select(int.Parse).ToArray();
            return Tuple.Create(scores[0], scores[1]);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rawDate"></param>
        /// <returns></returns>
        DateTime GetDate(string rawDate)
        {
            var parsableDate = rawDate.Replace("Local time", string.Empty).Replace("-", string.Empty).Trim();
            
            DateTime date;

            DateTime.TryParse(parsableDate, out date);
            
            return date;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rawScorers"></param>
        /// <returns></returns>
        List<Goal> GetGoals(IEnumerable<Tuple<string, string, IEnumerable<string>>> rawScorers)
        {
            var goalList = new List<Goal>();
            rawScorers
                .ToList()
                .ForEach(x =>
                    {
                        var player = Player.CreatePlayer(x.Item2, BuildAbsoluteUrl(x.Item1));
                        x.Item3.ToList().ForEach(minute =>
                                {
                                    var goal = GetGoal(minute);
                                    goal.Scorer = player;
                                    goalList.Add(goal);
                                }
                        );
                    }
                );
            return goalList;
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="rawMinute"></param>
        /// <returns></returns>
        Goal GetGoal(string rawMinute)
        {
            var goal = Goal.CreateGoal(rawMinute, null, 0);

            // figure out how to model 90+3 
            // it's not the same as 93 this means 3 minutes after the first half in extra time
            var minuteMatch = System.Text.RegularExpressions.Regex.Match(rawMinute, @"(?'time'\d+)'(?'added'\+\d+)?(?'og'\sOG)?(?'pen'\sPEN)?");

            int retMinute = 0;

            if (minuteMatch.Success)
            {
                var addedTime = int.Parse(string.IsNullOrEmpty(minuteMatch.Groups["added"].Value) ? "0" : minuteMatch.Groups["added"].Value);
                retMinute = int.Parse(minuteMatch.Groups["time"].Value);

                goal.Minute = retMinute + addedTime;

                goal.IsOwnGoal = minuteMatch.Groups["og"].Success;
                goal.IsPenalty = minuteMatch.Groups["pen"].Success;
            }

            return goal;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rawScore"></param>
        /// <returns></returns>
        Tuple<int?, int?> GetPenaltiesScore(string rawScore)
        {
            var scoreMatch = System.Text.RegularExpressions.Regex.Match(rawScore, @"\((?'score'\d+\s+-\s+\d+)\)");

            if (!scoreMatch.Success)
            {
                return new Tuple<int?, int?>(null, null);                
            }

            var score = scoreMatch.Groups["score"].Value.Replace(" ", string.Empty).Split('-').Select(int.Parse).ToArray();

            return new Tuple<int?, int?>(score[0], score[1]);
        }
    }
}
