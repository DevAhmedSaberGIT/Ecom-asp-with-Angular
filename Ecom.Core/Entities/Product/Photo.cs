﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.Core.Entities.Product
{
    public class Photo: BaseEntity<int>
    {
        public string ImageName { get; set; }
        public int ProductId { get; set; }
  
    }
}
