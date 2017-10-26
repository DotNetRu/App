using System;
using Xamarin.Forms;

namespace XamarinEvolve.Clients.UI
{
	public partial class RatingControl : ContentView
	{
		public RatingControl()
		{
		    this.InitializeComponent();
		}

		public void RemoveBehaviors()
		{
		    this.ClearBehaviors(this.StarGrid1);
		    this.ClearBehaviors(this.StarGrid2);
		    this.ClearBehaviors(this.StarGrid3);
		    this.ClearBehaviors(this.StarGrid4);
		    this.ClearBehaviors(this.StarGrid5);
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
				typeof(RatingControl), default(int));

		public int Rating
		{
		    get => (int)this.GetValue(RatingProperty);
		    set => this.SetValue(RatingProperty, value);
		}

	    public static readonly BindableProperty GroupNameProperty =
			BindableProperty.Create("GroupName",
				typeof(string),
                typeof(RatingControl), default(string), propertyChanged: OnGroupNameChanged);

		public string GroupName
		{
		    get => (string)this.GetValue(GroupNameProperty);
		    set => this.SetValue(GroupNameProperty, value);
		}

	    static void OnGroupNameChanged(BindableObject bindable, object oldValue, object newValue)
		{
			var control = (RatingControl)bindable;

			Regroup(control.StarGrid1, (string)newValue);
			Regroup(control.StarGrid2, (string)newValue);
			Regroup(control.StarGrid3, (string)newValue);
			Regroup(control.StarGrid4, (string)newValue);
			Regroup(control.StarGrid5, (string)newValue);
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

		public void OnStarRatingChanged(object sender, EventArgs e)
		{
			var behavior = sender as StarBehavior;
		    this.Rating = behavior?.StarRating ?? 0;
		}
	}
}
