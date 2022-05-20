using System.Windows;
using static Candy_shop.ValidateClass;
using static Candy_shop.UsefulFunctions;
using System.Linq;
using System;

namespace Candy_shop
{
    /// <summary>
    /// Логика взаимодействия для AddProduct.xaml
    /// </summary>
    public partial class AddProduct : Window
    {
        //db
        readonly CandyShopEntities _context = new CandyShopEntities();

        public AddProduct()
        {
            InitializeComponent();

            //добавляем производителей
            foreach (var item in _context.Manufacturers.ToList())
            {
                ManufacturerComboBox.Items.Add(item.NameOfManufacturer);
            }
        }

        private void AddProductButton_Click(object sender, RoutedEventArgs e)
        {
            //проверка на заполнение полей
            if (StringNotEmpty(ProductNameTextBox.Text) && StringNotEmpty(ProductPriceTextBox.Text)
                && StringNotEmpty(ProductExpirationTextBox.Text) && StringNotEmpty(PurchasePriceTextBox.Text)
                && StringNotEmpty(ManufacturerComboBox.Text))
            {
                //проверка на заглавную букву
                if (FirstLetterIsLarge(ProductNameTextBox.Text))
                {
                    //проверка на цифры
                    if (CheckingForNumbers(ProductExpirationTextBox.Text))
                    {
                        //проверка правильности цены
                        if (CheckingForNumbersWithPoint(ProductPriceTextBox.Text) &&
                            CheckingForNumbersWithPoint(PurchasePriceTextBox.Text))
                        {
                            //добавление в БД
                            _context.Products.Add(new Products
                            {
                                ManufacturerID_FK = _context.Manufacturers.ToList()
                                    .FirstOrDefault(o => o.NameOfManufacturer == ManufacturerComboBox.Text.Trim()).ManufacturerID,
                                NameOfProduct = ProductNameTextBox.Text,
                                Price = Convert.ToDecimal(ProductPriceTextBox.Text),
                                PurchasePrice = Convert.ToDecimal(PurchasePriceTextBox.Text),
                                ExpirationDate = Convert.ToInt32(ProductExpirationTextBox.Text)
                            });
                            // Сохранить изменения в базе данных
                            _context.SaveChanges();

                            MenuDirector menuDirector = new MenuDirector();
                            menuDirector.Show();
                            Close();
                        }
                        else
                        {
                            MsgView("В полях 'Цена' и 'Стоимость затрат' допущены ошибки");
                        }
                    }
                    else
                    {
                        MsgView("Поле 'Срок годности' должно быть написано цифрами");
                    }
                }
                else
                {
                    MsgView("Поле 'Название' должно начинаться в заглавной буквы и написано кириллицей");
                }
            }
            else
            {
                MsgView("Все обязательные поля должны быть заполнены");
            }
        }

        private void CancalButton_Click(object sender, RoutedEventArgs e)
        {
            MenuDirector menuDirector = new MenuDirector();
            menuDirector.Show();
            Close();
        }
    }
}
