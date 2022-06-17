using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using static Candy_shop.ValidateClass;
using static Candy_shop.UsefulFunctions;
using System.Windows.Input;

namespace Candy_shop
{
    /// <summary>
    /// Логика взаимодействия для MenuDirector.xaml
    /// </summary>
    public partial class MenuDirector : Window
    {
        //db
        readonly CandyShopEntities _context = new CandyShopEntities();

        public MenuDirector()
        {
            InitializeComponent();

            //заполняем списки сотрудников
            WorkersListBox.Items.Clear();

            foreach (var item in _context.Workers.ToList())
            {
                WorkersListBox.Items.Add($"{item.WorkerCode}  |  {item.LastName} {item.FirstName} {item.MiddleName}");

                if (item.WorkerCode.ToString().Substring(0, 1) == "Р")
                {
                    WorkersComboBox.Items.Add($"{item.WorkerCode}  |  {item.LastName} {item.FirstName} {item.MiddleName}");
                }
            }

            //событие двойного нажатия мыши на ListBox
            WorkersListBox.MouseDoubleClick += new MouseButtonEventHandler(WorkersListBox_DoubleClick);

            //заполняем списки продуктов
            List<ProductsData> products = new List<ProductsData>();

            foreach (Products item in _context.Products.ToList())
            {
                products.Add(new ProductsData()
                {
                    productID = item.ProductID,
                    nameOfProduct = item.NameOfProduct,
                    price = Convert.ToDouble(item.Price),
                    nameOfManufacturer = _context.Manufacturers.ToList().FirstOrDefault(o => o.ManufacturerID == item.ManufacturerID_FK).NameOfManufacturer,
                    expirationDate = item.ExpirationDate
                });

                ProductsComboBox.Items.Add($"#{item.ProductID} {item.NameOfProduct}");
            }
            ProductsDataGrid.ItemsSource = products;

            //заполняем список производителей
            ManufacturerListBox.Items.Clear();

            foreach (var item in _context.Manufacturers.ToList())
            {
                ManufacturerListBox.Items.Add($"{item.NameOfManufacturer}");
            }

            //заполняем остаток в кассе
            MoneyInCashRegisterLabel.Content = Convert.ToDouble(_context.CashRegister.ToList().FirstOrDefault().MoneyInCashRegister).ToString() + " руб";
        }

        #region сотрудники
        //добавить сотрудника
        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            OpenWindow(new AddWorker(), this);
        }

        //удалить сотрудника
        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (WorkersListBox.SelectedIndex != -1)
            {
                bool isMayDelete = true;

                //проверка, чтоб не удалили всех админов
                if (WorkersListBox.SelectedValue.ToString().Substring(0, 1) == "А" && _context.Workers.ToList().Where(o => o.Post == "Администратор").ToList().Count == 1)
                {
                    isMayDelete = false;

                    MsgView("Нельзя удалить всех администраторов");
                }
                if (isMayDelete)
                {
                    //теперь нужно удалить все ключи, а затем удалить сотрудника
                    string codeSelectWorker = WorkersListBox.SelectedValue.ToString().Split('|')[0].Trim();

                    //находим выбранного сотрудника в БД
                    Workers worker = _context.Workers.ToList().FirstOrDefault(o => o.WorkerCode == codeSelectWorker);

                    //удаляем
                    _context.Workers.Remove(worker);
                    _context.SaveChanges();

                    OpenWindow(new MenuDirector(), this);
                }
            }
            else
            {
                MsgView("Выберите сотрудника из списка");
            }
        }

        //событие двойного нажатия на сотрудника
        private void WorkersListBox_DoubleClick(object sender, EventArgs e)
        {
            if (WorkersListBox.SelectedItem != null)
            {
                string selectedWorkerCode = WorkersListBox.SelectedItem.ToString().Split('|')[0].Trim();

                OpenWindow(new Profile(selectedWorkerCode), this);
            }
        }
        #endregion

        #region товары
        //добавить товар
        private void AddProductButton_Click(object sender, RoutedEventArgs e)
        {
            OpenWindow(new AddProduct(true, null), this);
        }

        //удалить товар
        private void DeleteProductButton_Click(object sender, RoutedEventArgs e)
        {
            if (ProductsDataGrid.SelectedIndex != -1)
            {
                //получаем строку, которая была выделена
                ProductsData selectedString = (ProductsData)ProductsDataGrid.SelectedItem;

                //находим выбранный продукт в БД
                Products product = _context.Products.ToList().FirstOrDefault(o => o.ProductID == selectedString.productID);

                //удаляем
                _context.Products.Remove(product);
                _context.SaveChanges();

                OpenWindow(new MenuDirector(), this);
            }
            else
            {
                MsgView("Выберите товар из списка");
            }
        }

        //редактировать товар
        private void EditProductButton_Click(object sender, RoutedEventArgs e)
        {
            if (ProductsDataGrid.SelectedIndex != -1)
            {
                //получаем строку, которая была выделена
                ProductsData selectedString = (ProductsData)ProductsDataGrid.SelectedItem;

                //находим выбранный продукт в БД
                Products product = _context.Products.ToList().FirstOrDefault(o => o.ProductID == selectedString.productID);

                OpenWindow(new AddProduct(false, product), this);
            }
            else
            {
                MsgView("Выберите товар из списка");
            }
        }
        #endregion

        #region производители
        //добавить производителя
        private void AddManufacturerButton_Click(object sender, RoutedEventArgs e)
        {
            //проверка на заполнение полей
            if (StringNotEmpty(ManufacturerTextBox.Text))
            {
                //проверка на заглавную букву
                if (FirstLetterIsLarge(ManufacturerTextBox.Text))
                {
                    //добавление в БД
                    _context.Manufacturers.Add(new Manufacturers
                    {
                        NameOfManufacturer = ManufacturerTextBox.Text
                    });
                    // Сохранить изменения в базе данных
                    _context.SaveChanges();

                    OpenWindow(new MenuDirector(), this);
                }
                else
                {
                    MsgView("Поле 'Название' должно начинаться в заглавной буквы и написано кириллицей");
                }
            }
            else
            {
                MsgView("Поле должно быть заполнено");
            }
        }

        //удалить производителя
        private void DeleteManufacturerButton_Click(object sender, RoutedEventArgs e)
        {
            if (ManufacturerListBox.SelectedIndex != -1)
            {
                //находим выбранного сотрудника в БД
                Manufacturers manufacturer = _context.Manufacturers.ToList()
                    .FirstOrDefault(o => o.NameOfManufacturer == ManufacturerListBox.SelectedValue.ToString());

                //удаляем
                _context.Manufacturers.Remove(manufacturer);
                _context.SaveChanges();

                OpenWindow(new MenuDirector(), this);
            }
            else
            {
                MsgView("Выберите производителя из списка");
            }
        }
        #endregion

        //операции в кассе
        private void PerformOperationButton_Click(object sender, RoutedEventArgs e)
        {
            //проверка на заполнение полей
            if (StringNotEmpty(DepositTextBox.Text) && StringNotEmpty(WithdrawalTextBox.Text))
            {
                //проверка правильности написания возвратов и внесений
                if (CheckingForNumbersWithPoint(DepositTextBox.Text) &&
                    CheckingForNumbersWithPoint(WithdrawalTextBox.Text))
                {
                    //проверка на то, чтобы в кассе не осталось меньше нуля в кассе
                    if ((_context.CashRegister.ToList().FirstOrDefault().MoneyInCashRegister +
                        Convert.ToDecimal(DepositTextBox.Text) - Convert.ToDecimal(WithdrawalTextBox.Text)) >= 0)
                    {
                        //добавление внесения и изъятия в БД
                        _context.MoneyTransactions.Add(new MoneyTransactions
                        {
                            CashRegisterID_FK = _context.CashRegister.ToList().FirstOrDefault().CashRegisterID,
                            WorkerID_FK = _context.Workers.ToList().FirstOrDefault(o => o.WorkerCode == MainWindow.codeAuthUser).WorkerID,
                            Deposits = Convert.ToDecimal(DepositTextBox.Text),
                            Withdrawals = Convert.ToDecimal(WithdrawalTextBox.Text),
                            DateOfOperation = DateTime.Now
                        });
                        // Сохранить изменения в базе данных
                        _context.SaveChanges();

                        //изменяем деньги в кассе
                        CashRegister cashRegister = _context.CashRegister.ToList().FirstOrDefault();
                        cashRegister.MoneyInCashRegister += Convert.ToDecimal(DepositTextBox.Text) - Convert.ToDecimal(WithdrawalTextBox.Text);

                        // Сохранить изменения в базе данных
                        _context.SaveChanges();

                        OpenWindow(new MenuDirector(), this);
                    }
                    else
                    {
                        MsgView("Нельзя, чтобы в кассе было меньше нуля");
                    }
                }
                else
                {
                    MsgView("В полях 'Внесение' и 'Изъятие' допущены ошибки");
                }
            }
            else
            {
                MsgView("Все поля должны быть заполнены (если нужна только одна операция поставьте нуль '0' во второй)");
            }
        }

        //выход
        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            OpenWindow(new MainWindow(), this);
        }

        #region отчеты
        //отчет по всем сменам
        private void ShiftsReportButton_Click(object sender, RoutedEventArgs e)
        {
            List<List<string>> listWithInfo = new List<List<string>>();

            //создаем список для отчета
            foreach (var item in _context.Shifts.ToList())
            {
                listWithInfo.Add(new List<string> {
                    item.ShiftID.ToString(),
                    item.ShiftDate.ToShortDateString().ToString(),
                    _context.Workers.ToList().FirstOrDefault(o => o.WorkerID == item.WorkerID_FK).LastName + " " +
                    _context.Workers.ToList().FirstOrDefault(o => o.WorkerID == item.WorkerID_FK).FirstName + " " +
                    _context.Workers.ToList().FirstOrDefault(o => o.WorkerID == item.WorkerID_FK).MiddleName,
                    Convert.ToDouble(item.ArriveOfMoney).ToString(),
                    Convert.ToDouble(item.FlowOfNomey).ToString(),
                    Convert.ToDouble(item.MoneyAtStartShift).ToString() });
            }

            //создаем сам отчет
            CreateExcelFile("Отчет по всем сменам", 6,
                new Dictionary<char, string> { { 'A',"Смена №" }, { 'B', "Дата" }, { 'C', "Сотрудник" }, { 'D', "Приход денег" },
                  { 'E', "Расход денег" }, { 'F', "Денег в начале смены" } }, listWithInfo);
        }

        //отчет по дате
        private void ShiftInDateReportButton_Click(object sender, RoutedEventArgs e)
        {
            if (DateOfShiftDatePicker.SelectedDate != null)
            {
                //получаем номер смены по дате
                int numberOfShift = _context.Shifts.ToList().FirstOrDefault(o => o.ShiftDate.ToShortDateString() ==
                    Convert.ToDateTime(DateOfShiftDatePicker.SelectedDate).ToShortDateString()).ShiftID;

                List<List<string>> listWithInfo = new List<List<string>>();

                //я слабый, потому что не придумал, как это покороче написать
                int[] massCounts = new int[3] {
                    _context.SalesOfProducts.ToList().Where(o => o.ShiftID_FK == numberOfShift).Count(),
                    _context.ReceiptsOfProducts.ToList().Where(o => o.ShiftID_FK == numberOfShift).Count(),
                    _context.WritesOffOfProducts.ToList().Where(o => o.ShiftID_FK == numberOfShift).Count() };

                int maxCount = 0;
                for (int i = 0; i < massCounts.Length; i++)
                {
                    maxCount = maxCount < massCounts[i] ? massCounts[i] : maxCount;
                }

                //создаем список для отчета
                foreach (var item in _context.Warehouses.ToList())
                {
                    listWithInfo.Add(new List<string> {
                    _context.Workers.ToList().FirstOrDefault(o => o.WorkerID == _context.Shifts.ToList().FirstOrDefault(p => p.ShiftID == numberOfShift).WorkerID_FK).LastName + " " +
                    _context.Workers.ToList().FirstOrDefault(o => o.WorkerID == _context.Shifts.ToList().FirstOrDefault(p => p.ShiftID == numberOfShift).WorkerID_FK).FirstName + " " +
                    _context.Workers.ToList().FirstOrDefault(o => o.WorkerID == _context.Shifts.ToList().FirstOrDefault(p => p.ShiftID == numberOfShift).WorkerID_FK).MiddleName,
                    _context.Shifts.ToList().FirstOrDefault(o => o.ShiftID == numberOfShift).ArriveOfMoney.ToString(),
                    _context.Shifts.ToList().FirstOrDefault(o => o.ShiftID == numberOfShift).FlowOfNomey.ToString(),
                    _context.Shifts.ToList().FirstOrDefault(o => o.ShiftID == numberOfShift).MoneyAtStartShift.ToString(),
                    _context.Products.ToList().FirstOrDefault(o => o.ProductID == item.ProductID_FK).NameOfProduct,
                    _context.ReceiptsOfProducts.ToList().Where(o => o.ShiftID_FK == numberOfShift && o.ProductID_FK == item.ProductID_FK).Sum(p => p.CountOfProducts).ToString(),
                    _context.SalesOfProducts.ToList().Where(o => o.ShiftID_FK == numberOfShift && o.ProductID_FK == item.ProductID_FK).Sum(p => p.CountOfProducts).ToString(),
                    _context.WritesOffOfProducts.ToList().Where(o => o.ShiftID_FK == numberOfShift && o.ProductID_FK == item.ProductID_FK).Sum(p => p.CountOfProducts).ToString()});
                }

                //создаем сам отчет
                DateTime date = (DateTime)DateOfShiftDatePicker.SelectedDate;

                CreateExcelFile($"Отчет по смене №{numberOfShift} {date.ToShortDateString()}", 8,
                    new Dictionary<char, string> { { 'A',"Сотрудник" }, { 'B', "Приход денег" }, { 'C', "Расход денег" }, { 'D', "Денег в начале смены" },
                        { 'E', "Товар" }, { 'F', "Поступления (кол.)" }, { 'G', "Продажи (кол.)" }, { 'H', "Списания (кол.)" } },
                        listWithInfo);
            }
        }

        //отчет по сотруднику
        private void ShiftsWithWorkerReportButton_Click(object sender, RoutedEventArgs e)
        {
            if (WorkersComboBox.SelectedIndex != -1)
            {
                //получаем ID сотрудника
                int idWorker = _context.Workers.ToList().FirstOrDefault(o => o.WorkerCode == WorkersComboBox.Text.Split('|')[0].Trim()).WorkerID;

                List<List<string>> listWithInfo = new List<List<string>>();

                //создаем список для отчета
                foreach (var item in _context.Shifts.ToList().Where(o => o.WorkerID_FK == idWorker).ToList())
                {
                    listWithInfo.Add(new List<string> {
                    item.ShiftID.ToString(),
                    item.ShiftDate.ToShortDateString().ToString(),
                    Convert.ToDouble(item.ArriveOfMoney).ToString(),
                    Convert.ToDouble(item.FlowOfNomey).ToString(),
                    Convert.ToDouble(item.MoneyAtStartShift).ToString() });
                }

                //создаем сам отчет
                CreateExcelFile($"Отчет {WorkersComboBox.Text.Split('|')[0].Trim()}", 5,
                    new Dictionary<char, string> { { 'A',"Смена №" }, { 'B', "Дата" }, { 'C', "Приход денег" },
                        { 'D', "Расход денег" }, { 'E', "Денег в начале смены" } }, listWithInfo);
            }
        }

        //отчет по товару
        private void ShiftsWithProductReportButton_Click(object sender, RoutedEventArgs e)
        {
            if (ProductsComboBox.SelectedIndex != -1)
            {
                //получаем ID товара
                int idProduct = Convert.ToInt32(ProductsComboBox.Text.Split('#')[1].Split(' ')[0].Trim());

                List<List<string>> listWithInfo = new List<List<string>>();

                //создаем список для отчета
                //сначала поступления
                foreach (var item in _context.ReceiptsOfProducts.ToList().Where(o => o.ProductID_FK == idProduct).ToList())
                {
                    listWithInfo.Add(new List<string> {
                    item.ShiftID_FK.ToString(),
                    "Поступление",
                    item.CountOfProducts.ToString(), 
                    null });
                }
                
                //потом продажи
                foreach (var item in _context.SalesOfProducts.ToList().Where(o => o.ProductID_FK == idProduct).ToList())
                {
                    listWithInfo.Add(new List<string> {
                    item.ShiftID_FK.ToString(),
                    "Продажа",
                    item.CountOfProducts.ToString(),
                    null });
                }
                
                //и в завершении списания
                foreach (var item in _context.WritesOffOfProducts.ToList().Where(o => o.ProductID_FK == idProduct).ToList())
                {
                    listWithInfo.Add(new List<string> {
                    item.ShiftID_FK.ToString(),
                    "Списание",
                    item.CountOfProducts.ToString(),
                    item.Info});
                }

                //создаем сам отчет
                CreateExcelFile($"Отчет {_context.Products.ToList().FirstOrDefault(o => o.ProductID == idProduct).NameOfProduct}", 4,
                    new Dictionary<char, string> { { 'A',"Смена №" }, { 'B', "Операция" }, { 'C', "Количество" }, { 'D', "Доп инфо (если есть)" } }, listWithInfo);
            }
        }

        //отчет по транзакциям
        private void MoneyOperationsReportButton_Click(object sender, RoutedEventArgs e)
        {
            List<List<string>> listWithInfo = new List<List<string>>();

            //создаем список для отчета
            foreach (var item in _context.MoneyTransactions.ToList())
            {
                listWithInfo.Add(new List<string> {
                    item.DateOfOperation.ToShortDateString().ToString(),
                    _context.Workers.ToList().FirstOrDefault(o => o.WorkerID == item.WorkerID_FK).LastName + " " +
                    _context.Workers.ToList().FirstOrDefault(o => o.WorkerID == item.WorkerID_FK).FirstName + " " +
                    _context.Workers.ToList().FirstOrDefault(o => o.WorkerID == item.WorkerID_FK).MiddleName,
                    Convert.ToDouble(item.Deposits).ToString(),
                    Convert.ToDouble(item.Withdrawals).ToString() });
            }

            //создаем сам отчет
            CreateExcelFile("Отчет по всем транзакциям", 4,
                new Dictionary<char, string> { { 'A', "Дата" }, { 'B', "Сотрудник" }, { 'C', "Внесения" }, { 'D', "Изъятия" } }, listWithInfo);
        }
        #endregion
    }

    internal class ProductsData
    {
        public int productID { get; set; }
        public string nameOfProduct { get; set; }
        public double price { get; set; }
        public string nameOfManufacturer { get; set; }
        public int expirationDate { get; set; }
    }
}