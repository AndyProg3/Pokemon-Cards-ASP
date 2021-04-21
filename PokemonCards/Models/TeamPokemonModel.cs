using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PokemonCards.Models
{
    public class TeamPokemonModel
    {
        public int pt_id { get; set; }
        public TeamModel team { get; set; }
        public PokemonModel pokemon { get; set; }
        public int hp { get; set; }
    }
}