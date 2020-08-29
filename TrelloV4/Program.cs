    using Newtonsoft.Json;
using System;
using System.IO;
using TrelloV0._0._3;

namespace TrelloV4
{
    class ClientManager
    {
        public delegate void ClientHandler(string message);
        public event ClientHandler Notify;

        public User User;

        public BoardManager boardManager;

        public ClientManager(User User)
        {
            this.User = User;
            boardManager = JsonConvert.DeserializeObject<BoardManager>(File.ReadAllText("boards.json"));
            Notify += DisplayMessage;
            while (User.Messages.Count != 0)
            {
                Notify?.Invoke($"{User.Messages[0]}");
                User.Messages.RemoveAt(0);
            }
        }

        private static void DisplayMessage(string message)
        {
            Console.WriteLine(message);
            Console.ReadLine();
        }

        public void EditUser()
        {
            Console.WriteLine("Введите фамилию");
            string FirstName = Console.ReadLine();
            Console.WriteLine("Введите имя");
            string LastName = Console.ReadLine();
            Console.WriteLine("Введите отчество");
            string MiddleName = Console.ReadLine();
            this.User.Update(FirstName, LastName, MiddleName);
        }

        public void Save()
        {
            File.WriteAllText("boards.json", JsonConvert.SerializeObject(boardManager));
        }

        public void Open()
        {
            Console.WriteLine();
        }
    }
    
    class Program
    {
        static void Main(string[] args)
        {
            UserManager userManager = UserManager.Instance;
            userManager = JsonConvert.DeserializeObject<UserManager>(File.ReadAllText("users.json"));
            userManager.Add("Володин", "Николай","Юрьевич", "urkona@list.ru", "1111");
            File.WriteAllText("users.json", JsonConvert.SerializeObject(userManager));
            
            try
            {
                userManager = JsonConvert.DeserializeObject<UserManager>(File.ReadAllText("users.json"));
                //Если пароль или логин неверный то в e.Message будет сообщение об этом 
                ClientManager client = new ClientManager(userManager.Entry("urkona@list.ru", "1121"));
                File.WriteAllText("users.json", JsonConvert.SerializeObject(userManager));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            //неверный логин
            try
            {
                userManager = JsonConvert.DeserializeObject<UserManager>(File.ReadAllText("users.json"));
                //Если пароль или логин неверный то в e.Message будет сообщение об этом 
                ClientManager client = new ClientManager(userManager.Entry("urkona@list", "1111"));
                File.WriteAllText("users.json", JsonConvert.SerializeObject(userManager));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

        }
    }
}
