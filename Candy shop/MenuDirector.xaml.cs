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

            //заполняем список сотрудников
            WorkersListBox.Items.Clear();

            foreach (var item in _context.Workers.ToList())
            {
                WorkersListBox.Items.Add($"{item.WorkerCode}  |  {item.LastName} {item.FirstName} {item.MiddleName}");
            }

            //событие двойного нажатия мыши на ListBox
            WorkersListBox.MouseDoubleClick += new MouseButtonEventHandler(WorkersListBox_DoubleClick);

            //заполняем список продуктов
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
                string codeSelectWorker = WorkersListBox.SelectedValue.ToString().Split('|')[0].Trim();

                //находим выбранного сотрудника в БД
                Workers worker = _context.Workers.ToList().FirstOrDefault(o => o.WorkerCode == codeSelectWorker);

                //удаляем
                _context.Workers.Remove(worker);
                _context.SaveChanges();

                OpenWindow(new MenuDirector(), this);
            }
            else
            {
                MsgView("Выберите сотрудника из списка");
            }
        }

        //добавить товар
        private void AddProductButton_Click(object sender, RoutedEventArgs e)
        {
            OpenWindow(new AddProduct(true, null), this);
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

        //дальше отчеты
        private void ShiftsReportButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ShiftInDateReportButton_Click(object sender, RoutedEventArgs e)
        {
            if (DateOfShiftDatePicker.SelectedDate != null)
            {

            }
        }

        private void ShiftsWithWorkerReportButton_Click(object sender, RoutedEventArgs e)
        {
            if (WorkersComboBox.SelectedIndex != -1)
            {

            }
        }

        private void ShiftsWithProductReportButton_Click(object sender, RoutedEventArgs e)
        {
            if (ProductsComboBox.SelectedIndex != -1)
            {

            }
        }

        private void MoneyOperationsReportButton_Click(object sender, RoutedEventArgs e)
        {

        }
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