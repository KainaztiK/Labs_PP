﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.DataTransferObjects
{
    public class ProductDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
        public string Product_desciption { get; set; }
    }
}
