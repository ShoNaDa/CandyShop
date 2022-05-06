using System.Windows;
using static Candy_shop.ValidateClass;
using static Candy_shop.UsefulFunctions;

namespace Candy_shop
{
    /// <summary>
    /// Логика взаимодействия для MenuSaler.xaml
    /// </summary>
    public partial class MenuSaler : Window
    {
        public MenuSaler()
        {
            InitializeComponent();
        }

        private void AddProductButton_Click(object sender, RoutedEventArgs e)
        {
            //проверка на заполнение полей
            if (StringNotEmpty(ProductsComboBox.Text) && StringNotEmpty(ProductCountTextBox.Text))
            {
                //проверка на цифры
                if (CheckingForNumbers(ProductCountTextBox.Text))
                {

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

        private void WithdrawProductButton_Click(object sender, RoutedEventArgs e)
        {
            //проверка на заполнение полей
            if (StringNotEmpty(ProductsListComboBox.Text) && StringNotEmpty(ProductCountForWithdrawTextBox.Text))
            {
                //проверка на цифры
                if (CheckingForNumbers(ProductCountForWithdrawTextBox.Text))
                {

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

        private void SaleProductButton_Click(object sender, RoutedEventArgs e)
        {
            //проверка на заполнение полей
            if (StringNotEmpty(ProductsForSaleComboBox.Text) && StringNotEmpty(ProductCountForSaleTextBox.Text))
            {
                //проверка на цифры
                if (CheckingForNumbers(ProductCountForSaleTextBox.Text))
                {

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
    }
}
