using System.Windows;

namespace SupermarketPL.Views
{
    public partial class ManagerEditEmployeeView : Window
    {
        public ManagerEditEmployeeView()
        {
            InitializeComponent();
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            //��� ����� ������������ ���������� � ��� �����
            this.Close();
            MessageBox.Show("Employee edited successfully!");
        }
    }
}