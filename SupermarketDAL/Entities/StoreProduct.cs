using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SupermarketDAL.Entities
{
    public class StoreProduct
    {
        public string UPC { get; set; }
        public string UPC_prom { get; set; }
        public int IdProduct { get; set; }
        public decimal SellingPrice { get; set; }
        public int ProductsNumber { get; set; }
        public bool PromotionalProduct { get; set; }
    }

}
