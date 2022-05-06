using System;
using System.Linq;
using System.Windows;
using System.Windows.Media.Imaging;

namespace Candy_shop
{
    /// <summary>
    /// Логика взаимодействия для MenuDirector.xaml
    /// </summary>
    public partial class MenuDirector : Window
    {
        //db
        readonly CandyShopEntities _context = new CandyShopEntities();

        public MenuDirector()
        {
            InitializeComponent();

            //строка с текущим пользователем из БД
            var userInfo = _context.Workers.Where(o => o.WorkerCode == MainWindow.codeAuthUser).FirstOrDefault();

            lNameLabel.Content = userInfo.LastName;
            fNameLabel.Content = userInfo.FirstName;
            mNameLabel.Content = userInfo.MiddleName;
            phoneLabel.Content = userInfo.Phone;
            postLabel.Content = userInfo.Post;
            birthdayLabel.Content = userInfo.Birthday.ToShortDateString();

            if (userInfo.Photo != null)
            {
                image.ImageSource = new BitmapImage(new Uri(userInfo.Photo));
            }
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
