using System.Windows;
using System.Windows.Controls;
using SupermarketDAL.Entities;
using SupermarketPL.Model;

namespace SupermarketPL.Views
{
    public partial class CashierView : Window
    {
        private CashierController controller;
		private List<Goods> _goodsList;
		private List<CustomerModel> _customersList;
		private List<Goods> _updatedGoodsList;

        public CashierView()
        {
            InitializeComponent();
            this.controller = new CashierController();
			List<Goods> goodsList = controller.GetGoods();
			_goodsList = goodsList;
			_updatedGoodsList = goodsList;

			goodsDataGrid.ItemsSource = goodsList;

            List<CustomerModel> customersList = controller.GetCustomers();
			_customersList = customersList;
			customerDataGrid.ItemsSource = customersList;
			customerDataGrid.CellEditEnding += CustomerDataGrid_CellEditEnding;

			List<CategoryModel> categoriesList = controller.GetCategories();

            categoryComboBox.Items.Add("All");

            foreach (var category in categoriesList)
            {
				categoryComboBox.Items.Add(category.Name);
			}

			categoryComboBox.SelectionChanged += CategoryComboBox_SelectionChanged;

			productNameSearchTextBox.TextChanged += ProductNameSearchTextBox_TextChanged;
			customerNameSearchTextBox.TextChanged += CustomerNameSearchTextBox_TextChanged;
		}

		private void CustomerNameSearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
		{
			TextBox textBox = sender as TextBox;
			string searchText = textBox.Text;

			if (!string.IsNullOrEmpty(searchText))
			{
				List<CustomerModel> filteredGoods = _customersList
					.Where(g => g.LastName.StartsWith(searchText, System.StringComparison.OrdinalIgnoreCase))
					.ToList();

				customerDataGrid.ItemsSource = filteredGoods;
			}
			else
			{
				customerDataGrid.ItemsSource = _customersList;
			}
		}

		private void CategoryComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			ComboBox comboBox = sender as ComboBox;
			string selectedCategory = comboBox.SelectedItem as string;

			if (selectedCategory != null)
			{
				if (selectedCategory == "All")
				{
					goodsDataGrid.ItemsSource = _goodsList;
					return;
				}
				List<Goods> goods = controller.GetGoodsByCategory(selectedCategory);
				_updatedGoodsList = goods;
				goodsDataGrid.ItemsSource = goods;
				MessageBox.Show($"������� ��������: {selectedCategory}");
			}
		}

		private void ProductNameSearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
		{
			TextBox textBox = sender as TextBox;
			string searchText = textBox.Text;

			if (!string.IsNullOrEmpty(searchText))
			{
				List<Goods> filteredGoods = _goodsList
					.Where(g => g.Name.StartsWith(searchText, System.StringComparison.OrdinalIgnoreCase))
					.ToList();

				goodsDataGrid.ItemsSource = filteredGoods;
				_updatedGoodsList = filteredGoods;
			}
			else
			{
				goodsDataGrid.ItemsSource = _goodsList;
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


	}
}