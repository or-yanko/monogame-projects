using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Todd.Moduls
{
    public class Score
    {
        public Score()
        {
        }

        public Score(DateTime date, CoinsAndHearts coinsAndHearts, int timeInSeconds, string level)
        {
            this.date = date;
            this.coinsAndHearts = coinsAndHearts;
            this.timeInSeconds = timeInSeconds;
            this.level = level;
        }

        public DateTime date { get; set; }
        public CoinsAndHearts coinsAndHearts { get; set; }//how many coins earned and how many hearts spent
        public int timeInSeconds { get; set; }
        public string level { get; set; }

    }
}
