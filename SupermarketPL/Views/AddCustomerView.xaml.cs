using System.Windows;

namespace SupermarketPL.Views;

public partial class AddCustomerView : Window
{
    public AddCustomerView()
    {
        InitializeComponent();
    }

    private void AddButton_Click(object sender, RoutedEventArgs e)
    {
        //Додавання покупця в базу даних
        MessageBox.Show("Customer added to the database!");
    }
}