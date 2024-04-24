using SupermarketDAL.DB;
using SupermarketDAL.Entities;
using SupermarketPL.Model;
using SupermarketPL.Model;

public class CashierController
{
    private DatabaseHelper dbHelper;

    public CashierController()
    {
		dbHelper = new DatabaseHelper("../../../../SupermarketDAL/zlagoda.db");
	}

    public List<Goods> GetGoods()
    {
		var goods = dbHelper.GetProductsList();

		List<Goods> result = new List<Goods>();

		foreach (var item in goods)
		{
			result.Add(new Goods()
			{
				ProductId = item.IdProduct,
				Name = item.ProductName,
				Manufacturer = item.Producer,
				Characteristics = item.Characteristics
			});
		}

		return result;
	}



}