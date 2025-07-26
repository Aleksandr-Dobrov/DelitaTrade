using DelitaTrade.Common;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;

namespace DelitaTrade.ViewModels.Controllers
{
    public class LoginRememberController(IConfiguration config)
    {
        const int _timeOutDays = 7;
        public UserValidationForm? TryLoadRememberUser()
        {
            if (IsRememberUser())
            {
                string? encore = config.GetSection("UserRememberEncore").GetValue(typeof(string), "passwordEncore") as string;
                int passEncore = 0;
                if (encore == null || int.TryParse(encore, out passEncore) == false) throw new ArgumentNullException(nameof(encore));

                var section = config.GetSection("UserRememberAccount");
                var userName = section.GetValue(typeof(string), "userName") as string;
                var password = section.GetValue(typeof(string), "password") as string;

                userName = EncodePassword(passEncore, userName!);
                password = EncodePassword(passEncore, password!);

                return new UserValidationForm
                {
                    LoginName = userName!,
                    Password = password!
                };
            }
            return null;
        }

        public void SaveRememberUser(string userName, string password)
        {
            string? encore = config.GetSection("UserRememberEncore").GetValue(typeof(string), "passwordEncore") as string;
            int passEncore = 0;
            if (encore == null || int.TryParse(encore, out passEncore) == false) throw new ArgumentNullException(nameof(encore));

            SaveToRememberUser(EncodePassword(passEncore, userName), EncodePassword(passEncore, password), DateTime.Now.ToString(CultureInfo.InvariantCulture));
        }

        public void ClearRememberUser()
        {
            SaveToRememberUser(string.Empty, string.Empty, string.Empty);
        }

        private bool IsRememberUser()
        {
            var section = config.GetSection("UserRememberAccount");
            var userName = section.GetValue(typeof(string), "userName") as string;
            var password = section.GetValue(typeof(string), "password") as string;
            return string.IsNullOrEmpty(userName) == false && string.IsNullOrEmpty(password) == false && IsRememberUserTimeOut() == false;
        }

        private static string EncodePassword(int passEncore, string password)
        {
            byte[] data = Encoding.Unicode.GetBytes(password!);

            for (int i = 0; i < data.Length; i++)
            {
                data[i] = (byte)(data[i] ^ passEncore);
            }

            string decodedPassword = Encoding.Unicode.GetString(data);
            return decodedPassword;
        }


        private static void SaveToRememberUser(string userName, string password, string dateTime)
        {
            var json = File.ReadAllText("UserRememberAccount.json");
            var jsonObject = JsonSerializer.Deserialize<Dictionary<string, object>>(json) ?? new Dictionary<string, object>();
            jsonObject["UserRememberAccount"] = new Dictionary<string, string>
            {
                { "userName", userName },
                { "password", password },
                { "lastLogin", dateTime }
            };

            File.WriteAllText("userRememberAccount.json", JsonSerializer.Serialize(jsonObject, new JsonSerializerOptions
            {
                WriteIndented = true,
                Encoder = JavaScriptEncoder.Create(UnicodeRanges.All)
            }));
        }

        private bool IsRememberUserTimeOut()
        {
            var section = config.GetSection("UserRememberAccount");
            var lastLogin = section.GetValue(typeof(string), "lastLogin") as string;
            if (string.IsNullOrEmpty(lastLogin) == false)
            {
                if(DateTime.TryParse(lastLogin, CultureInfo.InvariantCulture, out DateTime lastLoginDate)) 
                {
                    var timeOut = DateTime.Now - lastLoginDate;
                    return timeOut.TotalDays > _timeOutDays;
                }
            }
            return true;
        }
    }
}
