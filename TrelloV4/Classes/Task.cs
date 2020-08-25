using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;

namespace TrelloV0._0._3
{
    class Task
    {
        public delegate void TaskHandler(string message);
        public event TaskHandler Notify;

        [JsonProperty("name")]
        private string name { get; set; }
        [JsonProperty("text")]
        private string text { get; set; }
        [JsonProperty("idCreator")]
        public uint idCreator;
        [JsonProperty("status")]
        private TaskStatus status
        {
            get; set;
        }

        [JsonProperty("idPerformer")]
        public uint idPerformer;

        public List<uint> SubscribersID;

        public bool Subscribe(uint UserID)
        {
            if (!SubscribersID.Contains(UserID))
            {
                SubscribersID.Add(UserID);
                return true;
            }
            return false; 
        }
        
        public bool Unsubscribe(uint UserID)
        {
            if (SubscribersID.Contains(UserID))
            {
                SubscribersID.Remove(UserID);
                return true;
            }
            return false; 
        }

        public string GetName(uint id)
        {
            string s = "";
            if (idCreator == id) s += "+";
            else s += "-";

            if (idPerformer == id) s += "+";
            else s += "-";

            s += "\"" + name + " \"\n";
            s += "\tСтатус задачи - "+((TaskStatus) Convert.ToInt32(this.status)) + " ";
            return s+"\n";
        }

        public void OutMessgae(string Message)
        {
            if (SubscribersID.Count != 0)
            {
                UserManager userManager = UserManager.Instance;
                userManager = JsonConvert.DeserializeObject<UserManager>(File.ReadAllText("users.json"));
                for (int i = 0; i < SubscribersID.Count; i++)
                {
                    for (int j = 0; j < userManager.List.Count; j++)
                    {
                        if (SubscribersID[i]== userManager.List[j].ID) userManager.List[j].Messages.Add(Message);
                    }
                }
                File.WriteAllText("users.json", JsonConvert.SerializeObject(userManager));
            }
        }

        public Task(string name, string text, uint idCreator)
        {
            this.idCreator = idCreator;
            this.name = name;
            this.text = text;
            status = TaskStatus.Create;
            this.idPerformer = 0;
            SubscribersID = new List<uint>();
            SubscribersID.Add(idCreator);
        }

        public bool Take(uint ID)
        {
            if (ID != 0)
            {
                if ((this.status == TaskStatus.Create) || (this.status == TaskStatus.Fix))
                {
                    this.status = TaskStatus.Process;
                    idPerformer = ID;
                    OutMessgae($"Задача {name} переведена на выполнение");
                    //Notify?.Invoke($"Задача {name} переведена на выполнение");
                    return true;
                }
            }
            return false;
        }
      
        public bool Check(uint ID)
        {
            if (idPerformer != 0 && (ID == idCreator || ID == idPerformer))
            {
                if ((this.status == TaskStatus.Process) || (this.status == TaskStatus.Fix))
                {
                    this.status = TaskStatus.Check;
                    idPerformer = ID;
                    OutMessgae($"Задача {name} переведена на проверку");
                   // Notify?.Invoke($"Задача {name} переведена на проверку");
                    return true;
                }
            }
            return false;
        }



        public bool Fix(uint ID)
        {
            if (idPerformer != 0 && (ID == idCreator || ID == idPerformer))
            {
                this.status = TaskStatus.Fix;
                idPerformer = ID;
                OutMessgae($"Задача {name} переведена в исправление");
                //Notify?.Invoke($"Задача {name} переведена в исправление");
                return true;   
            }
            return false;
        }

        public bool Archive(uint ID)
        {
            if (idPerformer != 0 && ID == idCreator)
            {
                this.status = TaskStatus.Archive;
                idPerformer = ID; 
                OutMessgae($"Задача {name} переведена в архив");
                //Notify?.Invoke($"Задача {name} переведена в архив");
                return true;   
            }
            return false;
        }

        [JsonConstructor]
        public Task(string name, string text, uint idCreator, TaskStatus status,uint idPerformer)
        {
            this.idCreator = idCreator;
            this.name = name;
            this.text = text;
            this.status = status;
            this.idPerformer = idPerformer;
        }
        
        public void Edit(string name, string text)
        {
            this.name = name;
            this.text = text;
        }
        
        public bool Create()
        {            
            if (this.status != TaskStatus.Create)
            {
                this.status = TaskStatus.Create;
                return true;
            }
            return false;
        }
        
        public bool IsChooseStatusRight(TaskStatus comStatus)
        {
            if (comStatus == status) return true;
            return false;
        }
    }
}
