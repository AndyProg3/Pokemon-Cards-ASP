using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PokemonCards.Models
{
    public class PokemonImageModel
    {
        public int id { get; set; }
        public string img_location { get; set; }
        public int order { get; set; }

        public PokemonImageModel(int id, string img_location, int order)
        {
            this.id = id;
            this.img_location = img_location;
            this.order = order;
        }
    }
}