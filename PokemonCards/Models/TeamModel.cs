﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PokemonCards.Models
{
    public class TeamModel
    {
        public int id { get; set; }
        public string isComp { get; set; }

        public TeamModel(int id, string isComp)
        {
            this.id = id;
            this.isComp = isComp;
        }
    }
}