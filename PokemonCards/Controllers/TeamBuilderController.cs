using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PokemonCards.Controllers
{
    public class TeamBuilderController : Controller
    {
        // GET: TeamBuilder
        public ActionResult Index()
        {
            return View();
        }
    }
}