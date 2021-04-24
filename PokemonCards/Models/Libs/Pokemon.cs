using System    ;
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

            string _query = "select *, " +
                            "(select t2.name from pokemon_types pt2 join types t2 on pt2.type_id = t2.type_id where pt2.[order] = 1 and pt2.pokemon_id = p.pokemon_id) type1, " +
                            "(select t2.type_id from pokemon_types pt2 join types t2 on pt2.type_id = t2.type_id where pt2.[order] = 1 and pt2.pokemon_id = p.pokemon_id) type1_id, " +
                            "(select t2.name from pokemon_types pt2 join types t2 on pt2.type_id = t2.type_id where pt2.[order] = 2 and pt2.pokemon_id = p.pokemon_id) type2, " +
                            "(select t2.type_id from pokemon_types pt2 join types t2 on pt2.type_id = t2.type_id where pt2.[order] = 2 and pt2.pokemon_id = p.pokemon_id) type2_id " +
                            " from pokemon p ";

            if (ids != null && ids.Count > 0)
                _query += "where p.pokemon_id in (@id)";

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

                    
                    if(nwReader["type1"] != DBNull.Value)
                        tmp.types.Add(new TypeModel((int)nwReader["type1_id"], (string)nwReader["type1"]));

                    if(nwReader["type2"] != DBNull.Value)
                        tmp.types.Add(new TypeModel((int)nwReader["type2_id"], (string)nwReader["type2"]));

                    poke.Add(tmp);
                }

                nwReader.Close();

                foreach(var tmp in poke)
                {
                    tmp.images = GetImages(tmp.id, con, false);
                    tmp.moves = GetMoves(tmp.id, con, false);
                }

                Database.Close(ref con);

                return poke;
            }
        }

        public static List<PokemonImageModel> GetImages(int id, SqlConnection con = null, bool close = true)
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
                Database.Close(ref con, close);

                return img;
            }
        }

        public static List<PokemonMoveModel> GetMoves(int id, SqlConnection con = null, bool close = true)
        {
            List<PokemonMoveModel> moves = new List<PokemonMoveModel>();
            con = Database.GetCon(con);

            using (SqlCommand cmd = new SqlCommand("select m.move_id, m.name move_name, m.power, m.accuracy, m.pp, t.name type_name, t.type_id " +
                                                " from pokemon p " +
                                                " join pokemon_moves pt on pt.pokemon_id = p.pokemon_id " +
                                                " join moves m on m.move_id = pt.move_id " +
                                                " join types t on m.type_id = t.type_id " +
                                                " where p.pokemon_id = (@id) and m.power != 0 and m.accuracy != 0", con))
            {
                cmd.Parameters.AddWithValue("@id", id);

                Database.Open(ref con);

                SqlDataReader nwReader = cmd.ExecuteReader();
                while (nwReader.Read())
                {
                    PokemonMoveModel tmp =
                        new PokemonMoveModel((int)nwReader["move_id"],
                            (string)nwReader["move_name"],
                            (int)nwReader["power"],
                            (int)nwReader["accuracy"],
                            (int)nwReader["pp"]);

                    tmp.type = new TypeModel((int)nwReader["type_id"], (string)nwReader["type_name"]);

                    moves.Add(tmp);
                }

                nwReader.Close();
                Database.Close(ref con, close);

                return moves;
            }
        }

        public static int GetDamageFactor(int attackingTypeId, int targetTypeId, SqlConnection con = null, bool close = true)
        {
            con = Database.GetCon(con);

            using (SqlCommand cmd = new SqlCommand("select damage_factor from type_affect " +
                                                    " where damage_type_id = (@attackId)" +
                                                    " and   target_type_id = (@targetId)", con))
            {
                cmd.Parameters.AddWithValue("@attackId", attackingTypeId);
                cmd.Parameters.AddWithValue("@targetId", targetTypeId);

                Database.Open(ref con);

                int factor = Convert.ToInt32(cmd.ExecuteScalar());

                Database.Close(ref con, close);

                return factor;
            }
        }
    }
}