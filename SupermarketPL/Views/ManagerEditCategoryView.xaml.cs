using System.Windows;

namespace SupermarketPL.Views
{
    public partial class ManagerEditCategoryView : Window
    {
        public ManagerEditCategoryView()
        {
            InitializeComponent();
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        { 
            //тут треба відредагувати категорію
            this.Close();
            MessageBox.Show("Category edited successfully!");
        }
    }
}