using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace PokemonCards.Models.Libs
{
    public static class Fight
    {
        public static FightingModel FindFight(int userTeamId, SqlConnection con = null)
        {
            con = Database.GetCon(con);

            using (SqlCommand cmd = new SqlCommand("select * from fighting where user_team_id = (@userId)" +
                " and fight_id = (select max(fight_id) from fighting where user_team_id = (@userId))", con))
            {
                FightingModel fight;
                cmd.Parameters.AddWithValue("@userId", userTeamId);

                Database.Open(ref con);

                int fightId = 0;
                int compTeamId = 0;
                List<int> compPoke = new List<int>();
                List<int> userPoke = new List<int>();

                SqlDataReader nwReader = cmd.ExecuteReader();
                while (nwReader.Read())
                {
                    fightId = (int)nwReader["fight_id"];
                    compTeamId = (int)nwReader["comp_team_id"];
                    compPoke.Add((int)nwReader["comp_pokemon_id"]);
                    userPoke.Add((int)nwReader["user_pokemon_id"]);
                }

                nwReader.Close();

                if(fightId != 0)
                {
                    fight = new FightingModel(fightId,
                        TeamBuilder.GetTeam(compTeamId, con),
                        TeamBuilder.GetTeam(userTeamId, con));


                    fight.comp_pokemon = Pokemon.GetPokemon(compPoke, con)[0];
                    fight.user_pokemon = Pokemon.GetPokemon(userPoke, con)[0];

                    Database.Close(ref con);
                    return fight;
                }

                Database.Close(ref con);

                return null;
            }
        }

        public static FightingModel CreateFight(int compTeamId, int userTeamId, int compPokemonId, int userPokemonId, SqlConnection con = null)
        {
            con = Database.GetCon(con);

            using (SqlCommand cmd = new SqlCommand("INSERT INTO fighting (comp_team_id, user_team_id, comp_pokemon_id, user_pokemon_id) output INSERTED.ID VALUES (@compId, @userId, @compPoke, @userPoke)", con))
            {
                cmd.Parameters.AddWithValue("@compId", compTeamId);
                cmd.Parameters.AddWithValue("@userId", userTeamId);
                cmd.Parameters.AddWithValue("@compPoke", compPokemonId);
                cmd.Parameters.AddWithValue("@userPoke", userPokemonId);

                Database.Open(ref con);

                int val = (int)cmd.ExecuteScalar();

                Database.Close(ref con);

                return FindFight(userTeamId, con);
            }
        }

        public static void UpdateFightPokemon(int fightId, int pokeId, bool isComp, SqlConnection con = null)
        {
            con = Database.GetCon(con);

            string field = "";

            if (isComp)
                field = "comp_pokemon_id";
            else
                field = "user_pokemon_id";

            using (SqlCommand cmd = new SqlCommand("update fighting set " + field + " = (@pokeId) where fight_id = (@fightId)", con))
            {
                cmd.Parameters.AddWithValue("@pokeId", pokeId);
                cmd.Parameters.AddWithValue("@fightId", fightId);

                Database.Open(ref con);

                cmd.ExecuteNonQuery();

                Database.Close(ref con);
            }
        }
        public static FightingLogModel LogAttack(int fightId, string msg, bool isComp, SqlConnection con = null)
        {
            con = Database.GetCon(con);

            using (SqlCommand cmd = new SqlCommand("INSERT INTO fighting_log (fight_id, attack_msg, is_comp) output INSERTED.ID VALUES (@fightId, @msg, @iscomp)", con))
            {
                cmd.Parameters.AddWithValue("@fightId", fightId);
                cmd.Parameters.AddWithValue("@msg", msg);

                if(isComp)
                    cmd.Parameters.AddWithValue("@iscomp", 'Y');
                else
                    cmd.Parameters.AddWithValue("@userPoke", 'N');

                Database.Open(ref con);

                int val = (int)cmd.ExecuteScalar();

                Database.Close(ref con);

                FightingLogModel logOut = new FightingLogModel(val, msg, isComp);

                return logOut;
            }
        }
    }
}