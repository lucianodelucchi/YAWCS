using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DecimeQueSeSiente.Model
{
    public class Goal
    {
        public string Representation { get; set; }
        public int Minute { get; set; }
        public bool IsOwnGoal { get; set; }
        public bool IsPenalty { get; set; }
        public Player Scorer { get; set; }

        public bool InExtraTime
        {
            get
            {
                return this.Minute > 90 && !this.Representation.Contains("+");
            }
        }

        private Goal(string representation, Player scorer, int minute = 0, bool isOwnGoal = false, bool isPenalty = false, bool inExtraTime = false)
        {
            this.Representation = representation;
            this.Minute = minute;
            this.IsOwnGoal = isOwnGoal;
            this.IsPenalty = isPenalty;
            this.Scorer = scorer;
        }

        public static Goal CreateGoal(string representation, Player scorer, int minute)
        {
            return new Goal(representation, scorer, minute);
        }

        public static Goal CreateGoal(string representation, Player scorer, int minute = 0, bool isOwnGoal = false, bool isPenalty = false)
        {
            return new Goal(representation, scorer, minute, isOwnGoal, isPenalty);
        }
    }
}
