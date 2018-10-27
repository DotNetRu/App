using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace XamarinEvolve.Clients.UI
{
    public class StarBehavior : Behavior<View>
    {
        TapGestureRecognizer tapRecognizer;
        static readonly List<StarBehavior> defaultBehaviors = new List<StarBehavior>();
        static readonly Dictionary<string, List<StarBehavior>> starGroups = new Dictionary<string, List<StarBehavior>>();

		public event EventHandler RatingChanged;

        public static readonly BindableProperty GroupNameProperty =
            BindableProperty.Create("GroupName",
                typeof(string),
                typeof(StarBehavior),
                null,
                propertyChanged: OnGroupNameChanged);

        public string GroupName
        {
            get => (string)this.GetValue(GroupNameProperty);
            set => this.SetValue(GroupNameProperty, value);
        }

        public static readonly BindableProperty StarRatingProperty =
            BindableProperty.Create("StarRating",
                typeof(int),
                typeof(StarBehavior), default(int));

        public int StarRating
        {
            get => (int)this.GetValue(StarRatingProperty);
            set 
            {
                this.SetValue(StarRatingProperty, value);
                this.RatingChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        static void OnGroupNameChanged(BindableObject bindable, object oldValue, object newValue)
        {
            StarBehavior behavior = (StarBehavior)bindable;
            string oldGroupName = (string)oldValue;
            string newGroupName = (string)newValue;

            // Remove existing behavior from Group
            if (string.IsNullOrEmpty(oldGroupName))
            {
                defaultBehaviors.Remove(behavior);
            }
            else
            {
                List<StarBehavior> behaviors = starGroups[oldGroupName];
                behaviors.Remove(behavior);

                if (behaviors.Count == 0)
                {
                    starGroups.Remove(oldGroupName);
                }
            }

            // Add New Behavior to the group
            if (string.IsNullOrEmpty(newGroupName))
            {
                defaultBehaviors.Add(behavior);
            }
            else
            {
                List<StarBehavior> behaviors = null;

                if (starGroups.ContainsKey(newGroupName))
                {
                    behaviors = starGroups[newGroupName];
                }
                else
                {
                    behaviors = new List<StarBehavior>();
                    starGroups.Add(newGroupName, behaviors);
                }

                behaviors.Add(behavior);
            }

        }


        public static readonly BindableProperty IsStarredProperty =
            BindableProperty.Create("IsStarred", 
                typeof(bool), 
                typeof(StarBehavior), 
                false,
                propertyChanged: OnIsStarredChanged);

        public bool IsStarred
        {
            get => (bool)this.GetValue(IsStarredProperty);
            set => this.SetValue(IsStarredProperty, value);
        }

        static void OnIsStarredChanged(BindableObject bindable, object oldValue, object newValue)
        {
            StarBehavior behavior = (StarBehavior)bindable;

            if ((bool)newValue)
            {
                string groupName = behavior.GroupName;
                List<StarBehavior> behaviors = null;

                if (string.IsNullOrEmpty(groupName))
                {
                    behaviors = defaultBehaviors;
                }
                else
                {
                    behaviors = starGroups[groupName];
                }

                bool itemReached = false;
                int count = 1, position = 0;

                // all positions to left IsStarred = true and all position to the right is false
                foreach (var item in behaviors)
                {
                    if (item != behavior && !itemReached)
                    {
                        item.IsStarred = true;
                    }

                    if (item == behavior)
                    {
                        itemReached = true;
                        item.IsStarred = true;
                        position = count;
                    }

                    if (item != behavior && itemReached) item.IsStarred = false;

                    item.StarRating = position;
                    count++;
                }
            }
        }

        public StarBehavior()
        {
            defaultBehaviors.Add(this);
        }

        protected override void OnAttachedTo(View bindable)
        {
            this.tapRecognizer = new TapGestureRecognizer();
            this.tapRecognizer.Tapped += this.OnTapRecognizerTapped;
            bindable.GestureRecognizers.Add(this.tapRecognizer);
		}

        protected override void OnDetachingFrom(View bindable)
        {
            bindable.GestureRecognizers.Remove(this.tapRecognizer);
            this.tapRecognizer.Tapped -= this.OnTapRecognizerTapped;
            defaultBehaviors.Clear();
            starGroups.Clear();
        }

        void OnTapRecognizerTapped(object sender, EventArgs args)
        {
            // HACK: PropertyChange does not fire, if the value is not changed :-(
            this.IsStarred = false;
            this.IsStarred = true;
        }
    }
}

