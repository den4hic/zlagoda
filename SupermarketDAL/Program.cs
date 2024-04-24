using SupermarketDAL.DB;
using SupermarketDAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SupermarketDAL
{
	internal class Program
	{
		static void Main(string[] args)
		{
			DatabaseHelper dbHelper = new DatabaseHelper("../../../zlagoda.db");
			List<CostumerCard> list = dbHelper.GetCostumerCardsList();
			

		}
	}
}
