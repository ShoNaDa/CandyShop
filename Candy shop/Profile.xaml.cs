using System;
using System.Linq;
using System.Windows;
using System.Windows.Media.Imaging;

namespace Candy_shop
{
    /// <summary>
    /// Логика взаимодействия для Profile.xaml
    /// </summary>
    public partial class Profile : Window
    {
        //db
        readonly CandyShopEntities _context = new CandyShopEntities();

        public Profile(string code)
        {
            InitializeComponent();

            //строка с текущим пользователем из БД
            var userInfo = _context.Workers.Where(o => o.WorkerCode == code).FirstOrDefault();

            //заполняем профиль
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

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            MenuDirector menuDirector = new MenuDirector();
            menuDirector.Show();
            Close();
        }
    }
}
