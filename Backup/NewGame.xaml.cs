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
    public partial class NewGame : PhoneApplicationPage
    {
        public NewGame()
        {
            InitializeComponent();
        }

        private void btnStartGame_Click(object sender, RoutedEventArgs e)
        {
            App.ViewModel.User = new Model.User() { Name = this.txtName.Text, Gender=(Model.UserGender)this.lstGender.SelectedIndex, Grade = (Model.UserGrade)this.lstGrade.SelectedIndex};
            App.ViewModel.LoadData();
            this.NavigationService.Navigate(new Uri("/Game.xaml", UriKind.Relative));

        }

        private void btnHome_Click(object sender, EventArgs e)
        {
            this.NavigationService.Navigate(new Uri("/MainPage.xaml",UriKind.Relative));
        }

        private void btnHelp_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Enter your user information. Hit Start Game button to launch Your Adventure");
        }


    }
}