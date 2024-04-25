using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SupermarketDAL.Entities
{
    public class CustomerCard
    {
        public string CardNumber { get; set; }
        public string CustSurname { get; set; }
        public string CustName { get; set; }
        public string CustPatronymic { get; set; }
        public string PhoneNumber { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string Index { get; set; }
        public int Percentage { get; set; }
    }

}
