using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace EventPlatform.Common.Core.Utils
{
    public class KeycloakRealmHelper
    {
        public static async Task CreateRealm(string name)
        {
            var url = @"http://localhost:8080/admin/realms";

            using var Http = new HttpClient();

            var httpContent = new StringContent(Realm.Replace("_newrealmname_", name), Encoding.UTF8, "application/json");
            Http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", await LoginAdmin());

            var response = await Http.PostAsync(url, httpContent);

            var responseObject = await response.Content.ReadAsStringAsync();
        }

        private static async Task<string> LoginAdmin()
        {
            var postData = new Dictionary<string, string>
            {
                { "grant_type", "password" },
                { "client_id", "admin-cli" },
                { "username", "admin" },
                { "password", "admin" }
            };

            var url = @"http://localhost:8080/realms/master/protocol/openid-connect/token";

            using var Http = new HttpClient();
            using var content = new FormUrlEncodedContent(postData);

            content.Headers.Clear();
            content.Headers.Add("Content-Type", "application/x-www-form-urlencoded");

            var response = await Http.PostAsync(url, content);

            var responseObject = await response.Content.ReadAsStringAsync();

            using var jsonDoc = JsonDocument.Parse(responseObject);
            return jsonDoc.RootElement.GetProperty("access_token").ToString();
        }

        public static string Realm => File.ReadAllText(Directory.GetCurrentDirectory() + "/realm-export-template.json");
    }
}
