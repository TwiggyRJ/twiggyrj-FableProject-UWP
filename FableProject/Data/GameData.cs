using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FableProject.Data
{
    public class GameDataFormat
    {
        public string ID { get; set; }
        public string Story { get; set; }
        public string PastPages { get; set; }
        public string CurrentPage { get; set; }
        public string Items { get; set; }
    }

    class GameData
    {

        Storage storage = new Storage();

        public string rDatakey = "roamingDetails";
        string sgDataKey = "saveGameDetails";
        string saveData = "";


        public List<string> ProgressData = new List<string>();


        public string LoadData(string id)
        {

            string roamingSetting = storage.LoadSettings(rDatakey);

            if (roamingSetting == "true")
            {
                saveData = storage.LoadRoamingSettings(sgDataKey);
            }
            else
            {
                saveData = storage.LoadSettings(sgDataKey);
            }

            //ProgressData = saveData.ToList<GameDataFormat>;

            return saveData;
        }

        public void SaveData(string id, string story, string pastpages, string page, string item)
        {

            string roamingSetting = storage.LoadSettings(rDatakey);

            if (id == "1")
            {
                sgDataKey = "saveGameDetails";
            }
            else if (id == "2")
            {
                sgDataKey = "saveGameDetails2";
            }
            else if(id == "3")
            {
                sgDataKey = "saveGameDetails3";
            }
            else if(id == "4")
            {
                sgDataKey = "saveGameDetails4";
            }
            else if(id == "5")
            {
                sgDataKey = "saveGameDetails5";
            }
            else
            {
                sgDataKey = "saveGameDetails";
            }

            ProgressData.Add(id);
            ProgressData.Add(story);
            ProgressData.Add(pastpages);
            ProgressData.Add(page);
            ProgressData.Add(item);

            /*
            ProgressData[0].ID = id;
            ProgressData[0].Story = story;
            ProgressData[0].PastPages = ProgressData[0].PastPages + "/" + page;
            ProgressData[0].CurrentPage = page;
            ProgressData[0].Items = ProgressData[0].Items + "/" + item;
            */

            StringBuilder builder = new StringBuilder();
            foreach (string saveData in ProgressData) // Loop through all strings
            {
                builder.Append(saveData).Append("|"); // Append string to StringBuilder
            }
            string result = builder.ToString();

            if (roamingSetting == "true")
            {
                storage.SaveRoamingSettings(sgDataKey, result);
            }
            else
            {
                storage.SaveSettings(sgDataKey, result);
            }

        }
    }
}
