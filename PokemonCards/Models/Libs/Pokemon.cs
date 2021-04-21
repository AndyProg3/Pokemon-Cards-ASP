using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;


namespace PokemonCards.Models.Libs
{
    public static class Pokemon
    {
        /// <summary>
        /// Gets all Pokemon from database
        /// If you want to get specific pokemon pass in a list of the ids you want
        /// </summary>
        /// <param name="ids">Optional list of Pokemon Ids to get</param>
        /// <param name="con">Optional param for use if already connected to database</param>
        /// <returns>List of PokemonModel</returns>
        public static List<PokemonModel> GetPokemon(List<int> ids = null, SqlConnection con = null)
        {
            List<PokemonModel> poke = new List<PokemonModel>();
            con = Database.GetCon(con);

            string _query = "select * from pokemon ";

            if (ids != null && ids.Count > 0)
                _query += "where pokemon_id in (@id)";

            using (SqlCommand cmd = new SqlCommand(_query, con))
            {
                if (ids != null && ids.Count > 0)
                    cmd.Parameters.AddWithValue("@id", string.Join(",", ids));

                Database.Open(ref con);

                SqlDataReader nwReader = cmd.ExecuteReader();
                while (nwReader.Read())
                {
                    PokemonModel tmp =
                        new PokemonModel((int)nwReader["pokemon_id"],
                            (string)nwReader["name"], (int)nwReader["hp"],
                            (int)nwReader["weight"], (int)nwReader["level"]);

                    poke.Add(tmp);
                }

                nwReader.Close();

                foreach(var tmp in poke)
                {
                    tmp.images = GetImages(tmp.id, con);
                }

                Database.Close(ref con);

                return poke;
            }
        }

        public static List<PokemonImageModel> GetImages(int id, SqlConnection con = null)
        {
            List<PokemonImageModel> img = new List<PokemonImageModel>();
            con = Database.GetCon(con);

            using (SqlCommand cmd = new SqlCommand("select * from pokemon_images where pokemon_id = (@id)", con))
            {
                cmd.Parameters.AddWithValue("@id", id);

                Database.Open(ref con);

                SqlDataReader nwReader = cmd.ExecuteReader();
                while (nwReader.Read())
                {
                    PokemonImageModel tmp =
                        new PokemonImageModel((int)nwReader["pi_id"],
                            (string)nwReader["img_location"], 
                            (int)nwReader["order"]);

                    img.Add(tmp);
                }

                nwReader.Close();
                Database.Close(ref con);

                return img;
            }
        }
    }
}