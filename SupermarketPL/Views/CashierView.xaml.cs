using System.Windows;
using SupermarketDAL.Entities;
using SupermarketPL.Model;

namespace SupermarketPL.Views
{
    public partial class CashierView : Window
    {
        private CashierController controller;

        public CashierView()
        {
            InitializeComponent();
            this.controller = new CashierController();
			List<Goods> goodsList = controller.GetGoods();

			goodsDataGrid.ItemsSource = goodsList;
		}
    }
}