using SupermarketDAL.DB;
using SupermarketDAL.Entities;
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

	public List<CustomerModel> GetCustomers()
	{
		var customers = dbHelper.GetCostumerCardsList();

		List<CustomerModel> result = new List<CustomerModel>();

		foreach (var item in customers)
		{
			result.Add(new CustomerModel()
			{
				CardNumber = item.CardNumber,
				FullName = item.CustName + item.CustSurname,
				Address = item.Street,
				PhoneNumber = item.PhoneNumber
			});
		}

		return result;
	}

	public List<CategoryModel> GetCategories()
	{
		var categories = dbHelper.GetCategoriesList();

		List<CategoryModel> result = new List<CategoryModel>();

		foreach (var item in categories)
		{
			result.Add(new CategoryModel()
			{
				Name = item.CategoryName
			});
		}

		return result;
	}

	public List<Goods> GetGoodsByCategory(string selectedCategory)
	{
		var goods = dbHelper.GetProductsListByCategory(selectedCategory);

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