using FableProject.Pages;
using FableProject.Presentation;
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
            statusHide();

            var kshatriyaCobolt = Color.FromArgb(0, 63, 81, 181);
            var titleBar = ApplicationView.GetForCurrentView().TitleBar;
            titleBar.BackgroundColor = kshatriyaCobolt;
            titleBar.ForegroundColor = Colors.White;
            titleBar.ButtonBackgroundColor = kshatriyaCobolt;
            titleBar.ButtonForegroundColor = Colors.White;

            this.InitializeComponent();

            //SplitView "Hamburger" Menu items
            var vm = new ShellViewModel();
            vm.MenuItems.Add(new MenuItem { Icon = "", Title = "Welcome", PageType = typeof(WelcomePage) });
            vm.MenuItems.Add(new MenuItem { Icon = "", Title = "Page 1", PageType = typeof(Page1) });
            vm.MenuItems.Add(new MenuItem { Icon = "", Title = "Page 2", PageType = typeof(Page2) });
            vm.MenuItems.Add(new MenuItem { Icon = "", Title = "Page 3", PageType = typeof(Page3) });

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
