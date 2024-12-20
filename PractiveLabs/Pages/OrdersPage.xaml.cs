using PractiveLabs.Model;
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

namespace PractiveLabs.Pages
{
    /// <summary>
    /// Логика взаимодействия для OrdersPage.xaml
    /// </summary>
    public partial class OrdersPage : Page
    {
        public OrdersPage()
        {
            InitializeComponent();
            LoadOrders();
        }

        private void LoadOrders() => OrdersListView.ItemsSource = Practice_421_MigurEntities1.GetContext().order.ToList();
        private void ApplyFilter() 
        {
            var query = Practice_421_MigurEntities1.GetContext().order.AsQueryable();

            if (!string.IsNullOrWhiteSpace(UserIdFilterTextBox.Text))
            {
                if (int.TryParse(UserIdFilterTextBox.Text, out int userId))
                {
                    query = query.Where(ord => ord.user_id == userId);
                }
                else
                {
                    MessageBox.Show(
                        "Введите корректный ID", 
                        "Ошибка", 
                        MessageBoxButton.OK, 
                        MessageBoxImage.Error
                        );
                    return;
                }
            }

            if (!string.IsNullOrWhiteSpace(ProductIdFilterTextBox.Text))
            {
                if (int.TryParse(ProductIdFilterTextBox.Text, out int productId))
                {
                    query = query.Where(ord => ord.product_id == productId);
                }
                else
                {
                    MessageBox.Show(
                        "Введите корректный ID",
                        "Ошибка",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error
                        );
                    return;
                }
            }

            if (DateFilterPicker.SelectedDate.HasValue)
            {
                DateTime selectedDate = DateFilterPicker.SelectedDate.Value;
                query = query.Where(ord => ord.date == selectedDate);
            }

            OrdersListView.ItemsSource = query.ToList();
        }

        private void UserIdFilterTextBox_TextChanged(object sender, TextChangedEventArgs e) => ApplyFilter();

        private void ProductIdFilterTextBox_TextChanged(object sender, TextChangedEventArgs e) => ApplyFilter();
        private void DateFilterPicker_SelectedDateChanged_1(object sender, SelectionChangedEventArgs e) => ApplyFilter();
        private void AddProduct_Click(object sender, RoutedEventArgs e) => NavigationService.Navigate(new OrderEditPage());

        private void EditProduct_Click(object sender, RoutedEventArgs e)
        {
            var selectedOrder = OrdersListView.SelectedItem as order;
            if (selectedOrder != null)
            {
                NavigationService.Navigate(new OrderEditPage(selectedOrder));
            }
            else
            {
                MessageBox.Show(
                        "Выберите заказ для редактирования",
                        "Ошибка",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error
                        );
            }
        }
        private void DeleteProduct_Click(object sender, RoutedEventArgs e)
        {
            var selectedOrder = OrdersListView.SelectedItem as order;
            if (selectedOrder != null)
            {
                var context = Practice_421_MigurEntities1.GetContext();
                context.order.Remove(selectedOrder);
                context.SaveChanges();
                LoadOrders();
                MessageBox.Show(
                       "Заказ успешнно удален",
                       "Инфо",
                       MessageBoxButton.OK,
                       MessageBoxImage.Information
                       );
            }
            else
            {
                MessageBox.Show(
                       "Выберите заказ для удаления",
                       "Ошибка",
                       MessageBoxButton.OK,
                       MessageBoxImage.Error
                       );
            }
        }
    }
}
