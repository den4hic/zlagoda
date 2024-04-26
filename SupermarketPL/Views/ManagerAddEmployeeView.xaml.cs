using System.Windows;

namespace SupermarketPL.Views
{
    public partial class ManagerAddEmployeeView : Window
    {
        public ManagerAddEmployeeView()
        {
            InitializeComponent();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            //тут треба додати працівника в базу даних
            this.Close();
            MessageBox.Show("Employee added successfully!");
        }
    }
}