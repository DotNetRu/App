using System;
using ToastIOS;
using UIKit;

namespace PreToast
{
	public partial class ViewController : UIViewController
	{
		public ViewController (IntPtr handle) : base (handle)
		{
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			// Perform any additional setup after loading the view, typically from a nib.
			//ButtonOK.TouchUpInside += (sender, e) => {
			//	Toast.MakeText("How are you today ?",Toast.LENGTH_SHORT).Show();
			//};
		}

		public override void DidReceiveMemoryWarning ()
		{
			base.DidReceiveMemoryWarning ();
			// Release any cached data, images, etc that aren't in use.
		}
			

		partial void ButtonCustomToast_TouchUpInside (UIButton sender)
		{
			Toast.MakeText("This is Customed Toast").SetCornerRadius (15).SetGravity (ToastGravity.Center)
				.SetDuration(3000).SetUseShadow (true).SetFontSize (12).Show(ToastType.Error);
		}

		partial void ButtonAndroidToast_TouchUpInside (UIButton sender)
		{
			Toast.MakeText("Android Like Toast ?",Toast.LENGTH_SHORT).Show();
		}

		partial void ButtonSimplestToast_TouchUpInside (UIButton sender)
		{
			Toast.MakeText("Simplest").Show();
		}

		partial void ButtonSimplestImageToast_TouchUpInside (UIButton sender)
		{
			Toast.MakeText("Simplest").Show(ToastType.Info);
		}
	}
}

