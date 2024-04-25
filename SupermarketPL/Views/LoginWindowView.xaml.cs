using System.Windows;
using SupermarketDAL.DB;
using SupermarketDAL.Entities;

namespace SupermarketPL.Views
{
    public partial class LoginWindow : Window
    {
        private DatabaseHelper dbHelper;

        public LoginWindow()
        {
            InitializeComponent();
			dbHelper = new DatabaseHelper("../../../../SupermarketDAL/zlagoda.db");
		}

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string username = usernameTextBox.Text;
            string password = passwordBox.Password;

            // Hash the password
            string hashedPassword = HashPassword(password);

            // Check the credentials
            Employee employee = dbHelper.GetEmployeeByUsernameAndPassword(username, hashedPassword);
            if (employee != null)
            {
                // Open the ManagerView or CashierView based on the role of the user
                if (employee.EmplRole == "Manager")
                {
                    ManagerView managerView = new ManagerView();
                    managerView.Show();
                }
                else if (employee.EmplRole == "Cashier")
                {
                    CashierView cashierView = new CashierView();
                    cashierView.Show();
                }

                this.Close();
            }
            else
            {
                MessageBox.Show("Invalid username or password.");
            }
        }

        private string HashPassword(string password)
        {
            return password;
        }
    }
}