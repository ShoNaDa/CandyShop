using System.Text.RegularExpressions;

namespace Candy_shop
{
    internal class ValidateClass
    {
        //функция проверки на маленькие буквы всех букв, кроме первой
        static internal bool CheckingForLetters(string _str)
        {
            bool isLetter = true;

            foreach (char item in _str.Substring(1))
            {
                if (Regex.IsMatch(item.ToString(), @"[а-я]") == false)
                {
                    isLetter = false;
                }
            }

            return isLetter;
        }

        //функция проверки на цифры
        static internal bool CheckingForNumbers(string _str)
        {
            bool isNum = true;

            foreach(char item in _str)
            {
                if (Regex.IsMatch(item.ToString(), @"[0-9]") == false)
                {
                    isNum = false;
                }
            }

            return isNum;
        }

        //функция проверки на float
        static internal bool CheckingForNumbersWithPoint(string _str)
        {
            bool isNum = true;
            bool isPoint = false;

            //проверка, чтоб не было ничего кроме цифр и точки
            foreach (char item in _str)
            {
                if ((Regex.IsMatch(item.ToString(), @"[0-9]") == false) && item != ',')
                {
                    isNum = false;
                }

                //если попалась точка, но уже не первая
                if (item == ',' && isPoint == true)
                {
                    isNum = false;
                    break;
                }

                //если попалась точка
                if (item == ',' && isPoint == false)
                {
                    isPoint = true;
                }
            }

            //проверка, чтоб точка не начиналась вначале и не было ее в конце
            if (_str[0] == ',' || _str[_str.Length - 1] == ',')
            {
                isNum = false;
            }

            return isNum;
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
    }
}
