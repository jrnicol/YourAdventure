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
    public partial class LoadGame : PhoneApplicationPage
    {
        public LoadGame()
        {
            InitializeComponent();
            this.DataContext = App.ViewModel;
        }

        private void btnLoadGame_Click(object sender, RoutedEventArgs e)
        {
            var SavedGame = this.lstSavedGames.SelectedItem as string;
            if (SavedGame != null)
            {
                App.ViewModel.LoadGame(SavedGame);
                this.NavigationService.Navigate(new Uri("/Game.xaml", UriKind.Relative));
            }
        }

        private void btnHome_Click(object sender, EventArgs e)
        {
            if (App.ViewModel.User != null)
            {
               MessageBoxResult result = MessageBox.Show("Would you like to save your progress?", "Save Current Game?", MessageBoxButton.OKCancel);
               if (result == MessageBoxResult.OK)
                   App.ViewModel.SaveGame();
            }
            this.NavigationService.Navigate(new Uri("/MainPage.xaml",UriKind.Relative));
        }

        private void btnHelp_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Select a game to load. Hit the Load Selected Game button to take you back to a previous game. If no saved game exists hit the home button in the application bar to start a new game.");
        }


    }
}