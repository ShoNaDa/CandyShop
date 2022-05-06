using System.Windows;
using static Candy_shop.ValidateClass;
using static Candy_shop.UsefulFunctions;

namespace Candy_shop
{
    /// <summary>
    /// Логика взаимодействия для AddProduct.xaml
    /// </summary>
    public partial class AddProduct : Window
    {
        public AddProduct()
        {
            InitializeComponent();
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
                    //проверка на маленькие буквы
                    if (CheckingForLetters(ProductNameTextBox.Text))
                    {
                        //проверка на цифры
                        if (CheckingForNumbers(ProductExpirationTextBox.Text))
                        {
                            //проверка правильности цены
                            if (CheckingForNumbersWithPoint(ProductPriceTextBox.Text) &&
                                CheckingForNumbersWithPoint(PurchasePriceTextBox.Text))
                            {

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
                        MsgView("Поле 'Название' должно быть написано кириллицей");
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
