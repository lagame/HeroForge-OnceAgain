using System.Configuration;

namespace HeroForge_OnceAgain.Utils
{
    public class AppGlobal
    {
        private static string _connectionStringKey = "DatabaseConnection";

        public static string ConnectionString
        {
            get
            {
                string connectionString = "";

                // Verifica se a chave de conexão está presente no arquivo de configuração
                if (ConfigurationManager.ConnectionStrings[_connectionStringKey] != null)
                {
                    connectionString = ConfigurationManager.ConnectionStrings[_connectionStringKey].ConnectionString;
                }

                return connectionString;
            }
        }
    }
}
