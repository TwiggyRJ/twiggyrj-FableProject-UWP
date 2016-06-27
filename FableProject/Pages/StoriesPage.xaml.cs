
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
    public sealed partial class StoriesPage : Page
    {
        public StoriesPage()
        {
            this.InitializeComponent();

            searchProgressRing.IsActive = true;
            searchStories(App.siteURL+"/dev/project/service/stories.php", "all");
        }
        private async void searchStories(string target, string toGet)
        {

            var client = new HttpClient();

            var uri = UriExtensions.CreateUriWithQuery(new Uri(target),
            new NameValueCollection { { "story", toGet } });

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
                var title = "No Search Results :(";
                var message = "Uh, Oh, Spadoodios! We could not find any results for your search. We feel sad now...";
                errorDialog(title, message);
            }
        }

        private void getSearchResults(string JSON)
        {
            var viewModel = new StoriesDataSource(JSON, "search");
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

        private void gridviewStories_SelectionClicked(object sender, ItemClickEventArgs e)
        {
            Stories item = e.ClickedItem as Stories;
            string itemID = item.ID;
            string itemTitle = item.Title;

            Frame.Navigate(typeof(SelectedStoryPage), itemID);
        }

        private void typeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string story = ((ComboBoxItem)typeComboBox.SelectedItem).Content.ToString();

            searchStories(App.siteURL + "/dev/project/service/stories.php", story);
        }

        private void getButton_Event(object sender, RoutedEventArgs e)
        {
            string story = mySearchBox.Text;

            searchStories(App.siteURL + "/dev/project/service/stories.php", story);
        }

        private void searchHide(object sender, RoutedEventArgs e)
        {
            if(searchGrid.Visibility == Visibility.Visible)
            {
                searchGrid.Visibility = Visibility.Collapsed;
                searchProgressRing.Margin = new Thickness(0, -100, 0, 0);
            }
            else
            {
                searchGrid.Visibility = Visibility.Visible;
                searchProgressRing.Margin = new Thickness(0, -50, 0, 0);
            }
        }
    }
}
