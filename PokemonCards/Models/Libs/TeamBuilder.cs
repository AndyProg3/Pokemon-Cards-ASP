using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using PokemonCards.Models.Libs;

namespace PokemonCards.Models
{
    public static class TeamBuilder
    {
        /// <summary>
        /// Insert a new team into the Team db
        /// </summary>
        /// <param name="con">Optional param for use if already connected to database</param>
        /// <returns>The user's Team Id</returns>
        public static int CreateTeam(SqlConnection con = null)
        {
            con = Database.GetCon(con);

            using (SqlCommand cmd = new SqlCommand("INSERT INTO Team (is_comp) output INSERTED.ID VALUES (@comp)", con))
            {
                cmd.Parameters.AddWithValue("@comp", 'N');
                Database.Open(ref con);

                int val = (int)cmd.ExecuteScalar();

                Database.Close(ref con);

                return val;
            }
        }

        /// <summary>
        /// Get all pokemon for a given team_id
        /// </summary>
        /// <param name="team_id">The ID for a team</param>
        /// <param name="con">Optional param for use if already connected to database</param>
        /// <returns>List of TeamPokemonModels</returns>
        public static List<TeamPokemonModel> GetTeam(int team_id, SqlConnection con = null)
        {
            List<TeamPokemonModel> poke = new List<TeamPokemonModel>();
            con = Database.GetCon(con);

            using (SqlCommand cmd = new SqlCommand("select * from team_pokemon where team_id = @team", con))
            {
                cmd.Parameters.AddWithValue("@team", team_id);
                Database.Open(ref con);

                List<int> pokemonIds = new List<int>();
                List<int> teamIds = new List<int>();
                List<int> hps = new List<int>();
                SqlDataReader nwReader = cmd.ExecuteReader();
                while (nwReader.Read())
                {
                    teamIds.Add((int)nwReader["team_id"]);
                    pokemonIds.Add((int)nwReader["pokemon_id"]);
                    hps.Add((int)nwReader["hp"]);
                }

                nwReader.Close();

                if (teamIds.Count > 0)
                {
                    List<PokemonModel> pokemon = Pokemon.GetPokemon(pokemonIds, con);

                    for (int c = 0; c < pokemon.Count; c++)
                    {
                        TeamPokemonModel tmp = new TeamPokemonModel();
                        tmp.team_id = teamIds[c];
                        tmp.pokemon = pokemon[c];
                        tmp.hp = hps[c];

                        poke.Add(tmp);
                    }
                }

                Database.Close(ref con);

                return poke;
            }
        }
        
        /// <summary>
        /// This will take in an action from and decide what should be done with the arguments
        /// </summary>
        /// <param name="team_id">ID of the team referencing the Pokemon Id</param>
        /// <param name="action">The action to take (add || remove)</param>
        /// <param name="id">The Pokemon Id</param>
        /// <returns>True or False value for error output</returns>
        public static bool TeamPokemonAction(int team_id, string action, int id)
        {
            if (action == "add")
                return addTeamPokemon(team_id, id);
            else if (action == "remove")
                return removeTeamPokemon(team_id, id);

            return false;
        }

        /// <summary>
        /// Adds a pokemon to a given team
        /// </summary>
        /// <param name="team_id">The team id for the pokemon to reference</param>
        /// <param name="id">The Pokemon Id</param>
        /// <param name="con">Optional param for use if already connected to database</param>
        /// <returns>A true or false value for error output</returns>
        private static bool addTeamPokemon(int team_id, int id, SqlConnection con = null)
        {
            con = Database.GetCon(con);
            List<int> ids = new List<int>();
            ids.Add(id);

            List<PokemonModel> poke = Pokemon.GetPokemon(ids);

            using (SqlCommand cmd = new SqlCommand("INSERT INTO team_pokemon (team_id, pokemon_id, hp) VALUES (@team, @poke, @hp)", con))
            {
                cmd.Parameters.AddWithValue("@team", team_id);
                cmd.Parameters.AddWithValue("@poke", poke[0].id);
                cmd.Parameters.AddWithValue("@hp", poke[0].hp);
                Database.Open(ref con);

                int val = cmd.ExecuteNonQuery();

                Database.Close(ref con);

                if (val > 0)
                    return true;
                else
                    return false;
            }
        }


        /// <summary>
        /// Removes a pokemon on a given team
        /// </summary>
        /// <param name="team_id">The team id for the pokemon to reference</param>
        /// <param name="id">The Pokemon Id</param>
        /// <param name="con">Optional param for use if already connected to database</param>
        /// <returns>A true or false value for error output</returns>
        private static bool removeTeamPokemon(int team_id, int id, SqlConnection con = null)
        {
            con = Database.GetCon(con);

            using (SqlCommand cmd = new SqlCommand("delete from team_pokemon where team_id = @team and pokemon_id = @poke", con))
            {
                cmd.Parameters.AddWithValue("@team", team_id);
                cmd.Parameters.AddWithValue("@poke", id);
                Database.Open(ref con);

                int val = cmd.ExecuteNonQuery();

                Database.Close(ref con);

                if (val > 0)
                    return true;
                else
                    return false;
            }
        }
    }
}