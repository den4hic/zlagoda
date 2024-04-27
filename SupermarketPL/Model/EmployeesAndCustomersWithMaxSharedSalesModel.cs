using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SupermarketPL.Model
{
	public class EmployeesAndCustomersWithMaxSharedSalesModel
	{
        public string EmployeeId { get; set; }
        public string EmployeeSurname { get; set; }
        public string EmployeeName { get; set; }
        public string CustomerId { get; set; }
        public string CustomerSurname { get; set; }
        public string CustomerName { get; set; }
        public int TotalSharedSales { get; set; }
    }
}
