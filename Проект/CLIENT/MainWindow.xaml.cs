using System.Windows;
using WmsClient.Views;

namespace WmsClient
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            MainContent.Content = new ProductsView(); // раздел по умолчанию
        }

        private void NavProducts_Click(object sender, RoutedEventArgs e) => MainContent.Content = new ProductsView();
        private void NavRawMaterials_Click(object sender, RoutedEventArgs e) => MainContent.Content = new RawMaterialsView();
        private void NavEquipment_Click(object sender, RoutedEventArgs e) => MainContent.Content = new EquipmentView();
        private void NavDepartments_Click(object sender, RoutedEventArgs e) => MainContent.Content = new DepartmentsView();
        private void NavUsers_Click(object sender, RoutedEventArgs e) => MainContent.Content = new UsersView();
        private void NavSuppliers_Click(object sender, RoutedEventArgs e) => MainContent.Content = new SuppliersView();
    }
}
