using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace FableProject.Presentation
{
    class Dialog
    {

        public static async void standardDialog(string title, string content, int commands, object sender)
        {
            var dialog = new Windows.UI.Popups.MessageDialog(content, title);

            if(commands == 1)
            {
                dialog.Commands.Add(new Windows.UI.Popups.UICommand("OK") { Id = 0 });

                dialog.DefaultCommandIndex = 0;

                var result = await dialog.ShowAsync();

                if(sender != null)
                {
                    var btn = sender as Button;
                    btn.Content = $"Result: {result.Label} ({result.Id})";
                }

            }
            if (commands == 99)
            {
                dialog.Commands.Add(new Windows.UI.Popups.UICommand("Restart App") { Id = 0 });

                dialog.DefaultCommandIndex = 0;

                var result = await dialog.ShowAsync();

                if (sender == null)
                {
                    var btn = sender as Button;
                    btn.Content = $"Result: {result.Label} ({result.Id})";
                    btn.Click += delegate
                    {
                        CoreApplication.Exit();
                    };
                }

            }
            else if(commands == 2)
            {
                dialog.Commands.Add(new Windows.UI.Popups.UICommand("Yes") { Id = 0 });
                dialog.Commands.Add(new Windows.UI.Popups.UICommand("No") { Id = 1 });

                dialog.DefaultCommandIndex = 0;
                dialog.CancelCommandIndex = 1;

                var result = await dialog.ShowAsync();

                if (sender != null)
                {
                    var btn = sender as Button;
                    btn.Content = $"Result: {result.Label} ({result.Id})";
                }

            }
            else if(commands == 3)
            {
                dialog.Commands.Add(new Windows.UI.Popups.UICommand("Yes") { Id = 0 });
                dialog.Commands.Add(new Windows.UI.Popups.UICommand("No") { Id = 1 });

                dialog.DefaultCommandIndex = 0;
                dialog.CancelCommandIndex = 1;


                if (Windows.System.Profile.AnalyticsInfo.VersionInfo.DeviceFamily != "Windows.Mobile")
                {
                    // Adding a 3rd command will crash the app when running on Mobile !!!
                    dialog.Commands.Add(new Windows.UI.Popups.UICommand("Maybe later") { Id = 2 });
                }

                var result = await dialog.ShowAsync();

                if (sender != null)
                {
                    var btn = sender as Button;
                    btn.Content = $"Result: {result.Label} ({result.Id})";
                }

            }

        }
    }
}
