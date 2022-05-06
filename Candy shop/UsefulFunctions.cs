using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Candy_shop
{
    internal class UsefulFunctions
    {
        //string
        static public string message = string.Empty;

        //функция для msg
        static internal void MsgView(string _str)
        {
            message = _str;

            MessageWindow messageWindow = new MessageWindow();
            messageWindow.ShowDialog();
        }

        //функция хэширования
        public static string Hash(string password)
        {
            //переводим строку в байт-массим  
            byte[] bytes = Encoding.Unicode.GetBytes(password);

            //создаем объект для получения средст шифрования  
            MD5CryptoServiceProvider CSP = new MD5CryptoServiceProvider();

            //вычисляем хеш-представление в байтах  
            byte[] byteHash = CSP.ComputeHash(bytes);

            //создаем СтрингБилдер для того, чтобы работать со строкой в цикле (так быстрее)
            var hash = new StringBuilder();

            //формируем одну цельную строку из массива  
            foreach (byte b in byteHash)
            {
                hash.Append(string.Format("{0:x2}", b));
            }
            return hash.ToString();
        }
    }
}
