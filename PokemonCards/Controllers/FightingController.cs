using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PokemonCards.Controllers
{
    public class FightingController : Controller
    {
        // GET: Fighting
        public ActionResult Index()
        {
            return View();
        }
    }
}