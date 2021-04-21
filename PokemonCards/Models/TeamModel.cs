using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PokemonCards.Models
{
    public class TeamModel
    {
        public int id { get; set; }
        public string is_comp { get; set; }

        public TeamModel(int id, string is_comp)
        {
            this.id = id;
            this.is_comp = is_comp;
        }
    }
}