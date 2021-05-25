using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Todd.Moduls
{
    public class CoinsAndHearts
    {
        public CoinsAndHearts(int goldCoins, int silverCoins, int bronzeCoins, int hearts)
        {
            this.goldCoins = goldCoins;
            this.silverCoins = silverCoins;
            this.bronzeCoins = bronzeCoins;
            this.hearts = hearts;
        }
        public CoinsAndHearts()
        {
            this.goldCoins = 0;
            this.silverCoins = 0;
            this.bronzeCoins = 0;
            this.hearts = 0;
        }

        public int goldCoins { get; set; }
        public int silverCoins { get; set; }
        public int bronzeCoins { get; set; }
        public int hearts { get; set; }

    }
}
