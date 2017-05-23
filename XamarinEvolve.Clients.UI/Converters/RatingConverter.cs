﻿using System;
using Xamarin.Forms;
using XamarinEvolve.DataObjects;
using System.Globalization;

namespace XamarinEvolve.Clients.UI
{
	/// <summary>
	/// Rating converter for display text
	/// </summary>
	class RatingConverter : IValueConverter
	{
		private static readonly string[] defaultValues = { "Choose a rating", "Not a fan", "It was ok", "Good", "Great", "Love it!" };

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			var rating = (int)value;

			var values = defaultValues;

			if (parameter != null && parameter is string)
			{
				values = ((string)parameter).Split(',');
			}

			if (rating >= 0 && rating < values.Length)
			{
				return values[rating];
			}

			return string.Empty;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}

	/// <summary>
	/// Determins if the rating section should be visible
	/// </summary>
	class RatingVisibleConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{

#if DEBUG || ENABLE_TEST_CLOUD
			//return true;
#endif

			var session = value as Session;
			if (session == null)
				return false;

			if (!session.StartTime.HasValue)
				return false;

			if (session.StartTime.Value == DateTime.MinValue)
				return false;

			//if it has started or is about to start
			if (session.StartTime.Value.AddMinutes(-15).ToUniversalTime() < DateTime.UtcNow)
				return true;

			return false;
		}

		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}

}
