using FableProject.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
        public string name;
        public string difficulty;


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
            else if (roamingSetting == "false")
            {
                name = storage.LoadSettings(nDataKey);
                difficulty = storage.LoadSettings(gameDFDatakey);
            }
            else if (roamingSetting == "Null")
            {
                name = storage.LoadSettings(nDataKey);
                difficulty = storage.LoadSettings(gameDFDatakey);
            }

            if (difficulty == "0")
            {
                countdown = 89;
            }
            else if (difficulty == "1")
            {
                countdown = 44;
            }
            else if (difficulty == "2")
            {
                countdown = 29;
            }
            else if (difficulty == "3")
            {
                countdown = 29;
            }


        }


        private void startInteraction(object sender, RoutedEventArgs e)
        {
            var timer = new DispatcherTimer();
            timer.Tick += counter;
            timer.Interval = new TimeSpan(0, 0, 0, 1);
            timer.Start();

            interactionsGrid.Visibility = Visibility.Visible;

            if (difficulty == "2")
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
    }
}
