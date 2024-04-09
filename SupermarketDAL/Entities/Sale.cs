using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SupermarketDAL.Entities
{
    public class Sale
    {
        public string UPC { get; set; }
        public string CheckNumber { get; set; }
        public int ProductNumber { get; set; }
        public decimal SellingPrice { get; set; }
    }

}
