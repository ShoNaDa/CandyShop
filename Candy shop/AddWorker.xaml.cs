using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Candy_shop
{
    /// <summary>
    /// Логика взаимодействия для AddWorker.xaml
    /// </summary>
    public partial class AddWorker : Window
    {
        public AddWorker()
        {
            InitializeComponent();
        }

        private void editPhotoButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void CancalButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void AddWorkerButton_Click(object sender, RoutedEventArgs e)
        {
            GetCode getCode = new GetCode();
            getCode.Show();
            Close();
        }
    }
}
