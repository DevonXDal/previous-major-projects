using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DevonDalrympleUnitConverter.MVC.Models
{
    public class AveragePhoneCapacityModel : ConversionModels.OutputModel
    {
        [Required]
        [Display(Name ="Average Phone Capacity")]
        [Range(int.MinValue, int.MaxValue, ErrorMessage = "Please enter valid integer Number")]
        [RegularExpression(@".*\S+.*$", ErrorMessage = "Field Cannot be Blank Or Whitespace")]
        public String AveragePhoneCapacity { get; set; }

        public AveragePhoneCapacityModel()
        {

        }
    }
}
