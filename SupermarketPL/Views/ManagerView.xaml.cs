using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using SupermarketDAL.Entities;
using SupermarketPL.Model;
using Xceed.Wpf.Toolkit.PropertyGrid;

namespace SupermarketPL.Views
{
    public partial class ManagerView : Window
    {
		private List<string> employeeRoleList = new List<string> { "Manager", "Cashier" };
		private Employee _employee;
        private ManagerController controller;
		private ObservableCollection<Goods> _goodsList = new ObservableCollection<Goods>();
		private ObservableCollection<Category> _categoriesList = new ObservableCollection<Category>();
		private ObservableCollection<string> _categoriesNameList = new ObservableCollection<string>();
		private ObservableCollection<CustomerModel> _customersList = new ObservableCollection<CustomerModel>();
		private ObservableCollection<GoodsInStockModel> _goodsInStockList = new ObservableCollection<GoodsInStockModel>();

		public ManagerView()
		{
			Initialize(null);
		}

		public ManagerView(Employee employee)
        {
			Initialize(employee);
		}

		private void Initialize(Employee employee)
		{
			InitializeComponent();
			this._employee = employee;
			this.controller = new ManagerController();

			_categoriesList.CollectionChanged += (sender, e) =>
			{
				if (e.Action == NotifyCollectionChangedAction.Add)
				{
					foreach (Category category in e.NewItems)
					{
						_categoriesNameList.Add(category.CategoryName);
					}
				}
				else if (e.Action == NotifyCollectionChangedAction.Remove)
				{
					foreach (Category category in e.OldItems)
					{
						_categoriesNameList.Remove(category.CategoryName);
					}
				}
			};

			List<EmployeeModel> employeesList = controller.GetEmployees();

			employeeDataGrid.ItemsSource = employeesList;
			employeeDataGrid.CellEditEnding += EmployeeDataGrid_CellEditEnding;


			List<Goods> goodsList = controller.GetGoods();

			foreach (var item in goodsList)
			{
				_goodsList.Add(item);
			}


			goodsDataGrid.ItemsSource = _goodsList;
			goodsDataGrid.CellEditEnding += GoodsDataGrid_CellEditEnding;

			List<Category> categoriesList = controller.GetCategories();

			foreach (var item in categoriesList)
			{
				_categoriesList.Add(item);
			}

			categoriesDataGrid.ItemsSource = _categoriesList;
			categoriesDataGrid.CellEditEnding += CategoriesDataGrid_CellEditEnding;
			List<GoodsInStockModel> goodsInStockList = controller.GetStocks();

			foreach (var item in goodsInStockList)
			{
				_goodsInStockList.Add(item);
			}

			goodsInStockDataGrid.ItemsSource = _goodsInStockList;




			List<CustomerModel> customersList = controller.GetCustomers();

			_categoriesNameList.Insert(0, "All");

			foreach (var item in customersList)
			{
				_customersList.Add(item);
			}


			customerDataGrid.ItemsSource = _customersList;
			customerDataGrid.CellEditEnding += CustomerDataGrid_CellEditEnding;

			categoriesComboBox.ItemsSource = _categoriesNameList;

			positionComboBox.ItemsSource = employeeRoleList;
			positionComboBox.SelectionChanged += PositionComboBox_SelectionChanged;
		}

		private void PositionComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			throw new NotImplementedException();
		}

		private void PositionEmployeeGridComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			var selectedEmployee = employeeDataGrid.SelectedItem as EmployeeModel;

			if(_employee.IdEmployee == selectedEmployee.EmployeeId)
			{
				MessageBox.Show("You can't change your properties");
				return;
			}

			selectedEmployee.Position = employeeRoleList[(sender as ComboBox).SelectedIndex];

			controller.UpdateEmployee(selectedEmployee);
		}

		private void CustomerDataGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
		{
			if (e.EditAction == DataGridEditAction.Commit)
			{
				DataGrid grid = sender as DataGrid;

				if (grid != null && e.Row != null && e.Column != null)
				{
					var editedElement = e.EditingElement as TextBox;

					if (editedElement != null)
					{
						string updatedValue = editedElement.Text;
						CustomerModel customer = e.Row.Item as CustomerModel;

						if (customer != null)
						{
							string columnHeader = e.Column.Header.ToString();

							switch (columnHeader)
							{
								case "Card Number":
									customer.CardNumber = updatedValue;
									break;
								case "First Name":
									customer.FirstName = updatedValue;
									break;
								case "Last Name":
									customer.LastName = updatedValue;
									break;
								case "Patronymic Name":
									customer.PatronymicName = updatedValue;
									break;
								case "Phone Number":
									customer.PhoneNumber = updatedValue;
									break;
								case "City":
									customer.City = updatedValue;
									break;
								case "Street":
									customer.Street = updatedValue;
									break;
								case "Index":
									customer.Index = updatedValue;
									break;
								case "Discount":
									if (int.TryParse(updatedValue, out int discount))
									{
										customer.Discount = discount;
									}
									break;
								default:
									break;
							}

							controller.UpdateCustomer(customer);
						}
					}
				}
			}
		}

		private void CategoriesDataGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
		{
			if (e.EditAction == DataGridEditAction.Commit)
			{
				DataGrid grid = sender as DataGrid;

				if (grid != null && e.Row != null && e.Column != null)
				{
					var editedElement = e.EditingElement as TextBox;

					if (editedElement != null)
					{
						string updatedValue = editedElement.Text;
						Category category = e.Row.Item as Category;

						if (category != null)
						{
							category.CategoryName = updatedValue;

							controller.UpdateCategory(category);
						}
					}
				}
			}
		}

		private void GoodsDataGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
		{
			if (e.EditAction == DataGridEditAction.Commit)
			{
				DataGrid grid = sender as DataGrid;

				if (grid != null && e.Row != null && e.Column != null)
				{
					var editedElement = e.EditingElement as TextBox;

					if (editedElement != null)
					{
						string updatedValue = editedElement.Text;
						Goods goods = e.Row.Item as Goods;

						if (goods != null)
						{
							string columnHeader = e.Column.Header.ToString();

							switch (columnHeader)
							{
								case "CategoryId":
									if (int.TryParse(updatedValue, out int categoryId))
									{
										goods.CategoryId = categoryId;
									}
									break;
								case "Name":
									goods.Name = updatedValue;
									break;
								case "Manufacturer":
									goods.Manufacturer = updatedValue;
									break;
								case "Characteristics":
									goods.Characteristics = updatedValue;
									break;
								default:
									break;
							}

							controller.UpdateGoods(goods);
						}
					}
				}
			}
		}

		private void EmployeeDataGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
		{
			if (e.EditAction == DataGridEditAction.Commit)
			{
				DataGrid grid = sender as DataGrid;

				if (grid != null && e.Row != null && e.Column != null)
				{
					var editedElement = e.EditingElement as TextBox;

					if (editedElement != null)
					{
						string updatedValue = editedElement.Text;
						EmployeeModel employee = e.Row.Item as EmployeeModel;

						if (_employee.IdEmployee == employee.EmployeeId)
						{
							MessageBox.Show("You can't change your properties");
							return;
						}

						if (employee != null)
						{
							string columnHeader = e.Column.Header.ToString();

							switch (columnHeader)
							{
								case "First Name":
									employee.FirstName = updatedValue;
									break;
								case "Last Name":
									employee.LastName = updatedValue;
									break;
								case "Patronymic Name":
									employee.PatronymicName = updatedValue;
									break;
								case "Position":
									employee.Position = updatedValue;
									break;
								case "Salary":
									if (decimal.TryParse(updatedValue, out decimal salary))
									{
										employee.Salary = salary;
									}
									break;
								case "Phone Number":
									employee.PhoneNumber = updatedValue;
									break;
								case "City":
									employee.City = updatedValue;
									break;
								case "Street":
									employee.Street = updatedValue;
									break;
								case "Index":
									employee.Index = updatedValue;
									break;
								default:
									break;
							}

							controller.UpdateEmployee(employee);
						}
					}
				}
			}
		}

		private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e) 
        {
            var searchText = searchTextBox.Text.ToLower();
            //employeeInfoComboBox.ItemsSource = employees
            //    .Where(employee => employee.FullName.ToLower().Contains(searchText))
            //    .ToList();
        }

		private void DeleteButton_Click(object sender, RoutedEventArgs e)
		{
			// Отримати виділений рядок (selected item) з goodsDataGrid
			var selectedGoods = goodsDataGrid.SelectedItem as Goods;

			if (selectedGoods != null)
			{
				if (_goodsList.Contains(selectedGoods))
				{
					_goodsList.Remove(selectedGoods);
				}

				controller.DeleteGoods(selectedGoods);
			}
		}

		private void DeleteCategoryButton_Click(object sender, RoutedEventArgs e)
		{
			var selectedCategory = categoriesDataGrid.SelectedItem as Category;

			if (selectedCategory != null)
			{
				if (_categoriesList.Contains(selectedCategory))
				{
					_categoriesList.Remove(selectedCategory);
				}

				//controller.DeleteCategory(selectedGoods);
			}
		}

		private void DeleteCustomerButton_Click(object sender, RoutedEventArgs e)
		{
			var selectedCustomer = customerDataGrid.SelectedItem as CustomerModel;

			if (selectedCustomer != null)
			{
				if (_customersList.Contains(selectedCustomer))
				{
					_customersList.Remove(selectedCustomer);
				}

				controller.DeleteCustomer(selectedCustomer);
			}
		}
		private void AddCategoryButton_Click(object sender, RoutedEventArgs e)
		{
			ManagerAddCategoryView addCategoryWindow = new ManagerAddCategoryView();
			addCategoryWindow.Show();
		}
		private void EditCategoryButton_Click(object sender, RoutedEventArgs e)
		{
			ManagerEditCategoryView editCategoryWindow = new ManagerEditCategoryView();
			editCategoryWindow.Show();
		}
		private void AddGoodButton_Click(object sender, RoutedEventArgs e)
		{
			SupermarketPL.Views.ManagerAddGoodView addGoodWindow = new SupermarketPL.Views.ManagerAddGoodView();
			addGoodWindow.Show();
		}

		private void EditGoodButton_Click(object sender, RoutedEventArgs e)
		{
			SupermarketPL.Views.ManagerEditGoodView editGoodWindow = new SupermarketPL.Views.ManagerEditGoodView();
			editGoodWindow.Show();
		}

		private void AddGoodInStock_Click(object sender, RoutedEventArgs e)
		{
			SupermarketPL.Views.ManagerAddGoodInStockView addGoodInStockWindow = new SupermarketPL.Views.ManagerAddGoodInStockView();
			addGoodInStockWindow.Show();
		}

		private void EditGoodInStock_Click(object sender, RoutedEventArgs e)
		{
			SupermarketPL.Views.ManagerEditGoodInStockView editGoodInStockWindow = new SupermarketPL.Views.ManagerEditGoodInStockView();
			editGoodInStockWindow.Show();
		}
		private void UPCSearchButton_Click(object sender, RoutedEventArgs e)
		{
			UPCSearchView upcSearchView = new UPCSearchView();
			//Тут треба дістати товар за допомогою UPC з upcSearchTextBox і передати його в upcSearchView

			upcSearchView.Show();
		}
		private void ProfileButton_Click(object sender, RoutedEventArgs e)
		{
			EmployeeModel employee = employeeDataGrid.SelectedItem as EmployeeModel;

			if (employee == null)
			{
				MessageBox.Show("Please select an employee to view profile");
				return;
			}

			Employee employee1 = new Employee
			{
				IdEmployee = employee.EmployeeId,
				EmplName = employee.FirstName,
				EmplSurname = employee.LastName,
				EmplPatronymic = employee.PatronymicName,
				EmplRole = employee.Position,
				Salary = employee.Salary,
				PhoneNumber = employee.PhoneNumber,
				City = employee.City,
				Street = employee.Street,
				ZipCode = employee.Index
			};
			EmployeeProfileView profileWindow = new EmployeeProfileView(employee1);
			profileWindow.Show();
		}

		private void AddEmployee_Click(object sender, RoutedEventArgs e)
		{
			ManagerAddEmployeeView addEmployeeWindow = new ManagerAddEmployeeView();
			addEmployeeWindow.Show();
		}

		private void EditEmployee_Click(object sender, RoutedEventArgs e)
		{
			ManagerEditEmployeeView editEmployeeWindow = new ManagerEditEmployeeView();
			editEmployeeWindow.Show();
		}
		private void AddButton_Click(object sender, RoutedEventArgs e)
		{
			AddCustomerView addCustomerView = new AddCustomerView();
			addCustomerView.Show();
		}
		private void EditButton_Click(object sender, RoutedEventArgs e)
		{
			EditCustomerView editCustomerView = new EditCustomerView();
			editCustomerView.Show();
		}
		private void PrintGoodsButton_Click(object sender, RoutedEventArgs e)
		{
			string outputPath = "GoodsList.pdf";
			controller.CreateGoodsPdf(controller.GetGoods(), outputPath);
			MessageBox.Show("Goods report saved as " + outputPath);
		}
		private void PrintCategoriesButton_Click(object sender, RoutedEventArgs e)
		{
			string outputPath = "CategoriesList.pdf";
			controller.CreateCategoriesPdf(controller.GetCategories(), outputPath);
			MessageBox.Show("Categories report saved as " + outputPath);
		}

		private void PrintCustomersButton_Click(object sender, RoutedEventArgs e)
		{
			string outputPath = "CustomersList.pdf";
			controller.CreateCustomersPdf(controller.GetCustomers(), outputPath);
			MessageBox.Show("Customers report saved as " + outputPath);
		}

		private void PrintEmployeesButton_Click(object sender, RoutedEventArgs e)
		{
			string outputPath = "EmployeesList.pdf";
			controller.CreateEmployeesPdf(controller.GetEmployees(), outputPath);
			MessageBox.Show("Employees report saved as " + outputPath);
		}
		private void PrintGoodsInStockButton_Click(object sender, RoutedEventArgs e)
		{
			string outputPath = "GoodsInStockList.pdf";
			controller.CreateGoodsInStockPdf(controller.GetStocks(), outputPath);
			MessageBox.Show("Goods in Stock report saved as " + outputPath);
		}
		private void PrintReceiptsButton_Click(object sender, RoutedEventArgs e)
		{
			/*string outputPath = "ReceiptsList.pdf";
			controller.CreateReceiptsPdf(controller.GetReceipts(), outputPath);
			MessageBox.Show("Receipts report saved as " + outputPath);*/
		}
    }
}