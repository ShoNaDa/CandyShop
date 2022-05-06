using System.Windows;

namespace Candy_shop
{
    /// <summary>
    /// Логика взаимодействия для MessageWindow.xaml
    /// </summary>
    public partial class MessageWindow : Window
    {
        public MessageWindow()
        {
            InitializeComponent();

            messageTextBlock.Text = UsefulFunctions.message;
        }

        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
