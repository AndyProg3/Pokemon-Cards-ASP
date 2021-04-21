using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PokemonCards.Models
{
    public class PokemonModel
    {
        public int id { get; set; }
        public string name { get; set; }

        public List<PokemonImageModel> images { get; set; }
        public int hp { get; set; }
        public int weight { get; set; }
        public int level { get; set; }

        public PokemonModel(int id,string name, int hp, 
                    int weight, int level)
        {
            this.id = id;
            this.name = name;
            this.hp = hp;
            this.weight = weight;
            this.level = level;
        }

        public string GetDisplayImage()
        {
            if(images.Count > 0)
                return images[0].img_location;

            return "";
        }
    }
}