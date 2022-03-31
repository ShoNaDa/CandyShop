using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows;

namespace Candy_shop
{
    /// <summary>
    /// Логика взаимодействия для AddWorker.xaml
    /// </summary>
    public partial class AddWorker : Window
    {
        //List
        //List<char> Alphabetic = new List<char>() { 'а', '' };
        //string
        static public string uniqueCode = string.Empty;

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
            //проверка на заполнение полей
            if (StringNotEmpty(lNameTextBox.Text) && StringNotEmpty(fNameTextBox.Text) && StringNotEmpty(mNameTextBox.Text)
                && StringNotEmpty(postComboBox.Text))
            {
                //проверка на заглавную букву
                if (FirstLetterIsLarge(lNameTextBox.Text) && FirstLetterIsLarge(fNameTextBox.Text) && FirstLetterIsLarge(mNameTextBox.Text))
                {
                    //создаем уникальный код
                    uniqueCode = postComboBox.Text[0].ToString();

                    // TODO: надо проверять БД
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
                    MessageBox.Show("Поля в ФИО должны начинаться в заглавной буквы");
                }
            }
            else
            {
                MessageBox.Show("Все обязательные поля должны быть заполнены");
            }
        }

        //функция проверки на заполнение
        private bool StringNotEmpty(string _str)
        {
            return _str != string.Empty && _str != "" && _str.Trim() != "" ? true : false;
        }

        //функция проверки на заглавную первую букву
        private bool FirstLetterIsLarge(string _str)
        {
            return Regex.IsMatch(_str[0].ToString(), @"[А-Я]");
        }
    }
}
