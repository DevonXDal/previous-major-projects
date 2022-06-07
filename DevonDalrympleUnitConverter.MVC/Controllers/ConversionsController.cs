using DevonDalrympleUnitConverter.MVC.Models;
using DevonDalrympleUnitConverter.MVC.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;

namespace DevonDalrympleUnitConverter.MVC.Controllers
{
    public class ConversionsController : Controller
    {
        // Now using PRG strategy
        //https://stackoverflow.com/questions/33923958/mvc-validation-model-state-using-viewmodel
        //https://stackoverflow.com/questions/8599639/how-can-i-check-if-my-model-is-valid-from-inside-the-razor-view

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(Values values)
        {
            if (ModelState.IsValid && values.MilesModel != null) //1
            {
                return RedirectToAction("MilesToKilometers", values.MilesModel);
            }
            else if (ModelState.IsValid && values.KilometersModel != null) //2
            {
                return RedirectToAction("KilometersToMiles", values.KilometersModel);
            }
            else if (ModelState.IsValid && values.CelsiusModel != null) //3
            {
                return RedirectToAction("CelsiusToFahrenheit", values.CelsiusModel);
            }
            else if (ModelState.IsValid && values.FahrenheitModel != null) //4
            {
                return RedirectToAction("FahrenheitToCelsius", values.FahrenheitModel);
            }
            else if (ModelState.IsValid && values.KilogramsModel != null) //5
            {
                return RedirectToAction("KilogramsToPounds", values.KilogramsModel);
            }
            else if (ModelState.IsValid && values.PoundsModel != null) //6
            {
                return RedirectToAction("PoundsToKilograms", values.PoundsModel);
            }
            else if (ModelState.IsValid && values.TonsOfTNTModel != null) //7
            {
                return RedirectToAction("TonsOfTNTToAveragePhoneCapacity", values.TonsOfTNTModel);
            }
            else if (ModelState.IsValid && values.AveragePhoneCapacityModel != null) //8
            {
                return RedirectToAction("AveragePhoneCapacityToTonsOfTNT", values.AveragePhoneCapacityModel);
            }
            else
            {
                return View(values);
            }
        }

        public ActionResult Legacy()
        {
            return View();
        }

        // GET: ConversionsController1cs/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        public IActionResult MilesToKilometers(int id)
        {
            double km = new UnitOf.Length().FromMiles(id).ToKilometers();
            ViewData["Miles"] = id;
            ViewData["Kilometers"] = km;
            return View();
        }

        [HttpGet]
        public IActionResult MilesToKilometers(MilesModel values)
        {
            if (ModelState.IsValid)
            {
                double mi = Convert.ToDouble(values.Miles);
                double km = new UnitOf.Length().FromMiles(mi).ToKilometers();
                values.OutputValue = km.ToString();
                return View(values);
            }
            else
            {
                return RedirectToAction("Index", values);
            }
        }

        public IActionResult KilometersToMiles(int id)
        {
            double mi = new UnitOf.Length().FromKilometers(id).ToMiles();
            ViewData["Miles"] = mi;
            ViewData["Kilometers"] = id;
            return View();
        }

        [HttpGet]
        public IActionResult KilometersToMiles(KilometersModel values)
        {
            if (ModelState.IsValid)
            {
                double km = Convert.ToDouble(values.Kilometers);
                double mi = new UnitOf.Length().FromKilometers(km).ToMiles();
                values.OutputValue = mi.ToString();
                return View(values);
            }
            else
            {
                return RedirectToAction("Index", values);
            }
        }

        public IActionResult CelsiusToFahrenheit(int id)
        {
            double x = new UnitOf.Temperature().FromCelsius(id).ToFahrenheit();
            ViewData["Fahrenheit"] = x;
            ViewData["Celsius"] = id;
            return View();
        }

        [HttpGet]
        public IActionResult CelsiusToFahrenheit(CelsiusModel values)
        {
            if (ModelState.IsValid)
            {
                double celsius = Convert.ToDouble(values.Celsius);
                double fahrenheit = new UnitOf.Temperature().FromCelsius(celsius).ToFahrenheit();
                values.OutputValue = fahrenheit.ToString();
                return View(values);
            }
            else
            {
                return RedirectToAction("Index", values);
            }
        }
        public IActionResult FahrenheitToCelsius(int id)
        {
            double x = new UnitOf.Temperature().FromFahrenheit(id).ToCelsius();
            ViewData["Fahrenheit"] = id;
            ViewData["Celsius"] = x;
            return View();
        }
        
        [HttpGet]
        public IActionResult FahrenheitToCelsius(FahrenheitModel values)
        {
            if (ModelState.IsValid)
            {
                double fahrenheit = Convert.ToDouble(values.Fahrenheit);
                double celsius = new UnitOf.Temperature().FromFahrenheit(fahrenheit).ToCelsius();
                values.OutputValue = celsius.ToString();
                return View(values);
            }
            else
            {
                return RedirectToAction("Index", values);
            }
        }

        public IActionResult TonsOfTNTToAveragePhoneCapacity(int id)
        {
            double x = new UnitOf.Energy().FromTonsOfTNT(id).ToWattHours();
            double phoneCapacity = x / 20.0; //Assumes 4 amp-hours at 5 V
            ViewData["AveragePhoneCharge"] = phoneCapacity;
            ViewData["TonsOfTNT"] = id;
            return View();
        }

        [HttpGet]
        public IActionResult TonsOfTNTToAveragePhoneCapacity(TonsOfTNTModel values)
        {
            if (ModelState.IsValid)
            {
                double tonsOfTNT = Convert.ToDouble(values.TonsOfTNT);
                double averagePhoneCapacity = new UnitOf.Energy().FromTonsOfTNT(tonsOfTNT).ToWattHours();
                values.OutputValue = averagePhoneCapacity.ToString();
                return View(values);
            }
            else
            {
                return RedirectToAction("Index", values);
            }
        }

        public IActionResult AveragePhoneCapacityToTonsOfTNT(int id)
        {
            double phoneCapacity = id * 20.0; //Assumes 4 amp-hours at 5 V
            double x = new UnitOf.Energy().FromWattHours(phoneCapacity).ToTonsOfTNT();
            ViewData["AveragePhoneCharge"] = id;
            ViewData["TonsOfTNT"] = x;
            return View();
        }

        [HttpGet]
        public IActionResult AveragePhoneCapacityToTonsOfTNT(AveragePhoneCapacityModel values)
        {
            if (ModelState.IsValid)
            {
                double phoneCapacity = Convert.ToDouble(values.AveragePhoneCapacity) * 20;
                double tonsOfTNT = new UnitOf.Energy().FromWattHours(phoneCapacity).ToTonsOfTNT();
                values.OutputValue = tonsOfTNT.ToString();
                return View(values);
            }
            else
            {
                return RedirectToAction("Index", values);
            }
        }

        public IActionResult PoundsToKilograms(int id)
        {
            double x = new UnitOf.Mass().FromPounds(id).ToKilograms();
            ViewData["Pounds"] = id;
            ViewData["Kilograms"] = x;
            return View();
        }

        [HttpGet]
        public IActionResult PoundsToKilograms(PoundsModel values)
        {
            if (ModelState.IsValid)
            {
                double pounds = Convert.ToDouble(values.Pounds);
                double kilograms = new UnitOf.Length().FromMiles(pounds).ToKilometers();
                values.OutputValue = kilograms.ToString();
                return View(values);
            }
            else
            {
                return RedirectToAction("Index", values);
            }
        }

        public IActionResult KilogramsToPounds(int id)
        {
            double x = new UnitOf.Mass().FromKilograms(id).ToPounds();
            ViewData["Kilograms"] = id;
            ViewData["Pounds"] = x;
            return View();
        }

        [HttpGet]
        public IActionResult KilogramsToPound(KilogramsModel values)
        {
            if (ModelState.IsValid)
            {
                double kilograms = Convert.ToDouble(values.Kilograms);
                double pounds = new UnitOf.Mass().FromKilograms(kilograms).ToPounds();
                values.OutputValue = pounds.ToString();
                return View(values);
            }
            else
            {
                return RedirectToAction("Index", values);
            }
        }
    }
}