using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
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

            List<ProductsData> products = new List<ProductsData>();

            foreach (Products item in _context.Products.ToList())
            {
                products.Add(new ProductsData() {
                    productID = item.ProductID,
                    nameOfProduct = item.NameOfProduct,
                    price = Convert.ToDouble(item.Price),
                    nameOfManufacturer = _context.Manufacturers.ToList().FirstOrDefault(o => o.ManufacturerID == item.ManufacturerID_FK).NameOfManufacturer,
                    expirationDate = item.ExpirationDate});
            }
            ProductsDataGrid.ItemsSource = products;
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            AddWorker addWorker = new AddWorker();
            addWorker.Show();
            Close();
        }

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

                MenuDirector menuDirector = new MenuDirector();
                menuDirector.Show();
                Close();
            }
            else
            {
                UsefulFunctions.MsgView("Выберите сотрудника из списка");
            }
        }

        private void AddProductButton_Click(object sender, RoutedEventArgs e)
        {
            AddProduct addProduct = new AddProduct();
            addProduct.Show();
            Close();
        }

        //событие двойного нажатия на сотрудника
        private void WorkersListBox_DoubleClick(object sender, EventArgs e)
        {
            if (WorkersListBox.SelectedItem != null)
            {
                string selectedWorkerCode = WorkersListBox.SelectedItem.ToString().Split('|')[0].Trim();

                Profile profile = new Profile(selectedWorkerCode);
                profile.Show();
                Close();
            }
        }

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

                MenuDirector menuDirector = new MenuDirector();
                menuDirector.Show();
                Close();
            }
            else
            {
                UsefulFunctions.MsgView("Выберите сотрудника из списка");
            }
        }

        private void EditProductButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
    public class ProductsData
    {
        public int productID { get; set; }
        public string nameOfProduct { get; set; }
        public double price { get; set; }
        public string nameOfManufacturer { get; set; }
        public int expirationDate { get; set; }
    }
}
