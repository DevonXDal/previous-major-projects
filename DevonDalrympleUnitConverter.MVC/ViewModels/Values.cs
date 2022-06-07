using DevonDalrympleUnitConverter.MVC.Models;
using DevonDalrympleUnitConverter.MVC.Models.ConversionModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;

namespace DevonDalrympleUnitConverter.MVC.ViewModels
{
    public class Values
    {
        //https://stackoverflow.com/questions/51552487/form-validation-of-empty-strings
       
       public SelectListModel DropDownModel { get; set; }

        public MilesModel MilesModel { get; set; }

        public KilometersModel KilometersModel { get; set; }

        public CelsiusModel CelsiusModel { get; set; }

        public FahrenheitModel FahrenheitModel { get; set; }

        
        public TonsOfTNTModel TonsOfTNTModel { get; set; }

        public AveragePhoneCapacityModel AveragePhoneCapacityModel { get; set; }

       
        public KilogramsModel KilogramsModel { get; set; }

        public PoundsModel PoundsModel { get; set; }

        public Values()
        {
            //DropDownModel = new SelectListModel();
        }
    }
}
