using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevonDalrympleUnitConverter.MVC.Models.ConversionModels
{
    public class SelectItemModel
    {
        public String Text { get; set; }

        public int Value { get; set; }

        public SelectItemModel(String text, int value)
        {
            Text = text;
            Value = value;
        }
    }
}
