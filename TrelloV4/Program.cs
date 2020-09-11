using Newtonsoft.Json;
using System;
using System.IO;
using TrelloV0._0._3;
using System.Threading;
using System.Threading.Tasks;
using System.Text;

namespace TrelloV4
{
    class Manager
    {
        UserManager userManager;

        ClientManager client;

        public Manager()
        {
            System.Threading.Tasks.Task asyncTask = WriteText("Старт");
            userManager = UserManager.Instance;

            userManager = JsonConvert.DeserializeObject<UserManager>(File.ReadAllText("users.json")) != null ? JsonConvert.DeserializeObject<UserManager>(File.ReadAllText("users.json")) : userManager;
        }

        static async System.Threading.Tasks.Task WriteText(string content)
        {
            string file = "logger.txt";
            char[] Content = Encoding.Default.GetChars(Encoding.Default.GetBytes($"{DateTime.Now} {content}"));
            using (StreamWriter outputFile = new StreamWriter(file, true))
            {
                await outputFile.WriteLineAsync(Content, 0, Content.Length);
            }
        }

        public  bool Registration(string FirstName, string LastName, string MiddleName, string Mail, string Password)
        {
            System.Threading.Tasks.Task asyncTask;
            if (userManager.Add(FirstName, LastName, MiddleName, Mail, Password))
            {
                File.WriteAllText("users.json", JsonConvert.SerializeObject(userManager));
                asyncTask = WriteText($"Регистрация true: \nfN: {FirstName}, \nlN: {LastName}, \nmN: {MiddleName}, \nm: {Mail}, \np: {Password}");
                return true;
            }
            asyncTask = WriteText($"Регистрация false: \nfN: {FirstName}, \nlN: {LastName}, \nmN: {MiddleName}, \nm: {Mail}, \np: {Password}");
            return false;

        }
        
        public bool Entry(string Mail, string Password)
        {
            System.Threading.Tasks.Task asyncTask;
            try
            {
                ClientManager client = new ClientManager(userManager.Entry(Mail, Password));
                asyncTask = WriteText($"Вход m:{Mail}");
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                asyncTask = WriteText($"Fatal: {e.Message}");
            }
            return false;
        }

    }

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
            Manager manager = new Manager();
            
            Console.WriteLine(manager.Registration("Володин", "Николай", "Юрьевич", "urkona@list.ru", "1111"));

            Console.WriteLine(manager.Entry("urkona@list.ru", "1111"));

        }
    }
}