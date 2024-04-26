using System.Windows;

namespace SupermarketPL.Views;

public partial class EditCustomerView : Window
{
    public EditCustomerView()
    {
        InitializeComponent();
    }

    private void EditCustomer_Click(object sender, RoutedEventArgs e)
    {
        //Редагування покупця в базі даних
        MessageBox.Show("Customer edited in the database!");
    }
}