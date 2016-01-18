using FableProject.Functions;
using FableProject.Data;
using FableProject.Presentation;
using FableProject.Themes;
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
using FableProject.DataModel;

namespace FableProject.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SettingsPage : Page
    {
        public SettingsPage()
        {
            this.InitializeComponent();

            Storage storage = new Storage();

            string sDataKey = "userDetails";
            string uDataKey = "usernameDetails";
            string pDataKey = "passwordDetails";
            string rDatakey = "roamingDetails";
            string stDatakey = "statusBarDetails";

            string roamingSetting = storage.LoadSettings(rDatakey);

            string userData = "Null";
            string usernameDetails = "Null";

            roamingEnable.IsOn = false;
            hideStatus.IsOn = false;

            if (roamingSetting == "true")
            {

                roamingEnable.IsOn = true;
                userData = storage.LoadRoamingSettings(sDataKey);
                usernameDetails = storage.LoadRoamingSettings(uDataKey);
                string passwordDetails = storage.LoadRoamingSettings(pDataKey);

                string statusData = storage.LoadRoamingSettings(stDatakey);

                if (statusData == "false")
                {
                    hideStatus.IsOn = true;
                }

            }
            else if (roamingSetting == "false")
            {
                userData = storage.LoadSettings(sDataKey);
                usernameDetails = storage.LoadSettings(uDataKey);
                string passwordDetails = storage.LoadSettings(pDataKey);

                string statusData = storage.LoadSettings(stDatakey);

                if (statusData == "false")
                {
                    hideStatus.IsOn = true;
                }

            }
            else if (roamingSetting == "Null")
            {

                userData = storage.LoadSettings(sDataKey);
                usernameDetails = storage.LoadSettings(uDataKey);
                string passwordDetails = storage.LoadSettings(pDataKey);

                string statusData = storage.LoadSettings(stDatakey);

                if (statusData == "false")
                {
                    hideStatus.IsOn = true;
                }

            }

            if (userData != "Null")
            {

                //var viewModel = new UserDataSource(userData, usernameDetails, passwordDetails);

                AuthForm.Visibility = Visibility.Collapsed;

                SettingsPivot.Items.Remove(SettingsPivot.Items.Single(p => ((PivotItem)p).Name == "RegForm"));

            }

        }

        //About Pivot Page

        private async void EmailK_OnClick(object sender, RoutedEventArgs e)
        {
            var emailMessage = new EmailMessage();

            emailMessage.To.Add(new EmailRecipient("aaron@kshatriya.co.uk"));
            emailMessage.Subject = "Feedback";
            emailMessage.Body = "Please Provide your Feedback.";

            // call EmailManager to show the compose UI in the screen
            await EmailManager.ShowComposeNewEmailAsync(emailMessage);

        }

        private async void WebK_OnClick(object sender, RoutedEventArgs e)
        {
            //Going to Kshatriya.co.uk

            await Launcher.LaunchUriAsync(new Uri("http://www.kshatriya.co.uk"));

        }

        //General Pivot Page

        private void roamingSet(/*object sender, RoutedEventArgs e*/)
        {
            string rDataKey = "roamingDetails";
            string raDataKey = "roamingAlertDetails";
            string sDataKey = "userDetails";
            string uDataKey = "usernameDetails";
            string pDataKey = "passwordDetails";

            Storage storage = new Storage();

            string userData = storage.LoadSettings(sDataKey);
            string usernameData = storage.LoadSettings(uDataKey);
            string passwordData = storage.LoadSettings(pDataKey);
            string roamingAlertData = storage.LoadSettings(raDataKey);

            if (userData != "Null")
            {
                storage.SaveRoamingSettings(sDataKey, userData);
                storage.SaveRoamingSettings(uDataKey, usernameData);
                storage.SaveRoamingSettings(pDataKey, passwordData);
                storage.SaveRoamingSettings(rDataKey, "true");
            }

            if (roamingAlertData != "true")
            {
                string title = "Roaming";

                storage.SaveSettings(rDataKey, "true");
                string template = "You have {0} roaming of your settings with your Microsoft Account, {1}";
                string message = string.Format(template, "enabled", "don't worry you can always disable it later.");
                feedbackDialog(title, message);

                storage.SaveRoamingSettings(raDataKey, "true");
            }
        }


        private void roamingUnset(/*object sender, RoutedEventArgs e*/)
        {
            string rDataKey = "roamingDetails";
            string raDataKey = "roamingAlertDetails";
            string sDataKey = "userDetails";
            string uDataKey = "usernameDetails";
            string pDataKey = "passwordDetails";


            Storage storage = new Storage();

            string userData = storage.LoadRoamingSettings(sDataKey);
            string usernameData = storage.LoadRoamingSettings(uDataKey);
            string passwordData = storage.LoadRoamingSettings(pDataKey);
            string roamingAlertData = storage.LoadSettings(raDataKey);


            if (userData != "Null")
            {
                storage.SaveSettings(sDataKey, userData);
                storage.SaveSettings(uDataKey, usernameData);
                storage.SaveSettings(pDataKey, passwordData);
                storage.SaveSettings(rDataKey, "false");
            }

            if (roamingAlertData != "true")
            {
                string title = "Roaming";

                storage.SaveSettings(rDataKey, "false");
                string template = "You have {0} roaming of your settings with your Microsoft Account, {1}";
                string message = string.Format(template, "disabled", "don't worry you can always re-enable it later.");
                feedbackDialog(title, message);

                storage.SaveRoamingSettings(raDataKey, "false");
            }

        }

        //Authentication Pivot Pages

        private void Login_Event(object sender, RoutedEventArgs e)
        {
            //This piece of code is initiated when the login button is clicked

            //Gets the username and password from the text boxes in the GUI
            string username = myUsernameBox.Text;
            string password = myPasswordBox.Password;
            string eventStarted = "login";

            //Checks to see if the username is empty
            if (username == "" && password == "")
            {
                //If the username and password is empty then create a dialog box to inform the user. This is done to prevent the Web Service getting unnecessary requests that can effect the services performance
                string title = "No Username";
                string template = "You have not entered your Username! How do you expect me to log you in!? The Data you have entered is, Username: {0}, Password: {1}";
                string message = string.Format(template, username, password);
                feedbackDialog(title, message);
            }
            else if (username == null && password == null)
            {
                //If the username and password is a null value then create a dialog box to inform the user. This is done to prevent the Web Service getting unnecessary requests that can effect the services performance
                string title = "No Username";
                string template = "You have not entered your Username! How do you expect me to log you in!? The Data you have entered is, Username: {0}, Password: {1}";
                string message = string.Format(template, username, password);
                feedbackDialog(title, message);
            }
            else
            {
                //If the username and password variables contains a value

                //sends the following data to the createURI function that sends data to the service
                createURI(username, password, "http://www.kshatriya.co.uk/dev/project/service/auth.php", eventStarted);
            }
        }

        private void Register_Event(object sender, RoutedEventArgs e)
        {
            //This piece of code is initiated when the register button is clicked

            //Gets the username, password, name and email from the text boxes in the GUI and converts the raw DOB data into a format the Web Service will understand 
            string username = myRegUsernameBox.Text;
            string name = myRegNameBox.Text;
            string password = myRegPasswordBox.Password;
            string email = myRegEmailBox.Text;
            DateTime rawDOB = myDOB.Date.DateTime;
            string DOB = rawDOB.ToString("yyyyMMdd");
            string eventStarted = "register";

            string title = "Registration";

            //Checks to see if the desired variables are empty
            if (username == "" && password == "" && name == "" && email == "" && DOB == "")
            {
                //If they are empty then create a dialog box to inform the user. This is done to prevent the Web Service getting unnecessary requests that can effect the services performance
                string messageDetails = "not entered anything";
                string template = "You have {0} for your username, please complete the form.";
                string message = string.Format(template, messageDetails);
                feedbackDialog(title, message);
            }
            else if (username == null && password == null && name == null && email == null && DOB == null)
            {
                //If they are null then create a dialog box to inform the user. This is done to prevent the Web Service getting unnecessary requests that can effect the services performance
                string messageDetails = "not entered anything";
                string template = "You have {0} for your username, please complete the form.";
                string message = string.Format(template, messageDetails);
                feedbackDialog(title, message);
            }
            else
            {
                //If the desired variables contains a value

                //sends the following data to the createURI function that sends data to the service
                createURI(username, name, password, email, DOB, "http://www.kshatriya.co.uk/dev/project/service/auth.php", eventStarted);
            }
        }

        //Overloaded version of the createURI function for registration
        private async void createURI(string username, string name, string password, string email, string DOB, string target, string method)
        {
            //This is initiated if a registration event has been initiated

            //Creates an instance of a HTTP Client so the Web Service can be interacted with
            var client = new HttpClient();

            //Starts the progress Ring in the GUI to provide feedback to the User that the registration process has started
            registerProgressRing.IsActive = true;

            //Created a Key Value Pair i.e. "Name" => "Aaron Fryer", this will be data that will be picked up by the Web Service that isn't sent via Basic Authentication
            var postData = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("name", name),
                new KeyValuePair<string, string>("email", email),
                new KeyValuePair<string, string>("dob", DOB),
                new KeyValuePair<string, string>("action", "post")
            };

            //Encodes the Key Value Pairs into POST Data
            var content = new FormUrlEncodedContent(postData);

            //Sets the Authentication header to the values of the username and passwords and converts them to Base64 encryption
            var credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes(string.Format("{0}:{1}", username, password)));

            //Sets the Authentication header
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", credentials);

            // activates the request to the Web Service
            var response = client.PostAsync(target, content).Result;

            //Checks if the response from the client is successful
            if (response.IsSuccessStatusCode)
            {
                //It is successful log that user in
                createURI(username, password, target, "loginReg");
            }
            else
            {
                //Tell the user the request was not successful
                loginProgressRing.IsActive = false;
                var title = "Error with Authentication";
                var message = "Unable to register";
                feedbackDialog(title, message);
            }

        }

        //The Original createURI function, this is used for loginning in users
        private async void createURI(string username, string password, string target, string method)
        {
            //This is initiated if a login event has been initiated

            //Creates an instance of a HTTP Client so the Web Service can be interacted with
            var client = new HttpClient();

            //If the user initialy started the log in procedure
            if (method == "login")
            {
                //Activate the login Progress ring to inform the user the login process has started
                loginProgressRing.IsActive = true;
            }

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
                authenticated(responseString, username, password, method);
            }
            else
            {
                //Tell the user the request was not successful
                loginProgressRing.IsActive = false;
                var title = "Error with Authentication";
                var message = "Unable to login, Username or Password was incorrect";
                feedbackDialog(title, message);
            }

        }


        private void authenticated(string JSON, string username, string password, string eventStarted)
        {

            //Checks to see if the JSON is empty
            if (JSON != "")
            {
                //If not then send the data to the UserDataSource Class that will process it ready for it to be used across the application
                var loginData = new UserDataSource(JSON, username, password);

                //Checks to see how the process started
                if (eventStarted == "loginReg")
                {
                    //If the user started by registering then:

                    //Disable the registration progress ring
                    registerProgressRing.IsActive = false;

                    //Sets the dialog box title
                    string title = "Registration Successful";

                    //Hides the Authentication form and the registration page
                    AuthForm.Visibility = Visibility.Collapsed;
                    SettingsPivot.Items.Remove(SettingsPivot.Items.Single(p => ((PivotItem)p).Name == "RegForm"));
                    authed.Visibility = Visibility.Visible;

                    //Sets the data context for the page with the login data
                    this.DataContext = loginData;

                    // the message that will be displayed to the user
                    string message = "Your user credentials have been registered with the Fable Project, we will store your details to allow unguided login when you open the app. Your details will now be displayed and used to access the Fable Project. Please Restart the app for the application to enable content for logged in Users.";

                    feedbackDialog(title, message, 99);

                }
                else if (eventStarted == "login")
                {
                    //If the user started by logging in then

                    //Disable the login progress ring
                    loginProgressRing.IsActive = false;

                    //Sets the dialog box title
                    string title = "Login Successful";

                    //Hides the Authentication form and the registration page
                    AuthForm.Visibility = Visibility.Collapsed;
                    SettingsPivot.Items.Remove(SettingsPivot.Items.Single(p => ((PivotItem)p).Name == "RegForm"));
                    authed.Visibility = Visibility.Visible;

                    //Sets the data context for the page with the login data
                    this.DataContext = loginData;

                    // the message that will be displayed to the user the {0} is a placeholder that will be replaced by the content of the username variable
                    string template = "Your user credentials for {0} are confirmed, we will store your details to allow unguided login when you open the app. Your details will now be displayed and used to access the Fable Project. Please Restart the app for the application to enable content for logged in Users.";
                    string message = string.Format(template, username);

                    feedbackDialog(title, message, 99);
                }

            }

        }


        //Dialog Box

        private async void feedbackDialog(string title, string message)
        {
            object sender = null;
            int commands = 1;
            Dialog.standardDialog(title, message, commands, sender);

        }

        private async void feedbackDialog(string title, string message, int commands)
        {
            object sender = null;
            Dialog.standardDialog(title, message, commands, sender);

        }

        private void roamingToggled(object sender, RoutedEventArgs e)
        {
            if(roamingEnable.IsOn == false)
            {
                roamingUnset();
            }
            else
            {
                roamingSet();
            }
        }

        private void clearCache_Click(object sender, RoutedEventArgs e)
        {

            Storage storage = new Storage();

            string sDataKey = "userDetails";
            string uDataKey = "usernameDetails";
            string pDataKey = "passwordDetails";
            string rDatakey = "roamingDetails";
            string raDataKey = "roamingAlertDetails";

            string roamingSetting = storage.LoadSettings(rDatakey);

            string clearCache = "Null";

            if (roamingSetting == "true")
            {
                storage.SaveRoamingSettings(sDataKey, clearCache);
                storage.SaveRoamingSettings(uDataKey, clearCache);
                storage.SaveRoamingSettings(pDataKey, clearCache);
                storage.SaveRoamingSettings(rDatakey, clearCache);
                storage.SaveRoamingSettings(raDataKey, clearCache);
            }
            else
            {
                storage.SaveSettings(sDataKey, clearCache);
                storage.SaveSettings(uDataKey, clearCache);
                storage.SaveSettings(pDataKey, clearCache);
                storage.SaveSettings(rDatakey, clearCache);
                storage.SaveSettings(raDataKey, clearCache);
            }

            string title = "Clearing Application Data";

            string message = "You have cleared all application data, you will need to restart the application and re login to the application.";
            feedbackDialog(title, message, 99);

        }

        private void statusToggled(object sender, RoutedEventArgs e)
        {

            if (hideStatus.IsOn == false)
            {
                statusBarOn();
            }
            else
            {
                statusBarOff();
            }

        }

        private void statusBarOn()
        {

            Storage storage = new Storage();

            string rDatakey = "roamingDetails";
            string stDatakey = "statusBarDetails";

            string roamingSetting = storage.LoadSettings(rDatakey);

            if (roamingSetting == "true")
            {
                storage.SaveRoamingSettings(stDatakey, "false");
            }
            else
            {
                storage.SaveSettings(stDatakey, "false");
            }

            if (Windows.Foundation.Metadata.ApiInformation.IsTypePresent("Windows.UI.ViewManagement.StatusBar"))
            {
                var i = Windows.UI.ViewManagement.StatusBar.GetForCurrentView().ShowAsync();
            }

        }

        private void statusBarOff()
        {

            Storage storage = new Storage();

            string rDatakey = "roamingDetails";
            string stDatakey = "statusBarDetails";

            string roamingSetting = storage.LoadSettings(rDatakey);

            if (roamingSetting == "true")
            {
                storage.SaveRoamingSettings(stDatakey, "true");
            }
            else
            {
                storage.SaveSettings(stDatakey, "true");
            }

            if (Windows.Foundation.Metadata.ApiInformation.IsTypePresent("Windows.UI.ViewManagement.StatusBar"))
            {
                var i = Windows.UI.ViewManagement.StatusBar.GetForCurrentView().HideAsync();
            }

        }

    }
}
