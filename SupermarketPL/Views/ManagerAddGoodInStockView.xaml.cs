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
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            
            this.Close();
            MessageBox.Show("Good added to stock successfully!");
        }
    }
}