using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Windows.Threading;

namespace YourAdventure
{
    public partial class Game : PhoneApplicationPage
    {
        MediaElement BackgroundMusic;
        DispatcherTimer TransitionTimer;

        public Game()
        {
            InitializeComponent();
            this.DataContext = App.ViewModel;
            App.ViewModel.SceneThresholdInvoked += new EventHandler<Model.SceneThresholdEventArgs>(ViewModel_SceneThresholdInvoked);
            App.ViewModel.SaveAndQuit += ViewModel_SaveAndQuit;
            App.ViewModel.SaveAndLoad += ViewModel_SaveAndLoad;

            
            string url = "Content/Music/21 Final Confrontation.wma";
            BackgroundMusic = new MediaElement();
            BackgroundMusic.Source = new Uri(url, UriKind.RelativeOrAbsolute);
            BackgroundMusic.AutoPlay = true;
            BackgroundMusic.Volume = 1.0;
            BackgroundMusic.MediaEnded += new RoutedEventHandler(mm_MediaEnded);
            LayoutRoot.Children.Add(BackgroundMusic);
            BackgroundMusic.Play();

            TransitionTimer = new DispatcherTimer();
            TransitionTimer.Interval = new TimeSpan(0, 0, 15);
            TransitionTimer.Tick += new EventHandler(TransitionTimer_Tick);
            TransitionTimer.Start();
        }

        void ViewModel_SaveAndLoad(object sender, EventArgs e)
        {
            App.ViewModel.SaveGame();
            this.NavigationService.Navigate(new Uri("/LoadGame.xaml", UriKind.Relative));

        }

        void ViewModel_SaveAndQuit(object sender, EventArgs e)
        {
            App.ViewModel.SaveGame();
            this.NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
        }

        void TransitionTimer_Tick(object sender, EventArgs e)
        {
            var CurrentScene = App.ViewModel.CurrentScene;
            if (CurrentScene.TransitionToNext != null){
                TransitionTimer.Interval = CurrentScene.TransitionToNext.TimeTillNext;
                App.ViewModel.CurrentScene = CurrentScene.TransitionToNext.NextScene;
                TransitionTimer.Start();
            }
            else if (App.ViewModel.CurrentScene.Decisions.Count > 0)
            {
                this.TransitionTimer.Stop();
                this.blcNarrative.Visibility = System.Windows.Visibility.Collapsed;
                this.lstDecisions.Visibility = System.Windows.Visibility.Visible;
                this.lstDecisions.IsEnabled = true;
            }
        }

        void mm_MediaEnded(object sender, RoutedEventArgs e)
        {
            BackgroundMusic.Play();
        }

        void ViewModel_SceneThresholdInvoked(object sender, Model.SceneThresholdEventArgs e)
        {
            MessageBox.Show(string.Format("Your current Karma score of {0} is not enough to move onto the next level. You needed a karma score of {1} to move on. You are being moved to the start of the current level and your karma will be adjusted.", App.ViewModel.Karma, e.Threshold.RequiredKarma), "Insufficient Karma", MessageBoxButton.OK);
            //Reset the Karma to threshold of Scene returning to.
            App.ViewModel.Karma = e.Threshold.GoBackTo.Threshold.RequiredKarma;

            var NextScene = e.Threshold.GoBackTo;
            if (NextScene.TransitionToNext != null)
                TransitionTimer.Interval = NextScene.TransitionToNext.TimeTillNext;
            else
                TransitionTimer.Interval = new TimeSpan(0, 0, 15);
            App.ViewModel.CurrentScene = NextScene;
        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            
        }

        private void lstDecisions_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var DecListBox = sender as ListBox;
            var SelectedDecision = DecListBox.SelectedItem as Model.Decision;
            if (SelectedDecision != null)
            {
                App.ViewModel.Karma += SelectedDecision.KarmaAdjustment;

                var NextScene = SelectedDecision.LeadsTo;
                if (NextScene.TransitionToNext != null)
                    TransitionTimer.Interval = NextScene.TransitionToNext.TimeTillNext;
                else
                    TransitionTimer.Interval = new TimeSpan(0, 0, 15);
                App.ViewModel.CurrentScene = NextScene;

                this.blcNarrative.Visibility = System.Windows.Visibility.Visible;
                this.lstDecisions.Visibility = System.Windows.Visibility.Collapsed;
                this.lstDecisions.IsEnabled = false;
                TransitionTimer.Start();
            }
        }

        private void ToRTD_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("/RTD.xaml", UriKind.Relative));
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            App.ViewModel.SaveGame();
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            this.NavigationService.Navigate(new Uri("/LoadGame.xaml",UriKind.Relative));
        }

        private void btnHome_Click(object sender, EventArgs e)
        {   
            MessageBoxResult result = MessageBox.Show("You are now returning to the main menu. Would you like to save your progress?", "Return to Main Menu", MessageBoxButton.OKCancel);
            if (result == MessageBoxResult.OK)
                App.ViewModel.SaveGame();
            
            this.NavigationService.Navigate(new Uri("/MainPage.xaml",UriKind.Relative));
        }

        private void btnHelp_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Show the power of your positive thinking by making the right decisions and increasing your karma. If you have enough karma by the end of the level you will move onto the next level. If you are not sure what your supposed to be doing in any level, click the RTD button for more information.");
        }

        private void blcNarrative_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            TransitionTimer_Tick(sender, e);
        }


    }
}

