using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PokemonCards.Models
{
    public class FightingLogModel
    {
        public int id { get; set; }
        public string attackMsg { get; set; }
        public bool isComp { get; set; }

        public FightingLogModel(int id, string attackMsg, bool isComp)
        {
            this.id = id;
            this.attackMsg = attackMsg;
            this.isComp = isComp;
        }
    }
}