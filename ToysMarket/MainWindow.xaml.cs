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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ToysMarket
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public List<Product> Products { get; set; }
        public List<Product> ProductsForSearch { get; set; }
        public List<Producer> Producers { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            Producers = dbConnection.db.Producer.ToList();
            Products = dbConnection.db.Product.ToList();
            ProductsForSearch = Products;
            Producers.Insert(0, new Producer { Name = "Все" });

            DataContext = this;
        }

        private void tbSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            ApplyFiltres();
        }

        private void cbProducer_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ApplyFiltres();
        }

        private void ApplyFiltres()
        {
            var text = tbSearch.Text.ToLower();
            ProductsForSearch = Products.FindAll(p => p.Name.ToLower().Contains(text) || p.Description.ToLower().Contains(text));

            var producer = cbProducer.SelectedItem as Producer;
            if (producer.Name != "Все")
                ProductsForSearch = ProductsForSearch.FindAll(p => p.Producer == producer);

            dgProducts.ItemsSource = ProductsForSearch;
        }


        private void btnBuy_Click(object sender, RoutedEventArgs e)
        {
            var product = dgProducts.SelectedItem as Product;
            if (product == null)
            {
                MessageBox.Show("Выбери для начала продукт", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Hand);
                return;
            }

            Window dialog = new BuyWindow(product);

            dialog.ShowDialog();

            dgProducts.Items.Refresh();
        }

        private void btnSort_Click(object sender, RoutedEventArgs e)
        {
            if (btnSort.Content.ToString() == "/\\")
            {
                Products = Products.OrderByDescending(x => x.Price).ToList();
                btnSort.Content = "\\/";
            }
            else
            {
                Products = Products.OrderBy(x => x.Price).ToList();
                btnSort.Content = "/\\";
            }

            dgProducts.ItemsSource = Products;
        }
    }
}
