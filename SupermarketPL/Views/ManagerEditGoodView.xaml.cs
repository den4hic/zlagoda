using System.Windows;

namespace SupermarketPL.Views
{
    public partial class ManagerEditGoodView : Window
    {
        public ManagerEditGoodView()
        {
            InitializeComponent();
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            //тут треба відредагувати товар
            this.Close();
            MessageBox.Show("Good edited successfully!");
        }
    }
}