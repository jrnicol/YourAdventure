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

namespace YourAdventure.Model
{
    public class Decision
    {
        public override string ToString()
        {
            return this.Description;
        }

        public Scene LeadsTo
        {
            get;
            set;
        }

        public int KarmaAdjustment
        {
            get;
            set;
        }

        public string Description { get; set; }
    }
}
