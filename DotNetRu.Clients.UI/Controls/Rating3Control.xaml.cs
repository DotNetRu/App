using System;
using Xamarin.Forms;

namespace XamarinEvolve.Clients.UI
{
	public partial class Rating3Control : ContentView
	{
		public Rating3Control()
		{
			InitializeComponent();
		}

		public void RemoveBehaviors()
		{
			ClearBehaviors(StarGrid1);
			ClearBehaviors(StarGrid2);
			ClearBehaviors(StarGrid3);
		}

		void ClearBehaviors(Grid grid)
		{
			var items = grid.Behaviors.Count;
			for (int i = 0; i < items; i++)
				grid.Behaviors.RemoveAt(i);
		}

		public static readonly BindableProperty RatingProperty =
			BindableProperty.Create("Rating",
				typeof(int),
			    typeof(Rating3Control), default(int));

		public int Rating
		{
			set { SetValue(RatingProperty, value); }
			get { return (int)GetValue(RatingProperty); }
		}

		public static readonly BindableProperty GroupNameProperty =
			BindableProperty.Create("GroupName",
				typeof(string),
				typeof(Rating3Control), default(string), propertyChanged: OnGroupNameChanged);

		public string GroupName
		{
			set { SetValue(GroupNameProperty, value); }
			get { return (string)GetValue(GroupNameProperty); }
		}

		public void OnStarRatingChanged(object sender, EventArgs e)
		{
			var behavior = sender as StarBehavior;
			Rating = behavior?.StarRating ?? 0;
		}

		static void OnGroupNameChanged(BindableObject bindable, object oldValue, object newValue)
		{
			var control = (Rating3Control)bindable;
			Regroup(control.StarGrid1, (string)newValue);
			Regroup(control.StarGrid2, (string)newValue);
			Regroup(control.StarGrid3, (string)newValue);
		}

		static void Regroup(Grid grid, string newGroupName)
		{
			var items = grid.Behaviors.Count;
			for (int i = 0; i < items; i++)
			{
				var starBehavior = grid.Behaviors[i] as StarBehavior;

				if (starBehavior != null)
				{
					starBehavior.GroupName = newGroupName;
				}
			}
		}
	}
}
