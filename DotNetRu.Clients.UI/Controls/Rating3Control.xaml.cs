using System;
using Xamarin.Forms;

namespace XamarinEvolve.Clients.UI
{
	public partial class Rating3Control : ContentView
	{
		public Rating3Control()
		{
		    this.InitializeComponent();
		}

		public void RemoveBehaviors()
		{
		    this.ClearBehaviors(this.StarGrid1);
		    this.ClearBehaviors(this.StarGrid2);
		    this.ClearBehaviors(this.StarGrid3);
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
		    get => (int)this.GetValue(RatingProperty);
		    set => this.SetValue(RatingProperty, value);
		}

	    public static readonly BindableProperty GroupNameProperty =
			BindableProperty.Create("GroupName",
				typeof(string),
				typeof(Rating3Control), default(string), propertyChanged: OnGroupNameChanged);

		public string GroupName
		{
		    get => (string)this.GetValue(GroupNameProperty);
		    set => this.SetValue(GroupNameProperty, value);
		}

	    public void OnStarRatingChanged(object sender, EventArgs e)
		{
			var behavior = sender as StarBehavior;
		    this.Rating = behavior?.StarRating ?? 0;
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
