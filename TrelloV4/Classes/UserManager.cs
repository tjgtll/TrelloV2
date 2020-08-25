using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace TrelloV0._0._3
{
    class UserManager
    {
        [JsonProperty("List")]
        public List<User> List;

        private static UserManager users;
        [JsonProperty("counter")]
        private uint counter;

        private UserManager()
        {
            List = new List<User>();
            counter = 1;
        }

        public static UserManager Instance
        {
            get
            {
                if (users == null)
                    users = new UserManager();
                return users;
            }
        }
        
        public bool Add(string FirstName, string LastName, string MiddleName, string Mail, string Password)
        {
            for (int i = 0; i < List.Count; i++)
            {
                if (List[i].IsMailRegistered(Mail)) return false;
            }

            List.Add(new User(this.counter, FirstName, LastName, MiddleName, Mail, Password));
            counter++;
            return true;
        }

        public bool Remove(string Mail)
        {
            for (int i = 0; i < List.Count; i++)
            {
                if (List[i].IsMailRegistered(Mail))
                {
                    List.RemoveAt(i);
                    return true;
                }
            }
            return false;
        }

        public bool Remove(uint ID)
        {
            for (int i = 0; i < List.Count; i++)
            {
                if (List[i].ID == ID)
                {
                    List.RemoveAt(i);
                    return true;
                }
            }
            return false;
        }

        public string GetUserName(uint id)
        {
            string s = "";
            for (int i = 0; i < List.Count; i++)
            {
                if (List[i].ID == id) s = List[i].GetUserName;
            }
            return s;
        }

        public User Entry(string Mail, string Password)
        {
            for (int i = 0; i < List.Count; i++)
            {
                if (List[i].IsMailRegistered(Mail))
                {
                    if (List[i].IsPassword(Password)) return List[i];
                    else throw new Exception("Неверный пароль");
                }
            }
            throw new Exception("Пользователь не зарегистрирован");
        }

        public bool IsMailRegistered(string Mail)
        {
            for (int i = 0; i < List.Count; i++)
            {
                if (List[i].IsMailRegistered(Mail)) return true;
            }
            return false;
        }

        public int Remove(int Index)
        {
            List.RemoveAt(Index);
            return Index;
        }

        public int GetIndex(uint UserID)
        {
            for (int i = 0; i < List.Count; i++)
            {
                if (List[i].ID == UserID) return i;
            }
            return 0;
        }
    }
}
