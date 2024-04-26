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

	public List<GoodsInStockModel> GetStocks()
	{
		var goods = dbHelper.GetGoods();

		List<GoodsInStockModel> result = new List<GoodsInStockModel>();

		foreach (var item in goods)
		{
			result.Add(new GoodsInStockModel()
			{
				ProductId = item.IdProduct,
				Name = item.ProductName,
				Manufacturer = item.Producer,
				Characteristics = item.Characteristics,
				UPC = item.UPC,
				Price = item.SellingPrice,
				Quantity = item.ProductsNumber,
				Discount = item.PromotionalProduct
			});
		}

		return result;

	}

	public List<GoodsInStockModel> GetGoodsInStockByCategory(int categoryId)
	{
		var goodsInStock = dbHelper.GetGoodsListByCategoryID(categoryId);

		List<GoodsInStockModel> result = new List<GoodsInStockModel>();

		foreach (var item in goodsInStock)
		{
			result.Add(new GoodsInStockModel()
			{
				ProductId = item.IdProduct,
				Name = item.ProductName,
				Manufacturer = item.Producer,
				Characteristics = item.Characteristics,
				UPC = item.UPC,
				Price = item.SellingPrice,
				Quantity = item.ProductsNumber,
				Discount = item.PromotionalProduct
			});
		}

		return result;
	}

	public CustomerCard GetCustomerCardByNumber(string cardNumber)
	{
		return dbHelper.GetCustomerCardByNumber(cardNumber);
	}

	public void AddCheck(Check check)
	{
		dbHelper.InsertCheck(check);
	}

	public void AddSale(Sale sale)
	{
		dbHelper.InsertSale(sale);	
	}

	public StoreProduct GetGoodsByUPC(string upc)
	{
		return dbHelper.GetStoreProductByUPC(upc);
	}

	public void UpdateStoreProduct(StoreProduct storeProduct)
	{
		dbHelper.UpdateStoreProduct(storeProduct.UPC, storeProduct.UPC_prom, storeProduct.IdProduct, storeProduct.SellingPrice, storeProduct.ProductsNumber, storeProduct.PromotionalProduct);
	}

	public List<ReportGoodsModel> GetChecksByEmplId(string emplId)
	{
		var checks = dbHelper.GetChecksList();

		List<ReportGoodsModel> result = new List<ReportGoodsModel>();

		foreach (var item in checks)
		{
			if(item.IdEmployee != emplId)
			{
				continue;
			}
			result.Add(new ReportGoodsModel()
			{
				ReceiptNumber = item.CheckNumber,
				TotalCost = item.SumTotal,
				Date = item.PrintDate
			});
		}

		return result;
	}

	public List<Sale> GetSalesByCheckNumber(string receiptNumber)
	{
		return dbHelper.GetSalesListByCheckNumber(receiptNumber);
	}

	public Product GetProductByUPC(string uPC)
	{
		return dbHelper.GetProductByUPC(uPC);
	}

	public Check GetCheckById(string receiptNumber)
	{
		return dbHelper.GetCheckById(receiptNumber);
	}
	public void InsertCustomerCard(string cardNumber, string custSurname, string custName, string custPatronymic, string phoneNumber, string city, string street, string index, int percentage)
	{
		CustomerCard customerCard = new CustomerCard
		{
			CardNumber = cardNumber,
			CustSurname = custSurname,
			CustName = custName,
			CustPatronymic = custPatronymic,
			PhoneNumber = phoneNumber,
			City = city,
			Street = street,
			Index = index,
			Percentage = percentage
		};

		dbHelper.InsertCostumerCard(customerCard);
	}
}