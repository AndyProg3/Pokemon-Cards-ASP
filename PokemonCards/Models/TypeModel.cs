using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PokemonCards.Models
{
    public class TypeModel
    {
        public int typeId { get; set; }
        public string name { get; set; }

        public TypeModel(int typeId, string name)
        {
            this.typeId = typeId;
            this.name = name;
        }
    }
}