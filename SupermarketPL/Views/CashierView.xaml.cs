using System.Windows;
using SupermarketDAL.Entities;

namespace SupermarketPL
{
    public partial class CashierView : Window
    {
        private CashierController controller;

        public CashierView(CashierController controller)
        {
            InitializeComponent();
            this.controller = controller;

        }
    }
}