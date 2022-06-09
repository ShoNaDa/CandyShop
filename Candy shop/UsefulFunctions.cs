using Microsoft.Win32;
using Excel = Microsoft.Office.Interop.Excel;
using System.Windows.Input;
using System.Windows.Controls.Primitives;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using System.Collections.Generic;

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

        //функция открытия окна
        public static void OpenWindow(Window windowToOpen, Window windowToClose)
        {
            windowToOpen.Show();
            windowToClose.Close();
        }

        //функция создания Excel файла
        public static void CreateExcelFile(string fileName, uint countOfHeader,
            Dictionary<char, string> dictOfHeaders, List<List<string>> listWithInfo)
        {
            //создание диалоговое окно для создания файла excel
            SaveFileDialog excelFile = new SaveFileDialog
            {
                FileName = fileName,
                Filter = "Excel files |*.xlsx"
            };

            if (excelFile.ShowDialog() == true)
            {
                //создаем приложение
                Excel.Application app = new Excel.Application();

                //создаем книгу
                Excel.Workbook workbook = app.Workbooks.Add();

                //создаем лист
                Excel.Worksheet worksheet = app.Worksheets[1];

                //это лист с названиями ячеек заголовков (A, B, C...)
                List<char> listOfKeys = new List<char>();

                //Создаем заголовки
                worksheet.Name = fileName;

                foreach (var item in dictOfHeaders)
                {
                    worksheet.Range[$"{item.Key}1"].Value = item.Value;

                    listOfKeys.Add(item.Key);
                }

                //заполняем построчно Excel файл
                for (int i = 0; i < listWithInfo.Count; i++)
                {
                    int j = 0;

                    //заполняем строку файла
                    foreach (List<string> item in listWithInfo)
                    {
                        worksheet.Range[$"{listOfKeys[j]}{i + 2}"].Value = item[j];

                        j++;
                    }
                }

                //ставим ширину по дефолту в соответствии с содержимым
                for (int i = 1; i < listOfKeys.Count; i++)
                {
                    worksheet.Columns[i].AutoFit();
                }

                //ставим выравнивание заголовков по центру и жирным шрифтом
                worksheet.get_Range($"{listOfKeys[0]}1", $"{listOfKeys[listOfKeys.Count - 1]}1").Cells.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                worksheet.get_Range($"{listOfKeys[0]}1", $"{listOfKeys[listOfKeys.Count - 1]}1").Cells.Font.Bold = true;

                //сохраняем файл
                workbook.SaveAs(excelFile.FileName);
                workbook.Close();
                app.Quit();
            }
        }
    }
}
