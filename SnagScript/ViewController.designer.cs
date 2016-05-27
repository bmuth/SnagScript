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

namespace SnagScript
{
	[Register ("ViewController")]
	partial class ViewController
	{
		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIImageView imageMeds { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIImageView imagePatient { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UILabel labCollegeNo { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UILabel labDate { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UILabel labMsg1 { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UILabel labMsg2 { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UILabel labProviderName { get; set; }

		[Action ("UIButton5_TouchUpInside:")]
		[GeneratedCode ("iOS Designer", "1.0")]
		partial void UIButton5_TouchUpInside (UIButton sender);

		void ReleaseDesignerOutlets ()
		{
			if (imageMeds != null) {
				imageMeds.Dispose ();
				imageMeds = null;
			}
			if (imagePatient != null) {
				imagePatient.Dispose ();
				imagePatient = null;
			}
			if (labCollegeNo != null) {
				labCollegeNo.Dispose ();
				labCollegeNo = null;
			}
			if (labDate != null) {
				labDate.Dispose ();
				labDate = null;
			}
			if (labMsg1 != null) {
				labMsg1.Dispose ();
				labMsg1 = null;
			}
			if (labMsg2 != null) {
				labMsg2.Dispose ();
				labMsg2 = null;
			}
			if (labProviderName != null) {
				labProviderName.Dispose ();
				labProviderName = null;
			}
		}
	}
}
