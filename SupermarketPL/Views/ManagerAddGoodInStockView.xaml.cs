using System.Collections.ObjectModel;
using System.Windows;
using SupermarketDAL.Entities;
using SupermarketPL.Model;

namespace SupermarketPL.Views
{
    public partial class ManagerAddGoodInStockView : Window
    {
        ManagerController controller;
        ObservableCollection<GoodsInStockModel> goodsInStockList;
        ObservableCollection<Category> categoriesList;
        ObservableCollection<Goods> goodsList;
        public ManagerAddGoodInStockView(ManagerController controller, ObservableCollection<GoodsInStockModel> goodsInStockList, ObservableCollection<Category> categoriesList, ObservableCollection<Goods> goodsList)
        {
            InitializeComponent();
            this.controller = controller;
            this.goodsInStockList = goodsInStockList;
            this.categoriesList = categoriesList;
            this.goodsList = goodsList;
            // Populate productComboBox
            foreach (var good in goodsList)
            {
                productComboBox.Items.Add($"{good.ProductId} - {good.Name}");
            }
            discountComboBox.Items.Add("True");
            discountComboBox.Items.Add("False");
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            string selectedItem = productComboBox.SelectedItem as string;

            if (selectedItem != null)
            {
                string[] parts = selectedItem.Split(" - ");
                int idProduct = int.Parse(parts[0]);

                Goods selectedGood = goodsList.FirstOrDefault(g => g.ProductId == idProduct);

                if (selectedGood != null)
                {
                    int productId = selectedGood.ProductId;
                    string productName = selectedGood.Name;
                    int categoryId = selectedGood.CategoryId;
                    string producer = selectedGood.Manufacturer;
                    string characteristics = selectedGood.Characteristics;
                    string upc = upcTextBox.Text;
                    if (!decimal.TryParse(priceTextBox.Text, out decimal sellingPrice))
                    {
                        MessageBox.Show("Please enter a valid decimal number for Selling Price.");
                        return;
                    }

                    if (!int.TryParse(availableTextBox.Text, out int productsNumber))
                    {
                        MessageBox.Show("Please enter a valid integer for Products Number.");
                        return;
                    }
                    bool promotionalProduct;
                    bool.TryParse(discountComboBox.SelectedItem.ToString(), out promotionalProduct);

                    if (string.IsNullOrWhiteSpace(productName) || string.IsNullOrWhiteSpace(producer) || string.IsNullOrWhiteSpace(characteristics) || string.IsNullOrWhiteSpace(upc) || discountComboBox.SelectedItem == null)
                    {
                        MessageBox.Show("Please fill all fields.");
                        return;
                    }
                    
                    GoodsInStock newGoodInStock = new GoodsInStock
                    {
                        IdProduct = productId,
                        CategoryId = categoryId,
                        ProductName = productName,
                        Producer = producer,
                        Characteristics = characteristics,
                        UPC = upc,
                        SellingPrice = sellingPrice,
                        ProductsNumber = productsNumber,
                        PromotionalProduct = promotionalProduct
                    };
                    GoodsInStockModel newGoodInStockModel = new GoodsInStockModel
                    {
                        ProductId = productId,
                        CategoryId = categoryId,
                        Name = productName,
                        Manufacturer = producer,
                        Characteristics = characteristics,
                        UPC = upc,
                        Price = sellingPrice,
                        Quantity = productsNumber,
                        Discount = promotionalProduct
                    };


                    goodsInStockList.Add(newGoodInStockModel);

                    controller.InsertGoodInStock(newGoodInStock);
                }
            }

            this.Close();
            MessageBox.Show("Good added to stock successfully!");
        }
        
    }
}