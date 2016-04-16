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

        public List<StoriesSorted> Stories { get; set; }

        public AuthorPage()
        {
            this.InitializeComponent();

            Storage storage = new Storage();

            string sDataKey = "userDetails";
            string rDatakey = "roamingDetails";

            string roamingSetting = storage.LoadSettings(rDatakey);

            string userData = "Null";

            if (roamingSetting == "true")
            {
                userData = storage.LoadRoamingSettings(sDataKey);
            }
            else
            {
                userData = storage.LoadSettings(sDataKey);
            }

            if (userData != "Null")
            {
                storyGet("titleOnly", "http://www.kshatriya.co.uk/dev/project/service/stories.php");
                Authenticate.Visibility = Visibility.Collapsed;
            }
            else
            {
                var dialogTitle = "You are not Logged in :(";
                var message = "Please Login to use the features on this page. You are missing out by not being a user ;)";
                feedbackDialog(dialogTitle, message);
                NewStoriesPage.Visibility = Visibility.Collapsed;
                Authenticate.Visibility = Visibility.Visible;
            }

        }

        private void storyHelper(object sender, RoutedEventArgs e)
        {
            string title = "Creating Your Story";
            string messageInitial = "Complete the {0} form to create the Story, this will need to be done before you can create your pages you need to create your story and select the story you want to add the page to.";
            string context = "Story";
            string message = string.Format(messageInitial, context);


            helperBox(title, message, context);
        }


        private void pageHelper(object sender, RoutedEventArgs e)
        {
            string title = "Creating a Page for your Story";
            string messageInitial = "Complete the {0} form to create a Page, you will need to name the page, create the content for the page and the interaction for it that will be displayed at the end. If this is the first page or you have not set a first page yet then click the first page checkbox.";
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

        private void feedbackDialog(string title, string message)
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
            else
            {
                usernameDetails = storage.LoadSettings(uDataKey);
                passwordDetails = storage.LoadSettings(pDataKey);
            }


            string storyTitle = storyTitleBox.Text;
            string storyDesc = Convert.ToString(storyDescriptionTextBox.Document);
            string storyCat = ((ComboBoxItem)storyCategory.SelectedItem).Content.ToString();
            string storyImage = storyImageBox.Text;

            string title = "New Story";

            if (storyTitle == "" || storyTitle == null)
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
            string uDataKey = "usernameDetails";
            string pDataKey = "passwordDetails";

            string roamingSetting = storage.LoadSettings(rDataKey);
            string userIDDetails = "";
            string username = "";
            string password = "";

            if (roamingSetting == "true")
            {
                userIDDetails = storage.LoadRoamingSettings(idDataKey);
                username = storage.LoadRoamingSettings(uDataKey);
                password = storage.LoadRoamingSettings(pDataKey);
            }
            else
            {
                userIDDetails = storage.LoadSettings(idDataKey);
                username = storage.LoadSettings(uDataKey);
                password = storage.LoadSettings(pDataKey);
            }

            var client = new HttpClient();

            //Sets the Authentication header to the values of the username and passwords and converts them to Base64 encryption
            var credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes(string.Format("{0}:{1}", username, password)));

            //Sets the Authentication header
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", credentials);

            var uri = UriExtensions.CreateUriWithQuery(new Uri(target),
                new NameValueCollection { { "action", "get" } },
                new NameValueCollection { { "story", title } });

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
                var dialogTitle = "You don't have any Stories";
                var message = "You don't have any Stories on Fable Time Yet, but we can Fix that right away";
                feedbackDialog(dialogTitle, message);
            }
        }

        private void configuringPages(string JSON)
        {

            if (JSON != "")
            {

                var storiesData = new StoriesDataSource(JSON, "new");
                this.DataContext = storiesData;

                //selectedStory.ItemsSource = storiesData;

                Storage storage = new Storage();

                List<Stories> userStories = JsonConvert.DeserializeObject<List<Stories>>(JSON);

                selectedStory.ItemsSource = userStories;
                selectedStory.DisplayMemberPath = "Title";
                selectedStory.SelectedValuePath = "Title";

            }
            else
            {
                var dialogTitle = "You don't have any Stories";
                var message = "You don't have any Stories on Fable Time Yet, but we can Fix that right away.";
                feedbackDialog(dialogTitle, message);
            }
        }


        private async void getPageList(object sender, RoutedEventArgs e)
        {

            string story = selectedStory.SelectedValue.ToString();

            var client = new HttpClient();

            var uri = UriExtensions.CreateUriWithQuery(new Uri("http://www.kshatriya.co.uk/dev/project/service/page.php"),
            new NameValueCollection { { "story", story } },
            new NameValueCollection { { "page", "all" } });

            // call sync
            var response = client.GetAsync(uri).Result;
            var responseString = "";

            if (response.IsSuccessStatusCode)
            {
                responseString = await response.Content.ReadAsStringAsync();
                List<StoryPages> pageList = JsonConvert.DeserializeObject<List<StoryPages>>(responseString);

                selectedPage.ItemsSource = pageList;
                selectedPage.DisplayMemberPath = "Number";
                selectedPage.SelectedValuePath = "Number";
            }
            else
            {
                var title = "Error with Application";
                var message = "It's not you, it's me! Unfortuantely there is an error connecting with the Fable Time Service";
                errorDialog(title, message);
            }
        }


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
                authenticated(responseString, username, password, method, "");
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


        private void authenticated(string JSON, string username, string password, string eventStarted, string spec)
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
                    NewStoriesPage.Visibility = Visibility.Visible;
                    Authenticate.Visibility = Visibility.Collapsed;

                    //Sets the data context for the page with the login data
                    this.DataContext = loginData;

                    // the message that will be displayed to the user
                    string message = "Your user credentials have been registered with the Fable Project, we will store your details to allow unguided login when you open the app. Your details will now be displayed and used to access the Fable Project. Please Restart the app for the application to enable content for logged in Users.";

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
                    NewStoriesPage.Visibility = Visibility.Visible;
                    Authenticate.Visibility = Visibility.Collapsed;


                    // the message that will be displayed to the user the {0} is a placeholder that will be replaced by the content of the username variable
                    string template = "Your user credentials for {0} are confirmed, we will store your details to allow unguided login when you open the app. Your details will now be displayed and used to access the Fable Project. Please Restart the app for the application to enable content for logged in Users.";
                    string message = string.Format(template, username);

                    feedbackDialog(title, message);

                }

            }

        }

        private void createPage(object sender, RoutedEventArgs e)
        {

            string pageTitle = pageTitleBox.Text;
            string pageContent = pageContentsTextBox.Text;
            string pageContent_2 = pageChoiceTextBox.Text;
            string pageRoot = "";
            string pageNumber = pageNumberBox.Text;
            string pageInteraction = pageInteractionIntroTextBox.Text;
            string pageInteractionType = ((ComboBoxItem)interactionTypeCombo.SelectedItem).Content.ToString();
            string pageEInteraction = pageEInteractionTextBox.Text;
            string pageEInteractionAnswer = pageEInteractionAnswerBox.Text;
            string pageMInteraction = pageMInteractionTextBox.Text;
            string pageMInteractionAnswer = pageMInteractionAnswerBox.Text;
            string pageHInteraction = pageHInteractionTextBox.Text;
            string pageHInteractionAnswer = pageHInteractionAnswerBox.Text;
            string pageJInteraction = pageJInteractionTextBox.Text;
            string pageJInteractionAnswer = pageJInteractionAnswerBox.Text;
            string pageFirst = pageDefaultCheck.IsChecked.ToString();
            string pageOptionA = pageOptionABox.Text;
            string pageOptionB = pageOptionBBox.Text;
            string pageOptionADest = pageOptionADestBox.Text;
            string pageOptionBDest = pageOptionBDestBox.Text;
            string pageInteractionOption = pageInteractionRewardBox.Text;
            string pageInteractionFailure = pageInteractionFailureBox.Text;
            string pageReward = pageRewardBox.Text;

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

            string title = "New Page";

            if (selectedStory.SelectedIndex != -1)
            {
                pageRoot = selectedStory.SelectedValue.ToString();
                title = "New Page For " + pageRoot;
            }


            string template = "You have {0} I shake my head....";

            if (selectedStory.SelectedIndex == -1)
            {
                string messageDetails = "not selected which story this page belongs to!  How is anyone supposed to enjoy your content if this page is not assigned to a story :(";
                string message = string.Format(template, messageDetails);
                feedbackDialog(title, message);
            }
            else if (pageTitle == "" || pageTitle == null)
            {
                string messageDetails = "not entered a page title!  How is anyone supposed to know the twists in the story!";
                string message = string.Format(template, messageDetails);
                feedbackDialog(title, message);
            }
            else if (pageContent == "" || pageContent == null)
            {
                string messageDetails = "not entered any content for the Page!  How is anyone supposed to enjoy your content if there isn't any? 0 out of 10 for effort there :(";
                string message = string.Format(template, messageDetails);
                feedbackDialog(title, message);
            }
            else if (pageNumber == "" || pageNumber == null)
            {
                string messageDetails = "not entered the page number! How is Fable Time supposed to link the pages together? Telekinesis? I'm just an App! :(";
                string message = string.Format(template, messageDetails);
                feedbackDialog(title, message);
            }
            else if (pageOptionA == "" || pageOptionA == null)
            {
                string messageDetails = "not entered the next page number! How is the user supposed to progress?";
                string message = string.Format(template, messageDetails);
                feedbackDialog(title, message);
            }
            else
            {
                sendPage(usernameDetails, passwordDetails, "http://www.kshatriya.co.uk/dev/project/service/page.php", pageRoot, pageTitle, pageContent, pageContent_2, pageNumber, pageInteraction, pageInteractionType, pageEInteraction, pageEInteractionAnswer, pageMInteraction, pageMInteractionAnswer, pageHInteraction, pageHInteractionAnswer, pageJInteraction, pageJInteractionAnswer, pageOptionA, pageOptionADest, pageOptionB, pageOptionBDest, pageInteractionOption, pageInteractionFailure, pageReward, pageFirst);
            }

        }

        private void sendPage(string username, string password, string target, string storyTitle, string pageTitle, string pageContent, string pageContent_2, string pageNumber, string pageInteraction, string pageInteractionType, string pageEInteraction, string pageEInteractionAnswer, string pageMInteraction, string pageMInteractionAnswer, string pageHInteraction, string pageHInteractionAnswer, string pageJInteraction, string pageJInteractionAnswer, string pageOptionA, string pageOptionADest, string pageOptionB, string pageOptionBDest, string pageInteractionOption, string pageInteractionFailure, string pageReward, string pageFirst)
        {

            var client = new HttpClient();

            string action = "";

            if (newPageButton.Tag.ToString() == "Create")
            {
                action = "post";
            }
            else
            {
                action = "put";
            }

            var postData = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("storyTitle", storyTitle),
                new KeyValuePair<string, string>("title", pageTitle),
                new KeyValuePair<string, string>("content", pageContent),
                new KeyValuePair<string, string>("content_2", pageContent_2),
                new KeyValuePair<string, string>("pageNumber", pageNumber),
                new KeyValuePair<string, string>("pageInteraction", pageInteraction),
                new KeyValuePair<string, string>("pageInteractionType", pageInteractionType),
                new KeyValuePair<string, string>("pageEInteraction", pageEInteraction),
                new KeyValuePair<string, string>("pageEInteractionAnswer", pageEInteractionAnswer),
                new KeyValuePair<string, string>("pageMInteraction", pageMInteraction),
                new KeyValuePair<string, string>("pageMInteractionAnswer", pageMInteractionAnswer),
                new KeyValuePair<string, string>("pageHInteraction", pageHInteraction),
                new KeyValuePair<string, string>("pageHInteractionAnswer", pageHInteractionAnswer),
                new KeyValuePair<string, string>("pageJInteraction", pageJInteraction),
                new KeyValuePair<string, string>("pageJInteractionAnswer", pageEInteractionAnswer),
                new KeyValuePair<string, string>("pageOptionA", pageOptionA),
                new KeyValuePair<string, string>("pageOptionADest", pageOptionADest),
                new KeyValuePair<string, string>("pageOptionB", pageOptionB),
                new KeyValuePair<string, string>("pageOptionBDest", pageOptionBDest),
                new KeyValuePair<string, string>("pageInteractionOption", pageInteractionOption),
                new KeyValuePair<string, string>("pageInteractionFailure", pageInteractionFailure),
                new KeyValuePair<string, string>("pageReward", pageReward),
                new KeyValuePair<string, string>("pageFirst", pageFirst),
                new KeyValuePair<string, string>("action", action)
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
                var title = "Error with adding the Page";
                var message = "Unable to add your Page Unfortunately :(";
                feedbackDialog(title, message);
            }
        }

        private void modifyPage(object sender, SelectionChangedEventArgs e)
        {
            var target = "http://www.kshatriya.co.uk/dev/project/service/page.php";
            string pageRoot = "";
            string pageNumber = "";

            if (selectedStory.SelectedIndex != -1)
            {
                pageRoot = selectedStory.SelectedValue.ToString();
            }

            if (selectedPage.SelectedIndex != -1)
            {
                pageNumber = selectedPage.SelectedValue.ToString();
            }

            searchPages(target, pageRoot, pageNumber);
        }

        private async void searchPages(string target, string toGetStory, string toGetPage)
        {

            var client = new HttpClient();

            var uri = UriExtensions.CreateUriWithQuery(new Uri(target),
            new NameValueCollection { { "story", toGetStory } },
            new NameValueCollection { { "page", toGetPage } });

            // call sync
            var response = client.GetAsync(uri).Result;
            var responseString = "";

            if (response.IsSuccessStatusCode)
            {
                responseString = await response.Content.ReadAsStringAsync();
                getPages(responseString);
            }
            else
            {
                var title = "Error with Application";
                var message = "It's not you, it's me! Unfortuantely there is an error connecting with the Fable Time Service";
                errorDialog(title, message);
            }
        }

        private void getPages(string JSON)
        {
            List<StoryPages> pages = JsonConvert.DeserializeObject<List<StoryPages>>(JSON);

            pageTitleBox.Text = pages[0].Title;
            pageContentsTextBox.Text = pages[0].Content;
            pageChoiceTextBox.Text = pages[0].Content_2;
            pageNumberBox.Text = pages[0].Number;

            pageInteractionIntroTextBox.Text = pages[0].Interaction;
            interactionTypeCombo.SelectedItem = pages[0].Interaction_Type;
            pageEInteractionTextBox.Text = pages[0].Easy_Interaction;
            pageEInteractionAnswerBox.Text = pages[0].Easy_Interaction_Answer;
            pageMInteractionTextBox.Text = pages[0].Medium_Interaction;
            pageMInteractionAnswerBox.Text = pages[0].Medium_Interaction_Answer;
            pageHInteractionTextBox.Text = pages[0].Hard_Interaction;
            pageHInteractionAnswerBox.Text = pages[0].Hard_Interaction_Answer;
            pageJInteractionTextBox.Text = pages[0].Humour_Interaction;
            pageJInteractionAnswerBox.Text = pages[0].Humour_Interaction_Answer;

            pageOptionABox.Text = pages[0].option1;
            pageOptionBBox.Text = pages[0].option2;
            pageOptionADestBox.Text = pages[0].option1_Dest;
            pageOptionBDestBox.Text = pages[0].option2_Dest;
            pageInteractionRewardBox.Text = pages[0].optionSpecialSuccess;
            pageInteractionFailureBox.Text = pages[0].optionSpecialFailure;
            pageRewardBox.Text = pages[0].Goodies;

            newPageButton.Content = "Update Page";
            newPageButton.Tag = "Update";

        }

        private void errorDialog(string title, string messageDetails)
        {

            object sender = null;
            string message = messageDetails;
            int commands = 1;
            Dialog.standardDialog(title, message, commands, sender);

        }
    }
}
