using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SupermarketPL.Model
{
    public class ReportGoodsModel
    {
        public string ReceiptNumber { get; set; }
        public DateTime Date { get; set; }
        public decimal TotalCost { get; set; }
        public decimal VAT { get; set; }
    }
}
