using System.Linq;
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
        //если что есть один админ сразу pass = 1234, code = А-001

        //int
        private int countOfHoversOnCode = 0;
        private int countOfHoversOnPassword = 0;

        //db
        readonly CandyShopEntities _context = new CandyShopEntities();

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
                    string code = code1TextBox.Text + "-" + code2TextBox.Text;

                    //таблица Workers
                    var workers = _context.Workers;

                    bool codeExist = false;

                    //находим нужный код
                    if (workers.FirstOrDefault() != null)
                    {
                        foreach (var item in workers.FirstOrDefault().WorkerCode.ToList())
                        {
                            if (item.ToString() == code)
                            {
                                codeExist = true;

                                //проверяем введенный пароль
                                if (workers.Where(o => o.WorkerCode.ToString() == code).FirstOrDefault().WorkerPassword.ToString() == passwordTextBox.Text)
                                {
                                    //TODO: надо сделать хэширование пароля, проверку хэшированного пароля, и сделать одного админа дэфолтного
                                    MenuDirector menuDirector = new MenuDirector();
                                    menuDirector.Show();
                                    Close();
                                }
                                else
                                {
                                    MsgView("Пароль неправильный");
                                }

                                break;
                            }
                        }
                    }

                    if (!codeExist)
                    {
                        MsgView("Введенный код не найден");
                    }
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
            if (countOfHoversOnCode == 0)
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
            if (countOfHoversOnPassword == 0)
            {
                passwordTextBox.Foreground = Brushes.Black;
                passwordTextBox.Text = string.Empty;
                countOfHoversOnPassword++;
            }
        }
    }
}