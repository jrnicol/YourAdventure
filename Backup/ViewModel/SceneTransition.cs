using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Practices.Prism.ViewModel;
using System.Collections.Generic;
using System.IO.IsolatedStorage;
using System.IO;
using System.Collections.ObjectModel;
//What is RTD stand for
//How do I get it to go to the home screen
namespace YourAdventure.ViewModel
{
    public class SceneTransition:NotificationObject
    {
        Dictionary<string, Model.Scene> Scenes;
        Model.Scene currentScene;
        int karma = 0;
        bool dataLoaded = false;

        public event EventHandler<Model.SceneThresholdEventArgs> SceneThresholdInvoked;
        public event EventHandler SaveAndQuit;
        public event EventHandler SaveAndLoad;

        public SceneTransition()
        {
        }

        #region Hooks View into Model - Used in Data Binding
        public Model.Scene CurrentScene
        {
            get 
            {
                if (this.currentScene == null)
                    currentScene = Scenes["1-1"];
                return this.currentScene; 
            }
            set
            {
                if (this.currentScene != value)
                {
                    if (value.ID == "SaveAndQuit" && SaveAndQuit != null)
                            SaveAndQuit(value, new EventArgs());
                    if (value.ID == "SaveAndLoad" && SaveAndLoad != null)
                        SaveAndLoad(value, new EventArgs());
                    //If the scene has a threshold to continue and is being handled in the view raise the event.
                    if (value.Threshold != null && SceneThresholdInvoked != null && value.Threshold.RequiredKarma > this.karma)
                    {
                        SceneThresholdInvoked(value, new Model.SceneThresholdEventArgs(value.Threshold));
                    }
                    else
                    {
                        this.currentScene = value;
                        this.RaisePropertyChanged(() => this.CurrentScene);
                    }
                }
            }
        }
        public Model.User User { get; set; }
        public int Karma
        {
            get { return this.karma; }
            set
            {
                if (this.karma != value)
                {
                    this.karma = value;
                    this.RaisePropertyChanged(() => this.Karma);
                }
            }
        }
        #endregion

        #region Define and Load Program Data
        /*
         * Loads Scene structure, decision tree, and level thresholds into memory.
         */
        public void LoadData()
        {
            this.currentScene = null;
            this.karma = 0;

            string UserImgSrc = null;
            string LockerImgSrc = null;
            if (User.Gender == Model.UserGender.Boy)
            {
                UserImgSrc = "Content/Images/usermale.png";
                LockerImgSrc = "Content/Backgrounds/boylocker.jpg";
            }
            if (User.Gender == Model.UserGender.Girl)
            {
                UserImgSrc = "Content/Images/femaleuser.png";
                LockerImgSrc = "Content/Backgrounds/AnimeSchoolLocker-3.jpg";
            }
            List<Model.Scene> SceneList = new List<Model.Scene>();
            //Self Discovery and Tutorial Sequence
            #region Level 1 Scenes
            SceneList.Add(new Model.Scene()
            {
                ID = "1-1",
                Narrative = string.Format("Narrator: 'Hello {0}, and welcome to 'Your Adventure the Search for Sherpa'. This is a choose your own adventure style game. The narration will appear in the bottom of the screen. When you are finished reading it you can either click anywhere within this text box or wait for the timer.' (Click or Wait)", User.Name),
                BackgroundImgSrc = "Content/Backgrounds/future-education.png",
                UserImgSrc = UserImgSrc,
                RTD = new Model.RTD() { Objective = " -Learn about game", WhereAmI = " -Game Tutorial" },
                Decisions = new System.Collections.ObjectModel.ObservableCollection<Model.Decision>()
            });
            SceneList.Add(new Model.Scene()
            {
                ID = "1-11",
                Narrative = string.Format("Narrator: '{0}, after most narrations you will be presented with different situations followed by a series of choices. Decision points will appear at the top of the screen The IMPORTANT part is to click the text of the decision you want. The correct decision will increase you Karma Points, while wrong decisions will decrease them.'", User.Name),
                BackgroundImgSrc = "Content/Backgrounds/future-education.png",
                UserImgSrc = UserImgSrc,
                RTD = new Model.RTD() { Objective = " -Learn about game", WhereAmI = " -Game Tutorial" },
                Decisions = new System.Collections.ObjectModel.ObservableCollection<Model.Decision>()
            });
            SceneList.Add(new Model.Scene()
            {
                ID = "1-12",
                Narrative = string.Format("Narrator: '{0}, The karma points will show up in red at the bottom of the background image. If you are ever unsure of what to do their are two ways to seek assistance Next to see the help functions. {0}, would you like to see the application bar help features and make first decision?'", User.Name),
                BackgroundImgSrc = "Content/Backgrounds/future-education.png",
                UserImgSrc = UserImgSrc,
                RTD = new Model.RTD() { Objective = " -Learn about game", WhereAmI = " -Game Tutorial" },
                Decisions = new System.Collections.ObjectModel.ObservableCollection<Model.Decision>()
            });
            SceneList.Add(new Model.Scene()
            {
                ID = "1-13",
                Narrative = string.Format("Narrator: The application bar help function is accesses by pressing the '...' icon or one of the active buttons within the tool.  Clicking the '...' will show a description of the icon. Click on it now.", User.Name),
                BackgroundImgSrc = "Content/Backgrounds/future-education.png",
                UserImgSrc = UserImgSrc,
                RTD = new Model.RTD() { Objective = " -Learn about game", WhereAmI = " -Game Tutorial" },
                Decisions = new System.Collections.ObjectModel.ObservableCollection<Model.Decision>()
            });
            SceneList.Add(new Model.Scene()
            {
                ID = "1-14",
                Narrative = string.Format("Narrator: Now that you have seen the application bar I will show you the help Button.  This feature is labeled RTD shows your objective and mission depending on where you are at. This is the PRIMARY help function!", User.Name),
                BackgroundImgSrc = "Content/Backgrounds/future-education.png",
                UserImgSrc = UserImgSrc,
                RTD = new Model.RTD() { Objective = " -Learn about game", WhereAmI = " -Game Tutorial" },
                Decisions = new System.Collections.ObjectModel.ObservableCollection<Model.Decision>()
            });
            SceneList.Add(new Model.Scene()
            {
                ID = "1-15",
                Narrative = string.Format("Narrator:'Ok, {0}, looks like you are ready to go. Remember the positive outlook produces positive outcomes!!!'", User.Name),
                BackgroundImgSrc = "Content/Backgrounds/future-education.png",
                UserImgSrc = UserImgSrc,
                RTD = new Model.RTD() { Objective = " -Learn about game", WhereAmI = " -Game Tutorial" },
                Decisions = new System.Collections.ObjectModel.ObservableCollection<Model.Decision>()
            });
            SceneList.Add(new Model.Scene()
            {
                ID = "1-2",
                Narrative = string.Format("Narrator: First situation: Your classmate's parents have just been divorced and the news is all over Facebook; you overhear him sobbing after some kids make fun of him. This kind of bullying happens all the time. {0}, I hope you feel compelled to help him out.", User.Name, User.Grade),
                BackgroundImgSrc = "Content/Backgrounds/park.jpg",
                OtherCharImgSrc = "Content/Images/sadimo.png",
                UserImgSrc = UserImgSrc,
                RTD = new Model.RTD() { Objective = " - Discover the RTD", WhereAmI = " - I am at School" },
                Decisions = new System.Collections.ObjectModel.ObservableCollection<Model.Decision>(),
            });
            
            SceneList.Add(new Model.Scene()
            {
                ID = "1-3",
                Narrative = string.Format("{0} - WOW! What is this? Some kind of futuristic technology?... I Better take it this to my computer nerd neighbor for analysis.", User.Name),
                BackgroundImgSrc = LockerImgSrc,
                UserImgSrc = UserImgSrc,
                RTD = new Model.RTD() { Objective = " - Take the RTD", WhereAmI = " - You are at school looking inside your locker" },
                Decisions = new System.Collections.ObjectModel.ObservableCollection<Model.Decision>()
            });
            
            SceneList.Add(new Model.Scene()
            {
                ID = "1-4",
                Narrative = "On your way home the machine activates and starts making noises. You notice something similar to a micro-USB plug-in; maybe it will work with your neighbor's computer! Your mother calls and dinner is ready but you want to see your neighbor.",
                BackgroundImgSrc = "Content/Backgrounds/2ndScene.jpg",
                UserImgSrc = UserImgSrc,
                RTD = new Model.RTD() { Objective = " - Discovering What and Why the RTD found you", WhereAmI = " - In your neighborhood" },
                Decisions = new System.Collections.ObjectModel.ObservableCollection<Model.Decision>()
            });
            SceneList.Add(new Model.Scene()
            {
                ID = "1-5",
                Narrative = "You have decided to stop by your friend's place to see if he is able to learn anything from this technology.",
                BackgroundImgSrc = "Content/Backgrounds/rtdEducation.jpg",
                UserImgSrc = UserImgSrc,
                RTD = new Model.RTD() { Objective = " - Discovering What and Why the RTD found you", WhereAmI = " - In your neighborhood" },
                Decisions = new System.Collections.ObjectModel.ObservableCollection<Model.Decision>()
            });
            #endregion
            //RTD - Education
            #region Level 2 Scenes
            SceneList.Add(new Model.Scene()
            {
                ID = "2-1",
                Narrative = string.Format("Friend:,'Hello {0},What do you have to show me?' {0},'I found this in my locker today at school.  All of the sudden a voice begins describing the technology|RTD-I am an RTD and can travel through time along with detecting people's auras and tell if they are good natured | You stood out!", User.Name),
                BackgroundImgSrc = "Content/Backgrounds/rtdEducation.jpg",
                UserImgSrc = UserImgSrc,
                RTD = new Model.RTD() { Objective = " - Choose to Accept the Mission", WhereAmI = " - In your neighbor's bedroom" },
                Decisions = new System.Collections.ObjectModel.ObservableCollection<Model.Decision>()
            });

            SceneList.Add(new Model.Scene()
            {
                ID = "2-2",
                Narrative = "RTD: This is going to be a dangerous mission!  One of the most important leaders, Sherpa, has been abducted by a man named General Aveel and we believe that her disappearance will have dire consequences on the future.",
                BackgroundImgSrc = "Content/Backgrounds/rtdEducation.jpg",
                UserImgSrc = UserImgSrc,
                RTD = new Model.RTD() { Objective = " - Choose to Accept the Mission", WhereAmI = " - In your neighbor's bedroom" },
                Decisions = new System.Collections.ObjectModel.ObservableCollection<Model.Decision>()
            });
            SceneList.Add(new Model.Scene()
            {
                ID = "2-3",
                Narrative = "RTD: Thank you for accepting the mission.  With your permission I would like to send you to the future to meet our team.  Decide whether or not to use the RTD.",
                BackgroundImgSrc = "Content/Backgrounds/rtdEducation.jpg",
                UserImgSrc = UserImgSrc,
                RTD = new Model.RTD() { Objective = " - Choose to Accept the Mission", WhereAmI = " - In your friend's bedroom" },
                Decisions = new System.Collections.ObjectModel.ObservableCollection<Model.Decision>()
            });
            SceneList.Add(new Model.Scene()
            {
                ID = "2-35",
                Narrative = "RTD: Please do not contemplate your decision for long; the future depends on the choices we are making right now! Make your final decision next.",
                BackgroundImgSrc = "Content/Backgrounds/2ndScene.jpg",
                UserImgSrc = UserImgSrc,
                RTD = new Model.RTD() { Objective = " - Choose to Accept the Mission", WhereAmI = " - In your friend's bedroom" },
                Decisions = new System.Collections.ObjectModel.ObservableCollection<Model.Decision>()
            });
            SceneList.Add(new Model.Scene()
            {
                ID = "2-40",
                Narrative = string.Format("RTD: Goodnight {0} and I hope you will decide to come help us to save the world!!", User.Name),
                BackgroundImgSrc = "Content/Backgrounds/bedroom.jpg",
                UserImgSrc = UserImgSrc,
                RTD = new Model.RTD() { Objective = " - Choose to Accept the Mission", WhereAmI = " - In your bedroom" },
                Decisions = new System.Collections.ObjectModel.ObservableCollection<Model.Decision>()
            });
            SceneList.Add(new Model.Scene()
            {
                ID = "2-41",
                Narrative = string.Format("{0},'I have so much to think about tonight!  I cannot believe that I am barely a teenager and some machine from the future wants me to help save the world. This is a wild situation and I need to sleep on it!'", User.Name),
                BackgroundImgSrc = "Content/Backgrounds/bedroom.jpg",
                UserImgSrc = UserImgSrc,
                RTD = new Model.RTD() { Objective = " -Choose to Accept the Mission", WhereAmI = " -In your neighbor's bedroom" },
                Decisions = new System.Collections.ObjectModel.ObservableCollection<Model.Decision>()
            });
            SceneList.Add(new Model.Scene()
            {
                ID = "2-42",
                Narrative = string.Format("RTD: Thank you for accepting the mission. With your permission I would like to send you to the future to meet our team.  Decide whether or not to use the RTD ", User.Name),
                BackgroundImgSrc = "Content/Backgrounds/bedroomday.jpg",
                UserImgSrc = UserImgSrc,
                RTD = new Model.RTD() { Objective = " -Choose to Accept the Mission", WhereAmI = " -In your friend's bedroom" },
                Decisions = new System.Collections.ObjectModel.ObservableCollection<Model.Decision>()
            });
            SceneList.Add(new Model.Scene()
            {
                ID = "2-43",
                Narrative = string.Format("RTD: I am sorry that you choose to quite the mission Would you like to reconsider?", User.Name),
                BackgroundImgSrc = "Content/Backgrounds/bedroomday.jpg",
                UserImgSrc = UserImgSrc,
                RTD = new Model.RTD() { Objective = " -Choose to Accept the Mission", WhereAmI = " -In your friend's bedroom" },
                Decisions = new System.Collections.ObjectModel.ObservableCollection<Model.Decision>()
            });
            #endregion
           //RTD - First Use
            #region Level 3 Scenes
            SceneList.Add(new Model.Scene()
            {
                ID = "3-1",
                Narrative = "RTD: You are now on your way to meet Dr. L at his secret lair in the future.  I will be your guide  through time. If at anytime you need help or are confused about the objective; you will find my button on the screen.  To see the outside of Dr. L's lair",
                BackgroundImgSrc = "Content/Images/rtdtimetravel.jpg",
                UserImgSrc = UserImgSrc,
                RTD = new Model.RTD() { Objective = "Wow the RTD is cool", WhereAmI = "Traveling Forward to another time and place" },
                Decisions = new System.Collections.ObjectModel.ObservableCollection<Model.Decision>()
            });

            SceneList.Add(new Model.Scene()
            {
                ID = "3-2",
                Narrative = string.Format("{0} - So where am I now?  Find out!",User.Name),
                BackgroundImgSrc = "Content/Backgrounds/Scene3BG.jpg",
                UserImgSrc = UserImgSrc,
                RTD = new Model.RTD() { Objective = " -Figure out Your Mission", WhereAmI = " -Dr. L and Sherpa's Secret Lab in the future" },
                Decisions = new System.Collections.ObjectModel.ObservableCollection<Model.Decision>()
            });

            #endregion
            //Dr. L's Lair
            #region Level 4 Scenes
            SceneList.Add(new Model.Scene()
            {
                ID = "4-1",
                Narrative = String.Format("{0} - Why me? How did some futuristic technology decide that I would be best suited for this mission?  Dr. L says - The RTD has the ability to look into the future and it chooses you {0}.",User.Name),
                BackgroundImgSrc = "Content/Backgrounds/scene4BG.jpg",
                OtherCharImgSrc = "Content/Images/DrL.png",
                UserImgSrc = UserImgSrc,
                RTD = new Model.RTD() { Objective = " -Listen to Dr. L", WhereAmI = " -Dr. L and Sherpa's Secret Lab in the future" },
                Decisions = new System.Collections.ObjectModel.ObservableCollection<Model.Decision>()
            });
           
            SceneList.Add(new Model.Scene()
            {
                ID = "4-2",
                Narrative = string.Format("Dr. L : You have been choose because of your good nature. Our research shows to significant influence is most likely achieved when the person is young.  Help me {0}. You're my only hope. Respond Next", User.Name),
                OtherCharImgSrc = "Content/Images/DrL.png",
                UserImgSrc = UserImgSrc,
                BackgroundImgSrc = "Content/Backgrounds/scene4BG.jpg",
                RTD = new Model.RTD() { Objective = " -Listen to Dr. L", WhereAmI = " -Dr. L and Sherpa's Secret Lab in the future" },
                Decisions = new System.Collections.ObjectModel.ObservableCollection<Model.Decision>()
            });

            SceneList.Add(new Model.Scene()
            {
                ID = "4-3",
                Narrative = "Dr. L: I am so glad that you have chosen to be part of our quest, we are protectors of time and the future depends on what we do here and now!  When it comes to negativity a small amount can have a viral effect on the future!  See More",
                BackgroundImgSrc = "Content/Backgrounds/scene4BG.jpg",
                OtherCharImgSrc = "Content/Images/DrL.png",
                UserImgSrc = UserImgSrc,
                RTD = new Model.RTD() { Objective = " -Figure Out your Mission", WhereAmI = " -Dr. L and Sherpa's Secret Lab in the future" },
                Decisions = new System.Collections.ObjectModel.ObservableCollection<Model.Decision>()
            });

            SceneList.Add(new Model.Scene()
            {
                ID = "4-4",
                Narrative = string.Format("{0} - So where is Sherpa now?  What happened to her that made me so important? Dr. L tells you that Sherpa was taken from her home by Hu-Bots which are Robots controlled by the enemy.  These Robots can also travel through time.",User.Name),
                BackgroundImgSrc = "Content/Backgrounds/scene4BG.jpg",
                OtherCharImgSrc = "Content/Images/DrL.png",
                UserImgSrc = UserImgSrc,
                RTD = new Model.RTD() { Objective = " -Use the RTD for the first Time", WhereAmI = " -Dr. L and Sherpa's Secret Lab in the future" },
                Decisions = new System.Collections.ObjectModel.ObservableCollection<Model.Decision>()
            });

            SceneList.Add(new Model.Scene()
            {
                ID = "4-5",
                Narrative = string.Format("{0},'I will do my best to save the world but I need to know what I'm up against, Can I see these Hubots?'", User.Name),
                BackgroundImgSrc = "Content/Backgrounds/scene4BG.jpg",
                OtherCharImgSrc = "Content/Images/DrL.png",
                UserImgSrc = UserImgSrc,
                RTD = new Model.RTD() { Objective = " -Use the RTD for the first Time", WhereAmI = " -Dr. L and Sherpa's Secret Lab in the future"},
                Decisions = new System.Collections.ObjectModel.ObservableCollection<Model.Decision>()
            });

            #endregion
            //Sherpa is taken sequence
            #region Level 5 Scenes
            SceneList.Add(new Model.Scene()
            {
                ID = "5",
                Narrative = "RTD:  One of my many useful features is the ability to show replays of real events.  Like an instant replay for a sporting event I can show you events from any time period that happened to anyone.",
                BackgroundImgSrc = "Content/Images/rtdtimetravel.jpg",
                UserImgSrc = UserImgSrc,
                RTD = new Model.RTD() { Objective = " -Wow the RTD is cool", WhereAmI = " -Peering into the RTD" },
                Decisions = new System.Collections.ObjectModel.ObservableCollection<Model.Decision>()
            });
            SceneList.Add(new Model.Scene()
            {
                ID = "5-1",
                Narrative = "RTD: This is Sherpa's home before the attack. The Hubots came to destroy it and kidnap Sherpa.  The Hubots are controlled by General Aveel and are very dangerous",
                BackgroundImgSrc = "Content/Backgrounds/SherpasHome.jpg",
               
                RTD = new Model.RTD() { Objective = " -Learn from RTD", WhereAmI = " -Peering into the RTD" },
                Decisions = new System.Collections.ObjectModel.ObservableCollection<Model.Decision>()
            });
            SceneList.Add(new Model.Scene()
            {
                ID = "5-2",
                Narrative = "RTD: Then the HuBots came, these robots can transform into basically anything they have seen; this makes them especially dangerous!!  I will help you spot them along the way.",
                BackgroundImgSrc = "Content/Backgrounds/SherpasHome.jpg",
                OtherCharImgSrc = "Content/Images/hubots.png",
                RTD = new Model.RTD() { Objective = " -Learn about HuBots", WhereAmI = " -Peering into the RTD" },
                Decisions = new System.Collections.ObjectModel.ObservableCollection<Model.Decision>()
            });
            SceneList.Add(new Model.Scene()
            {
                ID = "5-3",
                Narrative = string.Format("RTD:  General Aveel has figured out someway to block her tracking device from my scanners, which is where you come in. We have determined that you, {0} will make the right choices and lead you to Sherpa!!", User.Name),
                BackgroundImgSrc = "Content/Backgrounds/SherpasHome.jpg",
                OtherCharImgSrc = "Content/Images/hubots.png",
                UserImgSrc = "Content/Images/hubots.png",
                RTD = new Model.RTD() { Objective = " -Learn about HuBots", WhereAmI = " -Peering into the RTD" },
                Decisions = new System.Collections.ObjectModel.ObservableCollection<Model.Decision>()
            });
            SceneList.Add(new Model.Scene()
            {
                ID = "5-4",
                Narrative = "RTD:  We detected Sherpa leaving with the Hubot's so we know she is alive.  Our best guess is that General Aveel is trying to reproduce the technology of the RTD.  Begin the mission.",
                BackgroundImgSrc = "Content/Backgrounds/homefire.jpg",
                OtherCharImgSrc = "Content/Images/hubots.png",
                UserImgSrc = "Content/Images/sherpa.png",
                RTD = new Model.RTD() { Objective = " -Learn about HuBots", WhereAmI = " -Tiering into the RTD" },
                Decisions = new System.Collections.ObjectModel.ObservableCollection<Model.Decision>()
            });

            #endregion
           //Albert Einstein Sequence
            #region Level 6 Scenes
            SceneList.Add(new Model.Scene()
            {
                ID = "6-1",
                Narrative = string.Format("RTD: Now that you have seen what happened to Sherpa and you know more about what I am capable of, you are ready for action.  Our first stop is to help none other than Albert Einstein.  {0}, you were chosen because children are the future and seem to be greatly effected by small acts of kindness!!", User.Name),
                BackgroundImgSrc = "Content/Images/rtdtimetravel.jpg",
                UserImgSrc = UserImgSrc,
                RTD = new Model.RTD() { Objective = " -Wow the RTD is cool", WhereAmI = " -Traveling Back in Time" },
                Decisions = new System.Collections.ObjectModel.ObservableCollection<Model.Decision>()
            });
            SceneList.Add(new Model.Scene()
            {
                ID = "6-2",
                Narrative = "RTD: Young Albert is at boarding school and feels alone.  This is a natural feeling for young boys and girls to have when they are away from home, but someone like Albert Einstein needs special schooling. Positively influence Einstein",
                BackgroundImgSrc = "Content/Backgrounds/einsteinbg.jpg",
                UserImgSrc = UserImgSrc,
                OtherCharImgSrc = "Content/Images/EinsteinKid.png",
                RTD = new Model.RTD() { Objective = " -Help young Albert", WhereAmI = " -Outside of Einstein's Boarding School" },
                Decisions = new System.Collections.ObjectModel.ObservableCollection<Model.Decision>()
            });
            SceneList.Add(new Model.Scene()
            {
                ID = "6-3",
                Narrative = "RTD: Albert feels much better already, after you have spoken to him.  He is no longer thinking of running away.  His aura has begun to change to more positive readings - thank you time traveler!! - Give Albert some advice from one kid to another",
                BackgroundImgSrc = "Content/Backgrounds/einsteinbg.jpg",
                UserImgSrc = UserImgSrc,
                OtherCharImgSrc = "Content/Images/EinsteinKid.png",
                RTD = new Model.RTD() { Objective = " -Encourage young Einstein to seek whatever goals his heart desires", WhereAmI = " -Outside of Einstein's Boarding School" },
                Decisions = new System.Collections.ObjectModel.ObservableCollection<Model.Decision>()
            });
            SceneList.Add(new Model.Scene()
            {
                ID = "6-4",
                Narrative = string.Format("Albert decides to stay in school and his theories go on to change science forever.  Thank you so much {0} you have really saved the day!! I have detected a new disturbance.  See who or what needs help.", User.Name),
                BackgroundImgSrc = "Content/Backgrounds/einsteinbg.jpg",
                UserImgSrc = UserImgSrc,
                RTD = new Model.RTD() { Objective = " -Encourage young Einstein to seek whatever goals his heart desires", WhereAmI = " -Outside of Einstein's Boarding School" },
                Decisions = new System.Collections.ObjectModel.ObservableCollection<Model.Decision>()
            });
            #endregion
            //Bill Gates Sequence
            #region Level 7 Scenes
            SceneList.Add(new Model.Scene()
            {
                ID = "7-1",
                Narrative = string.Format("RTD: The person that you are going to help this time is none other than Bill Gates.  {0}-Wow what an important person in technology!!", User.Name),
                BackgroundImgSrc = "Content/Images/rtdtimetravel.jpg",
                UserImgSrc = UserImgSrc,
                RTD = new Model.RTD() { Objective = " -Wow the RTD is cool", WhereAmI = " -Traveling to another time, forward this time: Back to the Future!!" },
                Decisions = new System.Collections.ObjectModel.ObservableCollection<Model.Decision>()
            });
            SceneList.Add(new Model.Scene()
            {
                ID = "7-15",
                Narrative = "RTD: As you know Bill Gates becomes the CEO of Microsoft one of the most influential technology companies in 20th century history.  We think that General Aveel will try and use his technological mind to help him construct his own RTD.",
                BackgroundImgSrc = "Content/Images/rtdtimetravel.jpg",
                UserImgSrc = UserImgSrc,
                RTD = new Model.RTD() { Objective = " -Wow the RTD is cool", WhereAmI = " -Traveling to another time, forward this time: Back to the Future!!" },
                Decisions = new System.Collections.ObjectModel.ObservableCollection<Model.Decision>()
            });
            SceneList.Add(new Model.Scene()
            {
                ID = "7-16",
                Narrative = string.Format("{0}-Getting to meet Bill Gates is a dream come true!  I hope I can make a difference with positivity", User.Name),
                BackgroundImgSrc = "Content/Images/rtdtimetravel.jpg",
                UserImgSrc = UserImgSrc,
                RTD = new Model.RTD() { Objective = " -Wow the RTD is cool", WhereAmI = " -Traveling to another time, forward this time: Back to the Future!!" },
                Decisions = new System.Collections.ObjectModel.ObservableCollection<Model.Decision>()
            });
            SceneList.Add(new Model.Scene()
            {
                ID = "7-2",
                Narrative = "RTD: You find Mr. Gates in a technology lab in his high school he appears to be upset. To approach Bill and try to offer him comfort.",
                BackgroundImgSrc = "Content/Backgrounds/billgatesbg.jpg",
                UserImgSrc = UserImgSrc,
                OtherCharImgSrc = "Content/Images/billgateskid.png",
                RTD = new Model.RTD() { Objective = " -Encourage Bill to follow his college degree to achieve his dreams", WhereAmI = " -High School Tech Lab" },
                Decisions = new System.Collections.ObjectModel.ObservableCollection<Model.Decision>()
            });
            SceneList.Add(new Model.Scene()
            {
                ID = "7-25",
                Narrative = string.Format("-(Click Here)", User.Name),
                BackgroundImgSrc = "Content/Backgrounds/billgatesbg.jpg",
                UserImgSrc = UserImgSrc,
                OtherCharImgSrc = "Content/Images/billgateskid.png",
                RTD = new Model.RTD() { Objective = " - Encourage Bill to make the 'right' choices to follow his dreams", WhereAmI = " - High School Tech Lab" },
                Decisions = new System.Collections.ObjectModel.ObservableCollection<Model.Decision>()
            });
            SceneList.Add(new Model.Scene()
            {
                ID = "7-3",
                Narrative = string.Format("Bill Gates: Following my heart is important. Thank you {0}, I feel much better now after talking this out.  The pressures of society make me feel like it is impossible to truly pursue my heart but after speaking with you I feel better.", User.Name),
                BackgroundImgSrc = "Content/Backgrounds/billgatesbg.jpg",
                UserImgSrc = UserImgSrc,
                OtherCharImgSrc = "Content/Images/billgateskid.png",
                RTD = new Model.RTD() { Objective = " - Encourage Bill to make the 'right' choices to follow his dreams", WhereAmI = " - High School Tech Lab" },
                Decisions = new System.Collections.ObjectModel.ObservableCollection<Model.Decision>()
            });
            SceneList.Add(new Model.Scene()
            {
                ID = "7-4",
                Narrative = "Bill:  My heart tells me to pursue my dreams and something tells me that the world is going to need my technological input!!",
                BackgroundImgSrc = "Content/Backgrounds/billgatesbg.jpg",
                UserImgSrc = UserImgSrc,
                OtherCharImgSrc = "Content/Images/billgateskid.png",
                RTD = new Model.RTD() { Objective = " - Encourage Bill to pick what he truly feels excited to follow his dreams", WhereAmI = " - High School Tech Lab" },
                Decisions = new System.Collections.ObjectModel.ObservableCollection<Model.Decision>()
            });

            #endregion
            // Ansel Adams Sequence
            #region Level 8 Scenes
            SceneList.Add(new Model.Scene()
            {
                ID = "8-1",
                Narrative = "RTD:  The next historical figure that needs our help is none other than Ansel Adams an early 19th century photographer.  Over the centuries humans have lost their connection to nature. Meet Ansel Adams next.",
                BackgroundImgSrc = "Content/Images/rtdtimetravel.jpg",
                UserImgSrc = UserImgSrc,
                OtherCharImgSrc = "Content/Images/Level 8/AnselPro.jpg ",
                RTD = new Model.RTD() { Objective = " - Wow time travel is cool !!", WhereAmI = " - Traveling to another time, Backward" },
                Decisions = new System.Collections.ObjectModel.ObservableCollection<Model.Decision>()
            });
            SceneList.Add(new Model.Scene()
            {
                ID = "8-2",
                Narrative = "Ansel:  I am confused about pursuing my dreams of being a photographer.  What future does a photographer have?  I have a passion for nature's protection but in a time where industry is so highly valued what financial future will I have. Will you talk to Ansel?",
                BackgroundImgSrc = "Content/Backgrounds/anseladamsbg.jpg",
                UserImgSrc = UserImgSrc,
                OtherCharImgSrc = "Content/Images/Level 8/Anselphotophy.png ",
                RTD = new Model.RTD() { Objective = " - Encourage Ansel to keep taking pictures", WhereAmI = " - A mountain range in the early 1900s" },
                Decisions = new System.Collections.ObjectModel.ObservableCollection<Model.Decision>()
            });
            SceneList.Add(new Model.Scene()
            {
                ID = "8-3",
                Narrative = string.Format("Ansel:  Following my dreams is what's important!  I feel much better now thank you so much {0}. I have a hard time being positive all the time, thanks for the help.", User.Name),
                BackgroundImgSrc = "Content/Backgrounds/anseladamsbg.jpg",
                UserImgSrc = UserImgSrc,
                OtherCharImgSrc = "Content/Images/Level 8/Anselphotophy.png ",
                RTD = new Model.RTD() { Objective = " - Encourage Ansel to both keep  taking pictures and protecting environment", WhereAmI = " - A mountain range in the early 1900s" },
                Decisions = new System.Collections.ObjectModel.ObservableCollection<Model.Decision>()
            });
            SceneList.Add(new Model.Scene()
            {
                ID = "8-4",
                Narrative = "RTD:  The RTD has detected something coming out of time warp! - Oh no the Hubots have found us. Get out of here quick!! Click here now!!",
                BackgroundImgSrc = "Content/Backgrounds/anseladamsbg.jpg",
                UserImgSrc = UserImgSrc,
                OtherCharImgSrc = "Content/Images/hubots.png",
                RTD = new Model.RTD() { Objective = " - Think more positive thoughts!", WhereAmI = " - A mountain range in the early 1900s" },
                Decisions = new System.Collections.ObjectModel.ObservableCollection<Model.Decision>()
            });
            SceneList.Add(new Model.Scene()
            {
                ID = "8-5",
                Narrative = "RTD:  That was close.  General Aveel must have detected our movements which should mean we are getting close to the source of the problem. Continue on",
                BackgroundImgSrc = "Content/Backgrounds/scene4BG.jpg",
                UserImgSrc = UserImgSrc,
                OtherCharImgSrc = "Content/Images/DrL.png",
                RTD = new Model.RTD() { Objective = " - Think more positive thoughts!", WhereAmI = " -  Dr. L's Lair" },
                Decisions = new System.Collections.ObjectModel.ObservableCollection<Model.Decision>()
            });
            SceneList.Add(new Model.Scene()
            {
                ID = "8-6",
                Narrative = string.Format("RTD:  My calculations agree with this proposition and my analysis concludes that we should pursue this plan.  This is why we picked someone like you, {0}.", User.Name),
                BackgroundImgSrc = "Content/Backgrounds/scene4BG.jpg",
                UserImgSrc = UserImgSrc,
                OtherCharImgSrc = "Content/Images/DrL.png",
                RTD = new Model.RTD() { Objective = " - Think more positive thoughts!", WhereAmI = " - Dr. L's Lair" },
                Decisions = new System.Collections.ObjectModel.ObservableCollection<Model.Decision>()
            });
            #endregion
            //Alternate Paths
            #region Level Gloomy Scenes
            // Alternate path for self discovery sequence
            SceneList.Add(new Model.Scene()
            {
                ID = "drypark",
                Narrative = "Narrator: Look at what has happened because you did not choose to be positive and help your classmate.  Little, seemingly insignificant actions can have a huge impact on the future.  What is happening?",
                BackgroundImgSrc = "Content/Images/rtdtimetravel.jpg",
                UserImgSrc = UserImgSrc,
                OtherCharImgSrc = "",
                RTD = new Model.RTD() { Objective = " - Think more positive thoughts!", WhereAmI = " - The park, but all life is gone." },
                Decisions = new System.Collections.ObjectModel.ObservableCollection<Model.Decision>()
            });
            SceneList.Add(new Model.Scene()
            {
                ID = "drypark1",
                Narrative = string.Format("Narrator: 'Look at what has happened because you did not choose the power of positivity; your Karma points have decreased and the same park is now a barren wasteland. {0} would you like to come back with me and try again?'", User.Name),
                BackgroundImgSrc = "Content/Backgrounds/deadforest.jpg",
                UserImgSrc = UserImgSrc,
                OtherCharImgSrc = "Content/Images",
                RTD = new Model.RTD() { Objective = " -See the damage of negativity", WhereAmI = " -The park, but all life is gone." },
                Decisions = new System.Collections.ObjectModel.ObservableCollection<Model.Decision>()
            });
            //Alternate Path for Einstein Sequence
            SceneList.Add(new Model.Scene()
            {
                ID = "winter",
                Narrative = "RTD: Look at what has happened because you did not choose the power of positivity; your Karma points have decreased and the same park is now a barren wasteland.  Would you like to come back with me and try again?",
                BackgroundImgSrc = "Content/Images/rtdtimetravel.jpg",
                UserImgSrc = UserImgSrc,
                OtherCharImgSrc = "",
                RTD = new Model.RTD() { Objective = " - See the consequences of negativity", WhereAmI = " - In the future where winter never ends!!" },
                Decisions = new System.Collections.ObjectModel.ObservableCollection<Model.Decision>()
            });
            SceneList.Add(new Model.Scene()
            {
                ID = "winter1",
                Narrative = string.Format("RTD: Without the theory of relativity the United States looses the Third World War and causes a nuclear. {0}, do you understand the consequences of your actions?", User.Name),
                BackgroundImgSrc = "Content/Backgrounds/winteriscoming.jpg",
                UserImgSrc = UserImgSrc,
                OtherCharImgSrc = "",
                RTD = new Model.RTD() { Objective = " -See the consequences of negativity", WhereAmI = "-In the future where winter never ends!!" },
                Decisions = new System.Collections.ObjectModel.ObservableCollection<Model.Decision>()
            });
            SceneList.Add(new Model.Scene()
            {
                ID = "winter2",
                Narrative = string.Format("RTD: Look at what has happened as a direct result of negativity; your Karma points have diminished and the future is in a perpetual state of winter. {0}, would you like to come back with me and try again?", User.Name),
                BackgroundImgSrc = "Content/Backgrounds/winteriscoming.jpg",
                UserImgSrc = UserImgSrc,
                OtherCharImgSrc = "",
                RTD = new Model.RTD() { Objective = " -See the consequences of negativity", WhereAmI = "-In the future where winter never ends!!" },
                Decisions = new System.Collections.ObjectModel.ObservableCollection<Model.Decision>()
            });
            //Alternate Path for Bill Gates Sequence
            SceneList.Add(new Model.Scene()
            {
                ID = "swamp",
                Narrative = "RTD: Look at what has happened because you did not choose the power of positivity; your Karma points have decreased and the same park is now a barren wasteland.  Would you like to come back with me and try again?",
                BackgroundImgSrc = "Content/Images/rtdtimetravel.jpg",
                UserImgSrc = UserImgSrc,
                OtherCharImgSrc = "",
                RTD = new Model.RTD() { Objective = " - See the consequences of negativity.", WhereAmI = " -A desolate swampland" },
                Decisions = new System.Collections.ObjectModel.ObservableCollection<Model.Decision>()
            });
            SceneList.Add(new Model.Scene()
            {
                ID = "swamp1",
                Narrative = "RTD: Look at what has happened because you did not choose the power of positivity; your Karma points have decreased and the same park is now a barren wasteland.  Would you like to come back with me and try again?",
                BackgroundImgSrc = "Content/Backgrounds/gloomyswampbg.png",
                UserImgSrc = UserImgSrc,
                OtherCharImgSrc = "",
                RTD = new Model.RTD() { Objective = " - See the consequences of negativity!", WhereAmI = " -A desolate swampland." },
                Decisions = new System.Collections.ObjectModel.ObservableCollection<Model.Decision>()
            });
            //Alternative paths for Ansel Adams sequence
            SceneList.Add(new Model.Scene()
            {
                ID = "desert",
                Narrative = "RTD: Look at what has happened because you did not choose the power of positivity; your Karma points have decreased and the same park is now a barren wasteland.  Would you like to come back with me and try again?",
                BackgroundImgSrc = "Content/Images/rtdtimetravel.jpg",
                UserImgSrc = UserImgSrc,
                OtherCharImgSrc = "",
                RTD = new Model.RTD() { Objective = " -See the consequences of negativity!", WhereAmI = " -A dry lifeless desert." },
                Decisions = new System.Collections.ObjectModel.ObservableCollection<Model.Decision>()
            });
            SceneList.Add(new Model.Scene()
            {
                ID = "desert1",
                Narrative = "RTD:  Look at what has happened because you did not choose the power of positivity; your Karma  points have decreased and the same park is now a barren wasteland.  Would you like to come back with me and try again?",
                BackgroundImgSrc = "Content/Backgrounds/desert.jpg",
                UserImgSrc = UserImgSrc,
                OtherCharImgSrc = "",
                RTD = new Model.RTD() { Objective = " -See the consequences of negativity!", WhereAmI = " -A dry lifeless desert" },
                Decisions = new System.Collections.ObjectModel.ObservableCollection<Model.Decision>()
            });
            #endregion
            //Come up with the plan to help baby Aveel
            #region Final Level
            SceneList.Add(new Model.Scene()
            {
                ID = "final",
                Narrative = string.Format("RTD: 'My calculations have pinpointed General Aveel as a baby; We believe that changing his outlook at an early age is crucial to the future of humanity.  {0},Do you agree that this is crucial and could stop future mishaps?'", User.Name),
                BackgroundImgSrc = "Content/Images/rtdtimetravel.jpg",
                UserImgSrc = UserImgSrc,
                OtherCharImgSrc = "",
                RTD = new Model.RTD() { Objective = " -Use the RTD to travel through time!", WhereAmI = " -On your way to meet baby General Aveel" },
                Decisions = new System.Collections.ObjectModel.ObservableCollection<Model.Decision>()
            });
            //General Aveel's Childhood home
            SceneList.Add(new Model.Scene()
            {
                ID = "final1",
                Narrative = string.Format("RTD:'This is the childhood home of General Aveel, if my calculations are correct he should still be a toddler and is at a very impressionable age.  Lets check around back.  {0}, decide how to encourage baby Aveel.'", User.Name),
                BackgroundImgSrc = "Content/Backgrounds/generalaveelhouse.jpg",
                UserImgSrc = UserImgSrc,
                OtherCharImgSrc = "Content/Images/AveelKid.png",
                RTD = new Model.RTD() { Objective = " -Encourage baby Aveel in a positive manor", WhereAmI = " -General Aveel's childhood home." },
                Decisions = new System.Collections.ObjectModel.ObservableCollection<Model.Decision>()
            });
            //Positive encouragement for Baby aveel
            SceneList.Add(new Model.Scene()
            {
                ID = "final2",
                Narrative = "RTD:'It looks like the toys we gave him are making him very happy.'",
                BackgroundImgSrc = "Content/Backgrounds/aveelbg.JPG",
                UserImgSrc = UserImgSrc,
                OtherCharImgSrc = "Content/Images/AveelKid.png",
                RTD = new Model.RTD() { Objective = " - Encourage baby Aveel in a positive manor.", WhereAmI = " - General Aveel's childhood home." },
                Decisions = new System.Collections.ObjectModel.ObservableCollection<Model.Decision>()
            });
            SceneList.Add(new Model.Scene()
            {
                ID = "final3",
                Narrative = string.Format("RTD: General Aveel's aura is much brighter already, I believe with consistent visits, where we can encourage positivity, that he will grow into a different man than he is today!!  Thank you so much {0} for your help", User.Name),
                BackgroundImgSrc = "Content/Backgrounds/aveelbg.JPG",
                UserImgSrc = UserImgSrc,
                OtherCharImgSrc = "Content/Images/AveelKid.png",
                RTD = new Model.RTD() { Objective = " -Encourage baby Aveel in a positive manor.", WhereAmI = " -General Aveel's childhood home." },
                Decisions = new System.Collections.ObjectModel.ObservableCollection<Model.Decision>()
            });
            //Make sure Sherpa is safe
            SceneList.Add(new Model.Scene()
            {
                ID = "final4",
                Narrative = string.Format("RTD: I am now taking you back to Sherpa's home, where she should be-safe and sound.  Thank you so much {0} for your help", User.Name),
                BackgroundImgSrc = "Content/Images/rtdtimetravel.jpg",
                UserImgSrc = UserImgSrc,
                RTD = new Model.RTD() { Objective = " -Make sure the plan worked.", WhereAmI = " -Sherpa's home." },
                Decisions = new System.Collections.ObjectModel.ObservableCollection<Model.Decision>()
            });
            SceneList.Add(new Model.Scene()
            {
                ID = "final5",
                Narrative = string.Format("Sherpa: Thank you so much {0} for your help!!  I knew if I stayed positive that the RTD would do the rest.", User.Name),
                BackgroundImgSrc = "Content/Backgrounds/SherpasHome.jpg",
                UserImgSrc = UserImgSrc,
                OtherCharImgSrc = "Content/Images/sherpa.png",
                RTD = new Model.RTD() { Objective = " -Make sure the plan worked.", WhereAmI = " -Sherpa's home." },
                Decisions = new System.Collections.ObjectModel.ObservableCollection<Model.Decision>()
            });
            //Take Sherpa with you back to Dr.L's hideout
            SceneList.Add(new Model.Scene()
            {
                ID = "final6",
                Narrative = string.Format("RTD: '{0}, would you like me to take you to the celebration at Dr. L's lair?'", User.Name),
                BackgroundImgSrc = "Content/Backgrounds/SherpasHome.jpg",
                UserImgSrc = UserImgSrc,
                OtherCharImgSrc = "Content/Images/sherpa.png",
                RTD = new Model.RTD() { Objective = " -Make sure the plan worked.", WhereAmI = " -Sherpa's home." },
                Decisions = new System.Collections.ObjectModel.ObservableCollection<Model.Decision>()
            });
        
            SceneList.Add(new Model.Scene()
            {
                ID = "final7",
                Narrative = string.Format("RTD: 'See, I told you that you were the best person for our mission!! Thank you so much for your help {0}", User.Name),
                BackgroundImgSrc = "Content/Images/rtdtimetravel.jpg",
                UserImgSrc = UserImgSrc,
                OtherCharImgSrc = "Content/Images/sherpa.png",

                RTD = new Model.RTD() { Objective = " -Make sure the plan worked.", WhereAmI = " - Sherpa's home." },
                Decisions = new System.Collections.ObjectModel.ObservableCollection<Model.Decision>()
            });
            #endregion
            //Victory Scene
            #region Victory
            SceneList.Add(new Model.Scene()
            {
                ID = "Victory",
                Narrative = string.Format("{0} - You have completed the mission and changed the future!  Because General Aveel is a good person now Sherpa is safe at home having never been taken.", User.Name),
                BackgroundImgSrc = "Content/Backgrounds/Scene4BG.jpg",
                UserImgSrc = UserImgSrc,
                OtherCharImgSrc = "Content/Images/DrL.png",
                RTD = new Model.RTD() { Objective = " -There is nothing more to do for now.", WhereAmI = " -It's NOT the end of the world as you know it. You feel fine." },
                Decisions = new System.Collections.ObjectModel.ObservableCollection<Model.Decision>()
            });
            //Book Plug!!
            SceneList.Add(new Model.Scene()
            {
                ID = "Victory1",
                Narrative = string.Format("{0}- Would you like to know more about Sherpa's adventures?  If so please check out the Book from Lori on Amazon.com (http://www.amazon.com/Sherpas-Adventure-Saving-Lori-Costew/dp/1481103911); also be ready for the release of the sequel sometime soon!", User.Name),
                BackgroundImgSrc = "Content/Backgrounds/Scene4BG.jpg",
                UserImgSrc = UserImgSrc,
                OtherCharImgSrc = "Content/Images/DrL.png",
                RTD = new Model.RTD() { Objective = " -There is nothing more to do for now.", WhereAmI = " -It's NOT the end of the world as you know it. You feel fine." },
                Decisions = new System.Collections.ObjectModel.ObservableCollection<Model.Decision>()
            });
            //Book Plug!!
            SceneList.Add(new Model.Scene()
            {
                ID = "Victory2",
                Narrative = string.Format("{0},'I feel so good about helping others be more positive! I believe it will help me be more positive in my own life!!'", User.Name),
                BackgroundImgSrc = "Content/Backgrounds/Scene4BG.jpg",
                UserImgSrc = UserImgSrc,
                OtherCharImgSrc = "Content/Images/sherpa.png",
                RTD = new Model.RTD() { Objective = "-There is nothing more to do for now.", WhereAmI = " -It's NOT the end of the world as you know it. You feel fine." },
                Decisions = new System.Collections.ObjectModel.ObservableCollection<Model.Decision>()
            });

            //Kludge to implement special Save and load.
            SceneList.Add(new Model.Scene() { ID = "SaveAndQuit", Decisions=new ObservableCollection<Model.Decision>() });
            SceneList.Add(new Model.Scene() { ID = "SaveAndLoad", Decisions = new ObservableCollection<Model.Decision>() });
            #endregion
            //Wrong Decision Scenes
            #region Bad Decisions
            SceneList.Add(new Model.Scene()
            {
                ID = "BadDecision",
                Narrative = string.Format("{0},'Oh no I have made a poor decision; can we go back and try again?'", User.Name),
                BackgroundImgSrc = "Content/Backgrounds/gloomyimages/badchoice.jpg",
                UserImgSrc = UserImgSrc,
                RTD = new Model.RTD() { Objective = "-Embrace the Power of Positivity", WhereAmI = "-The Land of bad decisions!" },
                Decisions = new System.Collections.ObjectModel.ObservableCollection<Model.Decision>()
            });
            SceneList.Add(new Model.Scene()
            {
                ID = "BadDecision1",
                Narrative = string.Format("{0},'Oh no I have made a poor decision; can we go back and try again?'", User.Name),
                BackgroundImgSrc = "Content/Backgrounds/gloomyimages/323_max.jpg",
                UserImgSrc = UserImgSrc,
                RTD = new Model.RTD() { Objective = "-Embrace the Power of Positivity", WhereAmI = "-The Land of bad decisions!" },
                Decisions = new System.Collections.ObjectModel.ObservableCollection<Model.Decision>()
            });
            SceneList.Add(new Model.Scene()
            {
                ID = "BadDecision2",
                Narrative = string.Format("{0},'Oh no I have made a poor decision; can we go back and try again?'", User.Name),
                BackgroundImgSrc = "Content/Backgrounds/gloomyimages/badchoice.jpg",
                UserImgSrc = UserImgSrc,
                RTD = new Model.RTD() { Objective = "-Embrace the Power of Positivity", WhereAmI = "-The Land of bad decisions!" },
                Decisions = new System.Collections.ObjectModel.ObservableCollection<Model.Decision>()
            });
            SceneList.Add(new Model.Scene()
            {
                ID = "BadDecision3",
                Narrative = string.Format("{0},'Oh no I have made a poor decision; can we go back and try again?'", User.Name),
                BackgroundImgSrc = "Content/Backgrounds/gloomyimages/323_max.jpg",
                UserImgSrc = UserImgSrc,
                RTD = new Model.RTD() { Objective = "-Embrace the Power of Positivity", WhereAmI = "-The Land of bad decisions!" },
                Decisions = new System.Collections.ObjectModel.ObservableCollection<Model.Decision>()
            });
            SceneList.Add(new Model.Scene()
            {
                ID = "BadDecision4",
                Narrative = string.Format("{0},'Oh no I have made a poor decision; can we go back and try again?'", User.Name),
                BackgroundImgSrc = "Content/Backgrounds/gloomyimages/badchoice.jpg",
                UserImgSrc = UserImgSrc,
                RTD = new Model.RTD() { Objective = "-Embrace the Power of Positivity", WhereAmI = "-The Land of bad decisions!" },
                Decisions = new System.Collections.ObjectModel.ObservableCollection<Model.Decision>()
            });
            SceneList.Add(new Model.Scene()
            {
                ID = "BadDecision5",
                Narrative = string.Format("{0},'Oh no I have made a poor decision; can we go back and try again?'", User.Name),
                BackgroundImgSrc = "Content/Backgrounds/gloomyimages/323_max.jpg",
                UserImgSrc = UserImgSrc,
                RTD = new Model.RTD() { Objective = "-Embrace the Power of Positivity", WhereAmI = "-The Land of bad decisions!" },
                Decisions = new System.Collections.ObjectModel.ObservableCollection<Model.Decision>()
            });
            SceneList.Add(new Model.Scene()
            {
                ID = "BadDecision6",
                Narrative = string.Format("{0},'Oh no I have made a poor decision; can we go back and try again?'", User.Name),
                BackgroundImgSrc = "Content/Backgrounds/gloomyimages/badchoice.jpg",
                UserImgSrc = UserImgSrc,
                RTD = new Model.RTD() { Objective = "-Embrace the Power of Positivity", WhereAmI = "-The Land of bad decisions!" },
                Decisions = new System.Collections.ObjectModel.ObservableCollection<Model.Decision>()
            });
            SceneList.Add(new Model.Scene()
            {
                ID = "BadDecision7",
                Narrative = string.Format("{0},'Oh no I have made a poor decision; can we go back and try again?'", User.Name),
                BackgroundImgSrc = "Content/Backgrounds/gloomyimages/badchoice.jpg",
                UserImgSrc = UserImgSrc,
                RTD = new Model.RTD() { Objective = "-Embrace the Power of Positivity", WhereAmI = "-The Land of bad decisions!" },
                Decisions = new System.Collections.ObjectModel.ObservableCollection<Model.Decision>()
            });
            SceneList.Add(new Model.Scene()
            {
                ID = "BadDecision8",
                Narrative = string.Format("{0},'Oh no I have made a poor decision; can we go back and try again?'", User.Name),
                BackgroundImgSrc = "Content/Backgrounds/gloomyimages/323_max.jpg",
                UserImgSrc = UserImgSrc,
                RTD = new Model.RTD() { Objective = "-Embrace the Power of Positivity", WhereAmI = "-The Land of bad decisions!" },
                Decisions = new System.Collections.ObjectModel.ObservableCollection<Model.Decision>()
            });
            SceneList.Add(new Model.Scene()
            {
                ID = "BadDecision9",
                Narrative = string.Format("{0},'Oh no I have made a poor decision; can we go back and try again?'", User.Name),
                BackgroundImgSrc = "Content/Backgrounds/gloomyimages/badchoice.jpg",
                UserImgSrc = UserImgSrc,
                RTD = new Model.RTD() { Objective = "-Embrace the Power of Positivity", WhereAmI = "-The Land of bad decisions!" },
                Decisions = new System.Collections.ObjectModel.ObservableCollection<Model.Decision>()
            });
            SceneList.Add(new Model.Scene()
            {
                ID = "BadDecision10",
                Narrative = string.Format("{0},'Oh no I have mad a poor decision; can we go back and try again?'", User.Name),
                BackgroundImgSrc = "Content/Backgrounds/gloomyimages/323_max.jpg",
                UserImgSrc = UserImgSrc,
                RTD = new Model.RTD() { Objective = "-Embrace the Power of Positivity", WhereAmI = "-The Land of bad decisions!" },
                Decisions = new System.Collections.ObjectModel.ObservableCollection<Model.Decision>()
            });
            SceneList.Add(new Model.Scene()
            {
                ID = "BadDecision11",
                Narrative = string.Format("{0},'Oh no I have mad a poor decision; can we go back and try again?'", User.Name),
                BackgroundImgSrc = "Content/Backgrounds/gloomyimages/badchoice.jpg",
                UserImgSrc = UserImgSrc,
                RTD = new Model.RTD() { Objective = "-Embrace the Power of Positivity", WhereAmI = "-The Land of bad decisions!" },
                Decisions = new System.Collections.ObjectModel.ObservableCollection<Model.Decision>()
            });
            SceneList.Add(new Model.Scene()
            {
                ID = "BadDecision12",
                Narrative = string.Format("{0},'Oh no I have made a poor decision; can we go back and try again?'", User.Name),
                BackgroundImgSrc = "Content/Backgrounds/gloomyimages/323_max.jpg",
                UserImgSrc = UserImgSrc,
                RTD = new Model.RTD() { Objective = "-Embrace the Power of Positivity", WhereAmI = "-The Land of bad decisions!" },
                Decisions = new System.Collections.ObjectModel.ObservableCollection<Model.Decision>()
            });
            SceneList.Add(new Model.Scene()
            {
                ID = "BadDecision13",
                Narrative = string.Format("{0},'Oh no I have made a poor decision; can we go back and try again?'", User.Name),
                BackgroundImgSrc = "Content/Backgrounds/gloomyimages/323_max.jpg",
                UserImgSrc = UserImgSrc,
                RTD = new Model.RTD() { Objective = "-Embrace the Power of Positivity", WhereAmI = "-The Land of bad decisions!" },
                Decisions = new System.Collections.ObjectModel.ObservableCollection<Model.Decision>()
            });
            SceneList.Add(new Model.Scene()
            {
                ID = "BadDecision14",
                Narrative = string.Format("{0},'Oh no I have made a poor decision; can we go back and try again?'", User.Name),
                BackgroundImgSrc = "Content/Backgrounds/gloomyimages/badchoice.jpg",
                UserImgSrc = UserImgSrc,
                RTD = new Model.RTD() { Objective = "-Embrace the Power of Positivity", WhereAmI = "-The Land of bad decisions!" },
                Decisions = new System.Collections.ObjectModel.ObservableCollection<Model.Decision>()
            });
            SceneList.Add(new Model.Scene()
            {
                ID = "BadDecision15",
                Narrative = string.Format("{0},'Oh no I have made a poor decision; can we go back and try again?'", User.Name),
                BackgroundImgSrc = "Content/Backgrounds/gloomyimages/323_max.jpg",
                UserImgSrc = UserImgSrc,
                RTD = new Model.RTD() { Objective = "-Embrace the Power of Positivity", WhereAmI = "-The Land of bad decisions!" },
                Decisions = new System.Collections.ObjectModel.ObservableCollection<Model.Decision>()
            });
            SceneList.Add(new Model.Scene()
            {
                ID = "BadDecision16",
                Narrative = string.Format("{0},'Oh no I have made a poor decision; can we go back and try again?'", User.Name),
                BackgroundImgSrc = "Content/Backgrounds/gloomyimages/badchoice.jpg",
                UserImgSrc = UserImgSrc,
                RTD = new Model.RTD() { Objective = "-Embrace the Power of Positivity", WhereAmI = "-The Land of bad decisions!" },
                Decisions = new System.Collections.ObjectModel.ObservableCollection<Model.Decision>()
            });
            SceneList.Add(new Model.Scene()
            {
                ID = "BadDecision17",
                Narrative = string.Format("{0},'Oh no I have made a poor decision; can we go back and try again?'", User.Name),
                BackgroundImgSrc = "Content/Backgrounds/gloomyimages/323_max.jpg",
                UserImgSrc = UserImgSrc,
                RTD = new Model.RTD() { Objective = "-Embrace the Power of Positivity", WhereAmI = "-The Land of bad decisions!" },
                Decisions = new System.Collections.ObjectModel.ObservableCollection<Model.Decision>()
            });
            SceneList.Add(new Model.Scene()
            {
                ID = "BadDecision18",
                Narrative = string.Format("{0},'Oh no I have made a poor decision; can we go back and try again?'", User.Name),
                BackgroundImgSrc = "Content/Backgrounds/gloomyimages/badchoice.jpg",
                UserImgSrc = UserImgSrc,
                RTD = new Model.RTD() { Objective = "-Embrace the Power of Positivity", WhereAmI = "-The Land of bad decisions!" },
                Decisions = new System.Collections.ObjectModel.ObservableCollection<Model.Decision>()
            });
            SceneList.Add(new Model.Scene()
            {
                ID = "BadDecision19",
                Narrative = string.Format("{0},'Oh no I have made a poor decision; can we go back and try again?'", User.Name),
                BackgroundImgSrc = "Content/Backgrounds/gloomyimages/323_max.jpg",
                UserImgSrc = UserImgSrc,
                RTD = new Model.RTD() { Objective = "-Embrace the Power of Positivity", WhereAmI = "-The Land of bad decisions!" },
                Decisions = new System.Collections.ObjectModel.ObservableCollection<Model.Decision>()
            });
            SceneList.Add(new Model.Scene()
            {
                ID = "BadDecision20",
                Narrative = string.Format("{0},'Oh no I have made a poor decision; can we go back and try again?'", User.Name),
                BackgroundImgSrc = "Content/Backgrounds/gloomyimages/badchoice.jpg",
                UserImgSrc = UserImgSrc,
                RTD = new Model.RTD() { Objective = "-Embrace the Power of Positivity", WhereAmI = "-The Land of bad decisions!" },
                Decisions = new System.Collections.ObjectModel.ObservableCollection<Model.Decision>()
            });
            #endregion
            //Add Defined Scenes into scene dictionary.
            Scenes = new Dictionary<string, Model.Scene>();
            foreach (Model.Scene scene in SceneList)
                Scenes.Add(scene.ID, scene);
            #region Decision Definitions
            #region Introduction - Tutorial decisions
            Scenes["1-1"].Decisions.Add(new Model.Decision()
            {
                Description = string.Format("{0}, to make a decision click the correct text!!", User.Name),
                KarmaAdjustment = 0,
                LeadsTo = Scenes["1-11"]
            });
            Scenes["1-11"].TransitionToNext = new Model.Transition() { NextScene = Scenes["1-12"], TimeTillNext = new TimeSpan(0, 0, 10) };
            Scenes["1-12"].TransitionToNext = new Model.Transition() { NextScene = Scenes["1-13"], TimeTillNext = new TimeSpan(0, 0, 10) };
            Scenes["1-13"].Decisions.Add(new Model.Decision()
            {
                Description = string.Format("Yes, I understand the application bar. (Click)", User.Name),
                KarmaAdjustment = 10,
                LeadsTo = Scenes["1-14"]
            });
            Scenes["1-13"].Decisions.Add(new Model.Decision()
            {
                Description = string.Format("I do not understand the Button. (Click)", User.Name),
                KarmaAdjustment = 10,
                LeadsTo = Scenes["1-13"]
            }); Scenes["1-14"].Decisions.Add(new Model.Decision()
            {
                Description = string.Format("I DO NOT understand the button labeled RTD. (Click)", User.Name),
                KarmaAdjustment = 0,
                LeadsTo = Scenes["1-14"]
            });
            Scenes["1-14"].Decisions.Add(new Model.Decision()
            {
                Description = string.Format("Yes, I understand button labeled RTD. (Click)", User.Name),
                KarmaAdjustment = 10,
                LeadsTo = Scenes["1-15"]
            });
            Scenes["1-15"].TransitionToNext = new Model.Transition() { NextScene = Scenes["1-2"], TimeTillNext = new TimeSpan(0, 0, 10) };
            #endregion
            //Game actually starts
            #region Park Scene
            Scenes["1-2"].Decisions.Add(new Model.Decision()
            {
                Description = string.Format("{0}-I like helping others, usually.", User.Name),
                KarmaAdjustment = -50,
                LeadsTo = Scenes["BadDecision"]
            });
            Scenes["1-2"].Decisions.Add(new Model.Decision()
            {
                Description = string.Format("{0}-I don't like helping others.", User.Name),
                KarmaAdjustment = -50,
                LeadsTo = Scenes["drypark"]
            }); 
            Scenes["1-2"].Decisions.Add(new Model.Decision()
            {
                Description = string.Format("{0}-I like helping people, even those who are different.", User.Name),
                KarmaAdjustment = 100,
                LeadsTo = Scenes["1-3"]
            });
            #endregion
            #region drypark
            //Drypark 
            Scenes["drypark"].Decisions.Add(new Model.Decision()
            {
                Description = string.Format("{0},'Who on earth is talking and where I am going?'", User.Name),
                KarmaAdjustment = 0,
                LeadsTo = Scenes["drypark1"]
            });
            Scenes["drypark1"].Decisions.Add(new Model.Decision()
            {
                Description = string.Format("{0}-Try again with a more positively influential decision!!", User.Name),
                KarmaAdjustment = 50,
                LeadsTo = Scenes["1-2"]
            });
            Scenes["drypark1"].Decisions.Add(new Model.Decision()
            {
                Description = string.Format("{0}-I am not worried about the this hipster!", User.Name),
                KarmaAdjustment = -50,
                LeadsTo = Scenes["1-3"]
            });

            #endregion 
            //User helps friend on the playground and discovers RTD in their locker
            #region Level 1 Decisions
           
            Scenes["1-3"].Decisions.Add(new Model.Decision() { 
                Description = string.Format("{0},'I must find out what this is!'-Continue", User.Name), 
                KarmaAdjustment = 50, 
                LeadsTo = Scenes["1-4"] });
            Scenes["1-3"].Decisions.Add(new Model.Decision() { 
                Description = string.Format("{0},'I don't like technology!'  Disregard find.", User.Name), 
                KarmaAdjustment = -50, 
                LeadsTo = Scenes["BadDecision1"] });//needs a branch
            Scenes["1-4"].Decisions.Add(new Model.Decision() { 
                Description = string.Format("Go with mom and eat dinner.", User.Name), 
                KarmaAdjustment = -50, 
                LeadsTo = Scenes["BadDecision19"] });
            Scenes["1-4"].Decisions.Add(new Model.Decision() { 
                Description = "Go to friends house.", 
                KarmaAdjustment = 50, 
                LeadsTo = Scenes["2-1"] });
            Scenes["1-4"].Decisions.Add(new Model.Decision() { 
                Description = "You have too much homework and are very hungry!", 
                KarmaAdjustment = -50, 
                LeadsTo = Scenes["BadDecision2"] });//needs a branch
            Scenes["1-5"].Decisions.Add(new Model.Decision() { 
                Description = string.Format("{0},'I will say hello to my friend.'", User.Name), 
                KarmaAdjustment = 50, 
                LeadsTo = Scenes["2-1"] });
            #endregion
            //User plugs RTD in their friend's computer and learns that the RTD has a mission for them.
            #region Level 2 Decisions
            Scenes["2-1"].Decisions.Add(new Model.Decision() { Description = string.Format("{0},'I wonder why I stood out?'", User.Name), KarmaAdjustment = 50, LeadsTo = Scenes["2-2"] });
            Scenes["2-1"].Decisions.Add(new Model.Decision() { Description = string.Format("{0}-Choose to Learn more from RTD", User.Name), KarmaAdjustment = 50, LeadsTo = Scenes["2-2"] });
            Scenes["2-2"].Decisions.Add(new Model.Decision() { Description = string.Format("{0},'I accept the mission to save Sherpa!", User.Name), KarmaAdjustment = 50, LeadsTo = Scenes["2-3"] });
            Scenes["2-2"].Decisions.Add(new Model.Decision() { Description = string.Format("{0},'Traveling through time is not in my schedule!", User.Name), KarmaAdjustment = -20, LeadsTo = Scenes["BadDecision3"] });//needs a branch
            Scenes["2-3"].Decisions.Add(new Model.Decision() { Description = string.Format("{0}-Use the RTD for first time to travel to the future!", User.Name), KarmaAdjustment = 50, LeadsTo = Scenes["3-1"] });
            Scenes["2-3"].Decisions.Add(new Model.Decision() { Description = "Wait to use RTD until the morning.", KarmaAdjustment = -20, LeadsTo = Scenes["2-35"] });
            //branch for deciding to sleep on the decision        
            Scenes["2-35"].Decisions.Add(new Model.Decision() { Description = string.Format("{0},'I will wait until the morning!'", User.Name), KarmaAdjustment = 50, LeadsTo = Scenes["2-40"] });
            Scenes["2-35"].Decisions.Add(new Model.Decision() { Description = string.Format("{0},'I have changed my mind, and will go now!'", User.Name), KarmaAdjustment = -20, LeadsTo = Scenes["3-1"] });
            Scenes["2-40"].TransitionToNext = new Model.Transition() { NextScene = Scenes["2-41"], TimeTillNext = new TimeSpan(0, 0, 10) };
            Scenes["2-41"].TransitionToNext = new Model.Transition() { NextScene = Scenes["2-42"], TimeTillNext = new TimeSpan(0, 0, 10) };
            Scenes["2-42"].TransitionToNext = new Model.Transition() { NextScene = Scenes["3-1"], TimeTillNext = new TimeSpan(0, 0, 10) };

            #endregion
            //Friends House
            #region on the way to Dr. L
            Scenes["3-1"].Decisions.Add(new Model.Decision() { Description = string.Format("{0},'I cannot wait to meet Dr. L'", User.Name), KarmaAdjustment = 100, LeadsTo = Scenes["3-2"] });
            Scenes["3-2"].Decisions.Add(new Model.Decision() { Description = string.Format("{0},'This is more than I can handle!!'", User.Name), KarmaAdjustment = -20, LeadsTo = Scenes["4-1"] });
            Scenes["3-2"].Decisions.Add(new Model.Decision() { Description = string.Format("{0},'I would still like to continue!'", User.Name), KarmaAdjustment = 50, LeadsTo = Scenes["4-1"] });
            Scenes["3-2"].Decisions.Add(new Model.Decision() { Description = string.Format("{0},'I would like to go home!'", User.Name), KarmaAdjustment = -50, LeadsTo = Scenes["BadDecision4"] });//branch

            #endregion
            //Dr. L lair
            #region Learn from Dr. L
            Scenes["4-1"].Decisions.Add(new Model.Decision() { Description = string.Format("{0},'Positive reinforcement is always better than negative.'", User.Name), KarmaAdjustment = 0, LeadsTo = Scenes["4-2"] });
            Scenes["4-1"].Decisions.Add(new Model.Decision() { Description = "You still don't understand why you, but feel good about yourself!'", KarmaAdjustment = 0, LeadsTo = Scenes["4-2"] });
            Scenes["4-2"].Decisions.Add(new Model.Decision() { Description = string.Format("{0},'I understand how I can help!'",User.Name), KarmaAdjustment = 0, LeadsTo = Scenes["4-3"] });
            Scenes["4-2"].Decisions.Add(new Model.Decision() { Description = string.Format("{0},'This is confusing!'", User.Name), KarmaAdjustment = 0, LeadsTo = Scenes["BadDecision5"] });// has branch
            Scenes["4-3"].Decisions.Add(new Model.Decision() { Description = string.Format("{0},'So my positivity is why I was chosen - Cool'!!", User.Name), KarmaAdjustment = 0, LeadsTo = Scenes["4-4"] });
            Scenes["4-3"].Decisions.Add(new Model.Decision() { Description = string.Format("{0},'I can embrace the power of positivity!!'", User.Name), KarmaAdjustment = 0, LeadsTo = Scenes["4-4"] });
            Scenes["4-3"].Decisions.Add(new Model.Decision() { Description = string.Format("{0},'I CAN NOT embrace the power of positivity.'", User.Name), KarmaAdjustment = 0, LeadsTo = Scenes["BadDecision6"] });//has branch

            Scenes["4-4"].Decisions.Add(new Model.Decision() { Description = string.Format("{0},'If positivity is the key, I can do this!!'",User.Name), KarmaAdjustment = 0, LeadsTo = Scenes["4-5"] });
            Scenes["4-4"].Decisions.Add(new Model.Decision() { Description = string.Format("{0},'I DONT'T KNOW if I can do this??'", User.Name), KarmaAdjustment = 0, LeadsTo = Scenes["BadDecision7"] });// has branch
            Scenes["4-5"].Decisions.Add(new Model.Decision() { Description = string.Format("{0},'I would like to see what happened to Sherpa!'",User.Name), KarmaAdjustment = 0, LeadsTo = Scenes["5"] });
            Scenes["4-5"].Decisions.Add(new Model.Decision() { Description = "To see what happened to Sherpa.", KarmaAdjustment = 0, LeadsTo = Scenes["5"] });
            #endregion
            //Sherpa's House
            #region Sherpa's house
            Scenes["5"].Decisions.Add(new Model.Decision() { Description = string.Format("{0}-Wow, its like instant replay-for everything!", User.Name), KarmaAdjustment = 0, LeadsTo = Scenes["5-1"] });
            Scenes["5-1"].TransitionToNext = new Model.Transition() { NextScene = Scenes["5-2"], TimeTillNext = new TimeSpan(0, 0, 6) };
            Scenes["5-2"].TransitionToNext = new Model.Transition() { NextScene = Scenes["5-3"], TimeTillNext = new TimeSpan(0, 0, 3) };
            Scenes["5-3"].TransitionToNext = new Model.Transition() { NextScene = Scenes["5-4"], TimeTillNext = new TimeSpan(0, 0, 3) };
            Scenes["5-4"].TransitionToNext = new Model.Transition() { NextScene = Scenes["6-1"], TimeTillNext = new TimeSpan(0, 0, 10) };
            #endregion
            //Albert Einstein Scenes
            #region Einstein Scenes
            Scenes["6-1"].TransitionToNext = new Model.Transition() { NextScene = Scenes["6-2"], TimeTillNext = new TimeSpan(0, 0, 7) };
            Scenes["6-2"].Decisions.Add(new Model.Decision() { Description=string.Format("{0},'I want to befriend Albert.'", User.Name), KarmaAdjustment= 100, LeadsTo = Scenes["6-3"]});
            Scenes["6-2"].Decisions.Add(new Model.Decision() { Description=string.Format("{0},'Go home. Saving the World is too much!'", User.Name), KarmaAdjustment=100, LeadsTo = Scenes["BadDecision20"]});//has branch
            Scenes["6-3"].Decisions.Add(new Model.Decision() { Description=string.Format("{0},'Tell Albert, 'his father loves him and wants the best.'", User.Name), KarmaAdjustment=100, LeadsTo = Scenes["6-4"]});
            Scenes["6-3"].Decisions.Add(new Model.Decision() { Description=string.Format("{0},'Tell Albert to run away from school.'", User.Name), KarmaAdjustment = 10, LeadsTo = Scenes["BadDecision8"]});
            Scenes["6-3"].Decisions.Add(new Model.Decision() { Description=string.Format("{0},'Albert cannot be helped.'", User.Name), KarmaAdjustment = -100, LeadsTo = Scenes["winter"] });
            Scenes["6-4"].Decisions.Add(new Model.Decision() { Description=string.Format("{0},'I would like to continue!!'",User.Name), KarmaAdjustment = 0, LeadsTo = Scenes["7-1"] });
            Scenes["6-4"].Decisions.Add(new Model.Decision() { Description=string.Format("RTD,'{0}, use the RTD again!''", User.Name), KarmaAdjustment = 0, LeadsTo = Scenes["7-1"] });

            //Einstein Alternate scene
            Scenes["winter"].Decisions.Add(new Model.Decision() { Description=string.Format("{0},'Oh gosh, what has happened?'", User.Name), KarmaAdjustment = 100, LeadsTo = Scenes["winter2"] });
            Scenes["winter1"].TransitionToNext = new Model.Transition() { NextScene=Scenes["winter2"], TimeTillNext = new TimeSpan(0, 0, 7) };
            Scenes["winter2"].Decisions.Add(new Model.Decision() { Description=string.Format("{0},'I'll try again!'", User.Name), KarmaAdjustment = 100, LeadsTo = Scenes["6-3"] });
            Scenes["winter2"].Decisions.Add(new Model.Decision() { Description=string.Format("{0},'I want to go home!'", User.Name), KarmaAdjustment = -50, LeadsTo = Scenes["BadDecision9"] });//has branch
            #endregion
            //Bill Gates Scene
            #region Bill Gates
            Scenes["7-1"].TransitionToNext = new Model.Transition() { NextScene = Scenes["7-15"], TimeTillNext = new TimeSpan(0, 0, 6) };
            Scenes["7-15"].TransitionToNext = new Model.Transition() { NextScene = Scenes["7-16"], TimeTillNext = new TimeSpan(0, 0, 6) };
            Scenes["7-16"].TransitionToNext = new Model.Transition() { NextScene = Scenes["7-2"], TimeTillNext = new TimeSpan(0, 0, 6) };
            Scenes["7-2"].Decisions.Add(new Model.Decision() { Description = string.Format("{0},'Bill, you must follow what your heart really wants!'", User.Name), LeadsTo = Scenes["7-3"] });
            Scenes["7-2"].Decisions.Add(new Model.Decision() { Description = string.Format("{0},' I would like to CONTINUE interacting with Bill", User.Name), KarmaAdjustment = 0, LeadsTo = Scenes["7-3"] });
            Scenes["7-2"].Decisions.Add(new Model.Decision() { Description = string.Format("{0},'Bill Gates DOESN't need your help!'", User.Name), KarmaAdjustment = 0, LeadsTo = Scenes["BadDecision10"] });
            Scenes["7-25"].TransitionToNext = new Model.Transition() { NextScene = Scenes["7-3"], TimeTillNext = new TimeSpan(0, 0, 10) };     
            Scenes["7-3"].Decisions.Add(new Model.Decision() { Description = string.Format("{0},'Thank you Bill!' Being positive is key!",User.Name), KarmaAdjustment = 0, LeadsTo = Scenes["7-4"] });
            Scenes["7-3"].Decisions.Add(new Model.Decision() { Description = string.Format("{0}, Tell Bill 'When things are hard-give up.'", User.Name), KarmaAdjustment = 0, LeadsTo = Scenes["swamp"] });
            Scenes["7-4"].Decisions.Add(new Model.Decision() { Description = string.Format("{0},'I can see the power of positivity!!'", User.Name), KarmaAdjustment = 0, LeadsTo = Scenes["8-1"] });
            Scenes["7-4"].Decisions.Add(new Model.Decision() { Description = string.Format("{0},'This is to much for me!'", User.Name), KarmaAdjustment = 0, LeadsTo = Scenes["BadDecision11"] });
            //Bill Gates Alternate 
            Scenes["swamp"].Decisions.Add(new Model.Decision() { Description = string.Format("{0},'OMG! What has happened this time?'", User.Name), KarmaAdjustment = 0, LeadsTo = Scenes["swamp1"] });
            Scenes["swamp"].Decisions.Add(new Model.Decision() { Description = string.Format("{0},'I would like to try again.'", User.Name), KarmaAdjustment = 0, LeadsTo = Scenes["swamp1"] });
            Scenes["swamp1"].Decisions.Add(new Model.Decision() { Description = string.Format("{0},'I will be more positive'", User.Name), KarmaAdjustment = 0, LeadsTo = Scenes["7-3"] });
            Scenes["swamp1"].Decisions.Add(new Model.Decision() { Description = string.Format("{0},'I am an Apple person anyway, go home!", User.Name), KarmaAdjustment = 0, LeadsTo = Scenes["BadDecision12"] });
            #endregion
            //Ansel Adams 
            #region Ansel Adams
            Scenes["8-1"].Decisions.Add(new Model.Decision() { Description = string.Format("{0} says,'I can't wait to help Ansel Adams my favorite photographer.", User.Name), KarmaAdjustment = 0, LeadsTo = Scenes["8-2"] });
            Scenes["8-1"].Decisions.Add(new Model.Decision() { Description = string.Format("{0},'I would like to interact with Ansel Adams!", User.Name), KarmaAdjustment = 0, LeadsTo = Scenes["8-2"] });
            Scenes["8-2"].Decisions.Add(new Model.Decision() { Description = string.Format("{0},'Approach Ansel'", User.Name), KarmaAdjustment = 100, LeadsTo = Scenes["8-3"] });
            Scenes["8-2"].Decisions.Add(new Model.Decision() { Description = string.Format("{0},'Tell Ansel he should grow up; be a man!", User.Name), KarmaAdjustment = 0, LeadsTo = Scenes["BadDecision13"] });
            Scenes["8-3"].Decisions.Add(new Model.Decision() { Description = string.Format("{0},'Befriend young Ansel with positivity!'", User.Name), KarmaAdjustment = 0, LeadsTo = Scenes["8-4"] });
            Scenes["8-3"].Decisions.Add(new Model.Decision() { Description = string.Format("{0},'Decide that pictures really don't matter!", User.Name), KarmaAdjustment = 0, LeadsTo = Scenes["desert"] });
            Scenes["8-4"].Decisions.Add(new Model.Decision() { Description = string.Format("{0},'I agree lets get out of here!'", User.Name), KarmaAdjustment = 0, LeadsTo = Scenes["8-5"] });
            Scenes["8-5"].Decisions.Add(new Model.Decision() { Description = string.Format("{0},'I think we should interact General Aveel as a child!!'", User.Name), KarmaAdjustment = 0, LeadsTo = Scenes["8-6"] });
            Scenes["8-5"].Decisions.Add(new Model.Decision() { Description = string.Format("{0}-To purpose this idea!", User.Name), KarmaAdjustment = 0, LeadsTo = Scenes["8-6"] });
            Scenes["8-6"].Decisions.Add(new Model.Decision() { Description = string.Format("{0},'Let's travel to the childhood home of General Aveel!'", User.Name), KarmaAdjustment = 0, LeadsTo = Scenes["final"] });
            Scenes["8-6"].Decisions.Add(new Model.Decision() { Description = string.Format("{0},'I would like to GIVE UP'", User.Name), KarmaAdjustment = 0, LeadsTo = Scenes["BadDecision14"] }); 
            //Ansel Adams Alternative
            Scenes["desert"].Decisions.Add(new Model.Decision() { Description = string.Format("{0},'I cannot believe how negativity spreads", User.Name), KarmaAdjustment = 0, LeadsTo = Scenes["desert1"] });
            Scenes["desert"].Decisions.Add(new Model.Decision() { Description = string.Format("{0},'I DON'T think Pursuing your dreams is important.'", User.Name), KarmaAdjustment = 0, LeadsTo = Scenes["desert1"] });
            Scenes["desert1"].Decisions.Add(new Model.Decision() { Description = string.Format("{0},'Return to help Ansel'", User.Name), KarmaAdjustment = 0, LeadsTo = Scenes["8-4"] });
            Scenes["desert1"].Decisions.Add(new Model.Decision() { Description = string.Format("{0},'I would like to GO HOME and go to bed!''", User.Name), KarmaAdjustment = 0, LeadsTo = Scenes["2-43"] });
            Scenes["2-43"].Decisions.Add(new Model.Decision() { Description = string.Format("{0},'I would like to come back and continue''", User.Name), KarmaAdjustment = 0, LeadsTo = Scenes["8-3"] });
            Scenes["2-43"].Decisions.Add(new Model.Decision() { Description = string.Format("{0},'I am going to bed!''", User.Name), KarmaAdjustment = 0, LeadsTo = Scenes["BadDecision18"] });

            #endregion
            //Final Scene
            #region Final Scene
            Scenes["final"].Decisions.Add(new Model.Decision() { Description = string.Format("{0},'I AGREE that we need to help Aveel as a child!!'", User.Name), KarmaAdjustment = 0, LeadsTo = Scenes["final1"] });
            Scenes["final"].Decisions.Add(new Model.Decision() { Description = string.Format("{0},'I would NOT LIKE like to pursue this path!!'", User.Name), KarmaAdjustment = 0, LeadsTo = Scenes["BadDecision15"] });
            Scenes["final1"].Decisions.Add(new Model.Decision() { Description = string.Format("{0},'Go around back to find baby Aveel!!'", User.Name), KarmaAdjustment = 0, LeadsTo = Scenes["final2"] });
            Scenes["final1"].Decisions.Add(new Model.Decision() { Description = string.Format("{0},'Baby Aveel is crying, you hate crying babies!'", User.Name), KarmaAdjustment = 0, LeadsTo = Scenes["BadDecision16"] });//needs branch
            Scenes["final2"].Decisions.Add(new Model.Decision() { Description = string.Format("{0}m'Give General Aveel some encouraging words!", User.Name), KarmaAdjustment = 0, LeadsTo = Scenes["final3"] });
            Scenes["final2"].Decisions.Add(new Model.Decision() { Description = string.Format("{0}m'Give Baby Aveel a toy train.'", User.Name), KarmaAdjustment = 0, LeadsTo = Scenes["final3"] });
            Scenes["final3"].Decisions.Add(new Model.Decision() { Description = string.Format("{0},'I feel so good about helping others!!'", User.Name), KarmaAdjustment = 0, LeadsTo = Scenes["final4"] });
            Scenes["final3"].Decisions.Add(new Model.Decision() { Description = string.Format("{0},'I DO NOT believe in helping others!!'", User.Name), KarmaAdjustment = 0, LeadsTo = Scenes["BadDecision17"] });
            Scenes["final4"].TransitionToNext = new Model.Transition() { NextScene = Scenes["final5"], TimeTillNext = new TimeSpan(0, 0, 6) };
            Scenes["final5"].TransitionToNext = new Model.Transition() { NextScene = Scenes["final6"], TimeTillNext = new TimeSpan(0, 0, 6) };
            Scenes["final6"].TransitionToNext = new Model.Transition() { NextScene = Scenes["final7"], TimeTillNext = new TimeSpan(0, 0, 6) };
            Scenes["final7"].Decisions.Add(new Model.Decision() { Description = string.Format("{0},'Positive Reinforcement is always the right choice!!'", User.Name), KarmaAdjustment = 0, LeadsTo = Scenes["Victory"] });
            #endregion
            #region Victory
            Scenes["Victory"].Decisions.Add(new Model.Decision()  { Description = string.Format("{0},'The power of positivity is truly astounding!'", User.Name), KarmaAdjustment = 0, LeadsTo = Scenes["Victory1"] });
            //Scenes["Victory1"].Decisions.Add(new Model.Decision() { Description = string.Format("{0},'I would like to preview Lori's book.'", User.Name), KarmaAdjustment = 0, LeadsTo = Scenes["Victory2"] });
            //Scenes["Victory1"].Decisions.Add(new Model.Decision() { Description = string.Format("{0}'I would like to quit", User.Name), KarmaAdjustment = 0, LeadsTo = Scenes["Victory2"] });
            Scenes["Victory1"].Decisions.Add(new Model.Decision() { Description = string.Format("{0},'Save and Quit.'", User.Name), KarmaAdjustment = 0, LeadsTo = Scenes["SaveAndQuit"] });
            Scenes["Victory1"].Decisions.Add(new Model.Decision() { Description = string.Format("{0},'Save and load a different game.'", User.Name), KarmaAdjustment = 0, LeadsTo = Scenes["SaveAndLoad"] });
            #endregion
            #region bad decisions transitions
            Scenes["BadDecision"].TransitionToNext = new Model.Transition() { NextScene = Scenes["1-2"], TimeTillNext = new TimeSpan(0, 0, 8) };//I would like these to be able to sent the user back where they came from to stop making more levels
            Scenes["BadDecision1"].TransitionToNext = new Model.Transition() { NextScene = Scenes["1-3"], TimeTillNext = new TimeSpan(0, 0, 8) };//I would like these to be able to sent the user back where they came from to stop making more levels
            Scenes["BadDecision2"].TransitionToNext = new Model.Transition() { NextScene = Scenes["1-4"], TimeTillNext = new TimeSpan(0, 0, 8) };//I would like these to be able to sent the user back where they came from to stop making more levels
            Scenes["BadDecision3"].TransitionToNext = new Model.Transition() { NextScene = Scenes["2-2"], TimeTillNext = new TimeSpan(0, 0, 8) };
            Scenes["BadDecision4"].TransitionToNext = new Model.Transition() { NextScene = Scenes["3-2"], TimeTillNext = new TimeSpan(0, 0, 8) };
            Scenes["BadDecision5"].TransitionToNext = new Model.Transition() { NextScene = Scenes["4-2"], TimeTillNext = new TimeSpan(0, 0, 8) };//I would like these to be able to sent the user back where they came from to stop making more levels
            Scenes["BadDecision6"].TransitionToNext = new Model.Transition() { NextScene = Scenes["4-3"], TimeTillNext = new TimeSpan(0, 0, 8) };
            Scenes["BadDecision7"].TransitionToNext = new Model.Transition() { NextScene = Scenes["4-4"], TimeTillNext = new TimeSpan(0, 0, 8) };
            Scenes["BadDecision8"].TransitionToNext = new Model.Transition() { NextScene = Scenes["6-3"], TimeTillNext = new TimeSpan(0, 0, 8) };
            Scenes["BadDecision9"].TransitionToNext = new Model.Transition() { NextScene = Scenes["winter2"], TimeTillNext = new TimeSpan(0, 0, 8) };
            Scenes["BadDecision10"].TransitionToNext = new Model.Transition() { NextScene = Scenes["7-2"], TimeTillNext = new TimeSpan(0, 0, 8) };
            Scenes["BadDecision11"].TransitionToNext = new Model.Transition() { NextScene = Scenes["7-4"], TimeTillNext = new TimeSpan(0, 0, 8) };
            Scenes["BadDecision12"].TransitionToNext = new Model.Transition() { NextScene = Scenes["swamp1"], TimeTillNext = new TimeSpan(0, 0, 8) };
            Scenes["BadDecision13"].TransitionToNext = new Model.Transition() { NextScene = Scenes["8-2"], TimeTillNext = new TimeSpan(0, 0, 8) };
            Scenes["BadDecision14"].TransitionToNext = new Model.Transition() { NextScene = Scenes["8-6"], TimeTillNext = new TimeSpan(0, 0, 8) };
            Scenes["BadDecision15"].TransitionToNext = new Model.Transition() { NextScene = Scenes["final"], TimeTillNext = new TimeSpan(0, 0, 8) };
            Scenes["BadDecision16"].TransitionToNext = new Model.Transition() { NextScene = Scenes["final1"], TimeTillNext = new TimeSpan(0, 0, 8) };
            Scenes["BadDecision17"].TransitionToNext = new Model.Transition() { NextScene = Scenes["final2"], TimeTillNext = new TimeSpan(0, 0, 8) };
            Scenes["BadDecision18"].TransitionToNext = new Model.Transition() { NextScene = Scenes["2-43"], TimeTillNext = new TimeSpan(0, 0, 8) };
            Scenes["BadDecision19"].TransitionToNext = new Model.Transition() { NextScene = Scenes["1-4"], TimeTillNext = new TimeSpan(0, 0, 8) };
            Scenes["BadDecision20"].TransitionToNext = new Model.Transition() { NextScene = Scenes["6-2"], TimeTillNext = new TimeSpan(0, 0, 8) };


            #endregion
            #endregion
            #region Threshold Definitions
            //Add Scene Thresholds
            Scenes["1-1"].Threshold = new Model.SceneThreshold() { GoBackTo = Scenes["1-1"], RequiredKarma = 0 };
            Scenes["2-1"].Threshold = new Model.SceneThreshold() { GoBackTo = Scenes["1-1"], RequiredKarma = 100 };
            Scenes["3-1"].Threshold = new Model.SceneThreshold() { GoBackTo = Scenes["2-1"], RequiredKarma = 150 };
            Scenes["4-1"].Threshold = new Model.SceneThreshold() { GoBackTo = Scenes["3-1"], RequiredKarma = 200 };
            Scenes["5-1"].Threshold = new Model.SceneThreshold() { GoBackTo = Scenes["4-1"], RequiredKarma = 250 };
            Scenes["6-1"].Threshold = new Model.SceneThreshold() { GoBackTo = Scenes["5-1"], RequiredKarma = 300 };
            Scenes["7-1"].Threshold = new Model.SceneThreshold() { GoBackTo = Scenes["6-1"], RequiredKarma = 350 };
            Scenes["8-1"].Threshold = new Model.SceneThreshold() { GoBackTo = Scenes["7-1"], RequiredKarma = 450 };
            Scenes["final"].Threshold = new Model.SceneThreshold() { GoBackTo = Scenes["8-1"], RequiredKarma = 550 };
            Scenes["Victory"].Threshold = new Model.SceneThreshold() { GoBackTo = Scenes["final"], RequiredKarma = 90 };

            dataLoaded = true;
            #endregion
        }
        #endregion

        public bool IsDataLoaded {
            get
            {
                return dataLoaded;
            }
       }
        #region Save and Load User State
        public void SaveGame()
        {
            using (IsolatedStorageFile isoStore = IsolatedStorageFile.GetUserStoreForApplication())
            {
                if (!isoStore.DirectoryExists("Saves"))
                     isoStore.CreateDirectory("Saves");
                using (StreamWriter GameSave = new StreamWriter(new IsolatedStorageFileStream("Saves/" + User.Name + Karma + ".sav", System.IO.FileMode.Create, isoStore)))
                {
                    GameSave.WriteLine(User.Name);
                    GameSave.WriteLine((int)User.Gender);
                    GameSave.WriteLine((int)User.Grade);
                    GameSave.WriteLine(CurrentScene.ID);
                    GameSave.WriteLine(Karma);
                }

                this.RaisePropertyChanged(() => this.SavedGameList);
            }
        }


        public bool LoadGame(string UserName)
        {
            bool LoadSuccess = false;
            using (IsolatedStorageFile isoStore = IsolatedStorageFile.GetUserStoreForApplication())
            {
                string SavePath = string.Format("Saves/{0}.sav",UserName);
                if (isoStore.FileExists(SavePath))
                {
                    using (StreamReader SaveReader = new StreamReader(new IsolatedStorageFileStream(SavePath,FileMode.Open,FileAccess.Read,isoStore)))
                    {
                        if (User == null)
                            User = new Model.User();
                        User.Name = SaveReader.ReadLine();
                        User.Gender = (Model.UserGender)int.Parse(SaveReader.ReadLine());
                        User.Grade = (Model.UserGrade)int.Parse(SaveReader.ReadLine());
                        LoadData();
                        CurrentScene = Scenes[SaveReader.ReadLine()];
                        Karma = int.Parse(SaveReader.ReadLine());
                        LoadSuccess = true;
                    }
                }
            }
            return LoadSuccess;
        }

        public ObservableCollection<String> SavedGameList
        {
            get
            {
                return new ObservableCollection<string>(GetSavedGameNames());
            }
        }

        private IEnumerable<string> GetSavedGameNames()
        {
            List<string> savedGameList = new List<string>();
            using (IsolatedStorageFile isoStore = IsolatedStorageFile.GetUserStoreForApplication())
            {
                if (isoStore.DirectoryExists("Saves"))
                    savedGameList.AddRange(isoStore.GetFileNames("Saves/*.sav"));
            }

            foreach (string savedGame in savedGameList)
                yield return savedGame.Replace(".sav", "");


        }
        #endregion
    }        
}