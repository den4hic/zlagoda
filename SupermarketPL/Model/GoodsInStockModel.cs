using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SupermarketPL.Model
{
    public class GoodsInStockModel
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public string Manufacturer { get; set; }
        public string Characteristics { get; set; }
        public string UPC { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public bool Discount { get; set; }
    }
}
