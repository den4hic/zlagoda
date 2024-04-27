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
                    string producer = selectedGood.Manufacturer;
                    string characteristics = selectedGood.Characteristics;
                    string upc = upcTextBox.Text;
                    decimal sellingPrice = decimal.Parse(priceTextBox.Text);
                    int productsNumber = int.Parse(availableTextBox.Text);
                    bool promotionalProduct;
                    bool.TryParse(discountComboBox.SelectedItem.ToString(), out promotionalProduct);

                    GoodsInStock newGoodInStock = new GoodsInStock
                    {
                        IdProduct = productId,
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