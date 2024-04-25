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
		var customers = dbHelper.GetCustomerCardsList();

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

	public List<Category> GetCategories()
	{
		var categories = dbHelper.GetCategoriesList();

		List<Category> result = new List<Category>();

		foreach (var item in categories)
		{
			result.Add(new Category()
			{
				CategoryNumber = item.CategoryNumber,
				CategoryName = item.CategoryName
			});
		}

		return result;
	}

	public List<Goods> GetGoodsByCategory(int selectedCategory)
	{
		var goods = dbHelper.GetProductsListByCategoryID(selectedCategory);

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

	public void UpdateCustomer(CustomerModel customer)
	{
		dbHelper.UpdateCustomerCard(customer.CardNumber, customer.LastName, customer.FirstName, customer.PatronymicName, customer.PhoneNumber, customer.City, customer.Street, customer.Index, customer.Discount);
	}
}