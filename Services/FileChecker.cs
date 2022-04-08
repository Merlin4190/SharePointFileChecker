using Microsoft.SharePoint.Client;
using Microsoft.SharePoint.Client.Search.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class FileChecker
    {
        public string ServerPath { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

        public FileChecker(string path, string username, string password)
        {
            ServerPath = path;
            Username = username;
            Password = password;
        }
        public async Task<bool> Search(string searchQuery, string libraryName)
        {
            try
            {
                ClientContext clientContext = new ClientContext(ServerPath);
                var password = new SecureString();
                foreach (char c in Password)
                {
                    password.AppendChar(c);
                }
                clientContext.Credentials = new SharePointOnlineCredentials(Username, password);
                List sharedDocumentsLibrary = clientContext.Web.Lists.GetByTitle(libraryName);

                CamlQuery query = new CamlQuery();

                query.ViewXml = "<View Scope='Recursive'></View>";

                var files = sharedDocumentsLibrary.GetItems(query);

                clientContext.Load(files);

                KeywordQuery keywordQuery = new KeywordQuery(clientContext);
                keywordQuery.QueryText = searchQuery;

                SearchExecutor searchExecutor = new SearchExecutor(clientContext);

                ClientResult<ResultTableCollection> results = searchExecutor.ExecuteQuery(keywordQuery);

                await clientContext.ExecuteQueryAsync();
                if (results.Value.Count > 0) return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            #region
            /*
            CamlQuery query = new CamlQuery();

            query.ViewXml = "<View Scope='Recuursive'></View>";

            var files = sharedDocumentsLibrary.GetItems(query);

            clientContext.Load(files);

            foreach (var file in files)
            {
                if (file.DisplayName.Equals(searchString)) return true;
                //if (file["FileLeafRef"].Equals(searchString)) return true;
            }
            */
            #endregion

            return false;
        }

        public async Task<bool> Delete(string searchString, string libraryName)
        {
            if(await Search(searchString, libraryName))
            {
                // TO DO
            }
            return false;
        }
    }
}