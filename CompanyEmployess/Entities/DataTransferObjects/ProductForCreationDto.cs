using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.DataTransferObjects
{
    public class ProductForCreationDto
    {
        public string Name { get; set; }
        public int Price { get; set; }
        public string Product_desciption { get; set; }
        public IEnumerable<ProductForCreationDto> Product { get; set; }
    }
}
