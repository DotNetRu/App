using System;
using System.Linq;
using Xamarin.Forms;

namespace DotNetRu.Clients.UI.Controls
{
	/// <summary>
	/// Simple Layout panel which performs wrapping on the boundaries.
	/// </summary>
	public class WrapLayout : Layout<View>
	{
		/// <summary>
		/// Backing Storage for the Orientation property
		/// </summary>
		public static readonly BindableProperty OrientationProperty =
			BindableProperty.Create("Orientation",
                typeof(StackOrientation),
                typeof(WrapLayout),
                StackOrientation.Horizontal, propertyChanged: (bindable, oldvalue, newvalue) => ((WrapLayout)bindable).OnSizeChanged());

		/// <summary>
		/// Orientation (Horizontal or Vertical)
		/// </summary>
		public StackOrientation Orientation {
			get => (StackOrientation)this.GetValue (OrientationProperty);
		    set => this.SetValue (OrientationProperty, value);
		}

		/// <summary>
		/// Backing Storage for the Spacing property
		/// </summary>
		public static readonly BindableProperty SpacingProperty =
		BindableProperty.Create("Spacing",
				typeof(double),
				typeof(WrapLayout),
				6d, propertyChanged: (bindable, oldvalue, newvalue) => ((WrapLayout)bindable).OnSizeChanged());

		/// <summary>
		/// Spacing added between elements (both directions)
		/// </summary>
		/// <value>The spacing.</value>
		public double Spacing {
			get => (double)this.GetValue (SpacingProperty);
		    set => this.SetValue (SpacingProperty, value);
		}

		/// <summary>
		/// This is called when the spacing or orientation properties are changed - it forces
		/// the control to go back through a layout pass.
		/// </summary>
		private void OnSizeChanged()
		{
		    this.ForceLayout();
		}

		// http://forums.xamarin.com/discussion/17961/stacklayout-with-horizontal-orientation-how-to-wrap-vertically#latest
		// 		protected override void OnPropertyChanged
		// 		(string propertyName = null)
		// 		{
		// 			base.OnPropertyChanged(propertyName);
		// 			if ((propertyName == WrapLayout.OrientationProperty.PropertyName) ||
		// 				(propertyName == WrapLayout.SpacingProperty.PropertyName)) {
		// 				this.OnSizeChanged();
		// 			}
		// 		}
		protected override SizeRequest OnMeasure(double widthConstraint, double heightConstraint)
		{
			if (this.WidthRequest > 0)
				widthConstraint = Math.Min(widthConstraint, this.WidthRequest);
			if (this.HeightRequest > 0)
				heightConstraint = Math.Min(heightConstraint, this.HeightRequest);

			double internalWidth = double.IsPositiveInfinity(widthConstraint) ? double.PositiveInfinity : Math.Max(0, widthConstraint);
			double internalHeight = double.IsPositiveInfinity(heightConstraint) ? double.PositiveInfinity : Math.Max(0, heightConstraint);

			return this.Orientation == StackOrientation.Vertical
				? this.DoVerticalMeasure(internalWidth, internalHeight)
					: this.DoHorizontalMeasure(internalWidth, internalHeight);
		}

		/// <summary>
		/// Does the vertical measure.
		/// </summary>
		/// <returns>The vertical measure.</returns>
		/// <param name="widthConstraint">Width constraint.</param>
		/// <param name="heightConstraint">Height constraint.</param>
		private SizeRequest DoVerticalMeasure(double widthConstraint, double heightConstraint)
		{
			int columnCount = 1;

			double width = 0;
			double height = 0;
			double minWidth = 0;
			double minHeight = 0;
			double heightUsed = 0;

			foreach (var item in this.Children)    
			{
				var size = item.Measure (widthConstraint, heightConstraint);
				width = Math.Max (width, size.Request.Width);

				var newHeight = height + size.Request.Height + this.Spacing;
				if (newHeight > heightConstraint) {
					columnCount++;
					heightUsed = Math.Max (height, heightUsed) + this.Spacing;
					height = size.Request.Height;
				} else
					height = newHeight;

				minHeight = Math.Max (minHeight, size.Minimum.Height);
				minWidth = Math.Max (minWidth, size.Minimum.Width);
			}

			if (columnCount > 1) {
				height = Math.Max (height, heightUsed + this.Padding.Top + this.Padding.Bottom + this.Margin.Top + this.Margin.Bottom);
				width *= columnCount;  // take max width
			}

			return new SizeRequest(new Size(width, height), new Size(minWidth, minHeight));
		}

		/// <summary>
		/// Does the horizontal measure.
		/// </summary>
		/// <returns>The horizontal measure.</returns>
		/// <param name="widthConstraint">Width constraint.</param>
		/// <param name="heightConstraint">Height constraint.</param>
		private SizeRequest DoHorizontalMeasure(double widthConstraint, double heightConstraint)
		{
			double width = 0d;
			double horizontalPaddingAndMargin = this.Padding.Left + this.Margin.Left + this.Margin.Right + this.Padding.Right;
			double height = 0d;
			double minWidth = width;
			double minHeight = height;

			double widthUsed = 0d;
			double heightUsed = 0d;

			double currentRowHeight = 0d;

			bool firstRow = true;

			foreach (var item in this.Children)    
			{
				var size = item.Measure(widthConstraint, heightConstraint);

				minHeight = Math.Max (minHeight, size.Minimum.Height);
				minWidth = Math.Max (minWidth, size.Minimum.Width);

				var newWidth = width + size.Request.Width;
				if (newWidth + this.Spacing >= widthConstraint) 
				{
					if (!firstRow)
					{
						heightUsed += currentRowHeight;
					}

					firstRow = false;

					// wrap to next row
					currentRowHeight = size.Request.Height + this.Spacing;
					widthUsed = Math.Max (width, widthUsed);
					width = size.Request.Width + this.Spacing + horizontalPaddingAndMargin;
				}
				else 
				{
					// stay on same row
					width = newWidth;
					currentRowHeight = Math.Max(size.Request.Height + this.Spacing, currentRowHeight);

					if (firstRow)
					{
						heightUsed = currentRowHeight;
					}
				}
			}
				
			width = Math.Max(width, widthUsed);
			height = Math.Round(heightUsed) + this.Padding.Top + this.Margin.Top + this.Margin.Bottom + this.Padding.Bottom;

			return new SizeRequest(new Size(width, height), new Size(minWidth, minHeight));
		}

		/// <summary>
		/// Positions and sizes the children of a Layout.
		/// </summary>
		/// <param name="x">A value representing the x coordinate of the child region bounding box.</param>
		/// <param name="y">A value representing the y coordinate of the child region bounding box.</param>
		/// <param name="width">A value representing the width of the child region bounding box.</param>
		/// <param name="height">A value representing the height of the child region bounding box.</param>
		protected override void LayoutChildren (double x, double y, double width, double height)
		{
			if (this.Orientation == StackOrientation.Vertical) 
			{
				double colWidth = 0;
				double yPos = y, xPos = x;

				foreach (var child in this.Children.Where(c => c.IsVisible)) 
				{
					var request = child.Measure (width, height);

					double childWidth = request.Request.Width;
					double childHeight = request.Request.Height;
					colWidth = Math.Max(colWidth, childWidth);

					if (yPos + childHeight > height) {
						yPos = y;
						xPos += colWidth + this.Spacing;
						colWidth = 0;
					}

					var region = new Rectangle (xPos, yPos, childWidth, childHeight);
					LayoutChildIntoBoundingRegion (child, region);
					yPos += region.Height + this.Spacing;
				}
			}
			else 
			{
				double rowHeight = 0;
				double yPos = y, xPos = x;

				foreach (var child in this.Children.Where(c => c.IsVisible)) 
				{
					var request = child.Measure (width, height);

					double childWidth = request.Request.Width;

					if (xPos + childWidth >= width) {
						xPos = x;
						yPos += rowHeight + this.Spacing;
						rowHeight = 0;
					}

					double childHeight = request.Request.Height;
					rowHeight = Math.Max(rowHeight, childHeight);

					var region = new Rectangle (xPos, yPos, childWidth, childHeight);
					LayoutChildIntoBoundingRegion (child, region);
					xPos += region.Width + this.Spacing;
				}
			}
		}
	}
}