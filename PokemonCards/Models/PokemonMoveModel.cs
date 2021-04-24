using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PokemonCards.Models
{
    public class PokemonMoveModel
    {
        public int id { get; set; }
        public string name { get; set; }
        public int power { get; set; }
        public int accuracy { get; set; }
        public int pp { get; set; }

        public TypeModel type { get; set; }

        public PokemonMoveModel(int id, string name, int power, int accuracy, int pp)
        {
            this.name = name;
            this.power = power;
            this.accuracy = accuracy;
            this.pp = pp;
        }

    }
}