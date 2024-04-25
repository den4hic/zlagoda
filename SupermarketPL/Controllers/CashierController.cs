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
				CategoryId = item.CategoryNumber,
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
				FirstName = item.CustName,
				LastName = item.CustSurname,
				PatronymicName = item.CustPatronymic,
				City = item.City,
				Street = item.Street,
				Index = item.Index,
				PhoneNumber = item.PhoneNumber,
				Discount = item.Percentage
				
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

	public void UpdateCustomer(CustomerModel customer)
	{
		dbHelper.UpdateCostumerCard(customer.CardNumber, customer.LastName, customer.FirstName, customer.PatronymicName, customer.PhoneNumber, customer.City, customer.Street, customer.Index, customer.Discount);
	}
}