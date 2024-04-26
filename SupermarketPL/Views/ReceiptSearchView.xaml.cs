using SupermarketDAL.DB;
using SupermarketDAL.Entities;
using SupermarketPL.Model;
using System.Windows;

namespace SupermarketPL.Views
{

    public partial class ReceiptIdSearchView : Window
    {
        private DatabaseHelper dbHelper;

        public ReceiptIdSearchView(Check check, List<BasketGoods> goods)
        {
            InitializeComponent();
            dbHelper = new DatabaseHelper("../../../../SupermarketDAL/zlagoda.db");
			FillReceiptInfo(check, goods);
        }

        private void FillReceiptInfo(Check check, List<BasketGoods> goods)
        {
            dateLabel.Content = check.PrintDate.ToString();
            totalCostLabel.Content = check.SumTotal.ToString();
            goodsDataGrid.ItemsSource = goods;
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}