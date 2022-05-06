using System.Linq;
using System.Windows;
using System.Windows.Media;
using static Candy_shop.ValidateClass;
using static Candy_shop.UsefulFunctions;
using System;

namespace Candy_shop
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //если что есть один админ сразу pass = 1234, code = А-001

        //string
        public static string codeAuthUser;

        //int
        private int countOfHoversOnCode = 0;
        private int countOfHoversOnPassword = 0;

        //db
        readonly CandyShopEntities _context = new CandyShopEntities();

        public MainWindow()
        {
            InitializeComponent();

            //если мы запускаем приложение впервые нужно создать админа
            if (_context.Workers.FirstOrDefault() == null)
            {
                _context.Workers.Add(new Workers
                {
                    WorkerCode = "А-001",
                    WorkerPassword = Hash("1234"),
                    LastName = "Админ",
                    FirstName = "Админ",
                    MiddleName = "Админ",
                    Post = "Администратор",
                    Phone = "+79000000000",
                    Birthday = Convert.ToDateTime("01.01.2000")
                });

                // Сохранить изменения в базе данных
                _context.SaveChanges();
            }
        }

        private void AuthButton_Click(object sender, RoutedEventArgs e)
        {
            //проверка на заполнение полей
            if (StringNotEmpty(code1TextBox.Text) && StringNotEmpty(code2TextBox.Text) && StringNotEmpty(passwordTextBox.Password))
            {
                //проверка длины
                if (code1TextBox.Text.Length == 1 && code2TextBox.Text.Length == 3)
                {
                    string code = $"{code1TextBox.Text}-{code2TextBox.Text}";

                    bool codeExist = false;

                    //находим нужный код
                    foreach (Workers item in _context.Workers.ToList())
                    {
                        if (item.WorkerCode.ToString() == code)
                        {
                            codeExist = true;

                            //проверяем введенный пароль (а точнее хэш)
                            if (item.WorkerPassword.ToString() == Hash(passwordTextBox.Password))
                            {
                                codeAuthUser = code;

                                if (item.Post == "Администратор")
                                {
                                    MenuDirector menuDirector = new MenuDirector();
                                    menuDirector.Show();
                                    Close();
                                }
                                else
                                {
                                    MenuSaler menuSaler = new MenuSaler();
                                    menuSaler.Show();
                                    Close();
                                }
                            }
                            else
                            {
                                MsgView("Пароль неправильный");
                            }

                            break;
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
                passwordTextBox.Password = string.Empty;
                countOfHoversOnPassword++;
            }
        }
    }
}