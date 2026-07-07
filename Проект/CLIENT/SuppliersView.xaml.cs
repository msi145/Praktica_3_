using System.Windows;
using System.Windows.Controls;
using WmsClient.Services;

namespace WmsClient.Views
{
    public partial class SuppliersView : UserControl
    {
        private readonly DatabaseService _db = new(AppSettings.ConnectionString);

        public SuppliersView()
        {
            InitializeComponent();
            Loaded += (_, _) => LoadData();
        }

        private void BtnRefresh_Click(object sender, RoutedEventArgs e) => LoadData();

        private void LoadData()
        {
            try
            {
                Grid.ItemsSource = _db.GetTable(
                    @"SELECT Id AS [ID], CompanyName AS [Организация],
                             ContactPhone AS [Телефон], City AS [Город]
                      FROM dbo.Suppliers").DefaultView;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка подключения к базе данных:\n{ex.Message}",
                    "Ошибка БД", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
