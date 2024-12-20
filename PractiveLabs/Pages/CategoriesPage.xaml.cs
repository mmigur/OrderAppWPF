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
    /// Логика взаимодействия для CategoriesPage.xaml
    /// </summary>
    public partial class CategoriesPage : Page
    {
        public CategoriesPage()
        {
            InitializeComponent();
            GetCategories();
        }

        public void GetCategories()
        {
            var categories = Practice_421_MigurEntities1
                .GetContext()
                .category
                .ToList();

            CategoryListView.ItemsSource = categories;
        }

        private void AddProduct_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new CategoriesEditPage());
            CategoryListView.ItemsSource = Practice_421_MigurEntities1.GetContext().category.ToList();
        }

        private void EditProduct_Click(object sender, RoutedEventArgs e)
        {
            var selectedCategory = CategoryListView.SelectedItems.Cast<category>().FirstOrDefault();
            if (selectedCategory != null)
            {
                NavigationService.Navigate(new CategoriesEditPage(selectedCategory));
                CategoryListView.ItemsSource = Practice_421_MigurEntities1.GetContext().category.ToList();
                GetCategories();
            }
            else
            {
                MessageBox.Show(
                    "Выберите категорию для редактирования",
                    "Ошибка",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error
                    );
            }
        }
        private void SearchTextBox_TextChanged(object sender, RoutedEventArgs e) => ApplyFilter();
        private void ApplyFilter()
        {
            var query = Practice_421_MigurEntities1.GetContext().category.AsQueryable();

            if (!string.IsNullOrWhiteSpace(SearchTextBox.Text))
            {
                query = query.Where(p => p.name.StartsWith(SearchTextBox.Text));
            }

            CategoryListView.ItemsSource = query.ToList();
        }

        private void DeleteProduct_Click(object sender, RoutedEventArgs e)
        {
            var selectedCategory = CategoryListView.SelectedItem as category;
            if (selectedCategory != null)
            {
                var context = Practice_421_MigurEntities1.GetContext();
                context.category.Remove(selectedCategory);
                context.SaveChanges();
                GetCategories();
                MessageBox.Show(
                       "Категория успешнно удалена",
                       "Инфо",
                       MessageBoxButton.OK,
                       MessageBoxImage.Information
                       );
            }
            else
            {
                MessageBox.Show(
                       "Выберите категорию для удаления",
                       "Ошибка",
                       MessageBoxButton.OK,
                       MessageBoxImage.Error
                       );
            }
        }
    }
}
