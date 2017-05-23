// WARNING
//
// This file has been generated automatically by Xamarin Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace PreToast
{
	[Register ("ViewController")]
	partial class ViewController
	{
		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIButton ButtonAndroidToast { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIButton ButtonCustomToast { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIButton ButtonSimplestImageToast { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIButton ButtonSimplestToast { get; set; }

		[Action ("ButtonAndroidToast_TouchUpInside:")]
		[GeneratedCode ("iOS Designer", "1.0")]
		partial void ButtonAndroidToast_TouchUpInside (UIButton sender);

		[Action ("ButtonCustomToast_TouchUpInside:")]
		[GeneratedCode ("iOS Designer", "1.0")]
		partial void ButtonCustomToast_TouchUpInside (UIButton sender);

		[Action ("ButtonSimplestImageToast_TouchUpInside:")]
		[GeneratedCode ("iOS Designer", "1.0")]
		partial void ButtonSimplestImageToast_TouchUpInside (UIButton sender);

		[Action ("ButtonSimplestToast_TouchUpInside:")]
		[GeneratedCode ("iOS Designer", "1.0")]
		partial void ButtonSimplestToast_TouchUpInside (UIButton sender);

		void ReleaseDesignerOutlets ()
		{
			if (ButtonAndroidToast != null) {
				ButtonAndroidToast.Dispose ();
				ButtonAndroidToast = null;
			}
			if (ButtonCustomToast != null) {
				ButtonCustomToast.Dispose ();
				ButtonCustomToast = null;
			}
			if (ButtonSimplestImageToast != null) {
				ButtonSimplestImageToast.Dispose ();
				ButtonSimplestImageToast = null;
			}
			if (ButtonSimplestToast != null) {
				ButtonSimplestToast.Dispose ();
				ButtonSimplestToast = null;
			}
		}
	}
}
