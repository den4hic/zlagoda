using System.Collections.ObjectModel;
using System.Windows;
using SupermarketDAL.Entities;
using SupermarketPL.Model;

namespace SupermarketPL.Views
{
    public partial class ManagerAddGoodView : Window
    {
        private ObservableCollection<Goods> goodsList { get; set; }
        private ObservableCollection<Category> categoriesList { get; set; }
        private ManagerController controller;
        private ObservableCollection<string> categoriesNameList { get; set; }
        public ManagerAddGoodView( ObservableCollection<Goods> goodsList,ObservableCollection<Category> categoriesList , ManagerController controller,ObservableCollection<string> categoriesNameList)
        {
            InitializeComponent();
            this.goodsList = goodsList;
            this.categoriesList = categoriesList;
            this.controller = controller;
            this.categoriesNameList = categoriesNameList;
            categoryComboBox.ItemsSource = categoriesNameList;
        }





        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            int categorynumber = categoryComboBox.SelectedIndex;
            string name = nameTextBox.Text;
            string producer = manufacturerTextBox.Text;
            string characteristics = characteristicsTextBox.Text;
            if (categorynumber == -1 || string.IsNullOrEmpty(name) || string.IsNullOrEmpty(producer) || string.IsNullOrEmpty(characteristics))
            {
                throw new ArgumentException("All fields must be filled in.");
            }

            Goods newGood = new Goods
            {
                CategoryId = categorynumber,
                Name = name,
                Manufacturer = producer,
                Characteristics = characteristics
                
            };
            goodsList.Add(newGood);
            controller.InsertGood(categorynumber,name, producer, characteristics);
            MessageBox.Show("Good added successfully!");
            this.Close();
        }
    }
}