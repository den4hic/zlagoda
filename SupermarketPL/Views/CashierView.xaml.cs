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

			customerDataGrid.ItemsSource = customersList;

            List<CategoryModel> categoriesList = controller.GetCategories();

            categoryComboBox.Items.Add("All");

            foreach (var category in categoriesList)
            {
				categoryComboBox.Items.Add(category.Name);
			}

			categoryComboBox.SelectionChanged += CategoryComboBox_SelectionChanged;

			productNameSearchTextBox.TextChanged += ProductNameSearchTextBox_TextChanged;
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
				MessageBox.Show($"Вибрано категорію: {selectedCategory}");
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

	}
}