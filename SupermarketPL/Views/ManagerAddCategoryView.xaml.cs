using System.Windows;

namespace SupermarketPL.Views
{
    public partial class ManagerAddCategoryView : Window
    {
        public ManagerAddCategoryView()
        {
            InitializeComponent();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            //тут треба додати категорію
            this.Close();
            MessageBox.Show("Category added successfully!");
        }
    }
}