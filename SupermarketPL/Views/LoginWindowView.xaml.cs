using System.Security.Cryptography;
using System.Text;
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
            string hashedPassword = HashPassword(password);

            Employee employee = dbHelper.GetEmployeeByUsernameAndPassword(username, hashedPassword);
            if (employee != null)
            {
                if (employee.EmplRole == "Manager")
                {
                    ManagerView managerView = new ManagerView(employee);
                    managerView.Show();
                }
                else if (employee.EmplRole == "Cashier")
                {
                    CashierView cashierView = new CashierView(employee);
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
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));

                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }
}