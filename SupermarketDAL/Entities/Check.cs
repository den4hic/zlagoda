using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SupermarketDAL.Entities
{
    public class Check
    {
        public string CheckNumber { get; set; }
        public string IdEmployee { get; set; }
        public string CardNumber { get; set; }
        public DateTime PrintDate { get; set; }
        public decimal SumTotal { get; set; }
        public decimal Vat { get; set; }
    }

}
