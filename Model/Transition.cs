using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YourAdventure.Model
{
    public class Transition
    {
        public Scene NextScene
        {
            get;
            set;
        }

        public TimeSpan TimeTillNext { get; set; }
    }
}
