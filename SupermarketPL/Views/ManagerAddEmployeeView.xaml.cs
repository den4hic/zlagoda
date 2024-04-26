using System.Windows;
using System.Windows.Input;
using SupermarketDAL.Entities;

namespace SupermarketPL.Views
{
    public partial class ManagerAddEmployeeView : Window
    {
        ManagerController controller;
        List<string> employeeRoleList;
        Employee employee;
        public ManagerAddEmployeeView(ManagerController controller, List<string> employeeRoleList  ,Employee _employee)
        {
            InitializeComponent();
            this.controller = controller;
            this.employeeRoleList = employeeRoleList;
            this.employee = _employee;
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            string EmplSurname = lastNameTextBox.Text;
            string EmplName = firstNameTextBox.Text;
            string EmplPatronymic = patronymicNameTextBox.Text;
            string EmplRole = positionComboBox.Text;
            string Salary = salaryTextBox.Text;
            DateTime? DateOfBirth = birthDatePicker.SelectedDate;
            DateTime? DateOfStart = workStartDatePicker.SelectedDate;
            string PhoneNumber = phoneNumberTextBox.Text;
            string City = cityTextBox.Text;
            string Street = streetTextBox.Text;
            string ZipCode = indexTextBox.Text;

            if (string.IsNullOrEmpty(EmplSurname) || string.IsNullOrEmpty(EmplName) || string.IsNullOrEmpty(EmplPatronymic) || string.IsNullOrEmpty(EmplRole) || string.IsNullOrEmpty(Salary) || DateOfBirth == null || DateOfStart == null || string.IsNullOrEmpty(PhoneNumber) || string.IsNullOrEmpty(City) || string.IsNullOrEmpty(Street) || string.IsNullOrEmpty(ZipCode))
            {
                MessageBox.Show("Please fill in all fields.");
                return;
            }

            if (!decimal.TryParse(Salary, out decimal salary))
            {
                MessageBox.Show("Please enter a valid decimal value for salary.");
                return;
            }

            Employee newEmployee = new Employee
            {
                EmplName= EmplName,
                EmplSurname = EmplSurname,
                EmplPatronymic = EmplPatronymic,
                EmplRole = EmplRole,
                Salary = salary,
                PhoneNumber = PhoneNumber,
                City = City,
                Street = Street,
                ZipCode = ZipCode
            };
            
            controller.InsertEmployee(newEmployee);
            MessageBox.Show("Employee added successfully!");
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
}