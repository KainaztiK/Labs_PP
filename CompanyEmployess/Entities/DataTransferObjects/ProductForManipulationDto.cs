using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Entities.DataTransferObjects
{
    public abstract class ProductForManipulationDto
    {
        [Required(ErrorMessage = "Product name is a required field.")]
        [MaxLength(30, ErrorMessage = "Maximum length for the Name is 30 characters.")]
        public string Name { get; set; }
        [Range(0, int.MaxValue, ErrorMessage = "Price is required and it can't be lower than 0")]
        public int Price { get; set; }
        [Required(ErrorMessage = "Product_desciption is a required field.")]
        [MaxLength(200, ErrorMessage = "Maximum length for the Product_desciption is 200 characters.")]
        public string Product_desciption { get; set; } 
    }
}
