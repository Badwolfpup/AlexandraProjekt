using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;

namespace TeacherStudentPlatform
{
    public class CredentialManager
    {
        private List<UserCredentials> credentials;
        private string filePath;

        public CredentialManager(string filePath)
        {
            this.filePath = filePath;
            LoadCredentials(filePath);
        }

        public class UserCredentials
        {
            public string Username { get; set; }
            public string Password { get; set; }
        }

        private void LoadCredentials(string filePath)
        {
            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);
                credentials = JsonConvert.DeserializeObject<List<UserCredentials>>(json);
            }
            else
            {
                credentials = new List<UserCredentials>();
            }
        }

        public bool ValidateCredentials(string username, string password)
        {
            // Validate credentials for all types of usernames
            return credentials.Exists(cred => cred.Username == username && cred.Password == password);
        }

    }
}
