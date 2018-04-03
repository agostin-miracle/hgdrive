using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsmGdrive
{
    public class Start
    {

        public static Google.Apis.Auth.OAuth2.UserCredential Authenticate()
        {
            Google.Apis.Auth.OAuth2.UserCredential credenciais;

            using (var stream = new System.IO.FileStream("client_id.json", System.IO.FileMode.Open, System.IO.FileAccess.Read))
            {
                var diretorioAtual = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
                var diretorioCredenciais = System.IO.Path.Combine(diretorioAtual, "credential");

                credenciais = Google.Apis.Auth.OAuth2.GoogleWebAuthorizationBroker.AuthorizeAsync(
                    Google.Apis.Auth.OAuth2.GoogleClientSecrets.Load(stream).Secrets,
                    new[] { Google.Apis.Drive.v3.DriveService.Scope.DriveReadonly },
                    "user",
                    System.Threading.CancellationToken.None,
                    new Google.Apis.Util.Store.FileDataStore(diretorioCredenciais, true)).Result;
            }

            return credenciais;
        }


        public static Google.Apis.Auth.OAuth2.UserCredential Authenticate(string RemoveOlderConfig )
        {
            if (!String.IsNullOrEmpty(RemoveOlderConfig))
            {
                RemoveChachedAuthentication(RemoveOlderConfig);
            }
            Google.Apis.Auth.OAuth2.UserCredential credenciais;

            using (var stream = new System.IO.FileStream("client_id.json", System.IO.FileMode.Open, System.IO.FileAccess.Read))
            {
                var diretorioAtual = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
                var diretorioCredenciais = System.IO.Path.Combine(diretorioAtual, "credential");

                credenciais = Google.Apis.Auth.OAuth2.GoogleWebAuthorizationBroker.AuthorizeAsync(
                    Google.Apis.Auth.OAuth2.GoogleClientSecrets.Load(stream).Secrets,
                    new[] { Google.Apis.Drive.v3.DriveService.Scope.Drive },
                    "user",
                    System.Threading.CancellationToken.None,
                    new Google.Apis.Util.Store.FileDataStore(diretorioCredenciais, true)).Result;
            }

            return credenciais;
        }

        public static bool RemoveChachedAuthentication(string path)
        {
            bool retorno = true;
            string path1 = Path.GetDirectoryName(path);
            Console.WriteLine(path1);
            string path2 = path1 + "\\credential\\Google.Apis.Auth.OAuth2.Responses.TokenResponse-user";
            if (File.Exists(path2))
            {
                try
                {
                    File.Delete(path2);
                }
                catch
                {
                    retorno = false;
                }
            }
            return retorno;
        }

        public static Google.Apis.Drive.v3.DriveService OpenService(Google.Apis.Auth.OAuth2.UserCredential credenciais)
        {
            return new Google.Apis.Drive.v3.DriveService(new Google.Apis.Services.BaseClientService.Initializer()
            {
                HttpClientInitializer = credenciais
            });
        }
    }
}
