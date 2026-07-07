namespace WmsClient
{
    // Единственное место, где хранятся настройки подключения.
    // Меняй только здесь — во всех остальных файлах эти значения не дублируются.
    public static class AppSettings
    {
        // Строка подключения к твоему серверу SQL Server.
        // Если у тебя SQL Server Express — обычно: Server=.\SQLEXPRESS;...
        // Если LocalDB — узнать точное имя можно в свойствах сервера в SSMS.
        public static string ConnectionString { get; set; } =
            "Server=(localdb)\\MSSQLLocalDB;Database=WmsWarehouseDb;Trusted_Connection=True;TrustServerCertificate=True;";

        // Адрес запущенного проекта WmsApi (см. launchSettings.json в проекте WmsApi)
        public static string ApiBaseUrl { get; set; } = "https://localhost:7192";
    }
}
