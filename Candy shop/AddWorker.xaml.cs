using Microsoft.Win32;
using System;
using System.Windows;
using System.Windows.Media.Imaging;
using static Candy_shop.ValidateClass;
using static Candy_shop.UsefulFunctions;

namespace Candy_shop
{
    /// <summary>
    /// Логика взаимодействия для AddWorker.xaml
    /// </summary>
    public partial class AddWorker : Window
    {
        //string
        static public string uniqueCode = string.Empty;

        public AddWorker()
        {
            InitializeComponent();
        }

        private void EditPhotoButton_Click(object sender, RoutedEventArgs e)
        {
            //создаем диалоговое окно
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Открыть картинку|*.jpg"
            };

            //если диалоговое окно открыто, то присваиваем выбранную фотографию в рамку
            if (openFileDialog.ShowDialog() == true)
            {
                image.ImageSource = new BitmapImage(new Uri(openFileDialog.FileName));
            }
            
        }

        private void CancalButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void AddWorkerButton_Click(object sender, RoutedEventArgs e)
        {
            //проверка на заполнение полей
            if (StringNotEmpty(lNameTextBox.Text) && StringNotEmpty(fNameTextBox.Text) && StringNotEmpty(mNameTextBox.Text)
                && StringNotEmpty(postComboBox.Text) && StringNotEmpty(passwordTextBox.Text) && StringNotEmpty(repeatPasswordTextBox.Text)
                && StringNotEmpty(dayTextBox.Text) && StringNotEmpty(monthTextBox.Text) && StringNotEmpty(yearTextBox.Text))
            {
                //проверка на заглавную букву
                if (FirstLetterIsLarge(lNameTextBox.Text) && FirstLetterIsLarge(fNameTextBox.Text) && FirstLetterIsLarge(mNameTextBox.Text))
                {
                    //проверка на маленькие буквы
                    if (CheckingForLetters(lNameTextBox.Text) && CheckingForLetters(fNameTextBox.Text) && CheckingForLetters(mNameTextBox.Text)
                        && CheckingForLetters(postComboBox.Text) && CheckingForLetters(lNameTextBox.Text))
                    {
                        bool phoneIsNorm = true;

                        //проверка телефона, если он заполнен
                        if (CheckingForLetters(phone1TextBox.Text) || CheckingForLetters(phone2TextBox.Text)
                            || CheckingForLetters(phone3TextBox.Text) || CheckingForLetters(phone4TextBox.Text))
                        {
                            //проверка длины
                            if (phone1TextBox.Text.Length == 3 && phone2TextBox.Text.Length == 3
                                && phone3TextBox.Text.Length == 2 && phone1TextBox.Text.Length == 2)
                            {
                                //проверка на цифры
                                if (CheckingForNumbers(phone1TextBox.Text) && CheckingForNumbers(phone2TextBox.Text)
                                    && CheckingForNumbers(phone3TextBox.Text) && CheckingForNumbers(phone4TextBox.Text))
                                {
                                    phoneIsNorm = true;
                                }
                                else
                                {
                                    phoneIsNorm = false;
                                }
                            }
                            else
                            {
                                phoneIsNorm = false;
                            }
                        }

                        if (phoneIsNorm)
                        {
                            //пароли должны совпадать
                            if (repeatPasswordTextBox.Text == passwordTextBox.Text)
                            {
                                //проверка др
                                if (dayTextBox.Text.Length == 2 && monthTextBox.Text.Length == 2
                                && yearTextBox.Text.Length == 4)
                                {
                                    if (CheckingForNumbers(dayTextBox.Text) && CheckingForNumbers(monthTextBox.Text)
                                    && CheckingForNumbers(yearTextBox.Text))
                                    {
                                        // TODO: надо проверять БД

                                        //создаем уникальный код
                                        uniqueCode = postComboBox.Text[0].ToString();
                                        
                                        Random rnd = new Random();
                                        int rand = rnd.Next(0, 1000);

                                        if (rand.ToString().Length == 1)
                                        {
                                            uniqueCode += "—00" + rand.ToString();
                                        }
                                        else if (rand.ToString().Length == 2)
                                        {
                                            uniqueCode += "—0" + rand.ToString();
                                        }
                                        else
                                        {
                                            uniqueCode += "—" + rand.ToString();
                                        }

                                        GetCode getCode = new GetCode();
                                        getCode.Show();
                                        Close();
                                    }
                                    else
                                    {
                                        MsgView("Поле 'Дата рождения'должна состоять из цифр");
                                    }
                                }
                                else
                                {
                                    MsgView("Поле 'Дата рождения' имеет не верную длину");
                                }
                            }
                            else
                            {
                                MsgView("Поля 'Пароль' и 'Повторите пароль' должны совпадать");
                            }
                        }
                        else
                        {
                            MsgView("Телефон заполнен не верно");
                        }
                    }
                    else
                    {
                        MsgView("Поля в ФИО должны быть написаны кириллицей");
                    }
                }
                else
                {
                    MsgView("Поля в ФИО должны начинаться в заглавной буквы и написаны кириллицей");
                }
            }
            else
            {
                MsgView("Все обязательные поля должны быть заполнены");
            }
        }
    }
}