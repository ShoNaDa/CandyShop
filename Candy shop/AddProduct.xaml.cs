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
        readonly Products productForEddition;

        //bool
        private readonly bool isAddingProduct;

        public AddProduct(bool isAdding, Products product)
        {
            InitializeComponent();

            //добавляем производителей
            foreach (var item in _context.Manufacturers.ToList())
            {
                ManufacturerComboBox.Items.Add(item.NameOfManufacturer);
            }

            isAddingProduct = isAdding;

            //ставим соответствующие заголовки
            if (isAddingProduct)
            {
                titleLabel.Content = "Добавление товара";
                AddProductButton.Content = "Добавить";
            }
            else
            {
                titleLabel.Content = "Изменение товара";
                AddProductButton.Content = "Изменить";

                ProductNameTextBox.Text = product.NameOfProduct;
                ProductPriceTextBox.Text = product.Price.ToString();
                ProductExpirationTextBox.Text = product.ExpirationDate.ToString();
                ManufacturerComboBox.Text = _context.Manufacturers.ToList()
                                        .FirstOrDefault(o => o.ManufacturerID == product.ManufacturerID_FK).ToString();
                PurchasePriceTextBox.Text = product.PurchasePrice.ToString();

                productForEddition = product;
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
                            //если происходит добавление товара, а не изменение
                            if (isAddingProduct)
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

                                OpenWindow(new MenuDirector(), this);
                            }
                            else
                            {
                                Products product = _context.Products.FirstOrDefault(o => o.ProductID == productForEddition.ProductID);

                                product.ManufacturerID_FK = _context.Manufacturers.ToList()
                                        .FirstOrDefault(o => o.NameOfManufacturer == ManufacturerComboBox.Text.Trim()).ManufacturerID;
                                product.NameOfProduct = ProductNameTextBox.Text;
                                product.Price = Convert.ToDecimal(ProductPriceTextBox.Text);
                                product.PurchasePrice = Convert.ToDecimal(PurchasePriceTextBox.Text);
                                product.ExpirationDate = Convert.ToInt32(ProductExpirationTextBox.Text);

                                _context.SaveChanges();

                                OpenWindow(new MenuDirector(), this);
                            }
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
            OpenWindow(new MenuDirector(), this);
        }
    }
}