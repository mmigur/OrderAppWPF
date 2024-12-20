using PractiveLabs.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
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
    /// Логика взаимодействия для ProductsPage.xaml
    /// </summary>
    public partial class ProductsPage : Page
    {
        public ProductsPage()
        {
            InitializeComponent();
            GetCategories();
            GetProducts();
        }
        public void GetProducts() 
        {
            ProductsListView.ItemsSource = Practice_421_MigurEntities1
                .GetContext()
                .product
                .Include("category")
                .ToList();
        }
        public void GetCategories() 
        {
            var categories = Practice_421_MigurEntities1.GetContext().category.ToList();
            categories.Insert(0, new category { id = 0, name = "Все категории" });

            CategoryFilterComboBox.ItemsSource = categories;
            CategoryFilterComboBox.SelectedIndex = 0;
        }

        private void ApplyFilter() 
        {
            var selectedCategory = CategoryFilterComboBox.SelectedItem as category;
            var query = Practice_421_MigurEntities1.GetContext().product.AsQueryable();

            if (selectedCategory != null && selectedCategory.id != 0)
            {
                query = query.Where(p => p.category_id == selectedCategory.id);
            }

            if (!string.IsNullOrWhiteSpace(SearchTextBox.Text))
            {
                query = query.Where(p => p.name.StartsWith(SearchTextBox.Text));
            }

            ProductsListView.ItemsSource = query.Include("category").ToList();
        }

        private void AddProduct_Click(object sender, RoutedEventArgs e) 
        {
            NavigationService.Navigate(new ProductEditPage());
            ProductsListView.ItemsSource = Practice_421_MigurEntities1.GetContext().product.ToList();
            GetProducts();
        }
        private void EditProduct_Click(object sender, RoutedEventArgs e) 
        {
            var selectedProduct = ProductsListView.SelectedItems.Cast<product>().FirstOrDefault();
            if (selectedProduct != null)
            {
                NavigationService.Navigate(new ProductEditPage(selectedProduct));
                ProductsListView.ItemsSource = Practice_421_MigurEntities1.GetContext().product.ToList();
                GetProducts();
            }
            else 
            {
                MessageBox.Show(
                    "Выберите продукт для редактирования", 
                    "Ошибка", 
                    MessageBoxButton.OK, 
                    MessageBoxImage.Error
                    );
            }
        }
        private void CategoryFilterComboBox_SelectionChanged(object sender, RoutedEventArgs e) => ApplyFilter();
        private void SearchTextBox_TextChanged(object sender, RoutedEventArgs e) => ApplyFilter();

        private void DeleteProduct_Click(object sender, RoutedEventArgs e)
        {
            var selectedProduct = ProductsListView.SelectedItem as product;
            if (selectedProduct != null)
            {
                var context = Practice_421_MigurEntities1.GetContext();
                context.product.Remove(selectedProduct);
                context.SaveChanges();
                GetProducts();
                MessageBox.Show(
                       "Продукт успешнно удален",
                       "Инфо",
                       MessageBoxButton.OK,
                       MessageBoxImage.Information
                       );
            }
            else
            {
                MessageBox.Show(
                       "Выберите продукт для удаления",
                       "Ошибка",
                       MessageBoxButton.OK,
                       MessageBoxImage.Error
                       );
            }
        }
    }
}
