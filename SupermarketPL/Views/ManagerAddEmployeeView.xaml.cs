using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using SupermarketDAL.Entities;
using SupermarketPL.Model;

namespace SupermarketPL.Views
{
    public partial class ManagerAddEmployeeView : Window
    {
        ManagerController controller;
        private ObservableCollection<EmployeeModel> employeeModelList { get; set; }
        public ManagerAddEmployeeView(ManagerController controller  , ObservableCollection<EmployeeModel> employeeModelList)
        {
            InitializeComponent();
            this.controller = controller; ;
            this.employeeModelList = employeeModelList;
            positionComboBox.ItemsSource = new ObservableCollection<string> { "Manager", "Cashier" };
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
            EmployeeModel newEmployeeModel = new EmployeeModel
            {
                FirstName = EmplName,
                LastName = EmplSurname,
                PatronymicName = EmplPatronymic,
                Position = EmplRole,
                Salary = salary,
                BirthDate = DateOfBirth.Value,
                WorkStartDate = DateOfStart.Value,
                PhoneNumber = PhoneNumber,
                City = City,
                Street = Street,
                Index = ZipCode
            };
            employeeModelList.Add(newEmployeeModel);
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