using System.Windows;

namespace SupermarketPL.Views
{
    public partial class ManagerAddGoodInStockView : Window
    {
        public ManagerAddGoodInStockView()
        {
            InitializeComponent();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            //“ут треба додати товар на склад в базу даних
            this.Close();
            MessageBox.Show("Good added to stock successfully!");
        }
    }
}