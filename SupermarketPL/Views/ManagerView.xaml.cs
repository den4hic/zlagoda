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
		private List<string> employeeRoleList = new List<string> { "All", "Manager", "Cashier" };
		private Employee _employee;
        private ManagerController controller;
		private List<ReportGoodsModel> reportGoodsModels;
		private List<EmployeeModel> employeeModels;
		private List<GoodsInStockModel> goodsInStockList;
		private ObservableCollection<Goods> _goodsList = new ObservableCollection<Goods>();
		private ObservableCollection<Category> _categoriesList = new ObservableCollection<Category>();
		private ObservableCollection<string> _categoriesNameList = new ObservableCollection<string>();
		private ObservableCollection<string> _employeesNameList = new ObservableCollection<string>();
		private ObservableCollection<CustomerModel> _customersList = new ObservableCollection<CustomerModel>();
		private ObservableCollection<GoodsInStockModel> _goodsInStockList = new ObservableCollection<GoodsInStockModel>();
		private ObservableCollection<EmployeeModel> _employeeModelList = new ObservableCollection<EmployeeModel>();
		private ObservableCollection<ReportGoodsModel> _reportGoodsList = new ObservableCollection<ReportGoodsModel>();

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

			_employeeModelList.CollectionChanged += (sender, e) =>
			{
				if (e.Action == NotifyCollectionChangedAction.Add)
				{
					foreach (EmployeeModel employee in e.NewItems)
					{
						if(employee.Position == "Cashier")
						{
							_employeesNameList.Add(employee.EmployeeId);
						}
					}
				}
				else if (e.Action == NotifyCollectionChangedAction.Remove)
				{
					foreach (EmployeeModel employee in e.OldItems)
					{
						if(employee.Position == "Cashier")
						{
							_employeesNameList.Remove(employee.EmployeeId);
						}
					}
				}
			};

			employeeModels = controller.GetEmployees();

			foreach (var item in employeeModels)
			{
				_employeeModelList.Add(item);
			}

			employeeDataGrid.ItemsSource = _employeeModelList;
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
			goodsInStockList = controller.GetStocks();

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
			categoriesComboBox.SelectionChanged += CategoriesComboBox_SelectionChanged;

			positionComboBox.ItemsSource = employeeRoleList;
			positionComboBox.SelectionChanged += PositionComboBox_SelectionChanged;

			_employeesNameList.Insert(0, "All");

			reportGoodsModels = controller.GetChecks();

			foreach (var item in reportGoodsModels)
			{
				_reportGoodsList.Add(item);
			}

			cashierComboBox.ItemsSource = _employeesNameList;
			cashierComboBox.SelectionChanged += EmployeeComboBox_SelectionChanged;

			receiptDataGrid.ItemsSource = _reportGoodsList;

			upcSearchTextBox.TextChanged += SearchUpcTextBox_TextChanged;

			goodsInStockDataGrid.CellEditEnding += GoodsInStockDataGrid_CellEditEnding;

			categoryNotSold.ItemsSource = _categoriesNameList;
			categoryNotSold.SelectionChanged += CategoryNotSold_SelectionChanged;

			checkBoxNoAccoundNoSold.Click += CheckBoxNoAccoundNoSold_Checked;

			searchCategoryAdvancedComboBox.ItemsSource = _categoriesNameList;
			searchCategoryAdvancedComboBox.SelectionChanged += SearchCategoryAdvancedComboBox_SelectionChanged;


		}

		private void SearchCategoryAdvancedComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			ComboBox comboBox = sender as ComboBox;
			string selectedCategory = comboBox.SelectedItem as string;

			if (selectedCategory != null)
			{
				if (selectedCategory == "All")
				{
					producerAndCategoryAdvancedDataGrid.ItemsSource = null;
					return;
				}
				List<ProducerAndCategoryAdvancedModel> list = controller.GetProductSoldNumberByCategoryIDAndGroupedByProducer(comboBox.SelectedIndex);
				producerAndCategoryAdvancedDataGrid.ItemsSource = list;
			}
		}

		private void GoodsAdvanced_Click(object sender, RoutedEventArgs e)
		{
			var text = searchGoodsAdvancedTextBox.Text;

			if (string.IsNullOrEmpty(text))
			{
				MessageBox.Show("Please enter a search query");
				return;
			}

			goodsAdvancedDataGrid.ItemsSource = controller.GetProductWithoutEmployeeSurnameStartsWith(text);
		}

		private void TabControl_Loaded(object sender, RoutedEventArgs e)
		{
			List<ManufacturerAndSalesModel> manufacturerAdvanced = controller.GetTotalSoldProductsForProducer();
			manufacturerAdvancedDataGrid.ItemsSource = manufacturerAdvanced;

			List<EmployeesAndCustomersWithMaxSharedSalesModel> employeesAndCustomersWithMaxSharedSalesModels = controller.GetEmployeesAndCustomersWithMaxSharedSales();
			employeeAndCustomerAdvancedDataGrid.ItemsSource = employeesAndCustomersWithMaxSharedSalesModels;
		}


		private void CheckBoxNoAccoundNoSold_Checked(object sender, RoutedEventArgs e)
		{
			var checkBox = sender as CheckBox;

			positionComboBox.SelectedIndex = 0;
			searchTextBox.Text = "";
			categoryNotSold.SelectedIndex = 0;

			if (checkBox.IsChecked == true)
			{
				List<EmployeeModel> newEmployeesModels = controller.GetEmployeesWithoutSalesAndAccount();
				
				_employeeModelList.Clear();

				foreach (var item in newEmployeesModels)
				{
					_employeeModelList.Add(item);	
				}
			}
			else
			{

				_employeeModelList.Clear();

				foreach (var item in employeeModels)
				{
					_employeeModelList.Add(item);
				}
			}
		}

		private void CategoryNotSold_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			ComboBox comboBox = sender as ComboBox;
			string selectedCategory = comboBox.SelectedItem as string;

			positionComboBox.SelectedIndex = 0;
			checkBoxNoAccoundNoSold.IsChecked = false;
			searchTextBox.Text = "";

			if (selectedCategory != null)
			{
				if (selectedCategory == "All")
				{
					_employeeModelList.Clear();

					foreach (var item in employeeModels)
					{
						_employeeModelList.Add(item);
					}
					return;
				}
				List<EmployeeModel> newEmployeesModels = controller.GetEmployeesWithoutSalesInCategory(selectedCategory);
				_employeeModelList.Clear();

				foreach (var item in newEmployeesModels)
				{

					_employeeModelList.Add(item);
				}
			}
		}

		private void CategoriesComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			ComboBox comboBox = sender as ComboBox;
			string selectedCategory = comboBox.SelectedItem as string;

			if (selectedCategory != null)
			{
				if (selectedCategory == "All")
				{
					_goodsInStockList.Clear();

					foreach (var item in goodsInStockList)
					{
						_goodsInStockList.Add(item);
					}

					if (upcSearchTextBox.Text != "")
					{
						List<GoodsInStockModel> filteredGoods = goodsInStockList
						.Where(g => g.UPC.StartsWith(upcSearchTextBox.Text, System.StringComparison.OrdinalIgnoreCase))
						.ToList();

						_goodsInStockList.Clear();

						foreach (var item in filteredGoods)
						{
							_goodsInStockList.Add(item);
						}
					}
					return;
				}
				List<GoodsInStockModel> stocks = controller.GetGoodsInStockByCategory(comboBox.SelectedIndex);
				_goodsInStockList.Clear();

				foreach (var item in stocks)
				{
					_goodsInStockList.Add(goodsInStockList.FirstOrDefault(x => x.ProductId == item.ProductId));
				}

				if (upcSearchTextBox.Text != "")
				{
					List<GoodsInStockModel> filteredGoods = goodsInStockList
					.Where(g => g.UPC.StartsWith(upcSearchTextBox.Text, System.StringComparison.OrdinalIgnoreCase))
					.ToList();

					_goodsInStockList.Clear();

					foreach (var item in stocks)
					{
						foreach (var item1 in filteredGoods)
						{
							if (item.ProductId == item1.ProductId)
							{
								_goodsInStockList.Add(item1);
							}
						}
					}
				}
			}
		}

		private void SearchUpcTextBox_TextChanged(object sender, TextChangedEventArgs e)
		{
			TextBox textBox = sender as TextBox;
			string searchText = textBox.Text;

			if (!string.IsNullOrEmpty(searchText))
			{
				List<GoodsInStockModel> filteredGoods = goodsInStockList
					.Where(g => g.UPC.StartsWith(searchText, System.StringComparison.OrdinalIgnoreCase))
					.ToList();

				_goodsInStockList.Clear();

				foreach (var item in filteredGoods)
				{
					_goodsInStockList.Add(item);
				}

				var selectedCategory = categoriesComboBox.SelectedItem as string;

				if (selectedCategory != null && selectedCategory != "All")
				{
					List<GoodsInStockModel> stocks = controller.GetGoodsInStockByCategory(categoriesComboBox.SelectedIndex);
					_goodsInStockList.Clear();

					

					foreach (var item in stocks)
					{
						foreach (var item1 in filteredGoods)
						{
							if (item.ProductId == item1.ProductId)
							{
								_goodsInStockList.Add(item1);
							}
						}
					}
				}
			}
			else
			{
				_goodsInStockList.Clear();

				foreach (var item in goodsInStockList)
				{
					_goodsInStockList.Add(item);
				}

				var selectedCategory = categoriesComboBox.SelectedItem as string;

				if (selectedCategory != null && selectedCategory != "All")
				{
					List<GoodsInStockModel> stocks = controller.GetGoodsInStockByCategory(categoriesComboBox.SelectedIndex);
					_goodsInStockList.Clear();

					foreach (var item in stocks)
					{
						_goodsInStockList.Add(goodsInStockList.FirstOrDefault(x => x.ProductId == item.ProductId));
					}
				}

			}
		}

		private void EmployeeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			fromDatePicker.Text = "";
			toDatePicker.Text = "";
			var emplId = (sender as ComboBox).SelectedItem.ToString();

			if(emplId == "All")
			{
				_reportGoodsList.Clear();

				foreach (var item in reportGoodsModels)
				{
					_reportGoodsList.Add(item);
				}

				return;
			}

			var checks = controller.GetChecksByEmplId(emplId);

			_reportGoodsList.Clear();

			foreach (var item in checks)
			{
				_reportGoodsList.Add(item);
			}
		}

		private void PositionComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			ComboBox comboBox = sender as ComboBox;
			string selectedCategory = comboBox.SelectedItem as string;

			if (selectedCategory != null)
			{
				if (selectedCategory == "All")
				{
					_employeeModelList.Clear();

					foreach (var item in employeeModels)
					{
						_employeeModelList.Add(item);
					}

					if (searchTextBox.Text != "")
					{
						List<EmployeeModel> filteredGoods = _employeeModelList
						.Where(g => g.LastName.StartsWith(searchTextBox.Text, System.StringComparison.OrdinalIgnoreCase))
						.ToList();

						_employeeModelList.Clear();

						foreach (var item in filteredGoods)
						{
							_employeeModelList.Add(item);
						}
					}
					return;
				}

				List<EmployeeModel> currentEmployee = _employeeModelList.ToList();
				
				_employeeModelList.Clear();

				foreach (var item in employeeModels)
				{
					
						
					if (item.Position == selectedCategory)
					{
						if (searchTextBox.Text != "")
						{
							if(item.LastName.StartsWith(searchTextBox.Text, System.StringComparison.OrdinalIgnoreCase))
							{
								_employeeModelList.Add(item);
							}
							
						} else
						{
							_employeeModelList.Add(item);
						}
					}
					
					
				}
			}
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

		private void DeleteGoodsInStockButton_Click(object sender, RoutedEventArgs e)
		{
			var selectedGoodsInStock = goodsInStockDataGrid.SelectedItem as GoodsInStockModel;

			if (selectedGoodsInStock != null)
			{
				if (_goodsInStockList.Contains(selectedGoodsInStock))
				{
					_goodsInStockList.Remove(selectedGoodsInStock);
				}

				controller.DeleteGoodsInStock(selectedGoodsInStock);
			}
		}

		private void GoodsInStockDataGrid_CellEditEnding(object? sender, DataGridCellEditEndingEventArgs e)
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
						GoodsInStockModel goodsInStock = e.Row.Item as GoodsInStockModel;

						if (goodsInStock != null)
						{
							string columnHeader = e.Column.Header.ToString();

							switch (columnHeader)
							{
								case "Name":
									goodsInStock.Name = updatedValue;
									break;
								case "Manufacturer":
									goodsInStock.Manufacturer = updatedValue;
									break;
								case "Characteristics":
									goodsInStock.Characteristics = updatedValue;
									break;
								case "UPC":
									goodsInStock.UPC = updatedValue;
									break;
								case "Price":
									if (decimal.TryParse(updatedValue, out decimal price))
									{
										goodsInStock.Price = price;
									}
									break;
								case "Quantity":
									if (int.TryParse(updatedValue, out int quantity))
									{
										goodsInStock.Quantity = quantity;
									}
									break;
								case "Discount":
									goodsInStock.Discount = (updatedValue == "True");
									break;
							}

							controller.UpdateGoodsInStock(goodsInStock);
						}
					}
				}
			}
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
			TextBox textBox = sender as TextBox;
			string searchText = textBox.Text;

			if (!string.IsNullOrEmpty(searchText))
			{
				List<EmployeeModel> filteredGoods = employeeModels
					.Where(g => g.LastName.StartsWith(searchText, System.StringComparison.OrdinalIgnoreCase))
					.ToList();

				List<EmployeeModel> currentEmployee = _employeeModelList.ToList();

				_employeeModelList.Clear();

				foreach (var item in filteredGoods)
				{
					foreach (var item1 in currentEmployee)
					{
						if (item.EmployeeId == item1.EmployeeId)
						{
							_employeeModelList.Add(item1);
						}
					}
				}
			}
			else
			{
				_employeeModelList.Clear();

				var selectedCategory = positionComboBox.SelectedItem as string;

				foreach (var item in employeeModels)
				{
					if (selectedCategory != null && selectedCategory != "All")
					{
						if (item.Position == selectedCategory)
						{
							_employeeModelList.Add(item);
						}
					} else
					{
						_employeeModelList.Add(item);
					}
				}

			}
		}

		private void DeleteButton_Click(object sender, RoutedEventArgs e)
		{
			
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

				controller.DeleteCategory(selectedCategory);
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
			ManagerAddCategoryView addCategoryWindow = new ManagerAddCategoryView(controller, _categoriesList);
			addCategoryWindow.Show();
		}
		private void EditCategoryButton_Click(object sender, RoutedEventArgs e)
		{
			ManagerEditCategoryView editCategoryWindow = new ManagerEditCategoryView();
			editCategoryWindow.Show();
		}
		private void AddGoodButton_Click(object sender, RoutedEventArgs e)
		{
			SupermarketPL.Views.ManagerAddGoodView addGoodWindow =
				new SupermarketPL.Views.ManagerAddGoodView(_goodsList, _categoriesList, controller, _categoriesNameList);
			addGoodWindow.Show();
		}

		private void EditGoodButton_Click(object sender, RoutedEventArgs e)
		{
			SupermarketPL.Views.ManagerEditGoodView editGoodWindow = new SupermarketPL.Views.ManagerEditGoodView();
			editGoodWindow.Show();
		}

		private void AddGoodInStock_Click(object sender, RoutedEventArgs e)
		{
			SupermarketPL.Views.ManagerAddGoodInStockView addGoodInStockWindow =
				new SupermarketPL.Views.ManagerAddGoodInStockView(controller, _goodsInStockList, _categoriesList,
					_goodsList);
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
			ManagerAddEmployeeView addEmployeeWindow =
				new ManagerAddEmployeeView(controller,   _employeeModelList);
			addEmployeeWindow.Show();
		}

		private void EditEmployee_Click(object sender, RoutedEventArgs e)
		{
			ManagerEditEmployeeView editEmployeeWindow = new ManagerEditEmployeeView();
			editEmployeeWindow.Show();
		}
		private void AddButton_Click(object sender, RoutedEventArgs e)
		{
			ManagerAddCustomerView addCustomerView = new ManagerAddCustomerView(_customersList,controller);
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
			//string outputPath = "ReceiptsList.pdf";
			//controller.CreateReceiptsPdf(controller.GetReceipts(), outputPath);
			//MessageBox.Show("Receipts report saved as " + outputPath);
		}

		private void SearchFromDateToDate_Click(object sender, RoutedEventArgs e)
		{
			cashierComboBox.SelectedIndex = 0;

			if (fromDatePicker.SelectedDate == null || toDatePicker.SelectedDate == null)
			{
				_reportGoodsList.Clear();

				foreach (var item in reportGoodsModels)
				{
					_reportGoodsList.Add(item);
				}
				return;
			}

			

			DateTime fromDate = fromDatePicker.SelectedDate.Value;
			DateTime toDate = toDatePicker.SelectedDate.Value;

			List<ReportGoodsModel> filteredGoods = reportGoodsModels
				.Where(g => g.Date >= fromDate && g.Date <= toDate)
				.ToList();

			_reportGoodsList.Clear();

			foreach (var item in filteredGoods)
			{
				_reportGoodsList.Add(item);
			}
		}

		private void DeleteReciptButton_Click(object sender, RoutedEventArgs e)
		{
			var selectedReceipt = receiptDataGrid.SelectedItem as ReportGoodsModel;

			if (selectedReceipt != null)
			{
				if (_reportGoodsList.Contains(selectedReceipt))
				{
					_reportGoodsList.Remove(selectedReceipt);
				}

				controller.DeleteCheck(selectedReceipt.ReceiptNumber);
			}
		}

		private void DeleteEmployee_Click(object sender, RoutedEventArgs e)
		{
			var selectedEmployee = employeeDataGrid.SelectedItem as EmployeeModel;

			if (selectedEmployee != null)
			{
				if(_employee.IdEmployee == selectedEmployee.EmployeeId)
				{
					MessageBox.Show("You can't delete yourself");
					return;
				}
				if (_employeeModelList.Contains(selectedEmployee))
				{
					_employeeModelList.Remove(selectedEmployee);
				}

				controller.DeleteEmployee(selectedEmployee.EmployeeId);
			}
		}
	}
}