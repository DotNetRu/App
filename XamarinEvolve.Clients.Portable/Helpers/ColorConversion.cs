using Xamarin.Forms;

namespace XamarinEvolve.Clients.Portable
{
	public static class ColorConversion
	{
		public static string ToHex(this Color c)
		{
			return string.Format("#{0:X2}{1:X2}{2:X2}", (int)(c.R * 256), (int)(c.G * 256), (int)(c.B * 256));
		}
	}
}

