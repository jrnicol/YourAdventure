using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace YourAdventure
{
    public partial class RTD : PhoneApplicationPage
    {
        public RTD()
        {
            InitializeComponent();
            this.DataContext = App.ViewModel;
        }

        private void BackToGame_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("/Game.xaml", UriKind.Relative));
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            App.ViewModel.SaveGame();
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            this.NavigationService.Navigate(new Uri("/LoadGame.xaml", UriKind.Relative));
        }

        private void btnHome_Click(object sender, EventArgs e)
        {

            MessageBoxResult result = MessageBox.Show("Would you like to save your progress?", "Save Current Game?", MessageBoxButton.OKCancel);
            if (result == MessageBoxResult.OK)
                App.ViewModel.SaveGame();

            this.NavigationService.Navigate(new Uri("/MainPage.xaml",UriKind.Relative));
        }

        private void btnHelp_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Here you will find information on what you are supposed to be doing in the current level as well as information about your current location and time. Hit the Back to the Game button to return to the game.");
        }
    }
}