using System.Text.RegularExpressions;

namespace Candy_shop
{
    internal class ValidateClass
    {
        //string
        static public string message = string.Empty;

        //функция проверки на маленькие буквы всех букв, кроме первой
        static internal bool CheckingForLetters(string _str)
        {
            return Regex.IsMatch(_str.Substring(1), @"[а-я]");
        }

        //функция проверки на цифры
        static internal bool CheckingForNumbers(string _str)
        {
            return Regex.IsMatch(_str, @"[0-9]");
        }

        //функция проверки на заполнение
        static internal bool StringNotEmpty(string _str)
        {
            return _str != string.Empty && _str != "" && _str.Trim() != "";
        }

        //функция проверки на заглавную первую букву
        static internal bool FirstLetterIsLarge(string _str)
        {
            return Regex.IsMatch(_str[0].ToString(), @"[А-Я]");
        }

        //функция для msg
        static internal void MsgView(string _str)
        {
            message = _str;

            MessageWindow messageWindow = new MessageWindow();
            messageWindow.ShowDialog();
        }
    }
}
