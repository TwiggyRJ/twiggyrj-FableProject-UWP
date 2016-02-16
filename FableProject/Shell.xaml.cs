using FableProject.Data;
using FableProject.Pages;
using FableProject.Presentation;
using FableProject.Themes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

namespace FableProject
{
    public sealed partial class Shell : UserControl
    {
        public Shell()
        {
            // Setting the stage

            Storage storage = new Storage();
            Icons icons = new Icons();

            var accentColor = (Color)this.Resources["SystemAccentColor"];

            var kshatriyaCobolt = Color.FromArgb(0, 63, 81, 181);
            var titleBar = ApplicationView.GetForCurrentView().TitleBar;
            titleBar.BackgroundColor = accentColor;
            titleBar.ForegroundColor = Colors.White;
            titleBar.ButtonBackgroundColor = accentColor;
            titleBar.ButtonForegroundColor = Colors.White;


            string sDataKey = "userDetails";
            string uDataKey = "usernameDetails";
            string pDataKey = "passwordDetails";
            string rDatakey = "roamingDetails";
            string stDatakey = "statusBarDetails";
            string aDataKey = "authorDetails";
            string adDatakey = "adminDetails";

            string roamingSetting = storage.LoadSettings(rDatakey);

            string userData = "Null";
            string usernameDetails = "Null";
            string authorData = "Null";
            string adminData = "Null";

            if (roamingSetting == "true")
            {
                userData = storage.LoadRoamingSettings(sDataKey);
                usernameDetails = storage.LoadRoamingSettings(uDataKey);
                string passwordDetails = storage.LoadRoamingSettings(pDataKey);
                string statusData = storage.LoadRoamingSettings(stDatakey);
                authorData = storage.LoadRoamingSettings(aDataKey);
                adminData = storage.LoadRoamingSettings(adDatakey);

                if (statusData == "false")
                {
                    statusHide();
                }
                
            }
            else if (roamingSetting == "false")
            {
                userData = storage.LoadSettings(sDataKey);
                usernameDetails = storage.LoadSettings(uDataKey);
                string passwordDetails = storage.LoadSettings(pDataKey);
                authorData = storage.LoadSettings(aDataKey);
                adminData = storage.LoadSettings(adDatakey);

                string statusData = storage.LoadSettings(stDatakey);

                if (statusData == "false")
                {
                    statusHide();
                }
            }
            else if (roamingSetting == "Null")
            {
                userData = storage.LoadSettings(sDataKey);
                usernameDetails = storage.LoadSettings(uDataKey);
                string passwordDetails = storage.LoadSettings(pDataKey);
                authorData = storage.LoadSettings(aDataKey);
                adminData = storage.LoadSettings(adDatakey);

                string statusData = storage.LoadSettings(stDatakey);

                if (statusData == "false")
                {
                    statusHide();
                }
            }


            this.InitializeComponent();

            //SplitView "Hamburger" Menu items
            var vm = new ShellViewModel();
            vm.MenuItems.Add(new MenuItem { Icon = icons.EmojiIcon(), Title = "Welcome", PageType = typeof(WelcomePage) });
            vm.MenuItems.Add(new MenuItem { Icon = "", Title = "Page 1", PageType = typeof(Page1) });
            vm.MenuItems.Add(new MenuItem { Icon = icons.BookIcon(), Title = "Stories", PageType = typeof(StoriesPage) });


            if (userData != "Null")
            {

                vm.MenuItems.Add(new MenuItem { Icon = icons.PersonIcon(), Title = usernameDetails, PageType = typeof(ProfilePage) });
  

                if (adminData == "1")
                {
                    vm.MenuItems.Add(new MenuItem { Icon = icons.AdminIcon(), Title = "Administration Panel", PageType = typeof(AdminPage) });
                }

            }
            else
            {

                vm.MenuItems.Add(new MenuItem { Icon = icons.PersonIcon(), Title = "Profile Page", PageType = typeof(ProfilePage) });

            }

            vm.MenuItems.Add(new MenuItem { Icon = icons.NewIcon(), Title = "Create New Content", PageType = typeof(AuthorPage) });
            vm.MenuItems.Add(new MenuItem { Icon = icons.SettingsIcon(), Title = "Settings", PageType = typeof(SettingsPage) });


            // select the first menu item
            vm.SelectedMenuItem = vm.MenuItems.First();

            this.ViewModel = vm;

            // add entry animations
            var transitions = new TransitionCollection { };
            var transition = new NavigationThemeTransition { };
            transitions.Add(transition);
            this.Frame.ContentTransitions = transitions;

        }

        private void statusHide()
        {
            if (Windows.Foundation.Metadata.ApiInformation.IsTypePresent("Windows.UI.ViewManagement.StatusBar"))
            {
                var i = Windows.UI.ViewManagement.StatusBar.GetForCurrentView().HideAsync();
            }
        }


        public ShellViewModel ViewModel { get; private set; }

        public Frame RootFrame
        {
            get
            {
                return this.Frame;
            }
        }
    }
}
