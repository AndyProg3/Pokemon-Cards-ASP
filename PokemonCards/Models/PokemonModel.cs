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
        public List<PokemonMoveModel> moves { get; set; }

        public List<TypeModel> types { get; set; }
        public int hp { get; set; }
        public int weight { get; set; }
        public int level { get; set; }

        public PokemonModel(int id, string name, int hp, 
                    int weight, int level)
        {
            this.id = id;
            this.name = name;
            this.hp = hp;
            this.weight = weight;
            this.level = level;

            types = new List<TypeModel>();
        }

        public string GetDisplayImage()
        {
            if(images.Count > 0)
                return images[0].img_location;

            return "";
        }

        public List<PokemonMoveModel> Get4RandomMoves()
        {
            Random rand = new Random();

            List<PokemonMoveModel> randMoves = new List<PokemonMoveModel>();

            for (int c = 0; c < 4; c++)
            {
                randMoves.Add(this.moves[rand.Next(0, moves.Count)]);
            }

            return randMoves;
        }

        public PokemonMoveModel FindMove(int id)
        {
            foreach(var c in moves)
            {
                if (c.id == id)
                    return c;
            }

            return null;
        }
    }
}