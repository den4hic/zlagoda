using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using SupermarketDAL.Entities;
using SupermarketPL.Model;

namespace SupermarketPL.Views
{
    public partial class CashierView : Window
    {
		private Employee _employee;
        private CashierController controller;
		private List<Goods> _goodsList;
		private List<CategoryModel> _categories;
		private ObservableCollection<CustomerModel> _customersList = new ObservableCollection<CustomerModel>();
		private List<GoodsInStockModel> goodsInBasketList;
		private List<Goods> _updatedGoodsList;
		private ObservableCollection<GoodsInStockModel> _stocks = new ObservableCollection<GoodsInStockModel>();
		private ObservableCollection<BasketGoods> _basketGoods = new ObservableCollection<BasketGoods>();

		public CashierView()
		{
			Initialize(null);
		}

        public CashierView(Employee employee)
        {
			Initialize(employee);
		}

		private void Initialize(Employee employee)
		{
			InitializeComponent();
			this._employee = employee;
			this.controller = new CashierController();
			List<Goods> goodsList = controller.GetGoods();

			foreach (var item in goodsList)
			{
				_goodsList.Add(item);
			}


			goodsDataGrid.ItemsSource = _goodsList;
			List<CustomerModel> customersList = controller.GetCustomers();

			foreach (var item in customersList)
			{
				_customersList.Add(item);
			}


			customerDataGrid.ItemsSource = _customersList;
			customerDataGrid.CellEditEnding += CustomerDataGrid_CellEditEnding;

			List<GoodsInStockModel> goodsInBasketList = controller.GetStocks();

			this.goodsInBasketList = goodsInBasketList;

			foreach (var item in goodsInBasketList)
			{
				_stocks.Add(item);
			}

			goodsInBasketDataGrid.ItemsSource = _stocks;

			List<Category> categoriesList = controller.GetCategories();

			categoryBasketComboBox.Items.Add("All");

			foreach (var category in categoriesList)
			{
				categoryBasketComboBox.Items.Add(category.CategoryName);
			}

			categoryComboBox.Items.Add("All");

			foreach (var category in categoriesList)
			{
				categoryComboBox.Items.Add(category.CategoryName);
			}

			categoryComboBox.SelectionChanged += CategoryComboBox_SelectionChanged;
			categoryBasketComboBox.SelectionChanged += BasketComboBox_SelectionChanged;

			productNameSearchTextBox.TextChanged += ProductNameSearchTextBox_TextChanged;
			nameSearchTextBox.TextChanged += GoodsInStockNameSearchTextBox_TextChanged;
			customerNameSearchTextBox.TextChanged += CustomerNameSearchTextBox_TextChanged;
			basketDataGrid.ItemsSource = _basketGoods;
		}

		private void BasketComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			ComboBox comboBox = sender as ComboBox;
			string selectedCategory = comboBox.SelectedItem as string;

			if (selectedCategory != null)
			{
				if (selectedCategory == "All")
				{
					_stocks.Clear();

					foreach (var item in goodsInBasketList)
					{
						_stocks.Add(item);
					}
					return;
				}
				List<GoodsInStockModel> stocks = controller.GetGoodsInStockByCategory(comboBox.SelectedIndex - 1);
				_stocks.Clear();

				foreach (var item in stocks)
				{

					_stocks.Add(goodsInBasketList.FirstOrDefault(x => x.ProductId == item.ProductId));
				}
			}
		}

		private void GoodsInStockNameSearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
		{
			TextBox textBox = sender as TextBox;
			string searchText = textBox.Text;

			if (!string.IsNullOrEmpty(searchText))
			{
				List<GoodsInStockModel> filteredGoods = _stocks
					.Where(g => g.Name.StartsWith(searchText, System.StringComparison.OrdinalIgnoreCase))
					.ToList();

				_stocks.Clear();

				foreach (var item in filteredGoods)
				{
					_stocks.Add(item);
				}
			}
			else
			{
				_stocks.Clear();

				foreach (var item in goodsInBasketList)
				{
					_stocks.Add(item);
				}
			}
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
				List<Goods> goods = controller.GetGoodsByCategory(comboBox.SelectedIndex - 1);
				_updatedGoodsList = goods;
				goodsDataGrid.ItemsSource = goods;
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

		private void BasketDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
		{
			var dataGrid = sender as DataGrid;
			if (dataGrid != null && dataGrid.SelectedItem != null)
			{
				var selectedRow = dataGrid.SelectedItem as BasketGoods;
				bool flag = false;

				foreach (var item in _stocks)
				{
					if (item.ProductId == selectedRow.BasketGoodsId)
					{
						item.Quantity++;
						int indexStock = _stocks.IndexOf(item);
						_stocks.RemoveAt(indexStock);
						_stocks.Insert(indexStock, item);
						flag = true;
						break;
					}
				}

				if(!flag)
				{
					foreach (var item in goodsInBasketList)
					{
						if (item.ProductId == selectedRow.BasketGoodsId)
						{
							item.Quantity++;
							int indexStock = _stocks.IndexOf(item);
							goodsInBasketList.RemoveAt(indexStock);
							goodsInBasketList.Insert(indexStock, item);
							break;
						}
					}
				}


				if (selectedRow.Quantity == 1)
				{
					_basketGoods.Remove(selectedRow);
					return;
				}

				selectedRow.Quantity--;

				int index = _basketGoods.IndexOf(selectedRow);
				_basketGoods.RemoveAt(index);
				_basketGoods.Insert(index, selectedRow);
			}
		}

		private void GoodsInBasketDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
		{
			
			var dataGrid = sender as DataGrid;
			if (dataGrid != null && dataGrid.SelectedItem != null)
			{
				var selectedRow = dataGrid.SelectedItem as GoodsInStockModel;

				if(selectedRow.Quantity == 0)
				{
					MessageBox.Show("����� ������� �� �����");
					return;
				}

				selectedRow.Quantity--;

				int indexStock = _stocks.IndexOf(selectedRow);
				_stocks.RemoveAt(indexStock);
				_stocks.Insert(indexStock, selectedRow);

				

				foreach (var item in _basketGoods)
				{
					if (item.BasketGoodsId == selectedRow.ProductId)
					{
						item.Quantity++;
						item.Price = item.Price * item.Quantity;
						int index = _basketGoods.IndexOf(item);
						_basketGoods.RemoveAt(index);
						_basketGoods.Insert(index, item);
						return;
					}
				}

				BasketGoods basketGoods = new BasketGoods()
				{
					BasketGoodsId = selectedRow.ProductId,
					Name = selectedRow.Name,
					Quantity = 1,
					Price = selectedRow.Price,
				};

				_basketGoods.Add(basketGoods);

				
			}
		}
		private void AddButton_Click(object sender, RoutedEventArgs e)
		{
			CashierAddCustomerView addCustomerView = new CashierAddCustomerView(_customersList);
			addCustomerView.Show();
		}
		private void EditButton_Click(object sender, RoutedEventArgs e)
		{
			EditCustomerView editCustomerView = new EditCustomerView();
			editCustomerView.Show();
		}
		private void UPCSearchButton_Click(object sender, RoutedEventArgs e)
		{
			UPCSearchView upcSearchView = new UPCSearchView();
			//Тут треба дістати товар за допомогою UPC з upcSearchTextBox і передати його в upcSearchView

			upcSearchView.Show();
		}
		
		private void ReceiptIDSearchButton_Click(object sender, RoutedEventArgs e)
		{
			//ReceiptIdSearchView receiptIdSearchView = new ReceiptIdSearchView();

			//Тут треба дістати чек за допомогою ID з receiptIdTextBox і передати його в receiptIdSearchView
			//receiptIdSearchView.Show();
		}
		private void ProfileButton_Click(object sender, RoutedEventArgs e)
		{
			//EmployeeProfileView profileWindow = new EmployeeProfileView(_employee);
			//profileWindow.Show();
		}


		private void CloseCheck_Click(object sender, RoutedEventArgs e)
		{
			//Ми додаймо чек,зберігажмо айді і робимо пошук по його ж айді
			//ReceiptIdSearchView receiptIdSearchView = new ReceiptIdSearchView();

			
			//receiptIdSearchView.Show();
		}

		private void SearchFromDateToDate_Click(object sender, RoutedEventArgs e)
		{
			throw new NotImplementedException();
		}
    }
}