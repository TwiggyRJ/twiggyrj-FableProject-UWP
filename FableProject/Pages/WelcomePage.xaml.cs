using FableProject.Data;
using FableProject.DataModel;
using FableProject.Functions;
using FableProject.Presentation;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
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
    public sealed partial class WelcomePage : Page
    {
        public WelcomePage()
        {
            this.InitializeComponent();

            searchProgressRing.IsActive = true;
            searchUpdates("http://www.kshatriya.co.uk/dev/project/service/updates.php", "latest");
        }

        private async void searchUpdates(string target, string toGet)
        {

            var client = new HttpClient();

            var uri = UriExtensions.CreateUriWithQuery(new Uri(target),
            new NameValueCollection { { "method", toGet } });

            // call sync
            var response = client.GetAsync(uri).Result;
            var responseString = "";

            if (response.IsSuccessStatusCode)
            {
                responseString = await response.Content.ReadAsStringAsync();
                getSearchResults(responseString);
            }
            else
            {
                searchProgressRing.IsActive = false;
                var title = "Error with Application";
                var message = "It's not you, it's me! Unfortuantely there is an error connecting with the Fable Time Service";
                errorDialog(title, message);
            }
        }

        private async void searchStories(string target, string toGet)
        {

            var client = new HttpClient();

            var uri = UriExtensions.CreateUriWithQuery(new Uri(target),
            new NameValueCollection { { "method", toGet } });

            // call sync
            var response = client.GetAsync(uri).Result;
            var responseString = "";

            if (response.IsSuccessStatusCode)
            {
                responseString = await response.Content.ReadAsStringAsync();
                notifyStory(responseString);
            }
            else
            {
                searchProgressRing.IsActive = false;
                var title = "No Search Results :(";
                var message = "Uh, Oh, Spadoodios! We could not find any results for your search. We feel sad now...";
                errorDialog(title, message);
            }
        }

        private void notifyStory(string JSON)
        {

            string rDatakey = "roamingDetails";
            string onloadDatakey = "onloadDetails";
            string uDataKey = "usernameDetails";

            Storage storage = new Storage();

            string roamingSetting = storage.LoadSettings(rDatakey);
            string name = "";
            string shown = "";

            if (roamingSetting == "true")
            {
                name = storage.LoadRoamingSettings(uDataKey);
                shown = storage.LoadRoamingSettings(onloadDatakey);
            }
            else
            {
                name = storage.LoadSettings(uDataKey);
                shown = storage.LoadSettings(onloadDatakey);
            }

            List<Stories> stories = JsonConvert.DeserializeObject<List<Stories>>(JSON);

            if (shown != stories[0].Title)
            {
                if (name == "Null")
                {
                    name = "Anon";
                }

                
                string greeting = "Hey " + name + "!";
                string message = "Our Latest Story is: " + stories[0].Title + "\nCreated by: " + stories[0].OwnerName + ". \nHave a read, we hope you enjoy it!";

                Notifications.standardToast(greeting, message, "app-defined-string");
                Notifications.standardTileNotification("Newest Story: " + stories[0].Title + "\n", stories[0].Description);

                if (roamingSetting == "true")
                {
                    storage.SaveRoamingSettings(onloadDatakey, stories[0].Title);
                }
                else
                {
                    storage.SaveSettings(onloadDatakey, stories[0].Title);
                }
            }

            searchProgressRing.IsActive = false;

            string slDataKey = "saveGameSlot";
            string slot = "";

            if (roamingSetting == "true")
            {
                slot = storage.LoadRoamingSettings(slDataKey);
            }
            else
            {
                slot = storage.LoadSettings(slDataKey);
            }

            if (slot != "1" || slot != "2" || slot != "3" || slot != "4" || slot != "5")
            {
                ContinueGrid.Visibility = Visibility.Collapsed;
            }

        }

        private void errorDialog(string title, string messageDetails)
        {
            object sender = null;
            string message = messageDetails;
            int commands = 1;
            Dialog.standardDialog(title, message, commands, sender);

        }

        private void getSearchResults(string JSON)
        {
            var viewModel = new UpdatesDataSource(JSON);
            this.DataContext = viewModel;

            string rDatakey = "roamingDetails";
            string onloadUpdateDatakey = "onloadUpdateDetails";

            Storage storage = new Storage();

            string roamingSetting = storage.LoadSettings(rDatakey);
            string shown = "";

            if (roamingSetting == "true")
            {
                shown = storage.LoadRoamingSettings(onloadUpdateDatakey);
            }
            else
            {
                shown = storage.LoadSettings(onloadUpdateDatakey);
            }

            List<Updates> updates = JsonConvert.DeserializeObject<List<Updates>>(JSON);

            if (shown != updates[0].Version)
            {

                updates[0].modTitle = "What's New in " + updates[0].Version + ":";
                Notifications.standardTileNotification(updates[0].modTitle, updates[0].About);

                if (roamingSetting == "true")
                {
                    storage.SaveRoamingSettings(onloadUpdateDatakey, updates[0].Version);
                }
                else
                {
                    storage.SaveSettings(onloadUpdateDatakey, updates[0].Version);
                }
            }

            searchStories("http://www.kshatriya.co.uk/dev/project/service/tile.php", "latestStory");
            
        }

        private void resumePlay(object sender, RoutedEventArgs e)
        {
            Storage storage = new Storage();

            string rDatakey = "roamingDetails";
            string roamingSetting = storage.LoadSettings(rDatakey);
            string slDataKey = "saveGameSlot";
            string slot = "";

            if (roamingSetting == "true")
            {
                slot = storage.LoadRoamingSettings(slDataKey);
            }
            else
            {
                slot = storage.LoadSettings(slDataKey);
            }

            if (slot == "1" || slot == "2" || slot == "3" || slot == "4" || slot == "5")
            {
                Frame.Navigate(typeof(StoryPage), "resumePlay");
            }
        }
    }
}
