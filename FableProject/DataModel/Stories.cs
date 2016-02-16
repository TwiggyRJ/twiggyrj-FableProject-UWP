using FableProject.Data;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FableProject.DataModel
{
    public class Stories
    {

        public string ID { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string Type { get; set; }

        public string OwnerID { get; set; }

        public string OwnerName { get; set; }

        public string Image { get; set; }

        public string Recommended { get; set; }

    }

    public class StoriesSorted
    {
        public string Title { get; set; }
        public List<Stories> Stories { get; set; }

    }


    public class StoriesDataSource
    {

        public List<StoriesSorted> Stories { get; set; }

        public StoriesDataSource(string JSON, string method)
        {

            List<Stories> stories = JsonConvert.DeserializeObject<List<Stories>>(JSON);

            var storiesByTitle = stories.GroupBy(x => x.Title)
                                .Select(x => new StoriesSorted { Title = x.Key, Stories = x.ToList() });

            Stories = storiesByTitle.ToList();

            string storyDataKey = "storyDetails";
            string rDatakey = "roamingDetails";
            string createdStoryTitleDataKey = "createdStoryTitleDetails";
            string createdStoryIDDataKey = "createdStoryIDDetails";

            Storage storage = new Storage();

            string roamingSetting = storage.LoadSettings(rDatakey);

            if (roamingSetting == "true")
            {
                storage.SaveRoamingSettings(storyDataKey, JSON);
                if(method == "new")
                {
                    storage.SaveRoamingSettings(createdStoryTitleDataKey, stories[0].Title);
                    storage.SaveRoamingSettings(createdStoryIDDataKey, stories[0].ID);
                    
                }
                
            }
            else if (roamingSetting == "false")
            {
                storage.SaveSettings(storyDataKey, JSON);
                if (method == "new")
                {
                    storage.SaveSettings(createdStoryTitleDataKey, stories[0].Title);
                    storage.SaveSettings(createdStoryIDDataKey, stories[0].ID);
                }
            }
            else if (roamingSetting == "Null")
            {
                storage.SaveSettings(storyDataKey, JSON);
                if (method == "new")
                {
                    storage.SaveSettings(createdStoryTitleDataKey, stories[0].Title);
                    storage.SaveSettings(createdStoryIDDataKey, stories[0].ID);
                }
            }

        }
    }
}
