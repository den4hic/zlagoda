using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using SupermarketDAL.Entities;
using SupermarketPL.Model;

namespace SupermarketPL.Views;

public partial class CashierAddCustomerView : Window
{
    private ObservableCollection<CustomerModel> customersList { get; set; }
    private CashierController controller;

    public CashierAddCustomerView(ObservableCollection<CustomerModel> customersList, CashierController controller)
    {
        InitializeComponent();
        this.customersList = customersList;
        this.controller = controller;

    }

    private void AddButton_Click(object sender, RoutedEventArgs e)
    {
        string cardNumber = cardNumberTextBox.Text;
        string firstName = firstNameTextBox.Text;
        string lastName = lastNameTextBox.Text;
        string patronymicName = patronymicNameTextBox.Text;
        string phoneNumber = phoneNumberTextBox.Text;
        string city = cityTextBox.Text;
        string street = streetTextBox.Text;
        string index = indexTextBox.Text;

        if (string.IsNullOrEmpty(cardNumber) || string.IsNullOrEmpty(firstName) || string.IsNullOrEmpty(lastName) || string.IsNullOrEmpty(patronymicName) || string.IsNullOrEmpty(phoneNumber) || string.IsNullOrEmpty(city) || string.IsNullOrEmpty(street) || string.IsNullOrEmpty(index) || string.IsNullOrEmpty(discountTextBox.Text))
        {
            MessageBox.Show("Please fill in all fields.");
            return;
        }

        if (!int.TryParse(discountTextBox.Text, out int discount))
        {
            MessageBox.Show("Please enter a valid integer value for discount.");
            return;
        }
        
        CustomerCard newCustomerCard = new CustomerCard
        {
            CardNumber = cardNumber,
            CustSurname = firstName,
            CustName = lastName,
            CustPatronymic = patronymicName,
            PhoneNumber = phoneNumber,
            City = city,
            Street = street,
            Index = index,
            Percentage = discount
        };

        CustomerModel newCustomer = new CustomerModel
        {
            CardNumber = cardNumber,
            FirstName = firstName,
            LastName = lastName,
            PatronymicName = patronymicName,
            PhoneNumber = phoneNumber,
            City = city,
            Street = street,
            Index = index,
            Discount = discount
        };
        

        controller.InsertCustomerCard(cardNumber, firstName, lastName, patronymicName, phoneNumber, city, street, index, discount);
        this.customersList.Add(newCustomer);        
        MessageBox.Show("Customer added to the database!");
        this.Close();
    }

    private void IntegerOnlyTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
    {
        if (e == null || string.IsNullOrEmpty(e.Text) || !char.IsDigit(e.Text, e.Text.Length - 1))
        {
            e.Handled = true;
            MessageBox.Show("Please enter only integer values.");
        }
    }
    
}