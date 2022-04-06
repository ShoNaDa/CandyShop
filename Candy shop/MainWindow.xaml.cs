using System.Windows;
using System.Windows.Media;
using static Candy_shop.ValidateClass;

namespace Candy_shop
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //int
        private int countOfHoversOnCode = 0;
        private int countOfHoversOnPassword = 0;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void AuthButton_Click(object sender, RoutedEventArgs e)
        {
            //проверка на заполнение полей
            if (StringNotEmpty(code1TextBox.Text) && StringNotEmpty(code2TextBox.Text) && StringNotEmpty(passwordTextBox.Text))
            {
                //проверка длины
                if (code1TextBox.Text.Length == 1 && code2TextBox.Text.Length == 3)
                {
                    // TODO: надо проверять код и пароль

                    AddWorker addWorker = new AddWorker();
                    addWorker.Show();
                    Close();
                }
                else
                {
                    MsgView("Уникальный код сотрудника должен соответствовать шаблону 'X-XXX'");
                }
            }
            else
            {
                MsgView("Все поля должны быть заполнены");
            }
        }

        //при наведении на элементы меняем их цвет и очищаем текст
        private void Code1TextBox_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if(countOfHoversOnCode == 0)
            {
                code1TextBox.Foreground = Brushes.Black;
                code1TextBox.Text = string.Empty;
                code2TextBox.Foreground = Brushes.Black;
                code2TextBox.Text = string.Empty;
                countOfHoversOnCode++;
            }
        }

        private void PasswordTextBox_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if(countOfHoversOnPassword == 0)
            {
                passwordTextBox.Foreground = Brushes.Black;
                passwordTextBox.Text = string.Empty;
                countOfHoversOnPassword++;
            }
        }
    }
}
