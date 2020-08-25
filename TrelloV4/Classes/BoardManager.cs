using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace TrelloV0._0._3
{
    class BoardManager
    {
        [JsonProperty("List")]
        public List<Board> List;
        [JsonProperty("counter")]
        private uint counter;
        private static BoardManager boards;

        private BoardManager()
        {
            List = new List<Board>();
        }

        public static BoardManager Instance
        {
            get
            {
                if (boards == null)
                    boards = new BoardManager();
                return boards;
            }
        }
       
        public bool Add(uint СreatorID, string Name)
        {
            List.Add(new Board(counter, СreatorID, Name));
            counter++;
            return true;
        }
      
        public string Get(uint id)
        {
            
            string s = "";
            for (int i = 0; i < List.Count; i++)
            {
                s += (i+1)+ ". " + List[i].GetName(id)+"\n";
            }
            return s;
            
        }

        public bool СheckingUserDeletion(int id)
        {
            for (int i = 0; i < List.Count; i++)
            {
                if (List[i].creatorID == id)
                return true;
            }

            return false;
        }

        public bool DeletingUserData(uint id)
        {
            bool ans = false;
            for (int i = List.Count-1; i >= 0; i--)
            {
                if (List[i].creatorID == id)
                {
                    List.RemoveAt(i);
                    ans = true;
                }
                else
                {
                    for (int j = List[i].tasks.Count-1; j >= 0; j--)
                    {
                        if (List[i].tasks[j].idCreator == id)
                        {
                            List[i].tasks.RemoveAt(i);
                            ans = true;
                        }
                        else {
                            if (List[i].tasks[j].idPerformer == id)
                            {
                                List[i].tasks[j].idPerformer = 0;
                                List[i].tasks[j].Create();


                                ans = true;
                            }
                        }
                        
                    }
                }
                    
            }

            return ans;
        }
    }
}
