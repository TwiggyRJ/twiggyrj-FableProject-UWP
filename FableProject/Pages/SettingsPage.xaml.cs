﻿using FableProject.Functions;
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


            if (Windows.Foundation.Metadata.ApiInformation.IsTypePresent("Windows.UI.ViewManagement.StatusBar"))
            {
                hideStatus.Visibility = Visibility.Visible;
                Thickness myModifiedGrid = roamingGrid.Margin;
                myModifiedGrid.Top = 0;
                roamingGrid.Margin = myModifiedGrid;
            }

            Storage storage = new Storage();

            string sDataKey = "userDetails";
            string uDataKey = "usernameDetails";
            string pDataKey = "passwordDetails";
            string rDatakey = "roamingDetails";
            string stDatakey = "statusBarDetails";
            string adDatakey = "adminDetails";

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
            else
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

            searchPolicies("http://www.kshatriya.co.uk/dev/project/service/updates.php", "privacy");

        }

        //About Pivot Page

        private async void EmailK_OnClick(object sender, RoutedEventArgs e)
        {
            var emailMessage = new EmailMessage();

            emailMessage.To.Add(new EmailRecipient("aaronfryer@live.co.uk"));
            emailMessage.Subject = "Feedback";
            emailMessage.Body = "Please Provide your Feedback.";

            // call EmailManager to show the compose UI in the screen
            await EmailManager.ShowComposeNewEmailAsync(emailMessage);

        }

        private async void EmailFeature_OnClick(object sender, RoutedEventArgs e)
        {
            var emailMessage = new EmailMessage();

            emailMessage.To.Add(new EmailRecipient("aaronfryer@live.co.uk"));
            emailMessage.Subject = "Feature Request";
            emailMessage.Body = "Please Provide your Feature Request.";

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
        private void createURI(string username, string name, string password, string email, string DOB, string target, string method)
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
                    string message = "Your user credentials have been registered with the Fable Project, we will store your details to allow unguided login when you open the app. Your details will now be displayed and used to access the Fable Project.";

                    feedbackDialog(title, message);

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
                    string template = "Your user credentials for {0} are confirmed, we will store your details to allow unguided login when you open the app. Your details will now be displayed and used to access the Fable Project. Please Restart the app if you are an Administrator.";
                    string message = string.Format(template, username);

                    feedbackDialog(title, message);
                }

            }

        }


        //Dialog Box

        private void feedbackDialog(string title, string message)
        {
            object sender = null;
            int commands = 1;
            Dialog.standardDialog(title, message, commands, sender);

        }

        private void feedbackDialog(string title, string message, int commands)
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
            string onloadDatakey = "onloadDetails";

            string roamingSetting = storage.LoadSettings(rDatakey);

            string clearCache = "Null";

            if (roamingSetting == "true")
            {
                storage.SaveRoamingSettings(sDataKey, clearCache);
                storage.SaveRoamingSettings(uDataKey, clearCache);
                storage.SaveRoamingSettings(pDataKey, clearCache);
                storage.SaveRoamingSettings(rDatakey, clearCache);
                storage.SaveRoamingSettings(raDataKey, clearCache);
                storage.SaveRoamingSettings(onloadDatakey, clearCache);
            }
            else
            {
                storage.SaveSettings(sDataKey, clearCache);
                storage.SaveSettings(uDataKey, clearCache);
                storage.SaveSettings(pDataKey, clearCache);
                storage.SaveSettings(rDatakey, clearCache);
                storage.SaveSettings(raDataKey, clearCache);
                storage.SaveSettings(onloadDatakey, clearCache);
            }

            string title = "Clearing Application Data";

            string message = "You have cleared all application data, if you have an account then you will need to log back in to the application.";
            feedbackDialog(title, message);

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

        private void dateSettingsChanged(object sender, SelectionChangedEventArgs e)
        {
            Storage storage = new Storage();

            string rDatakey = "roamingDetails";
            string dfDatakey = "dateFormatDetails";
            string roamingSetting = storage.LoadSettings(rDatakey);

            string dateFormat = ((ComboBoxItem)dateSettingsCombo.SelectedItem).Content.ToString();

            if(dateFormat == "Default")
            {
                dateFormatHeader.Text = "Default i.e. Changes on the Context";

                if (roamingSetting == "true")
                {
                    storage.SaveRoamingSettings(dfDatakey, "0");
                }
                else
                {
                    storage.SaveSettings(dfDatakey, "0");
                }
            }
            else if(dateFormat == "Full 'Proper' Date Layout")
            {
                dateFormatHeader.Text = "Wednesday 9 March 2016";

                if (roamingSetting == "true")
                {
                    storage.SaveRoamingSettings(dfDatakey, "1");
                }
                else
                {
                    storage.SaveSettings(dfDatakey, "1");
                }
            }
            else if (dateFormat == "Short")
            {
                dateFormatHeader.Text = "Wed 9 Mar 2016";

                if (roamingSetting == "true")
                {
                    storage.SaveRoamingSettings(dfDatakey, "2");
                }
                else
                {
                    storage.SaveSettings(dfDatakey, "2");
                }
            }
            else if (dateFormat == "Numeric")
            {
                dateFormatHeader.Text = "09/03/2016";

                if (roamingSetting == "true")
                {
                    storage.SaveRoamingSettings(dfDatakey, "3");
                }
                else
                {
                    storage.SaveSettings(dfDatakey, "3");
                }
            }
            else if (dateFormat == "Numeric (American)")
            {
                dateFormatHeader.Text = "3/9/2016";

                if (roamingSetting == "true")
                {
                    storage.SaveRoamingSettings(dfDatakey, "4");
                }
                else
                {
                    storage.SaveSettings(dfDatakey, "4");
                }
            }


        }


        private void difficultySettingsChanged(object sender, SelectionChangedEventArgs e)
        {
            Storage storage = new Storage();

            string rDatakey = "roamingDetails";
            string gameDFDatakey = "difficultyDetails";
            string roamingSetting = storage.LoadSettings(rDatakey);

            string difficulty = ((ComboBoxItem)difficultySettingsCombo.SelectedItem).Content.ToString();

            if (difficulty == "Easy")
            {
                difficultyInfoHeader.Text = "Easy: 90 Seconds to answer and easy questions, so no worries then.";

                if (roamingSetting == "true")
                {
                    storage.SaveRoamingSettings(gameDFDatakey, "0");
                }
                else
                {
                    storage.SaveSettings(gameDFDatakey, "0");
                }
            }
            else if (difficulty == "Normal")
            {
                difficultyInfoHeader.Text = "Normal: 60 Seconds to answer and average difficulty questions, enjoy for your 'meh' experience.";

                if (roamingSetting == "true")
                {
                    storage.SaveRoamingSettings(gameDFDatakey, "1");
                }
                else
                {
                    storage.SaveSettings(gameDFDatakey, "1");
                }
            }
            else if (difficulty == "Hard")
            {
                difficultyInfoHeader.Text = "Hard: 45 Seconds to answer and hard questions, unleash your inner nerd!";

                if (roamingSetting == "true")
                {
                    storage.SaveRoamingSettings(gameDFDatakey, "2");
                }
                else
                {
                    storage.SaveSettings(gameDFDatakey, "2");
                }
            }
            else if (difficulty == "Very Hard")
            {
                difficultyInfoHeader.Text = "Very Hard: 30 Seconds to answer but the questions are the same as on hard. Let's do this Countdown style!";

                if (roamingSetting == "true")
                {
                    storage.SaveRoamingSettings(gameDFDatakey, "3");
                }
                else
                {
                    storage.SaveSettings(gameDFDatakey, "3");
                }
            }
            else if (difficulty == "Guffaws")
            {
                difficultyInfoHeader.Text = "Guffaws: 30 Seconds to answer with funny and ridiculous questions, think of this like '8 out of 10 cats does Fable Time!'";

                if (roamingSetting == "true")
                {
                    storage.SaveRoamingSettings(gameDFDatakey, "4");
                }
                else
                {
                    storage.SaveSettings(gameDFDatakey, "4");
                }
            }


        }



        private async void searchPolicies(string target, string toGet)
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
                var title = "Error with Application";
                var message = "It's not you, it's me! Unfortuantely there is an error connecting with the Fable Time Service";
                errorDialog(title, message);
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
            var viewModel = new PoliciesDataSource(JSON);
            this.DataContext = viewModel;

        }


    }
}
