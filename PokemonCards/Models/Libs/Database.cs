using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;

namespace PokemonCards.Models
{
    public static class Database
    {
        public static SqlConnection GetCon(SqlConnection con)
        {
            if(con == null)
                return new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\thera\source\repos\PokemonCards\PokemonCards\App_Data\Pokemon.mdf;Integrated Security=True");

            return con;
        }

        public static void Open(ref SqlConnection con)
        {
            if (con.State != System.Data.ConnectionState.Open)
                con.Open();
        }

        public static void Close(ref SqlConnection con, bool close = true)
        {
            if (close == true)
            {
                if (con.State != System.Data.ConnectionState.Open)
                    con.Close();
            }
        }
    }
}