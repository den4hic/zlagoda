using SupermarketDAL.DB;
using SupermarketDAL.Entities;
using System.Windows;

namespace SupermarketPL.Views
{
    public partial class UPCSearchView : Window
    {
        private DatabaseHelper dbHelper;

        public UPCSearchView()
        {
            InitializeComponent();
            dbHelper = new DatabaseHelper("../../../../SupermarketDAL/zlagoda.db");
            FillTextBlocks();
        }

        private void FillTextBlocks()
        {
           //тут треба дістати товар за допомогою UPC

            nameLabel.Content = good.Name;
            priceLabel.Content = good.Price.ToString();
            quantityLabel.Content = good.Quantity.ToString();
            characteristicsLabel.Text = good.Characteristics;
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}