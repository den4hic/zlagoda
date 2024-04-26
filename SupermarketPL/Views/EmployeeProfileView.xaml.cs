using System.Windows;
using SupermarketDAL.DB;
using SupermarketDAL.Entities;

namespace SupermarketPL.Views
{

    public partial class EmployeeProfileView : Window
    {
        private DatabaseHelper dbHelper;

        public EmployeeProfileView(Employee employee)
        {
            InitializeComponent();
            dbHelper = new DatabaseHelper("../../../../SupermarketDAL/zlagoda.db");
            FillCashierInfo(employee);
        }

        private void FillCashierInfo(Employee cashier)
        {
            idLabel.Content = cashier.IdEmployee.ToString();
            firstNameLabel.Content = cashier.EmplName;
            lastNameLabel.Content = cashier.EmplSurname;
            patronymicNameLabel.Content = cashier.EmplPatronymic;
            positionLabel.Content = cashier.EmplRole;
            salaryLabel.Content = cashier.Salary.ToString();
            workStartDateLabel.Content = cashier.DateOfStart.ToString();
            birthDateLabel.Content = cashier.DateOfBirth.ToString();
            phoneNumberLabel.Content = cashier.PhoneNumber;
            cityLabel.Content = cashier.City;
            streetLabel.Content = cashier.Street;
            indexLabel.Content = cashier.ZipCode;
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}