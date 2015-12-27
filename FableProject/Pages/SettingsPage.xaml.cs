using FableProject.Data;
using FableProject.Presentation;
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
    public sealed partial class SettingsPage : Page
    {
        public SettingsPage()
        {
            this.InitializeComponent();
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

        private void roamingSet(object sender, RoutedEventArgs e)
        {
            string rDataKey = "roamingDetails";

            Storage storage = new Storage();

            string roamingData = storage.LoadSettings(rDataKey);

            string title = "Roaming";
            
            storage.SaveSettings(rDataKey, "true");
            string message = "enabled";
            feedbackDialog(title, message);

        }
        

        private void roamingUnset(object sender, RoutedEventArgs e)
        {
            string rDataKey = "roamingDetails";

            Storage storage = new Storage();

            string roamingData = storage.LoadSettings(rDataKey);

            string title = "Roaming";

            storage.SaveSettings(rDataKey, "false");
            string message = "disabled";
            feedbackDialog(title, message);

        }

        //Authentication Pivot Page

        private void Login_Event(object sender, RoutedEventArgs e)
        {
            
        }

        private void Register_Event(object sender, RoutedEventArgs e)
        {
            
        }

        //Dialog Box

        private async void feedbackDialog(string title, string messageDetails)
        {
            object sender = null;
            string template = "You have {0} Roaming your settings and data with our Windows App on your other devices with your Microsoft Account.";
            string message = string.Format(template, messageDetails);
            int commands = 1;
            Dialog.standardDialog(title, message, commands, sender);

        }

    }
}
