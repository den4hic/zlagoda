using SupermarketDAL.DB;
using SupermarketDAL.Entities;
using SupermarketPL.Model;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using System.Collections.Generic;

public class ManagerController
{
    private DatabaseHelper dbHelper;

    public ManagerController()
    {
		dbHelper = new DatabaseHelper("../../../../SupermarketDAL/zlagoda.db");
	}

	public List<EmployeeModel> GetEmployees()
	{
        var employees = dbHelper.GetEmployeesList();

		List<EmployeeModel> result = new List<EmployeeModel>();

		foreach (var item in employees)
		{
			result.Add(new EmployeeModel()
			{
				EmployeeId = item.IdEmployee,
				LastName = item.EmplSurname,
				FirstName = item.EmplName,
				PatronymicName = item.EmplPatronymic,
				Position = item.EmplRole,
				Salary = item.Salary,
				BirthDate = item.DateOfBirth,
				WorkStartDate = item.DateOfStart,
				PhoneNumber = item.PhoneNumber,
				City = item.City,
				Street = item.Street,
				Index = item.ZipCode
			});
		}
		

		return result;
	}

	public void UpdateEmployee(EmployeeModel employee)
	{
		dbHelper.UpdateEmployee(new Employee(){
			EmplName = employee.FirstName,
			EmplSurname = employee.LastName,
			EmplPatronymic = employee.PatronymicName,
			EmplRole = employee.Position,
			Salary = employee.Salary,
			DateOfBirth = employee.BirthDate,
			DateOfStart = employee.WorkStartDate,
			PhoneNumber = employee.PhoneNumber,
			City = employee.City,
			Street = employee.Street,
			ZipCode = employee.Index
		});
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


	public List<Category> GetCategories()
	{
		return dbHelper.GetCategoriesList();
	}

	public void UpdateGoods(Goods goods)
	{
		dbHelper.UpdateProduct(goods.ProductId, goods.CategoryId, goods.Name, goods.Manufacturer, goods.Characteristics);
	}

	public void UpdateCategory(Category category)
	{
		dbHelper.UpdateCategory(category.CategoryNumber, category.CategoryName);
	}

	public void DeleteGoods(Goods selectedGoods)
	{
		dbHelper.DeleteProduct(selectedGoods.ProductId);
	}

	public void DeleteCategory(Category selectedGoods)
	{
		dbHelper.DeleteCategory(selectedGoods.CategoryNumber);
	}

	public void UpdateCustomer(CustomerModel customer)
	{
		dbHelper.UpdateCustomerCard(customer.CardNumber, customer.LastName, customer.FirstName, customer.PatronymicName, customer.PhoneNumber, customer.City, customer.Street, customer.Index, customer.Discount);
	}

	public void DeleteCustomer(CustomerModel selectedCustomer)
	{
		dbHelper.DeleteCustomerCard(selectedCustomer.CardNumber);
	}
	public void CreateGoodsPdf(List<Goods> goodsList, string outputPath)
	{
		Document document = new Document();
		PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(outputPath, FileMode.Create));
		document.Open();

		PdfPTable table = new PdfPTable(5); 

		foreach (var goods in goodsList)
		{
			table.AddCell(goods.ProductId.ToString());
			table.AddCell(goods.CategoryId.ToString());
			table.AddCell(goods.Name);
			table.AddCell(goods.Manufacturer);
			table.AddCell(goods.Characteristics);
		}

		document.Add(table);
		document.Close();
	}
	public void CreateCategoriesPdf(List<Category> categoriesList, string outputPath)
{
    Document document = new Document();
    PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(outputPath, FileMode.Create));
    document.Open();

    PdfPTable table = new PdfPTable(2);

    foreach (var category in categoriesList)
    {
	    table.AddCell(category.CategoryNumber.ToString());
        table.AddCell(category.CategoryName);
    }

    document.Add(table);
    document.Close();
}

public void CreateCustomersPdf(List<CustomerModel> customersList, string outputPath)
{
    Document document = new Document();
    PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(outputPath, FileMode.Create));
    document.Open();

    PdfPTable table = new PdfPTable(9); 

    foreach (var customer in customersList)
    {
        table.AddCell(customer.CardNumber);
        table.AddCell(customer.FirstName);
        table.AddCell(customer.LastName);
        table.AddCell(customer.PatronymicName);
        table.AddCell(customer.PhoneNumber);
        table.AddCell(customer.City);
        table.AddCell(customer.Street);
        table.AddCell(customer.Index);
        table.AddCell(customer.Discount.ToString());
    }

    document.Add(table);
    document.Close();
}

public void CreateEmployeesPdf(List<EmployeeModel> employeesList, string outputPath)
{
    Document document = new Document();
    PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(outputPath, FileMode.Create));
    document.Open();

    PdfPTable table = new PdfPTable(12); 

    foreach (var employee in employeesList)
    {
	    table.AddCell(employee.EmployeeId.ToString());
        table.AddCell(employee.FirstName);
        table.AddCell(employee.LastName);
        table.AddCell(employee.PatronymicName);
        table.AddCell(employee.Position);
        table.AddCell(employee.Salary.ToString());
        table.AddCell(employee.BirthDate.ToString());
        table.AddCell(employee.BirthDate.ToString());
        table.AddCell(employee.PhoneNumber);
        table.AddCell(employee.City);
        table.AddCell(employee.Street);
        table.AddCell(employee.Index);
    }

    document.Add(table);
    document.Close();
}
public void CreateGoodsInStockPdf(List<GoodsInStockModel> goodsInStockList, string outputPath)
{
	Document document = new Document();
	PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(outputPath, FileMode.Create));
	document.Open();

	PdfPTable table = new PdfPTable(7); 

	foreach (var goodsInStock in goodsInStockList)
	{
		table.AddCell(goodsInStock.ProductId.ToString());
		table.AddCell(goodsInStock.Name);
		table.AddCell(goodsInStock.Manufacturer);
		table.AddCell(goodsInStock.Characteristics);
		table.AddCell(goodsInStock.UPC.ToString());
		table.AddCell(goodsInStock.Price.ToString());
		table.AddCell(goodsInStock.Quantity.ToString());
		//table.AddCell(goodsInStock.Sold.toString()); якщо добавимо треба поміняти колонки
	}

	document.Add(table);
	document.Close();
}
/*public void CreateReceiptsPdf(List<ReceiptModel> receiptsList, string outputPath)
{
	Document document = new Document();
	PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(outputPath, FileMode.Create));
	document.Open();

	PdfPTable table = new PdfPTable(4); // 4 columns for ReceiptId, CustomerId, Date, Total

	foreach (var receipt in receiptsList)
	{
		table.AddCell(receipt.ReceiptId.ToString());
		table.AddCell(receipt.Date.ToString());
		table.AddCell(receipt.Total.ToString());
	}

	document.Add(table);
	document.Close();
}*/
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