using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DevonDalrympleUnitConverter.MVC.Models
{
    public class TonsOfTNTModel : ConversionModels.OutputModel
    {
        [Required]
        [Display(Name = "Tons Of TNT")]
        [Range(int.MinValue, int.MaxValue, ErrorMessage = "Please enter valid integer Number")]
        [RegularExpression(@".*\S+.*$", ErrorMessage = "Field Cannot be Blank Or Whitespace")]
        public String TonsOfTNT { get; set; }

        public TonsOfTNTModel()
        {

        }
    }
}
