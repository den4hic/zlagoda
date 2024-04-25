using SupermarketDAL.DB;
using SupermarketDAL.Entities;
using SupermarketPL.Model;

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
}