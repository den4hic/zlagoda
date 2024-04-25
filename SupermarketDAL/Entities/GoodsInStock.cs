using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SupermarketDAL.Entities
{
    public class GoodsInStock
    {
        public int IdProduct { get; set; }
        public string ProductName { get; set; }
        public string Producer { get; set; }
        public string Characteristics { get; set; }
        public string UPC { get; set; }
        public decimal SellingPrice { get; set; }
        public int ProductsNumber { get; set; }
        public bool PromotionalProduct { get; set; }
    }
}
