using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;

namespace DevonDalrympleUnitConverter.MVC.Models.ConversionModels
{
    public class SelectListModel
    {
        public String SelectedValue { get; set; }

        public IEnumerable<SelectListItem> Items { get; set; } = new List<SelectListItem>
        {
            new SelectListItem
            {
                Value = "1",
                Text = "Tons of TNT to Average Phone Capacity"
            },

            new SelectListItem
            {
                Value = "2",
                Text = "Average Phone Capacity to Tons of TNT"
            },

            new SelectListItem
            {
                Value = "3",
                Text = "Pounds to Kilograms"
            },

            new SelectListItem
            {
                Value = "4",
                Text = "Kilograms to Pounds"
            }
        };

        public SelectListModel()
        {

            //Items = new List<SelectItemModel>();
            //Items.Add(new SelectItemModel("Tons of TNT to Average Phone Capacity", 1));
            //Items.Add(new SelectItemModel("Average Phone Capacity to Tons of TNT", 2));
            //Items.Add(new SelectItemModel("Pounds to Kilograms", 3));
            //Items.Add(new SelectItemModel("Kilograms to Pounds", 4));

        }
        
    }
}
