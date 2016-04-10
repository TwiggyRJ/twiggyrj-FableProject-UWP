using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;

namespace FableProject.Functions
{
    class Notifications
    {

        public static void standardToast(string title, string message, string defined)
        {
            var standardToastTemplate = "<toast launch=\"{2}\"> " +
                                            "<visual>" +
                                                "<binding template = \"ToastGeneric\">" +
                                                    "<text> {0} </text>" +
                                                    "<text> {1} </text>" +
                                                    "<image placement = \"appLogoOverride\" src = \"ms-appx:///Assets/Square150x150Logo.scale-400.png\" />" +
                                                "</binding>" +
                                            "</visual>" +
                                        "</toast> ";
            var standardToastNotification = string.Format(standardToastTemplate, title, message, defined);

            var xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(standardToastNotification);

            // create the toast notification and show to user
            var toastNotification = new ToastNotification(xmlDocument);
            var notification = ToastNotificationManager.CreateToastNotifier();
            notification.Show(toastNotification);

        }

        public static void standardToast_Navigate(string title, string message, string direction, string defined)
        {
            var standardToastTemplate = "<toast launch=\"{2}\"> " +
                                            "<visual>" +
                                                "<binding template = \"ToastGeneric\">" +
                                                    "<text> {0} </text>" +
                                                    "<text> {1} </text>" +
                                                    "<image placement = \"appLogoOverride\" src = \"ms-appx:///Assets/Square150x150Logo.scale-400.png\" />" +
                                                "</binding>" +
                                            "</visual>" +
                                            "<actions>" +
                                                "<action activationType = \"foreground\" content = \"Register\" arguments = \"{3}\" />" +
                                                "<action activationType = \"foreground\" content = \"Not Interested\" arguments = \"later\" />" +
                                            "</actions>" +
                                        "</toast> ";
            var standardToastNotification = string.Format(standardToastTemplate, title, message, direction, defined);

            var xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(standardToastNotification);

            // create the toast notification and show to user
            var toastNotification = new ToastNotification(xmlDocument);
            var notification = ToastNotificationManager.CreateToastNotifier();
            notification.Show(toastNotification);

        }

    }
}
