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
            //��� ����� ������ �����
            this.Close();
            MessageBox.Show("Good added successfully!");
        }
    }
}