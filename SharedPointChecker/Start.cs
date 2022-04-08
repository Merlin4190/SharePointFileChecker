using Services;
using System.Configuration;
using System;
using System.Threading.Tasks;

namespace SharedPointChecker
{
    public class Start
    {
        private static string searchString;
        private static string libraryName;
        public static async Task Run()
        {
            Client();
            var _serverPath = ConfigurationManager.AppSettings["ServerPath"];
            var _username = ConfigurationManager.AppSettings["Username"];
            var _password = ConfigurationManager.AppSettings["Password"];

            var fileChecker = new FileChecker(_serverPath, _username, _password);

            await fileChecker.Search(searchString, libraryName);

        }

        private static void Client()
        {
            Console.Write("Enter File Name: ");
            searchString = Console.ReadLine();
            Console.Write("Enter Library Name: ");
            libraryName = Console.ReadLine();
        }
    }
}
