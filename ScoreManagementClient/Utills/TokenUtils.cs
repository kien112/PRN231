using Newtonsoft.Json.Linq;
using ScoreManagementClient.Dtos.User;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace ScoreManagementClient.Utills
{
    public class TokenUtils
    {
        public static bool CanAccess(string? token, List<string> rolesCanAccess)
        {
            if(String.IsNullOrEmpty(token))
            {
                return false;
            }

            string? role = GetRoleInToken(token);

            if(role == null)
            {
                return false;
            }

            return rolesCanAccess.Contains(role);
        }

        public static string? GetRoleInToken(string token)
        {
            try
            {
                string base64UrlPayload = token.Split('.')[1];
                base64UrlPayload = AddPadding(base64UrlPayload);
                byte[] payloadBytes = Convert.FromBase64String(base64UrlPayload);
                string jsonPayload = Encoding.UTF8.GetString(payloadBytes);

                var payloadObject = JsonSerializer.Deserialize<dynamic>(jsonPayload);

                using (JsonDocument document = JsonDocument.Parse(jsonPayload))
                {
                    string? role = document.RootElement.GetProperty("http://schemas.microsoft.com/ws/2008/06/identity/claims/role").GetString();

                    return role;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        static string AddPadding(string base64Url)
        {
            int paddingLength = (4 - base64Url.Length % 4) % 4;
            return base64Url + new string('=', paddingLength);
        }
    }
}
