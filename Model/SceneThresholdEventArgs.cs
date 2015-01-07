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
    public class SceneThresholdEventArgs: EventArgs
    {
        public SceneThreshold Threshold;
        public SceneThresholdEventArgs(SceneThreshold Threshold)
        {
            this.Threshold = Threshold;
        }
    }
}
