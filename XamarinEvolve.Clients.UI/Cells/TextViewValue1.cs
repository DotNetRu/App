﻿using System;
using Xamarin.Forms;

namespace XamarinEvolve.Clients.UI
{
    public class TextViewValue1 : TextCell
    {
        public TextViewValue1()
        {
			DetailColor = (Color) Application.Current.Resources["DetailTextColor"];
        }
    }
}

