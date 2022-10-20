using System.Globalization;
using System.Net.Http.Json;

using Newtonsoft.Json;

using Safe_Sign.DTO.SGP;

namespace Safe_Sign.Util
{
    public static class SGPTools
    {
        public static bool TestConnectionWithSGP()
        {
            HttpClient client = new();

            Uri uri = new($"{Environment.GetEnvironmentVariable("SAFE_SIGN_SGPAPI")}/Auth/Verify");

            HttpRequestMessage request = new()
            {
                Method = HttpMethod.Get,
                RequestUri = uri
            };

            HttpResponseMessage response = client.SendAsync(request).Result;

            string result = response.StatusCode.ToString();

            if (!string.IsNullOrEmpty(result)) return true;

            else return false;
        }

        public static string SignInOnSGP(SGPSignInDTO user)
        {
            HttpClient client = new();

            Uri uri = new($"{Environment.GetEnvironmentVariable("SAFE_SIGN_SGPAPI")}/Auth/Login");

            HttpResponseMessage response;

            try
            {
                response = client.PostAsJsonAsync(uri, user).Result;
                response.EnsureSuccessStatusCode();
            }
            catch (Exception)
            {
                return string.Empty;
            }

            var authObject = JsonConvert.DeserializeObject<SGPAuthDTO>(response.Content.ReadAsStringAsync().Result);

            if (authObject != null && !string.IsNullOrEmpty(authObject.Access_token)) return authObject.Access_token;

            else return string.Empty;
        }

        public static SGPUserDTO? GetSGPUserData(SGPSignInDTO user, string sessionToken)
        {
            HttpClient client = new();
            client.DefaultRequestHeaders.Add("Authorization", Environment.GetEnvironmentVariable("SAFE_SIGN_SGPAPI_KEY"));

            Uri uri = new($"{Environment.GetEnvironmentVariable("SAFE_SIGN_SGPAPI")}/Pessoas/GetUsuarioByEmail" +
                $"?userEmail={user.Email}");

            HttpResponseMessage response;

            try
            {
                response = client.GetAsync(uri).Result;
                response.EnsureSuccessStatusCode();
            }
            catch (Exception)
            {
                return null;
            }

            var userData = JsonConvert.DeserializeObject<IList<SGPUserResponseDTO>>(response.Content.ReadAsStringAsync().Result);

            if (userData is null) return null;

            CultureInfo culture = new("pt-BR");

            SGPUserDTO userDTO = new()
            {
                Login = userData[0].Login_AD,
                FullName = userData[0].Nome,
                CPF = userData[0].CPF,
                BirthDate = userData[0].Dt_Nascimento.ToString("d", culture),
                Phone = userData[0].Telefone
            };

            return userDTO;
        }

        public static string ConvertFileToBase64(string filePath)
        {
            byte[] bytes = File.ReadAllBytes(filePath);

            string base64File = Convert.ToBase64String(bytes);

            return base64File;
        }
    }
}
