using System.Windows;

namespace SupermarketPL.Views
{
    public partial class ManagerEditGoodInStockView : Window
    {
        public ManagerEditGoodInStockView()
        {
            InitializeComponent();
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            //Тут треба відредагувати товар на складі в базі даних
            this.Close();
            MessageBox.Show("Good edited in stock successfully!");
        }
    }
}