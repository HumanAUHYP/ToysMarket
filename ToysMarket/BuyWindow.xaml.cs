using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ToysMarket
{
    /// <summary>
    /// Логика взаимодействия для BuyWindow.xaml
    /// </summary>
    public partial class BuyWindow : Window
    {
        public List<Product> Products { get; set; }
        public List<PaymentMethod> PaymentMethods { get; set; }
        public Product Product { get; set; }
        public List<Buyer> Buyers { get; set; }
        public List<DeliverMethod> DeliveryTypes { get; set; }

        public BuyWindow(Product product)
        {
            Product = product;
            Products = dbConnection.db.Product.ToList();
            PaymentMethods = dbConnection.db.PaymentMethod.ToList();
            Buyers = dbConnection.db.Buyer.ToList();
            DeliveryTypes = dbConnection.db.DeliverMethod.ToList();
            InitializeComponent();
            cdDelivery.SelectedIndex = 0;
            DataContext = this;
        }


        private void cdDelivery_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            tbAddress.Visibility = (cdDelivery.SelectedItem as DeliverMethod).Name == "Pickup" ? Visibility.Hidden : Visibility.Visible;
            tblAddress.Visibility = (cdDelivery.SelectedItem as DeliverMethod).Name == "Pickup" ? Visibility.Hidden : Visibility.Visible;
        }

        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            if (!int.TryParse(tbCount.Text, out int count))
            {
                MessageBox.Show("Впишите число!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Hand);
                return;
            }
            else if (count > Product.Count)
            {
                MessageBox.Show("У нас нет так много этого товара", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Hand);
                return;
            }
            else if (count < 0)
            {
                MessageBox.Show("Вы ввели отрицательное значение", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Hand);
                return;
            }
            else if (cbBuyer.SelectedItem as Buyer == null)
            {
                MessageBox.Show("Выберите покупателя", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Hand);
                return;
            }
            else if (cbPaymentMethod.SelectedItem as PaymentMethod == null)
            {
                MessageBox.Show("Выберите способ оплаты", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Hand);
                return;
            }
            else if (cdDelivery.SelectedItem as DeliverMethod == null)
            {
                MessageBox.Show("Выберите способ доставки", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Hand);
                return;
            }

            Order order = new Order
            {
                Address = tbAddress.Text.Length == 0 ? null : tbAddress.Text,
                BuyerId = (cbBuyer.SelectedItem as Buyer).Id,
                Count = count,
                DeliverMethodId = (cdDelivery.SelectedItem as DeliverMethod).Id,
                ProductId = Product.Id,
                PaymentMethodId = (cbPaymentMethod.SelectedItem as PaymentMethod).Id
            };

            dbConnection.db.Order.Add(order);
            Product.Count -= count;
            dbConnection.db.SaveChanges();

            this.Close();
        }

        private void cbBuyer_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            tbPhone.Text = (cbBuyer.SelectedItem as Buyer).Phone;
        }
    }
}
