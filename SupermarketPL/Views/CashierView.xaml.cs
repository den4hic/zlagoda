using System.Windows;
using SupermarketDAL.Entities;

namespace SupermarketPL.Views
{
    public partial class CashierView : Window
    {
        private CashierController controller;

        public CashierView()
        {
            InitializeComponent();
            this.controller = new CashierController();

        }
    }
}