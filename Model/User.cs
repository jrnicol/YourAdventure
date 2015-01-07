﻿using System;
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
    public class User
    {
        public string Name{
            get;set;
        }
        public UserGrade Grade{
            get;set;
        }
        public UserGender Gender{
            get;set;
        }

    }

      public enum UserGrade {Fifth,Sixth,Seventh,Eighth};
      public enum UserGender{Boy,Girl};     
}

