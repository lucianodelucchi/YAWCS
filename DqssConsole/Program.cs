using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reactive.Linq;

namespace DqssConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            var urls = new List<string> 
            {
                "http://www.fifa.com/worldcup/matches/round=255931/match=300186456",
                "http://www.fifa.com/worldcup/matches/round=255931/match=300186492",
                "http://www.fifa.com/worldcup/matches/round=255931/match=300186510",
                "http://www.fifa.com/worldcup/matches/round=255931/match=300186473",
                "http://www.fifa.com/worldcup/matches/round=255931/match=300186471",
                "http://www.fifa.com/worldcup/matches/round=255931/match=300186489",
                "http://www.fifa.com/worldcup/matches/round=255931/match=300186513",
                "http://www.fifa.com/worldcup/matches/round=255931/match=300186507",
                "http://www.fifa.com/worldcup/matches/round=255931/match=300186494",
                "http://www.fifa.com/worldcup/matches/round=255931/match=300186496",
                "http://www.fifa.com/worldcup/matches/round=255931/match=300186477",
                "http://www.fifa.com/worldcup/matches/round=255931/match=300186475",
                "http://www.fifa.com/worldcup/matches/round=255931/match=300186505",
                "http://www.fifa.com/worldcup/matches/round=255931/match=300186512",
                "http://www.fifa.com/worldcup/matches/round=255931/match=300186479",
                "http://www.fifa.com/worldcup/matches/round=255931/match=300186509",
                "http://www.fifa.com/worldcup/matches/round=255931/match=300186499",
                "http://www.fifa.com/worldcup/matches/round=255931/match=300186478",
                "http://www.fifa.com/worldcup/matches/round=255931/match=300186498",
                "http://www.fifa.com/worldcup/matches/round=255931/match=300186453",
                "http://www.fifa.com/worldcup/matches/round=255931/match=300186468",
                "http://www.fifa.com/worldcup/matches/round=255931/match=300186486",
                "http://www.fifa.com/worldcup/matches/round=255931/match=300186454",
                "http://www.fifa.com/worldcup/matches/round=255931/match=300186500",
                "http://www.fifa.com/worldcup/matches/round=255931/match=300186514",
                "http://www.fifa.com/worldcup/matches/round=255931/match=300186463",
                "http://www.fifa.com/worldcup/matches/round=255931/match=300186466",
                "http://www.fifa.com/worldcup/matches/round=255931/match=300186493",
                "http://www.fifa.com/worldcup/matches/round=255931/match=300186511",
                "http://www.fifa.com/worldcup/matches/round=255931/match=300186481",
                "http://www.fifa.com/worldcup/matches/round=255931/match=300186495",
                "http://www.fifa.com/worldcup/matches/round=255931/match=300186483",
                "http://www.fifa.com/worldcup/matches/round=255931/match=300186470",
                "http://www.fifa.com/worldcup/matches/round=255931/match=300186467",
                "http://www.fifa.com/worldcup/matches/round=255931/match=300186472",
                "http://www.fifa.com/worldcup/matches/round=255931/match=300186452",
                "http://www.fifa.com/worldcup/matches/round=255931/match=300186465",
                "http://www.fifa.com/worldcup/matches/round=255931/match=300186484",
                "http://www.fifa.com/worldcup/matches/round=255931/match=300186457",
                "http://www.fifa.com/worldcup/matches/round=255931/match=300186455",
                "http://www.fifa.com/worldcup/matches/round=255931/match=300186458",
                "http://www.fifa.com/worldcup/matches/round=255931/match=300186464",
                "http://www.fifa.com/worldcup/matches/round=255931/match=300186482",
                "http://www.fifa.com/worldcup/matches/round=255931/match=300186515",
                "http://www.fifa.com/worldcup/matches/round=255931/match=300186476",
                "http://www.fifa.com/worldcup/matches/round=255931/match=300186469",
                "http://www.fifa.com/worldcup/matches/round=255931/match=300186480",
                "http://www.fifa.com/worldcup/matches/round=255931/match=300186506",
                "http://www.fifa.com/worldcup/matches/round=255951/match=300186487",
                "http://www.fifa.com/worldcup/matches/round=255951/match=300186491",
                "http://www.fifa.com/worldcup/matches/round=255951/match=300186508",
                "http://www.fifa.com/worldcup/matches/round=255951/match=300186459",
                "http://www.fifa.com/worldcup/matches/round=255951/match=300186462",
                "http://www.fifa.com/worldcup/matches/round=255951/match=300186460",
                "http://www.fifa.com/worldcup/matches/round=255951/match=300186503",
                "http://www.fifa.com/worldcup/matches/round=255951/match=300186497",
                "http://www.fifa.com/worldcup/matches/round=255953/match=300186485",
                "http://www.fifa.com/worldcup/matches/round=255953/match=300186461",
                "http://www.fifa.com/worldcup/matches/round=255953/match=300186504",
                "http://www.fifa.com/worldcup/matches/round=255953/match=300186488",
                "http://www.fifa.com/worldcup/matches/round=255955/match=300186474",
                "http://www.fifa.com/worldcup/matches/round=255955/match=300186490",
                "http://www.fifa.com/worldcup/matches/round=255957/match=300186502",
                "http://www.fifa.com/worldcup/matches/round=255959/match=300186501"
            };

            var wcs = new DecimeQueSeSiente.Bootsrapper();
            var matches = wcs.ScrapeMatchesFromUrls(urls);

            var sharedSubscription = matches.Publish().RefCount();
                        
            var allGoals = sharedSubscription.SelectMany(match => match.AllGoals.Where(g => !g.IsPenalty));
            /* All goals ordered by minute */
            allGoals
                .ToList()
                .Subscribe(
                        g => g
                            .OrderBy(goal => goal.Minute)
                            .ToList()
                            .ForEach(goal => Console.WriteLine(goalRepresentation(goal))),
                        () => Console.WriteLine("Finished All goals ordered by minute")
            );

            /* Total goals */
            sharedSubscription
                .Sum(m => m.AllGoals.Count) //or .Aggregate(0, (acc, m) => acc + m.AllGoals.Count)
                .Subscribe(Console.WriteLine);

            /* Total penalties */
            //sharedSubscription
            //        .Where(m => m.DecidedOnPenalties)
            //        .Aggregate(0, (acc, m) => acc + m.HomePenalties.Value + m.AwayPenalties.Value)
            //        .Subscribe(t => Console.WriteLine("Total penalties: {0}", t));

            /* Al goals scored in additional (stoppage, injury) time but not in extra time */
            //allGoals
            //    .Where(goal => !goal.InExtraTime && goal.Minute > 90)
            //    .Subscribe(
            //        goal => Console.WriteLine(goalRepresentation(goal)),
            //        () => Console.WriteLine("Finished")
            //);

            Console.ReadKey();
        }

        static string FormatOutput(DecimeQueSeSiente.Model.Match match)
        {
            var sb = new StringBuilder();
            sb.AppendLine(string.Format("{0} vs {1} - {2}", match.HomeTeam, match.AwayTeam, match.Date.ToString("g")));
            sb.AppendLine(string.Format("{0} - {1}", match.HomeScore, match.AwayScore));
            
            if (match.Result != DecimeQueSeSiente.Model.Match.MatchResult.Draw)
            {
                match.HomeGoals.ToList().ForEach(goal => sb.AppendLine(goalRepresentation(goal)));
                sb.AppendLine();
                match.AwayGoals.ToList().ForEach(goal => sb.AppendLine(goalRepresentation(goal)));
                sb.AppendLine();
            }
            
            if (match.DecidedOnPenalties)
            {
                sb.AppendLine("On Penalties");
                sb.AppendLine(string.Format("{0} - {1}", match.HomePenalties, match.AwayPenalties));
                sb.AppendLine();
            }
            
            sb.AppendLine(match.Url);
            sb.AppendLine(string.Format("{0}", new String('-', 10)));
            sb.AppendLine();

            return sb.ToString();
        }

        static Func<DecimeQueSeSiente.Model.Goal, string> goalRepresentation =
            (goal) =>
            {
                return string.Format("{0} {1} ({2}) ", 
                                    goal.Scorer.Name, 
                                    goal.Minute, 
                                    goal.Representation);
            };
    }
}
