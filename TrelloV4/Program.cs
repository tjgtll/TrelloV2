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
            Console.WriteLine(userManager.Add("Володин", "Николай","Юрьевич", "urkona@list.ru", "1111"));
            File.WriteAllText("users.json", JsonConvert.SerializeObject(userManager));
            //ну например есть пользователь, при первом заходе сообщений 0
            Console.WriteLine("Первый проход");
            try
            {
                userManager = JsonConvert.DeserializeObject<UserManager>(File.ReadAllText("users.json"));
                ClientManager client = new ClientManager(userManager.Entry("urkona@list.ru", "1111"));
                File.WriteAllText("users.json", JsonConvert.SerializeObject(userManager));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }



            BoardManager boardManager = BoardManager.Instance;
            boardManager.Add(1, "доска 1");
            boardManager.List[0].Add(new Task("задача 1", "Тект задача 1", 1));
            boardManager.List[0].tasks[0].Take(1);
            boardManager.List[0].tasks[0].Check(1);
            Console.WriteLine("Второй проход");
            try
            {
                userManager = JsonConvert.DeserializeObject<UserManager>(File.ReadAllText("users.json"));
                ClientManager client = new ClientManager(userManager.Entry("urkona@list.ru", "1111"));
                File.WriteAllText("users.json", JsonConvert.SerializeObject(userManager));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
