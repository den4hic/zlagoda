using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SupermarketDAL.Entities
{
    public class Product
    {
        public int IdProduct { get; set; }
        public int CategoryNumber { get; set; }
        public string ProductName { get; set; }
        public string Producer { get; set; }
        public string Characteristics { get; set; }
    }
}
