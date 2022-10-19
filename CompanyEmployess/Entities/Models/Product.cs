using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models
{
    public class Product
    {
        [Column("ProductId")]
        public Guid Id { get; set; }
        [Required(ErrorMessage = "Employee name is a required field.")]
        [MaxLength(30, ErrorMessage = "Maximum length for the Name is 30 characters.")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Price is a required field.")]
        public string Price { get; set; }
        [Required(ErrorMessage = "Product_description is a required field.")]
        [MaxLength(100, ErrorMessage = "Maximum length for the Product_description is 100 characters.")]
        public string Product_desciption { get; set; }
    }
}
