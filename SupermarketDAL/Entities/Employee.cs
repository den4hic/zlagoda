using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SupermarketDAL.Entities
{
    public class Employee
    {
        public string IdEmployee { get; set; }
        public string EmplSurname { get; set; }
        public string EmplName { get; set; }
        public string EmplPatronymic { get; set; }
        public string EmplRole { get; set; }
        public decimal Salary { get; set; }
        public DateTime DateOfBirth { get; set; }
        public DateTime DateOfStart { get; set; }
        public string PhoneNumber { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string ZipCode { get; set; }
    }
}
