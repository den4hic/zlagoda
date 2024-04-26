using System.Windows;
using SupermarketDAL.DB;

namespace SupermarketPL.Views
{

    public partial class EmployeeProfileView : Window
    {
        private DatabaseHelper dbHelper;

        public EmployeeProfileView()
        {
            InitializeComponent();
            dbHelper = new DatabaseHelper("../../../../SupermarketDAL/zlagoda.db");
            FillCashierInfo();
        }

        private void FillCashierInfo()
        {
            

            //тут треба вивести його дані
           // idLabel.Content = cashier.Id.ToString();
           // firstNameLabel.Content = cashier.FirstName;
           // lastNameLabel.Content = cashier.LastName;
           // patronymicNameLabel.Content = cashier.PatronymicName;
           // positionLabel.Content = cashier.Position;
           // salaryLabel.Content = cashier.Salary.ToString();
           // workSartDateLabel.Content = cashier.WorkStartDate.ToString();
           // birthDateLabel.Content = cashier.BirthDate.ToString();
            //phoneNumberLabel.Content = cashier.PhoneNumber;
            //cityLabel.Content = cashier.City;
            //streetLabel.Content = cashier.Street;
           // indexLabel.Content = cashier.Index;
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}