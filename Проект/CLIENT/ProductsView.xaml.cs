using System.Windows;
using System.Windows.Controls;
using WmsClient.Models;
using WmsClient.Services;

namespace WmsClient.Views
{
    public partial class ProductsView : UserControl
    {
        private readonly DatabaseService _db = new(AppSettings.ConnectionString);
        private readonly IntegrationApiClient _api = new();
        private List<Product> _currentProducts = new();

        public ProductsView()
        {
            InitializeComponent();
            Loaded += (_, _) => LoadData();
        }

        // Шаг 1: WPF читает данные из БД напрямую по строке подключения
        private void BtnRefresh_Click(object sender, RoutedEventArgs e) => LoadData();

        private void LoadData()
        {
            try
            {
                _currentProducts = _db.GetProducts();
                Grid.ItemsSource = _currentProducts;
                StatusText.Text = $"Загружено позиций из БД: {_currentProducts.Count}";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка подключения к базе данных:\n{ex.Message}",
                    "Ошибка БД", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Шаг 2: то, что сейчас в таблице, WPF отправляет POST-запросом на API,
        // а API уже сам пересылает данные дальше в 1С
        private async void BtnSend_Click(object sender, RoutedEventArgs e)
        {
            if (_currentProducts.Count == 0)
            {
                MessageBox.Show("Сначала загрузите данные из БД.", "Внимание",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            BtnSend.IsEnabled = false;
            StatusText.Text = "Отправка в 1С...";

            var (success, message) = await _api.SendProductsToOneCAsync(_currentProducts);

            StatusText.Text = success
                ? $"Успешно отправлено в 1С. Ответ сервера: {message}"
                : $"Ошибка отправки в 1С: {message}";

            BtnSend.IsEnabled = true;
        }
    }
}
