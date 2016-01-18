using FableProject.Data;
using FableProject.DataModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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

        }
    }
}
