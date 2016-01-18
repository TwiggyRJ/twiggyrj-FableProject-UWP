using FableProject.Data;
using FableProject.DataModel;
using FableProject.Functions;
using FableProject.Presentation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using Windows.ApplicationModel.Email;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace FableProject.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AuthorPage : Page
    {
        public AuthorPage()
        {
            this.InitializeComponent();
            storyGet("The Tale of Bright Peak Barrow", "http://www.kshatriya.co.uk/dev/project/service/stories.php");
        }

        private void storyHelper(object sender, RoutedEventArgs e)
        {
            string title = "Creating Your Story";
            string messageInitial = "Complete the {0} form to create the Story, this will need to be done before you can create your pages you need to create your story and select the story you want to add the page to.";
            string context = "Story";
            string message = string.Format(messageInitial, context);


            helperBox(title, message, context);
        }


        private void helperBox(string title, string message, string context)
        {
            string template = "To create a new {0} you will need to go to the {1} page. {2}";
            string finalisedMessage = string.Format(template, context, context, message);

            feedbackDialog(title, finalisedMessage);
        }

        //Dialog Box

        private async void feedbackDialog(string title, string message)
        {
            object sender = null;
            int commands = 1;
            Dialog.standardDialog(title, message, commands, sender);

        }

        private void createStory(object sender, RoutedEventArgs e)
        {

            Storage storage = new Storage();

            string uDataKey = "usernameDetails";
            string pDataKey = "passwordDetails";
            string rDataKey = "roamingDetails";

            string roamingSetting = storage.LoadSettings(rDataKey);

            string usernameDetails = "";
            string passwordDetails = "";

            if (roamingSetting == "true")
            {
                usernameDetails = storage.LoadRoamingSettings(uDataKey);
                passwordDetails = storage.LoadRoamingSettings(pDataKey);
            }
            else if (roamingSetting == "false")
            {
                usernameDetails = storage.LoadSettings(uDataKey);
                passwordDetails = storage.LoadSettings(pDataKey);
            }
            else if (roamingSetting == "Null")
            {
                usernameDetails = storage.LoadSettings(uDataKey);
                passwordDetails = storage.LoadSettings(pDataKey);
            }


            string storyTitle = storyTitleBox.Text;
            string storyDesc = Convert.ToString(storyDescriptionTextBox.Document);
            string storyCat = ((ComboBoxItem)storyCategory.SelectedItem).Content.ToString();
            string storyImage = storyImageBox.Text;

            string title = "New Story";

            if (storyTitle == "")
            {
                string messageDetails = "not entered anything";
                string template = "You have {0} for the title for the story, please complete the form.";
                string message = string.Format(template, messageDetails);
                feedbackDialog(title, message);
            }
            else if (storyTitle == null)
            {
                string messageDetails = "not entered anything";
                string template = "You have {0} for the title for the story, please complete the form.";
                string message = string.Format(template, messageDetails);
                feedbackDialog(title, message);
            }
            else
            {
                if (usernameDetails != "Null" && passwordDetails != "Null")
                {
                    sendStory(usernameDetails, passwordDetails, "http://www.kshatriya.co.uk/dev/project/service/stories.php", storyTitle, storyDesc, storyCat, storyImage);
                }
            }
        }

        private void sendStory(string username, string password, string target, string storyTitle, string storyDesc, string storyCat, string storyImage)
        {

            var client = new HttpClient();

            var postData = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("title", storyTitle),
                new KeyValuePair<string, string>("description", storyDesc),
                new KeyValuePair<string, string>("type", storyCat),
                new KeyValuePair<string, string>("image", storyImage),
                new KeyValuePair<string, string>("action", "post")
            };

            var content = new FormUrlEncodedContent(postData);

            var credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes(string.Format("{0}:{1}", username, password)));

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", credentials);

            // call sync
            var response = client.PostAsync(target, content).Result;
            if (response.IsSuccessStatusCode)
            {
                storyGet(storyTitle, target);
            }
            else
            {
                var title = "Error with adding the Story";
                var message = "Unable to add your Story Unfortunately :(";
                feedbackDialog(title, message);
            }
        }

        private async void storyGet(string title, string target)
        {

            Storage storage = new Storage();

            string idDataKey = "userIdDetails";
            string rDataKey = "roamingDetails";

            string roamingSetting = storage.LoadSettings(rDataKey);
            string userIDDetails = "";

            if (roamingSetting == "true")
            {
                userIDDetails = storage.LoadRoamingSettings(idDataKey);
            }
            else if (roamingSetting == "false")
            {
                userIDDetails = storage.LoadSettings(idDataKey);
            }
            else if (roamingSetting == "Null")
            {
                userIDDetails = storage.LoadSettings(idDataKey);
            }

            var client = new HttpClient();

            var uri = UriExtensions.CreateUriWithQuery(new Uri(target),
                new NameValueCollection { { "action", "get" } },
                new NameValueCollection { { "story", title } },
                new NameValueCollection { { "special", userIDDetails } });

            // call sync
            var response = client.GetAsync(uri).Result;
            var responseString = "";

            if (response.IsSuccessStatusCode)
            {
                responseString = await response.Content.ReadAsStringAsync();
                configuringPages(responseString);
            }
            else
            {
                var dialogTitle = "Error with finding the Story";
                var message = "Unable to find your Story Unfortunately :(";
                feedbackDialog(dialogTitle, message);
            }
        }

        private void configuringPages(string JSON)
        {

            if (JSON != "")
            {

                var storiesData = new StoriesDataSource(JSON, "new");
                this.DataContext = storiesData;

                string createdStoryTitleDataKey = "createdStoryTitleDetails";
                string createdStoryIDDataKey = "createdStoryIDDetails";
                string rDatakey = "roamingDetails";

                string storyTitleData = "";
                string storyIDData = "";

                Storage storage = new Storage();

                string roamingSetting = storage.LoadSettings(rDatakey);

                if (roamingSetting == "true")
                {
                    storyTitleData = storage.LoadRoamingSettings(createdStoryTitleDataKey);
                    storyIDData = storage.LoadRoamingSettings(createdStoryIDDataKey);
                }
                else if (roamingSetting == "false")
                {
                    storyTitleData = storage.LoadSettings(createdStoryTitleDataKey);
                    storyIDData = storage.LoadSettings(createdStoryIDDataKey);
                }
                else if (roamingSetting == "Null")
                {
                    storyTitleData = storage.LoadSettings(createdStoryTitleDataKey);
                    storyIDData = storage.LoadSettings(createdStoryIDDataKey);
                }

                if(storyTitleData != "Null" && storyIDData != "Null")
                {
                    selectedStoryDrop.Content = storyTitleData;
                    selectedStoryDrop.Tag = storyIDData;
                }
                

            }
            else
            {
                var dialogTitle = "Error displaying the Story";
                var message = "Unable to display the story unfortuantely :(";
                feedbackDialog(dialogTitle, message);
            }
        }
    }
}
