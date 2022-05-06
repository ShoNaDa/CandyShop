using System.Windows;

namespace Candy_shop
{
    /// <summary>
    /// Логика взаимодействия для GetCode.xaml
    /// </summary>
    public partial class GetCode : Window
    {
        public GetCode()
        {
            InitializeComponent();

            uniqCodeLabel.Content = AddWorker.uniqueCode;
        }

        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            MenuDirector menuDirector = new MenuDirector();
            menuDirector.Show();
            Close();
        }
    }
}
