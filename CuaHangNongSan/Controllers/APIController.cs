using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Net;
using CuaHangNongSan.Models;

namespace CuaHangNongSan.Controllers
{
    public class APIController : Controller
    {
        public ActionResult Index()
        {
            var pokemonData = GetPokemonData();
            return View(pokemonData);
        }

        private List<Pok> GetPokemonData()
        {
            using (var webClient = new WebClient())
            {
                var jsonString = webClient.DownloadString("https://omo-api.doc.motdev.vn/pokemon.json");

                var serializer = new JavaScriptSerializer();
                var pokemonList = serializer.Deserialize<List<Pok>>(jsonString);

                return pokemonList;
            }
        }

    }
}