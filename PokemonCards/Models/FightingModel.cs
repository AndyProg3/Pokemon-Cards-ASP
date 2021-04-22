using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PokemonCards.Models.Libs;

namespace PokemonCards.Models
{
    public class FightingModel
    {
        public int id { get; set; }
        public List<TeamPokemonModel> compTeam { get; set; }
        public List<TeamPokemonModel> userTeam { get; set; }

        public PokemonModel comp_pokemon { get; set; }
        public PokemonModel user_pokemon { get; set; }

        public List<FightingLogModel> log { get; set; }

        public FightingModel(int id, List<TeamPokemonModel> compTeam, List<TeamPokemonModel> userTeam)
        {
            this.id = id;
            this.compTeam = compTeam;
            this.userTeam = userTeam;

            log = new List<FightingLogModel>();
        }

        public void LogAttack(string attackMsg, bool isComp)
        {
            log.Add(Fight.LogAttack(this.id, attackMsg, isComp));
        }
    }
}