using System.Windows;
using static Candy_shop.ValidateClass;
using static Candy_shop.UsefulFunctions;
using System.Linq;
using System;
using System.Collections.Generic;

namespace Candy_shop
{
    /// <summary>
    /// Логика взаимодействия для MenuSaler.xaml
    /// </summary>
    public partial class MenuSaler : Window
    {
        //db
        readonly CandyShopEntities _context = new CandyShopEntities();

        //bool
        private static bool isFirstOpening = true;

        public MenuSaler()
        {
            InitializeComponent();

            //заполняем списки товаров в ComboBox
            ProductsComboBox.Items.Clear();

            foreach (var item in _context.Products.ToList())
            {
                ProductsComboBox.Items.Add(item.NameOfProduct);
            }

            ProductsForSaleComboBox.Items.Clear();
            ProductsListComboBox.Items.Clear();

            foreach (var item in _context.Warehouses.ToList())
            {
                ProductsForSaleComboBox.Items.Add(_context.Products.ToList().FirstOrDefault(o => o.ProductID == item.ProductID_FK).NameOfProduct);
                ProductsListComboBox.Items.Add(_context.Products.ToList().FirstOrDefault(o => o.ProductID == item.ProductID_FK).NameOfProduct);
            }

            //открываем смену в БД, если не вошел
            if (isFirstOpening)
            {
                _context.Shifts.Add(new Shifts
                {
                    WorkerID_FK = _context.Workers.ToList().FirstOrDefault(o => o.WorkerCode == MainWindow.codeAuthUser).WorkerID,
                    ArriveOfMoney = 0,
                    FlowOfNomey = 0,
                    MoneyAtStartShift = _context.CashRegister.ToList().FirstOrDefault().MoneyInCashRegister,
                    ShiftDate = DateTime.Now
                });
                //Сохранить изменения в базе данных
                _context.SaveChanges();

                isFirstOpening = false;
            }

            //заполняем смену
            shiftNumberLabel.Content = "Смена №" + _context.Shifts.ToList().Count;

            MoneyInCashRegisterLabel.Content = Convert.ToDouble(_context.CashRegister.ToList().FirstOrDefault().MoneyInCashRegister) + " руб";

            ArriveTextBlock.Text = $"{Convert.ToDouble(_context.Shifts.ToList().FirstOrDefault(o => o.ShiftID == _context.Shifts.ToList().Max(p => p.ShiftID)).ArriveOfMoney)} руб";

            FlowTextBlock.Text = $"{Convert.ToDouble(_context.Shifts.ToList().FirstOrDefault(o => o.ShiftID == _context.Shifts.ToList().Max(p => p.ShiftID)).FlowOfNomey)} руб";

            //заполняем товары
            List<ProductsDataForSaler> products = new List<ProductsDataForSaler>();

            foreach (Warehouses item in _context.Warehouses.ToList())
            {
                products.Add(new ProductsDataForSaler()
                {
                    productID = item.ProductID_FK,
                    nameOfProduct = _context.Products.ToList().FirstOrDefault(o => o.ProductID == item.ProductID_FK).NameOfProduct,
                    price = Convert.ToDouble(_context.Products.ToList().FirstOrDefault(o => o.ProductID == item.ProductID_FK).Price),
                    count = item.CountOfProducts,
                    nameOfManufacturer = _context.Manufacturers.ToList().FirstOrDefault(o => o.ManufacturerID == _context.Products.ToList().
                        FirstOrDefault(p => p.ProductID == item.ProductID_FK).ManufacturerID_FK).NameOfManufacturer,
                    expirationDate = _context.Products.ToList().FirstOrDefault(o => o.ProductID == item.ProductID_FK).ExpirationDate
                });
            }
            ProductsDataGrid.ItemsSource = products;
        }

        #region товары
        //поступление товаров
        private void AddProductButton_Click(object sender, RoutedEventArgs e)
        {
            //проверка на заполнение полей
            if (StringNotEmpty(ProductsComboBox.Text) && StringNotEmpty(ProductCountTextBox.Text))
            {
                //проверка на цифры
                if (CheckingForNumbers(ProductCountTextBox.Text))
                {
                    //проверка на деньги
                    if ((_context.CashRegister.ToList().FirstOrDefault().MoneyInCashRegister -
                            Convert.ToInt32(ProductCountTextBox.Text) * _context.Products.ToList().FirstOrDefault(o => o.NameOfProduct == ProductsComboBox.Text).PurchasePrice) >= 0)
                    {
                        //это сколько денег расходуется
                        var flowCash = Convert.ToInt32(ProductCountTextBox.Text) * _context.Products.ToList().FirstOrDefault(o => o.NameOfProduct == ProductsComboBox.Text).PurchasePrice;

                        //добавление в БД поступление товаров
                        _context.ReceiptsOfProducts.Add(new ReceiptsOfProducts
                        {
                            ProductID_FK = _context.Products.ToList().FirstOrDefault(o => o.NameOfProduct == ProductsComboBox.Text).ProductID,
                            ShiftID_FK = _context.Shifts.Max(o => o.ShiftID),
                            CountOfProducts = Convert.ToInt32(ProductCountTextBox.Text)
                        });

                        //изменяем инфу о смене
                        _context.Shifts.ToList().FirstOrDefault(o => o.ShiftID == _context.Shifts.Max(p => p.ShiftID)).FlowOfNomey += flowCash;

                        //изменяем деньги в кассе
                        _context.CashRegister.ToList().FirstOrDefault().MoneyInCashRegister -= flowCash;

                        //изменяем (или добавляем) товары в складе
                        if (_context.Warehouses.ToList().FirstOrDefault(o => o.ProductID_FK == _context.Products.ToList().
                                FirstOrDefault(k => k.NameOfProduct == ProductsComboBox.Text).ProductID) == null)
                        {
                            _context.Warehouses.Add(new Warehouses
                            {
                                ProductID_FK = _context.Products.ToList().FirstOrDefault(k => k.NameOfProduct == ProductsComboBox.Text).ProductID,
                                CountOfProducts = Convert.ToInt32(ProductCountTextBox.Text)
                            });
                        }
                        else
                        {
                            _context.Warehouses.ToList().FirstOrDefault(o => o.ProductID_FK == _context.Products.ToList().
                                FirstOrDefault(k => k.NameOfProduct == ProductsComboBox.Text).ProductID).CountOfProducts += Convert.ToInt32(ProductCountTextBox.Text);
                        }

                        // Сохранить изменения в базе данных
                        _context.SaveChanges();

                        OpenWindow(new MenuSaler(), this);
                    }
                    else
                    {
                        MsgView("Нельзя, чтобы денег в кассе было меньше нуля");
                    }
                }
                else
                {
                    MsgView("Поле 'Количество' должно состоять из цифр");
                }
            }
            else
            {
                MsgView("Все поля должны быть заполнены");
            }
        }

        //списание товаров
        private void WithdrawProductButton_Click(object sender, RoutedEventArgs e)
        {
            //проверка на заполнение полей
            if (StringNotEmpty(ProductsListComboBox.Text) && StringNotEmpty(ProductCountForWithdrawTextBox.Text))
            {
                //проверка на цифры
                if (CheckingForNumbers(ProductCountForWithdrawTextBox.Text))
                {
                    //проверка на количество
                    if (_context.Warehouses.ToList().FirstOrDefault(o => o.ProductID_FK == _context.Products.ToList().
                            FirstOrDefault(k => k.NameOfProduct == ProductsListComboBox.Text).ProductID).CountOfProducts >=
                            Convert.ToInt32(ProductCountForWithdrawTextBox.Text))
                    {
                        //добавление в БД списание товаров
                        _context.WritesOffOfProducts.Add(new WritesOffOfProducts
                        {
                            ProductID_FK = _context.Products.ToList().FirstOrDefault(o => o.NameOfProduct == ProductsListComboBox.Text).ProductID,
                            ShiftID_FK = _context.Shifts.Max(o => o.ShiftID),
                            CountOfProducts = Convert.ToInt32(ProductCountForWithdrawTextBox.Text),
                            Info = MoreInfoTextBox.Text.Trim()
                        });

                        //вычитаем товары в складе
                        _context.Warehouses.ToList().FirstOrDefault(o => o.ProductID_FK == _context.Products.ToList().
                                FirstOrDefault(k => k.NameOfProduct == ProductsListComboBox.Text).ProductID).CountOfProducts -=
                                Convert.ToInt32(ProductCountForWithdrawTextBox.Text);

                        // Сохранить изменения в базе данных
                        _context.SaveChanges();

                        OpenWindow(new MenuSaler(), this);
                    }
                    else
                    {
                        MsgView("Нельзя, чтобы количество товара было меньше нуля");
                    }
                }
                else
                {
                    MsgView("Поле 'Количество' должно состоять из цифр");
                }
            }
            else
            {
                MsgView("Все поля должны быть заполнены");
            }
        }

        //продажа товаров
        private void SaleProductButton_Click(object sender, RoutedEventArgs e)
        {
            //проверка на заполнение полей
            if (StringNotEmpty(ProductsForSaleComboBox.Text) && StringNotEmpty(ProductCountForSaleTextBox.Text))
            {
                //проверка на цифры
                if (CheckingForNumbers(ProductCountForSaleTextBox.Text))
                {
                    //проверка на количество
                    if (_context.Warehouses.ToList().FirstOrDefault(o => o.ProductID_FK == _context.Products.ToList().
                            FirstOrDefault(k => k.NameOfProduct == ProductsForSaleComboBox.Text).ProductID).CountOfProducts >=
                            Convert.ToInt32(ProductCountForSaleTextBox.Text))
                    {
                        //это сколько денег поступает
                        var arriveCash = Convert.ToInt32(ProductCountForSaleTextBox.Text) * _context.Products.ToList().FirstOrDefault(o => o.NameOfProduct == ProductsForSaleComboBox.Text).Price;

                        //добавление в БД продажу товаров
                        _context.SalesOfProducts.Add(new SalesOfProducts
                        {
                            ProductID_FK = _context.Products.ToList().FirstOrDefault(o => o.NameOfProduct == ProductsForSaleComboBox.Text).ProductID,
                            ShiftID_FK = _context.Shifts.Max(o => o.ShiftID),
                            CountOfProducts = Convert.ToInt32(ProductCountForSaleTextBox.Text)
                        });

                        //изменяем инфу о смене
                        _context.Shifts.ToList().FirstOrDefault(o => o.ShiftID == _context.Shifts.Max(p => p.ShiftID)).ArriveOfMoney += arriveCash;

                        //изменяем деньги в кассе
                        _context.CashRegister.ToList().FirstOrDefault().MoneyInCashRegister += arriveCash;

                        //вычитаем товары в складе
                        _context.Warehouses.ToList().FirstOrDefault(o => o.ProductID_FK == _context.Products.ToList().
                                FirstOrDefault(k => k.NameOfProduct == ProductsForSaleComboBox.Text).ProductID).CountOfProducts -=
                                Convert.ToInt32(ProductCountForSaleTextBox.Text);

                        // Сохранить изменения в базе данных
                        _context.SaveChanges();

                        OpenWindow(new MenuSaler(), this);
                    }
                    else
                    {
                        MsgView("Нельзя, чтобы количество товара было меньше нуля");
                    }
                }
                else
                {
                    MsgView("Поле 'Количество' должно состоять из цифр");
                }
            }
            else
            {
                MsgView("Все поля должны быть заполнены");
            }
        }
        #endregion

        //выход
        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            isFirstOpening = true;

            ///TODO: надо сделать штуку с закрытием смены (типо мини отчет)
            OpenWindow(new MainWindow(), this);
        }
    }

    internal class ProductsDataForSaler
    {
        public int productID { get; set; }
        public string nameOfProduct { get; set; }
        public double price { get; set; }
        public int count { get; set; }
        public string nameOfManufacturer { get; set; }
        public int expirationDate { get; set; }
    }
}