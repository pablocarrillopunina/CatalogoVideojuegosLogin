using MySql.Data.MySqlClient;

namespace PracticaXMLDinamica.Data
{
    public class DatabaseHelper
    {
        private static readonly string connectionString =
            "server=localhost;port=3306;database=login_db;uid=root;pwd=root;";

        public static MySqlConnection GetConnection()
        {
            try
            {
                return new MySqlConnection(connectionString);
            }
            catch
            {
                return null;  // 
            }
        }
    }
}
