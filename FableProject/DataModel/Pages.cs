using FableProject.Data;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FableProject.DataModel
{
    public class Pages
    {
        public string ID { get; set; }

        public string Story { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public string Content_2 { get; set; }

        public string Number { get; set; }

        public string Interaction { get; set; }

        public string Interaction_Type { get; set; }

        public string Easy_Interaction { get; set; }

        public string Easy_Interaction_Answer { get; set; }

        public string Medium_Interaction { get; set; }

        public string Medium_Interaction_Answer { get; set; }

        public string Hard_Interaction { get; set; }

        public string Hard_Interaction_Answer { get; set; }

        public string Humour_Interaction { get; set; }

        public string Humour_Interaction_Answer { get; set; }

        public string option1 { get; set; }

        public string option1_Dest { get; set; }

        public string option2 { get; set; }

        public string option2_Dest { get; set; }

        public string optionSpecialSuccess { get; set; }

        public string optionSpecialFailure { get; set; }

        public string First { get; set; }

        public string modContent_2 { get; set; }
        public string modInteraction { get; set; }
        public string modQuestion { get; set; }
        public string modAnswer { get; set; }
        public string modInteractionMockery { get; set; }
    }

    public class PagesSorted
    {

        public string Title { get; set; }
        public List<Pages> Pages { get; set; }

    }

    public class PagesDataSource
    {

        public List<PagesSorted> Pages { get; set; }

        public PagesDataSource(string JSON)
        {


            string pageDataKey = "pageDetails";
            string rDatakey = "roamingDetails";
            string pageStoryTitle = "pageStoryTitleDetails";
            string pageTitleIDDataKey = "pageTitleIDDetails";
            string dfDatakey = "dateFormatDetails";
            string gameDFDatakey = "difficultyDetails";

            Storage storage = new Storage();

            string roamingSetting = storage.LoadSettings(rDatakey);
            string dateSetting = "";
            string difficultySetting = "";

            if (roamingSetting == "true")
            {
                dateSetting = storage.LoadRoamingSettings(dfDatakey);
                difficultySetting = storage.LoadRoamingSettings(gameDFDatakey);
            }
            else if (roamingSetting == "false")
            {
                dateSetting = storage.LoadSettings(dfDatakey);
                difficultySetting = storage.LoadSettings(gameDFDatakey);
            }
            else if (roamingSetting == "Null")
            {
                dateSetting = storage.LoadSettings(dfDatakey);
                difficultySetting = storage.LoadSettings(gameDFDatakey);
            }

            List<Pages> pages = JsonConvert.DeserializeObject<List<Pages>>(JSON);

            string questionPrepend = "Solve this "+ pages[0].Interaction_Type + ": ";
            string insult = "";


            if (difficultySetting == "0")
            {
                pages[0].modQuestion = questionPrepend + pages[0].Easy_Interaction;
                pages[0].modAnswer = pages[0].Easy_Interaction_Answer;
                insult = "Really??? I mean Really? That was so easy and you have a minute and a half!";
            }
            else if (difficultySetting == "1")
            {
                pages[0].modQuestion = questionPrepend + pages[0].Medium_Interaction;
                pages[0].modAnswer = pages[0].Medium_Interaction_Answer;
                insult = "you look even more average!";
            }
            else if (difficultySetting == "2" || difficultySetting == "3")
            {
                pages[0].modQuestion = questionPrepend + pages[0].Hard_Interaction;
                pages[0].modAnswer = pages[0].Hard_Interaction_Answer;
                insult = "your chance to win big.";
            }
            else if (difficultySetting == "4")
            {
                pages[0].modQuestion = questionPrepend + pages[0].Humour_Interaction;
                pages[0].modAnswer = pages[0].Humour_Interaction_Answer;
                insult = "you should get your mind out of the gutter!";
            }

            pages[0].modContent_2 = pages[0].Content_2 + "...";
            pages[0].modInteraction = pages[0].Interaction + "...";
            pages[0].modInteractionMockery = "Wow... The answer was " + pages[0].modAnswer + "! Unfortunately you have lost your pride and " + insult;

            var pagesByTitle = pages.GroupBy(x => x.Title)
                                .Select(x => new PagesSorted { Title = x.Key, Pages = x.ToList() });

            Pages = pagesByTitle.ToList();

            if (roamingSetting == "true")
            {
                storage.SaveRoamingSettings(pageDataKey, JSON);
                storage.SaveRoamingSettings(pageStoryTitle, pages[0].Story);
                storage.SaveRoamingSettings(pageTitleIDDataKey, pages[0].Title);
            }
            else if (roamingSetting == "false")
            {
                storage.SaveSettings(pageDataKey, JSON);
                storage.SaveSettings(pageStoryTitle, pages[0].Story);
                storage.SaveSettings(pageTitleIDDataKey, pages[0].Title);
            }
            else if (roamingSetting == "Null")
            {
                storage.SaveSettings(pageDataKey, JSON);
                storage.SaveSettings(pageStoryTitle, pages[0].Story);
                storage.SaveSettings(pageTitleIDDataKey, pages[0].Title);
            }

        }
    }
}
