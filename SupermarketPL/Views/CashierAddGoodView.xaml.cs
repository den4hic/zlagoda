using System;
using System.Windows;

namespace SupermarketPL.Views
{
    public partial class AddGoodView : Window
    {
        public AddGoodView()
        {
            InitializeComponent();
        }

        private void MinusButton_Click(object sender, RoutedEventArgs e)
        {
            int counter = int.Parse(counterTextBox.Text);
            if (counter > 0)
            {
                counter--;
            }
            counterTextBox.Text = counter.ToString();
        }

        private void PlusButton_Click(object sender, RoutedEventArgs e)
        {
            int counter = int.Parse(counterTextBox.Text);
            counter++;
            counterTextBox.Text = counter.ToString();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            // Implement the logic for the Add button here
            throw new NotImplementedException();
        }
    }
}