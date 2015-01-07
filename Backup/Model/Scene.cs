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
using System.Collections.ObjectModel;

namespace YourAdventure.Model
{
    public class Scene
    {
        public String Narrative
        {get;set;}

        public String BackgroundImgSrc
        {get;set;}

        public String UserImgSrc { get; set; }
        public string OtherCharImgSrc { get; set; }
        public ObservableCollection<Decision> Decisions
        {get;set;}

        public string ID { get; set; }

        public Model.RTD RTD{get;set;}
        public SceneThreshold Threshold { get; set; }
        public Transition TransitionToNext { get; set; }
    }
}
