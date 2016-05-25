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
	[Register ("OptionViewController")]
	partial class OptionViewController
	{
		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIButton CancelButton { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIButton OkButton { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UITextField tbCollegeNo { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UITextField tbEmailAddress { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UITextField tbProviderName { get; set; }

		[Action ("OkButton_TouchUpInside:")]
		[GeneratedCode ("iOS Designer", "1.0")]
		partial void OkButton_TouchUpInside (UIButton sender);

		[Action ("TextFieldChanged:")]
		[GeneratedCode ("iOS Designer", "1.0")]
		partial void TextFieldChanged (UITextField sender);

		void ReleaseDesignerOutlets ()
		{
			if (CancelButton != null) {
				CancelButton.Dispose ();
				CancelButton = null;
			}
			if (OkButton != null) {
				OkButton.Dispose ();
				OkButton = null;
			}
			if (tbCollegeNo != null) {
				tbCollegeNo.Dispose ();
				tbCollegeNo = null;
			}
			if (tbEmailAddress != null) {
				tbEmailAddress.Dispose ();
				tbEmailAddress = null;
			}
			if (tbProviderName != null) {
				tbProviderName.Dispose ();
				tbProviderName = null;
			}
		}
	}
}
