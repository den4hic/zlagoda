using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SupermarketPL.Model
{
	public class CustomerModel
	{
		public string CardNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PatronymicName { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string Index { get; set; }
        public string PhoneNumber { get; set; }
        public int Discount { get; set; }
    }
}
