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
    public sealed partial class ProfilePage : Page
    {
        public ProfilePage()
        {
            this.InitializeComponent();

            Storage storage = new Storage();

            string sDataKey = "userDetails";
            string uDataKey = "usernameDetails";
            string pDataKey = "passwordDetails";
            string rDatakey = "roamingDetails";

            string roamingSetting = storage.LoadSettings(rDatakey);

            string userData = "Null";
            string usernameDetails = "Null";
            string passwordDetails = "Null";

            if (roamingSetting == "true")
            {
                userData = storage.LoadRoamingSettings(sDataKey);
                usernameDetails = storage.LoadRoamingSettings(uDataKey);
                passwordDetails = storage.LoadRoamingSettings(pDataKey);
            }
            else if (roamingSetting == "false")
            {
                userData = storage.LoadSettings(sDataKey);
                usernameDetails = storage.LoadSettings(uDataKey);
                passwordDetails = storage.LoadSettings(pDataKey);
            }
            else if (roamingSetting == "Null")
            {
                userData = storage.LoadSettings(sDataKey);
                usernameDetails = storage.LoadSettings(uDataKey);
                passwordDetails = storage.LoadSettings(pDataKey);
            }

            if(userData != "Null")
            {
                var viewModel = new UserDataSource(userData, usernameDetails, passwordDetails);
                this.DataContext = viewModel;
            }

        }

        private void Login_Off_Event(object sender, RoutedEventArgs e)
        {

            string defaultValue = "Null";

            Storage storage = new Storage();

            string sDataKey = "userDetails";
            string uDataKey = "usernameDetails";
            string pDataKey = "passwordDetails";
            string rDatakey = "roamingDetails";

            string roamingSetting = storage.LoadSettings(rDatakey);

            if (roamingSetting == "true")
            {
                storage.SaveRoamingSettings(sDataKey, defaultValue);
                storage.SaveRoamingSettings(uDataKey, defaultValue);
                storage.SaveRoamingSettings(pDataKey, defaultValue);
            }
            else if (roamingSetting == "false")
            {
                storage.SaveSettings(sDataKey, defaultValue);
                storage.SaveSettings(uDataKey, defaultValue);
                storage.SaveSettings(pDataKey, defaultValue);
            }
            else if (roamingSetting == "Null")
            {
                storage.SaveSettings(sDataKey, defaultValue);
                storage.SaveSettings(uDataKey, defaultValue);
                storage.SaveSettings(pDataKey, defaultValue);
            }

            UserData.Visibility = Visibility.Collapsed;

        }

        private void WebsiteClick(object sender, TappedRoutedEventArgs e)
        {

            string url = WebsiteHeading.Text;
            userURL(url);
          
        }

        private void EmailClick(object sender, TappedRoutedEventArgs e)
        {
            var emailMessage = new EmailMessage();

            Storage storage = new Storage();

            string uDataKey = "usernameDetails";
            string rDatakey = "roamingDetails";

            string roamingSetting = storage.LoadSettings(rDatakey);

            string usernameDetails = "Null";

            if (roamingSetting == "true")
            {
                usernameDetails = storage.LoadRoamingSettings(uDataKey);
            }
            else if (roamingSetting == "false")
            {
                usernameDetails = storage.LoadSettings(uDataKey);
            }
            else if (roamingSetting == "Null")
            {
                usernameDetails = storage.LoadSettings(uDataKey);
            }

            string emailAddress = EmailHeading.Text;
            string username = UsernameHeading.Text;
            string messageTemplate = "Hello {0} I am {1} from Fable project...";
            string message = string.Format(messageTemplate, username, usernameDetails);

            string subjectTemplate = "Hello {0}";
            string subject = string.Format(subjectTemplate, username);

            emailMessage.To.Add(new EmailRecipient(emailAddress));
            emailMessage.Subject = subject;
            emailMessage.Body = message;

            userEmail(emailMessage);
            
        }

        public async void userURL(string url)
        {
            await Launcher.LaunchUriAsync(new Uri(url));
        }

        public async void userEmail(EmailMessage emailMessage)
        {
            await EmailManager.ShowComposeNewEmailAsync(emailMessage);
        }

        private void Edit_Profile_Button(object sender, RoutedEventArgs e)
        {

            if(About.Visibility == Visibility.Visible)
            {
                About.Visibility = Visibility.Collapsed;
                Editable.Visibility = Visibility.Visible;
            }
            else
            {
                About.Visibility = Visibility.Visible;
                Editable.Visibility = Visibility.Collapsed;
            }

        }

        private void updateProfile(object sender, RoutedEventArgs e)
        {
            Storage storage = new Storage();

            string rDatakey = "roamingDetails";
            string pDataKey = "passwordDetails";
            string roamingSetting = storage.LoadSettings(rDatakey);

            string name = myUpdNameBox.Text;
            string email = myUpdEmailBox.Text;
            string avatar = myUpdAvatarBox.Text;
            string newPassword = myUpdPasswordBox.Password;

            if (name == null)
            {
                name = NameHeading.Text;
            }

            if (email == null)
            {
                email = EmailHeading.Text;
            }

            if (avatar == null)
            {
                string avDataKey = "avatarDetails";

                if (roamingSetting == "true")
                {
                    avatar = storage.LoadRoamingSettings(avDataKey);
                }
                else if (roamingSetting == "false")
                {
                    avatar = storage.LoadSettings(avDataKey);
                }
                else if (roamingSetting == "Null")
                {
                    avatar = storage.LoadSettings(avDataKey);
                }

            }

            if (newPassword == null)
            {

                if (roamingSetting == "true")
                {
                    newPassword = storage.LoadRoamingSettings(pDataKey);
                }
                else if (roamingSetting == "false")
                {
                    newPassword = storage.LoadSettings(pDataKey);
                }
                else if (roamingSetting == "Null")
                {
                    newPassword = storage.LoadSettings(pDataKey);
                }
            }


            string password = "";

            if (roamingSetting == "true")
            {
                password = storage.LoadRoamingSettings(pDataKey);
            }
            else if (roamingSetting == "false")
            {
                password = storage.LoadSettings(pDataKey);
            }
            else if (roamingSetting == "Null")
            {
                password = storage.LoadSettings(pDataKey);
            }

            string username = UsernameHeading.Text;

            createURI(username, name, newPassword, password, email, avatar, "http://www.kshatriya.co.uk/dev/project/service/auth.php");
        }

        private async void createURI(string username, string name, string newPassword, string password, string email, string avatar, string target)
        {
            //This is initiated if a registration event has been initiated

            //Creates an instance of a HTTP Client so the Web Service can be interacted with
            var client = new HttpClient();

            //Starts the progress Ring in the GUI to provide feedback to the User that the registration process has started
            updateProgressRing.IsActive = true;

            //Created a Key Value Pair i.e. "Name" => "Aaron Fryer", this will be data that will be picked up by the Web Service that isn't sent via Basic Authentication
            var postData = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("name", name),
                new KeyValuePair<string, string>("email", email),
                new KeyValuePair<string, string>("avatar", avatar),
                new KeyValuePair<string, string>("newPassword", newPassword),
                new KeyValuePair<string, string>("action", "post")
            };

            //Encodes the Key Value Pairs into POST Data
            var content = new FormUrlEncodedContent(postData);

            //Sets the Authentication header to the values of the username and passwords and converts them to Base64 encryption
            var credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes(string.Format("{0}:{1}", username, password)));

            //Sets the Authentication header
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", credentials);

            // activates the request to the Web Service
            var response = client.PutAsync(target, content).Result;

            //Checks if the response from the client is successful
            if (response.IsSuccessStatusCode)
            {
                //It is successful log that user in
                createURI(username, password, target);
            }
            else
            {
                //Tell the user the request was not successful
                updateProgressRing.IsActive = false;
                var title = "Error with Authentication";
                var message = "Unable to register";
                feedbackDialog(title, message);
            }

        }


        private async void createURI(string username, string password, string target)
        {
            //This is initiated to refresh the user data has been initiated

            //Creates an instance of a HTTP Client so the Web Service can be interacted with
            var client = new HttpClient();

            //Create a query string that will be appended to the URL
            var uri = UriExtensions.CreateUriWithQuery(new Uri(target),
            new NameValueCollection { { "action", "get" } });

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
                authenticated(responseString, username, password);
            }
            else
            {
                //Tell the user the request was not successful
                updateProgressRing.IsActive = false;
                var title = "Error with Authentication";
                var message = "Unable to login, Username or Password was incorrect";
                feedbackDialog(title, message);
            }

        }


        private void authenticated(string JSON, string username, string password)
        {

            //Checks to see if the JSON is empty
            if (JSON != "")
            {
                //If not then send the data to the UserDataSource Class that will process it ready for it to be used across the application
                var loginData = new UserDataSource(JSON, username, password);

                    //If the user started by logging in then

                    //Disable the login progress ring
                    updateProgressRing.IsActive = false;

                    //Sets the dialog box title
                    string title = "Update Successful";

                    // the message that will be displayed to the user the {0} is a placeholder that will be replaced by the content of the username variable
                    string template = "Your user credentials for {0} are confirmed, we will store your details to allow unguided login when you open the app. Your details will now be displayed and used to access the Fable Project. Please Restart the app for the application to enable content for logged in Users.";
                    string message = string.Format(template, username);

                    feedbackDialog(title, message);

            }

        }


        private async void feedbackDialog(string title, string message)
        {
            object sender = null;
            int commands = 1;
            Dialog.standardDialog(title, message, commands, sender);

        }

    }
}
