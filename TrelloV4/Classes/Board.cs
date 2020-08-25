using Newtonsoft.Json;
using System.Collections.Generic;

namespace TrelloV0._0._3
{
    class Board
    {
        [JsonProperty("creatorID")]
        public uint creatorID;
        [JsonProperty("boardName")]
        private string boardName;
        [JsonProperty("tasks")]
        public List<Task> tasks;
        [JsonProperty("id")]
        private uint id;
        [JsonProperty("counter")]
        private uint counter;



        public Board(uint id, uint СreatorID, string name)
        {
            counter = 0;
            tasks = new List<Task>();
            this.creatorID = СreatorID;
            this.id = id;
            this.boardName = name;
        }

        public void Add(Task task)
        {
            tasks.Add(task);
            counter++;
        }

        public bool IsIdRegistered(uint id)
        {
            if (this.id == id) return true;
            return false;
        }

        public string GetName(uint id)
        {
            string s = "";
            s += boardName;
            if (this.creatorID == id) s += "(Создатель ВЫ)";
            return s;
        }

        public string GetTasksName(uint id)
        {

            string s = "";
            for (int i = 0; i < tasks.Count; i++)
            {
                s += (i + 1) + ". " + tasks[i].GetName(id);
            }
            return s;

        }
        public List<Task> GetTasks(uint id)
        {
            List<Task> list = new List<Task>();
            for (int i = 0; i < tasks.Count; i++)
            {
                if (id == tasks[i].idCreator || id == tasks[i].idPerformer)
                    list.Add(tasks[i]);
            }
            return list;

        }

        public bool Edit(uint creatorID, string name)
        {
            if (creatorID == this.creatorID)
            {
                this.boardName = name;
                return true;
            }
            return false; 
        }

        public bool ВeletingUserData(int ID)
        {

            return false;
        }
    }
}
