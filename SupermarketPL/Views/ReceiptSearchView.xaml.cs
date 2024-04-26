using SupermarketDAL.DB;
using SupermarketDAL.Entities;
using System.Windows;

namespace SupermarketPL.Views
{

    public partial class ReceiptIdSearchView : Window
    {
        private DatabaseHelper dbHelper;

        public ReceiptIdSearchView()
        {
            InitializeComponent();
            dbHelper = new DatabaseHelper("../../../../SupermarketDAL/zlagoda.db");
            FillReceiptInfo();
        }

        private void FillReceiptInfo()
        {

            //тут треба прийняти чек і вивести його дані
            //dateLabel.Content = receipt.Date.ToString();
            //totalCostLabel.Content = receipt.TotalCost.ToString();
            //Тут треба вивести всі товари з чека
            //goodsDataGrid.ItemsSource = receipt.Goods;
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}