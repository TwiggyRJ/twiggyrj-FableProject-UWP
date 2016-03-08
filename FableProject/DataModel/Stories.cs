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

        public string modRecommended { get; set; }

        public string modDate { get; set; }

        public DateTime created { get; set; }

        public string vis { get; set; }

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

            if(stories[0].Recommended == "0")
            {
                stories[0].modRecommended = "Nobody has recommended it :(";
            }
            else if(stories[0].Recommended == "1")
            {
                stories[0].modRecommended = "Yay! 1 person has recommended this story! Well it's better than nothing...";
            }
            else
            {
                stories[0].modRecommended = stories[0].Recommended + " Times";
            }

            stories[0].modDate = stories[0].created.ToString("dddd d MMMM yyyy");

            

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
