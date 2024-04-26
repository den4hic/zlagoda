using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SupermarketDAL.Entities
{
    public class UserAccount
    {
        public string Username { get; set; }
        public string HashedPassword { get; set; }
        public int IdEmployee { get; set; }
    }
}
