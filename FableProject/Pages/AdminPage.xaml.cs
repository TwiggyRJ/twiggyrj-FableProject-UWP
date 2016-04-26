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
    public sealed partial class AdminPage : Page
    {
        public AdminPage()
        {
            this.InitializeComponent();

            getAuthors();
        }

        private void postChangelog(object sender, RoutedEventArgs e)
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
            else
            {
                usernameDetails = storage.LoadSettings(uDataKey);
                passwordDetails = storage.LoadSettings(pDataKey);
            }

            string version= versionField.Text;
            string aboutVersion = objectiveField.Text;
            string versionContent_1 = updateItem1.Text;
            string versionContent_2 = updateItem2.Text;
            string versionContent_3 = updateItem3.Text;
            string versionContent_4 = updateItem4.Text;
            string versionContent_5 = updateItem5.Text;
            string versionContent_6 = updateItem6.Text;

            DateTime rawDate = updatedField.Date.DateTime;
            string updated = rawDate.ToString("yyyyMMdd");

            if (version != null || aboutVersion != null || versionContent_1 != null || updated != null)
            {
                sendPage(usernameDetails, passwordDetails, "", version, aboutVersion, updated, versionContent_1, versionContent_2, versionContent_3, versionContent_4, versionContent_5, versionContent_6);
            }
            else
            {
                var title = "Error with adding the Changelog";
                var message = "You have left some fields blank and it makes me sad :(";
                feedbackDialog(title, message);
            }
        }

        private void sendPage(string username, string password, string target, string version, string aboutVersion, string updated, string versionContent_1, string versionContent_2, string versionContent_3, string versionContent_4, string versionContent_5, string versionContent_6)
        {

            var client = new HttpClient();

            var postData = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("version", version),
                new KeyValuePair<string, string>("about", aboutVersion),
                new KeyValuePair<string, string>("updated", updated),
                new KeyValuePair<string, string>("content_1", versionContent_1),
                new KeyValuePair<string, string>("content_2", versionContent_2),
                new KeyValuePair<string, string>("content_3", versionContent_3),
                new KeyValuePair<string, string>("content_4", versionContent_4),
                new KeyValuePair<string, string>("content_5", versionContent_5),
                new KeyValuePair<string, string>("content_6", versionContent_6),
                new KeyValuePair<string, string>("action", "post")
            };

            var content = new FormUrlEncodedContent(postData);

            var credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes(string.Format("{0}:{1}", username, password)));

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", credentials);

            // call sync
            var response = client.PostAsync(target, content).Result;
            if (response.IsSuccessStatusCode)
            {
                var title = "New Changelog Added";
                var message = "Your new Changelog has been added, Awesome! :D";
                feedbackDialog(title, message);
            }
            else
            {
                var title = "Error with adding the Changelog";
                var message = "Unable to add your Changelog Unfortunately :(";
                feedbackDialog(title, message);
            }
        }

        private void getAuthors()
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
            else
            {
                usernameDetails = storage.LoadSettings(uDataKey);
                passwordDetails = storage.LoadSettings(pDataKey);
            }

            createURI(usernameDetails, passwordDetails);

        }


        private async void createURI(string username, string password)
        {
            //This is initiated if a registration event has been initiated

            //Creates an instance of a HTTP Client so the Web Service can be interacted with
            var client = new HttpClient();

            //Create a query string that will be appended to the URL
            var uri = UriExtensions.CreateUriWithQuery(new Uri("http://www.kshatriya.co.uk/dev/project/service/audit.php"),
            new NameValueCollection { { "action", "get" } },
            new NameValueCollection { { "method", "User_Dump" } });

            //Sets the Authentication header to the values of the username and passwords and converts them to Base64 encryption
            var credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes(string.Format("{0}:{1}", username, password)));

            //Sets the Authentication header
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", credentials);

            // activates the request to the Web Service
            var response = client.GetAsync(uri).Result;

            //Makes the responseString variable set and available for use in the loop
            var responseString = "";

            //Checks if the response from the client is successful
            if (response.IsSuccessStatusCode)
            {
                //It is successful get the JSON response and save it to a varaible and then send the JSON reponse to the authenticated function
                responseString = await response.Content.ReadAsStringAsync();
                var userData = new UserDataSource(responseString);

                this.DataContext = userData;
            }
            else
            {
                //Tell the user the request was not successful
                var title = "Error with Authentication";
                var message = "Unable to login, Username or Password was incorrect";
                feedbackDialog(title, message);
            }

        }


        private void feedbackDialog(string title, string message)
        {
            object sender = null;
            int commands = 1;
            Dialog.standardDialog(title, message, commands, sender);

        }

    }
}
