using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DecimeQueSeSiente.Model
{
    public class Match
    {
        public enum MatchResult { Draw, Home, Away }

        public string HomeTeam { get; set; }
        public string AwayTeam { get; set; }
        public int HomeScore { get; set; }
        public int AwayScore { get; set; }
        public DateTime Date { get; set; }
        public List<Goal> HomeGoals { get; set; }
        public List<Goal> AwayGoals { get; set; }
        public int? HomePenalties { get; set; }
        public int? AwayPenalties { get; set; }
        public string Url { get; set; }

        public List<Goal> AllGoals
        {
            get
            {
                return HomeGoals.Concat(AwayGoals).ToList();
            }
        }

        public MatchResult Result
        {
            get
            {
                if (HomeScore == AwayScore)
                {
                    return MatchResult.Draw;
                }

                if (HomeScore > AwayScore)
                {
                    return MatchResult.Home;
                }
                else
                {
                    return MatchResult.Away;
                }
            }
        }

        public bool DecidedOnPenalties
        {
            get
            {
                return this.Result == MatchResult.Draw && this.HomePenalties.HasValue;
            }
        }

        public bool HasGoals
        {
            get
            {
                return this.HomeScore > 0 || this.AwayScore > 0;
            }
        }

        public bool DecidedInExtraTime
        {
            get
            {
                return this.AllGoals.Exists(goal => goal.InExtraTime);
            }
        }

        private Match(string homeTeam, string awayTeam, int homeScore, int awayScore, DateTime date, string url)
        {
            this.HomeTeam = homeTeam;
            this.AwayTeam = awayTeam;
            this.HomeScore = homeScore;
            this.AwayScore = awayScore;
            this.Date = date;
            this.HomeGoals  = new List<Goal>();
            this.AwayGoals = new List<Goal>();
            this.Url = url;
        }

        public static Match CreateMatch(string homeTeam, string awayTeam, int homeScore, int awayScore, DateTime date, string url)
        {
            return new Match(homeTeam, awayTeam, homeScore, awayScore, date, url);
        }
    }
}
