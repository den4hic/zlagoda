using System.Windows;
using System.Windows.Controls;
using SupermarketDAL.Entities;

namespace SupermarketPL
{
    public partial class ManagerView : Window
    {
        private ManagerController controller;

        public ManagerView(ManagerController controller)
        {
            InitializeComponent();
            this.controller = controller;

        }
        
        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e) 
        {
            var searchText = searchTextBox.Text.ToLower();
            employeeInfoComboBox.ItemsSource = employees
                .Where(employee => employee.FullName.ToLower().Contains(searchText))
                .ToList();
        }
    }
}