using FableProject.Data;
using FableProject.DataModel;
using FableProject.Functions;
using FableProject.Presentation;
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
    public sealed partial class StoryPage : Page
    {

        public int countdown;
        public int countdownReset;
        public string name;
        public string difficulty;
        public string selectedPage { get; set; }
        public string selectedStory { get; set; }
        public DispatcherTimer timer = new DispatcherTimer();


        public string passedParameter;


        protected override void OnNavigatedTo(NavigationEventArgs e)

        {

            passedParameter = e.Parameter.ToString();

            var target = "http://www.kshatriya.co.uk/dev/project/service/page.php";

            searchPages(target, passedParameter, "First");

        }


        public StoryPage()
        {
            this.InitializeComponent();

            Storage storage = new Storage();
            string rDatakey = "roamingDetails";
            string nDataKey = "nameDetails";
            string roamingSetting = storage.LoadSettings(rDatakey);
            string gameDFDatakey = "difficultyDetails";

            difficulty = "0";

            if (roamingSetting == "true")
            {
                name = storage.LoadRoamingSettings(nDataKey);
                difficulty = storage.LoadRoamingSettings(gameDFDatakey);
            }
            else
            {
                name = storage.LoadSettings(nDataKey);

                if (name == "Null")
                {
                    name = "No Face";
                    Notifications.standardToast("Hey Anon!", "I know you want to remain Anonymous but there are benefits to registering!", "app-defined-string");
                }

                difficulty = storage.LoadSettings(gameDFDatakey);
            }

            if (difficulty == "0")
            {
                countdown = 89;
                countdownReset = 89;
            }
            else if (difficulty == "1")
            {
                countdown = 44;
                countdownReset = 44;
            }
            else if (difficulty == "2" || difficulty == "3" || difficulty == "4")
            {
                countdown = 29;
                countdownReset = 29;
            }

        }


        private void startInteraction(object sender, RoutedEventArgs e)
        {
           
            timer.Tick += counter;
            timer.Interval = new TimeSpan(0, 0, 0, 1);
            timer.Start();

            interactionsGrid.Visibility = Visibility.Visible;

            if (difficulty == "4")
            {
                countdownSound.Play();
            }
            else if (difficulty == "3")
            {
                countdownSound.Play();
            }


        }

        private void counter(object sender, object e)
        {
            countdown--;

            var span = new TimeSpan(0, 0, countdown);
            var clock = string.Format("{0}:{01:00}",
                                        (int)span.TotalMinutes,
                                        span.Seconds);

            countdownClock.Text = clock;

            if (countdown == 15)
            {
                myMockBox.Text = name + "... " + name + "... You have 15 seconds left!";
            }
            else if (countdown == 10)
            {
                myMockBox.Text = "This is the easiest problem you have ever had...";
            }
            else if (countdown == 5)
            {
                myMockBox.Text = "Wow... You have definitely won this one!";
            }
            else if (countdown == 0)
            {
                interactionsGrid.Visibility = Visibility.Collapsed;
                interactionStart.Visibility = Visibility.Collapsed;
                interactionAnswer.Visibility = Visibility.Visible;
            }
            
        }

        private void optionsChosen(object sender, RoutedEventArgs e)
        {
            var button = (sender as Button);
            var destination = button.Tag.ToString();


            var target = "http://www.kshatriya.co.uk/dev/project/service/page.php";

            searchPages(target, passedParameter, destination);
        }

        private void interactionOptions(object sender, RoutedEventArgs e)
        {
            var button = (sender as Button);
            var answer = button.Tag.ToString();
            string destination = "";

            string userAnswer = myAnswerBox.Text.ToUpper();

            if(userAnswer == answer)
            {
                destination = countdownClock.Tag.ToString();
            }
            else
            {
                destination = myMockBox.Tag.ToString();
            }

            myAnswerBox.Text = "";

            var target = "http://www.kshatriya.co.uk/dev/project/service/page.php";

            searchPages(target, passedParameter, destination);

        }


        private async void searchPages(string target, string toGetStory, string toGetPage)
        {

            interactionsGrid.Visibility = Visibility.Collapsed;
            interactionStart.Visibility = Visibility.Visible;

            countdown = 0;
            countdown = countdownReset;
            countdownSound.Stop();
            timer.Stop();
            timer = new DispatcherTimer();

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

        private void getSearchResults(string JSON)
        {
            var viewModel = new PagesDataSource(JSON);
            this.DataContext = viewModel;
            searchProgressRing.IsActive = false;

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
