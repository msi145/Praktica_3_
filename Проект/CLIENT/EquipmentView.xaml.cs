using System.Windows;
using System.Windows.Controls;
using WmsClient.Services;

namespace WmsClient.Views
{
    public partial class EquipmentView : UserControl
    {
        private readonly DatabaseService _db = new(AppSettings.ConnectionString);

        public EquipmentView()
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
                    @"SELECT e.Id AS [ID], e.Name AS [Оборудование],
                             d.Name AS [Цех], e.Status AS [Статус]
                      FROM dbo.Equipment e
                      LEFT JOIN dbo.Departments d ON e.DepartmentId = d.Id").DefaultView;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка подключения к базе данных:\n{ex.Message}",
                    "Ошибка БД", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
