using System.Collections.ObjectModel;
using System.Windows;
using SupermarketDAL.Entities;

namespace SupermarketPL.Views
{
    public partial class ManagerAddCategoryView : Window
    {
        ManagerController controller;
        ObservableCollection<Category> categoriesList;
        public ManagerAddCategoryView(ManagerController controller,ObservableCollection<Category> _categoriesList)
        {
            InitializeComponent();
            this.controller = controller;
            this.categoriesList = _categoriesList;
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            string categoryName = categoryNameTextBox.Text;

            if (string.IsNullOrEmpty(categoryName))
            {
                MessageBox.Show("Please enter a category name.");
                return;
            }

            Category newCategory = new Category
            {
                CategoryName = categoryName
            };
            controller.InsertCategory(categoryName);
            categoriesList.Clear();
            var categories = controller.GetCategories();
            foreach (var category in categories)
            {
                categoriesList.Add(category);
            }
            MessageBox.Show("Category added successfully!");
            this.Close();
        }
    }
}