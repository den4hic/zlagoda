using System.Windows;

namespace SupermarketPL.Views
{
    public partial class ManagerAddGoodView : Window
    {
        public ManagerAddGoodView()
        {
            InitializeComponent();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            //тут треба додати товар
            this.Close();
            MessageBox.Show("Good added successfully!");
        }
    }
}