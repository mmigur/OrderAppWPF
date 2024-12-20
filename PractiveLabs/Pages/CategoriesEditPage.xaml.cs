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
    /// Логика взаимодействия для CategoriesEditPage.xaml
    /// </summary>
    public partial class CategoriesEditPage : Page
    {
        private Practice_421_MigurEntities1 _context;
        private category _categories;
        public CategoriesEditPage(category selectedCategory = null)
        {
            InitializeComponent();
            
            _context = Practice_421_MigurEntities1.GetContext();
            _categories = selectedCategory ?? new category();
            LoadCategory();
        }

        public void LoadCategory() 
        {
            CategoryNameTextBox.Text = _categories.name;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(CategoryNameTextBox.Text))
            {
                InfoTextBlock.Text = "Заполните все поля!";
                return;
            }

            _categories.name = CategoryNameTextBox.Text.Trim();

            var existsCategory = _context.category.FirstOrDefault(cat => cat.name == _categories.name && cat.id != _categories.id);

            if (existsCategory != null)
            {
                MessageBox.Show("Категория с таким именем уже есть!", "Информация", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (_categories.id == 0)
            {
                int max = _context.category.Any() ? _context.category.Max(cat => cat.id) : 0;
                _categories.id = max + 1;
                _context.category.Add(_categories);
            }
            else
            {
                var existsCategoryToUpdate = _context.category.Find(_categories.id);
                if (existsCategoryToUpdate != null)
                {
                    existsCategoryToUpdate.name = _categories.name;
                }
            }

            _context.SaveChanges();

            MessageBox.Show("Категория сохранена успешно", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
            NavigationService.GoBack();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e) => NavigationService.GoBack();
    }
}
