using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DecimeQueSeSiente.Model
{
    public class Player
    {
        public string Name { get; set; }
        public int Number { get; set; }
        public string Country { get; set; }
        public string Url { get; set; }

        private Player(string name, int number, string country, string url)
        {
            this.Name = name;
            this.Number = number;
            this.Country = country;
            this.Url = url;
        }

        public static Player CreatePlayer(string name, int number, string country, string url)
        {
            return new Player(name, number, country, url);
        }

        public static Player CreatePlayer(string name, string url)
        {
            return new Player(name, 0, null, url);
        }
    }
}
