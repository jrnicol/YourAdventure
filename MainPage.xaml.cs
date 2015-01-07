using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;

namespace YourAdventure
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();
            
            this.Loaded += new RoutedEventHandler(MainPage_Loaded);
        }

        // Load data for the ViewModel Items
        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
        }

        private void Continue_Game_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("/LoadGame.xaml", UriKind.Relative));
        }

        private void New_Game_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("/NewGame.xaml", UriKind.Relative));
        }

        private void btnHelp_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Select Start A New Game button to begin Your Adventure anew. Select Load Game button to continue your adventures where you left off.");
        }
    }
}