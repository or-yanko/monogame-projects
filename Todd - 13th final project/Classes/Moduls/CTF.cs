using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Todd.Sprites;

namespace Todd.Moduls
{
    public class CTF
    {
        public DateTime date { get; set; }
        public string name { get; set; }
        public Boolean isDone { get; set; }
        public CTF(DateTime date, string name, bool isDone)
        {
            this.date = date;
            this.name = name;
            this.isDone = isDone;
        }

        public CTF(string name)
        {
            this.date = new DateTime(2020, 1, 1);
            this.name = name;
            this.isDone = false;
        }
        public CTF()
        {
            this.date = new DateTime(2020, 1, 1);
            this.name = "";
            this.isDone = false;
        }





    }


}

