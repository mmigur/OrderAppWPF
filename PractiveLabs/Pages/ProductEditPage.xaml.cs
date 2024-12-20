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
    /// Логика взаимодействия для ProductEditPage.xaml
    /// </summary>
    public partial class ProductEditPage : Page
    {
        private Practice_421_MigurEntities1 _context;
        private product _products;

        public ProductEditPage(product products = null)
        {
            InitializeComponent();
            var catogories = Practice_421_MigurEntities1.GetContext().category.ToList();
            catogories.Insert(0, new category { id = 0, name = "Все категории"});

            CategoryComboBox.ItemsSource = catogories;
            CategoryComboBox.SelectedIndex = 0;

            _context = Practice_421_MigurEntities1.GetContext();

            _products = products != null ? products : new product();

            if (products != null)
            {
                ProductNameTextBox.Text = products.name;
                CategoryComboBox.SelectedValue = products.category_id;
            }
        }

        private void CategoryFilterComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(ProductNameTextBox.Text) || CategoryComboBox.SelectedValue == null)
            {
                InfoTextBlock.Text = "Заполните все поля!";
                return;
            }

            _products.name = ProductNameTextBox.Text.Trim();
            _products.category_id = (int)CategoryComboBox.SelectedValue;

            int max = 0;
            if (_products.id == 0)
            {
                if (_context.product.Count() != 0)
                {
                    max = _context.product.Max(pr => pr.id);
                    _products.id = max + 1;
                    _context.product.Add(_products);
                }
            }

            _context.SaveChanges();

            MessageBox.Show("Продукт успешно сохранен", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e) => NavigationService.GoBack();
    }
}
