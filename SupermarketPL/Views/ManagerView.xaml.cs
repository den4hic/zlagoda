using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using SupermarketDAL.Entities;
using SupermarketPL.Model;

namespace SupermarketPL.Views
{
    public partial class ManagerView : Window
    {
        private ManagerController controller;
		private ObservableCollection<Goods> _goodsList = new ObservableCollection<Goods>();
		private ObservableCollection<Category> _categoriesList = new ObservableCollection<Category>();
		private ObservableCollection<string> _categoriesNameList = new ObservableCollection<string>();
		private ObservableCollection<CustomerModel> _customersList = new ObservableCollection<CustomerModel>();

		public ManagerView()
        {
            InitializeComponent();
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

			List<CustomerModel> customersList = controller.GetCustomers();

			_categoriesNameList.Insert(0, "All");

			foreach (var item in customersList)
			{
				_customersList.Add(item);
			}


			customerDataGrid.ItemsSource = _customersList;
			customerDataGrid.CellEditEnding += CustomerDataGrid_CellEditEnding;

			
			categoriesComboBox.ItemsSource = _categoriesNameList;

			//foreach (var category in _categoriesList)
			//{
			//	categoriesComboBox.Items.Add(category.CategoryName);
			//}

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
	}
}