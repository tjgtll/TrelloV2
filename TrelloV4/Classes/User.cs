using Newtonsoft.Json;
using System.Collections.Generic;

namespace TrelloV0._0._3
{
    class User
    {
        [JsonProperty("id")]
        public uint ID;
        [JsonProperty("firstName")]
        private string firstName;
        [JsonProperty("lastName")]
        private string lastName;
        [JsonProperty("middleName")]
        private string middleName;
        [JsonProperty("mail")]
        private string mail;
        [JsonProperty("password")]
        private string password;
        [JsonProperty("Messages")]
        public List<string> Messages;

        internal bool IsMailRegistered(string Mail)
        {
            if (Mail == this.mail) return true;
            else return false;
        }

        public User(uint ID, string FirstName, string LastName, string MiddleName, string Mail, string Password)
        {
            this.ID = ID;
            this.firstName = FirstName;
            this.lastName = LastName;
            this.middleName = MiddleName;
            this.mail = Mail;
            this.password = Password;
            Messages = new List<string>();
        }

        public string GetUserName
        {
            get
            {
                string s = "";
                s += lastName + " ";
                s += firstName + " ";
                s += middleName;
                return s;
            }
        }

        public void Update(string FirstName, string LastName, string MiddleName)
        {
            if (FirstName !="")
                this.firstName = FirstName;
            if (LastName != "")
                this.firstName = LastName;
            if (MiddleName != "")
                this.firstName = MiddleName;
        }

        public bool IsPassword(string Password)
        {
            if (password == Password) return true;
            return false;
        }
    }
}
