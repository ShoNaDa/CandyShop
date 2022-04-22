using System.Windows;

namespace Candy_shop
{
    /// <summary>
    /// Логика взаимодействия для MenuDirector.xaml
    /// </summary>
    public partial class MenuDirector : Window
    {
        public MenuDirector()
        {
            InitializeComponent();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            AddWorker addWorker = new AddWorker();
            addWorker.Show();
            Close();
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void AddProductButton_Click(object sender, RoutedEventArgs e)
        {
            AddProduct addProduct = new AddProduct();
            addProduct.Show();
            Close();
        }

        private void DeleteProductButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void EditProductButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
