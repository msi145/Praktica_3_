using System.Data;
using Microsoft.Data.SqlClient;
using WmsClient.Models;

namespace WmsClient.Services
{
    // Единственный класс во всём приложении, который открывает SqlConnection.
    // WPF обращается к базе данных напрямую — никакого API здесь нет и не нужно.
    public class DatabaseService
    {
        private readonly string _connectionString;

        public DatabaseService(string connectionString)
        {
            _connectionString = connectionString;
        }

        // Универсальный метод: выполняет любой SELECT и возвращает готовую таблицу для DataGrid
        public DataTable GetTable(string sql)
        {
            using var conn = new SqlConnection(_connectionString);
            using var adapter = new SqlDataAdapter(sql, conn);
            var table = new DataTable();
            adapter.Fill(table);
            return table;
        }

        // Отдельный типизированный метод для продукции — он нужен,
        // чтобы можно было сериализовать список в JSON и отправить в API/1С
        public List<Product> GetProducts()
        {
            var list = new List<Product>();

            using var conn = new SqlConnection(_connectionString);
            conn.Open();
            using var cmd = new SqlCommand("SELECT Id, Sku, Name, Quantity, Zone FROM dbo.Products", conn);
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                list.Add(new Product
                {
                    Id = reader.GetInt32(0),
                    Sku = reader.GetString(1),
                    Name = reader.GetString(2),
                    Quantity = reader.GetInt32(3),
                    Zone = reader.GetString(4)
                });
            }

            return list;
        }
    }
}
