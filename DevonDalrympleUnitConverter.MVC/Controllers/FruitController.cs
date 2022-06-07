using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace DevonDalrympleUnitConverter.MVC.Controllers
{
    public class FruitController : Controller
    {

        List<string> _fruit = new List<string>()
        {
            "Apple",
            "Orange",
            "Pear",
            "Lettuce", //Hey wait a second!!
            "Lemon",
            "Lime",
            "Banana",
            "Peach"
        };


        public IEnumerable<string> Index()
        {
            return _fruit;
        }

        public IActionResult Display(int id)
        {
            if (id >= 0 && id < _fruit.Count)
            {
                return Ok(_fruit[id]);
            }
            return BadRequest();
        }

    }
}
