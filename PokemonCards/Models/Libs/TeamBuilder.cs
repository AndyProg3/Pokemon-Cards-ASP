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

            using (SqlCommand cmd = new SqlCommand("select * from team join team_pokemon on team_pokemon.team_id = team.id where team.id = @team", con))
            {
                cmd.Parameters.AddWithValue("@team", team_id);
                Database.Open(ref con);

                List<int> ptIds = new List<int>();
                List<int> pokemonIds = new List<int>();
                List<TeamModel> team = new List<TeamModel>();
                List<int> hps = new List<int>();
                SqlDataReader nwReader = cmd.ExecuteReader();
                while (nwReader.Read())
                {
                    team.Add(new TeamModel((int)nwReader["team_id"], (string)nwReader["is_comp"]));
                    pokemonIds.Add((int)nwReader["pokemon_id"]);
                    hps.Add((int)nwReader["hp"]);
                    ptIds.Add((int)nwReader["pt_id"]);
                }

                nwReader.Close();

                if (team.Count > 0)
                {
                    List<PokemonModel> pokemon = new List<PokemonModel>();

                    foreach (int id in pokemonIds) {
                        List<int> i = new List<int>();
                        i.Add(id);
                        pokemon.Add(Pokemon.GetPokemon(i, con)[0]);
                    }

                    for (int c = 0; c < pokemon.Count; c++)
                    {
                        TeamPokemonModel tmp = new TeamPokemonModel();
                        tmp.team = team[c];
                        tmp.pokemon = pokemon[c];
                        tmp.hp = hps[c];
                        tmp.pt_id = ptIds[c];

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
        public static string TeamPokemonAction(int team_id, string action, int id)
        {
            if (action == "add")
                return addTeamPokemon(team_id, id);
            else if (action == "remove")
                return removeTeamPokemon(team_id, id);

            return "EAction undefined.";
        }

        /// <summary>
        /// Adds a pokemon to a given team
        /// </summary>
        /// <param name="team_id">The team id for the pokemon to reference</param>
        /// <param name="id">The Pokemon Id</param>
        /// <param name="con">Optional param for use if already connected to database</param>
        /// <returns>A true or false value for error output</returns>
        private static string addTeamPokemon(int team_id, int id, SqlConnection con = null)
        {
            //Check to see if team already has 5 pokemon
            //If so send error message back
            if (GetTeam(team_id, con).Count() >= 5)
            {
                return "EYour team is full.";
            }

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
                    return "SSuccessfully added.";
                else
                    return "EError adding Pokemon to team.";
            }
        }


        /// <summary>
        /// Removes a pokemon on a given team
        /// </summary>
        /// <param name="team_id">The team id for the pokemon to reference</param>
        /// <param name="id">The Pokemon Id</param>
        /// <param name="con">Optional param for use if already connected to database</param>
        /// <returns>A true or false value for error output</returns>
        private static string removeTeamPokemon(int team_id, int id, SqlConnection con = null)
        {
            con = Database.GetCon(con);

            using (SqlCommand cmd = new SqlCommand("delete from team_pokemon where team_id = @team and pt_id = @id", con))
            {
                cmd.Parameters.AddWithValue("@team", team_id);
                cmd.Parameters.AddWithValue("@id", id);
                Database.Open(ref con);

                int val = cmd.ExecuteNonQuery();

                Database.Close(ref con);

                if (val > 0)
                    return "SSuccessfully removed.";
                else
                    return "EError removing Pokemon from team.";
            }
        }
    }
}