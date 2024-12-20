using ClosedXML.Excel;
using PractiveLabs.Model;
using System;
using System.Collections.Generic;
using System.IO;
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
using Path = System.IO.Path;

namespace PractiveLabs.Pages
{
    /// <summary>
    /// Логика взаимодействия для ReportPage.xaml
    /// </summary>
    public partial class ReportPage : Page
    {
        public ReportPage()
        {
            InitializeComponent();
            LoadCategories();
        }

        private void LoadCategories()
        {
            var categories = Practice_421_MigurEntities1.GetContext().category.ToList();
            categories.Insert(0, new category { id = 0, name = "Все категории" });
            CategoryFilterComboBox.ItemsSource = categories;
            CategoryFilterComboBox.SelectedIndex = 0;
        }

        private void GenerateReport_Click(object sender, RoutedEventArgs e)
        {
            var startDate = StartDatePicker.SelectedDate;
            var endDate = EndDatePicker.SelectedDate;
            var selectedCategory = CategoryFilterComboBox.SelectedItem as category;

            if (startDate == null || endDate == null)
            {
                MessageBox.Show("Пожалуйста, выберите период для отчета.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var query = Practice_421_MigurEntities1.GetContext().order.AsQueryable();

            if (selectedCategory != null && selectedCategory.id != 0)
            {
                query = query.Where(o => o.product.category_id == selectedCategory.id);
            }

            query = query.Where(o => o.date >= startDate && o.date <= endDate);

            var orders = query.OrderBy(o => o.date).ToList();

            if (orders.Count == 0)
            {
                MessageBox.Show("Нет данных для формирования отчета.", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            // Вызов метода для создания Excel-файла с использованием ClosedXML
            GenerateExcelReport(orders, startDate.Value, endDate.Value, selectedCategory);
        }

        private void GenerateExcelReport(List<order> orders, DateTime startDate, DateTime endDate, category selectedCategory)
        {
            // Создаем новый Excel-файл
            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Отчет по платежам");

                // Заголовок
                worksheet.Cell("A1").Value = "Отчет по платежам";
                worksheet.Cell("A2").Value = $"Период: {startDate.ToShortDateString()} - {endDate.ToShortDateString()}";
                worksheet.Cell("A3").Value = $"Категория: {(selectedCategory?.name ?? "Все категории")}";

                // Шапка таблицы
                worksheet.Cell("A5").Value = "ID";
                worksheet.Cell("B5").Value = "Продукт";
                worksheet.Cell("C5").Value = "Категория";
                worksheet.Cell("D5").Value = "Цена";
                worksheet.Cell("E5").Value = "Количество";
                worksheet.Cell("F5").Value = "Сумма";
                worksheet.Cell("G5").Value = "Дата";

                // Данные
                int row = 6;
                decimal totalSum = 0;
                foreach (var order in orders)
                {
                    worksheet.Cell($"A{row}").Value = order.id;
                    worksheet.Cell($"B{row}").Value = order.product.name;
                    worksheet.Cell($"C{row}").Value = order.product.category.name;
                    worksheet.Cell($"D{row}").Value = order.price;
                    worksheet.Cell($"E{row}").Value = order.count;
                    worksheet.Cell($"F{row}").Value = order.sum;
                    worksheet.Cell($"G{row}").Value = order.date.HasValue ? order.date.Value.ToShortDateString() : "Нет даты";

                    totalSum += Convert.ToDecimal(order.sum);
                    row++;
                }

                // Итоговая сумма
                worksheet.Cell($"A{row}").Value = "Итого:";
                worksheet.Cell($"F{row}").Value = totalSum;

                // Сохранение файла с использованием введенного имени
                var desktopPath = "C:\\Users\\pr121migur\\Desktop\\Мигур ПР-421 Практика\\PractiveLabs\\PractiveLabs\\Data\\";
                var fileName = FileNameTextBox.Text.Trim(); // Получаем имя файла из TextBox
                if (string.IsNullOrWhiteSpace(fileName))
                {
                    fileName = "Отчет"; // Если имя не указано, используем "Отчет"
                }
                var filePath = Path.Combine(desktopPath, $"{fileName}.xlsx");

                // Проверка на существование файла с таким именем
                if (File.Exists(filePath))
                {
                    int counter = 1;
                    while (File.Exists(filePath))
                    {
                        filePath = Path.Combine(desktopPath, $"{fileName}_{counter}.xlsx");
                        counter++;
                    }
                }

                workbook.SaveAs(filePath);

                MessageBox.Show($"Отчет успешно сформирован и сохранен в папку Data: {filePath}", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }
}
