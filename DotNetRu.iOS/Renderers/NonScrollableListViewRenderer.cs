﻿using System;
using DotNetRu.Clients.UI.Controls;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

using XamarinEvolve.iOS;

[assembly:ExportRenderer(typeof(NonScrollableListView), typeof(NonScrollableListViewRenderer))]
[assembly:ExportRenderer(typeof(AlwaysScrollView), typeof(AlwaysScrollViewRenderer))]
namespace XamarinEvolve.iOS
{
    public class NonScrollableListViewRenderer : ListViewRenderer
    {
        public static void Initialize()
        {
            var test = DateTime.UtcNow;
        }

        protected override void OnElementChanged(ElementChangedEventArgs<ListView> e)
        {
            base.OnElementChanged(e);
            if (this.Control != null) this.Control.ScrollEnabled = false;
            
        }
    }

    public class AlwaysScrollViewRenderer : ScrollViewRenderer
    {
        public static void Initialize()
        {
            var test = DateTime.UtcNow;
        }

        protected override void OnElementChanged(VisualElementChangedEventArgs e)
        {
            base.OnElementChanged(e);
            this.AlwaysBounceVertical = true;
        }
    }
}

