using PractiveLabs.Model;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace PractiveLabs.Pages
{
    /// <summary>
    /// Логика взаимодействия для OrderEditPage.xaml
    /// </summary>
    public partial class OrderEditPage : Page
    {
        private Practice_421_MigurEntities1 _context;
        private order _orders;
        public OrderEditPage(order selectedOrder = null)
        {
            InitializeComponent();

            var orders = Practice_421_MigurEntities1.GetContext().order.ToList();

            _context = Practice_421_MigurEntities1.GetContext();

            _orders = selectedOrder != null ? selectedOrder : new order();

            if (selectedOrder != null)
            {
                NameTextBox.Text = selectedOrder.name.ToString();
                ProductIdTextBox.Text = selectedOrder.product_id.ToString();
                UserIdTextBox.Text = selectedOrder.user_id.ToString();
                PriceTextBox.Text = selectedOrder.price.ToString();
                CountTextBox.Text = selectedOrder.count.ToString();
                SumTextBox.Text = selectedOrder.sum.ToString();
                DateOrderPicker.SelectedDate = selectedOrder.date;
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {

            if (string.IsNullOrWhiteSpace(ProductIdTextBox.Text) ||
                string.IsNullOrWhiteSpace(UserIdTextBox.Text) ||
                string.IsNullOrWhiteSpace(PriceTextBox.Text) ||
                string.IsNullOrWhiteSpace(CountTextBox.Text) ||
                string.IsNullOrWhiteSpace(SumTextBox.Text) ||
                string.IsNullOrWhiteSpace(NameTextBox.Text) ||
                DateOrderPicker.SelectedDate == null)
            {
                MessageBox.Show("Заполните все поля", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            _orders.name = NameTextBox.Text;
            _orders.product_id = Convert.ToInt32(ProductIdTextBox.Text);
            _orders.user_id = Convert.ToInt32(UserIdTextBox.Text);
            _orders.price = Convert.ToDecimal(PriceTextBox.Text);
            _orders.count = Convert.ToInt32(CountTextBox.Text);
            _orders.sum = (Convert.ToInt32(PriceTextBox.Text) * Convert.ToInt32(CountTextBox.Text));
            _orders.date = DateOrderPicker.SelectedDate;

            int max = 0;
            if (_orders.id == 0)
            {
                if (_context.order.Count() != 0)
                {
                    max = _context.order.Max(ord => ord.id);
                    _orders.id = max + 1;
                    _context.order.Add(_orders);
                }
            }

            _context.SaveChanges();

            MessageBox.Show("Заказ успешно сохранен", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e) => NavigationService.GoBack();
    }
}
