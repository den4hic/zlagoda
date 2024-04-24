using System.Windows;
using SupermarketDAL.DB;
using SupermarketDAL.Entities;

namespace SupermarketPL
{
    public partial class LoginWindow : Window
    {
        private DatabaseHelper dbHelper;

        public LoginWindow()
        {
            InitializeComponent();
            dbHelper = new DatabaseHelper("zlagoda.db");
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
                    ManagerView managerView = new ManagerView(new ManagerController());
                    managerView.Show();
                }
                else if (employee.EmplRole == "Cashier")
                {
                    CashierView cashierView = new CashierView(new CashierController());
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