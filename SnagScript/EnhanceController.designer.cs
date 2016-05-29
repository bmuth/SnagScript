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
	[Register ("EnhanceController")]
	partial class EnhanceController
	{
		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UISlider BrightnessSlider { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIButton CancelButton { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UISlider ContrastSlider { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIImageView EditImageView { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIButton OkButton { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UISlider SaturationSlider { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UILabel SliderValue { get; set; }

		[Action ("CancelButton_TouchUpInside:")]
		[GeneratedCode ("iOS Designer", "1.0")]
		partial void CancelButton_TouchUpInside (UIButton sender);

		[Action ("OkButton_TouchUpInside:")]
		[GeneratedCode ("iOS Designer", "1.0")]
		partial void OkButton_TouchUpInside (UIButton sender);

		void ReleaseDesignerOutlets ()
		{
			if (BrightnessSlider != null) {
				BrightnessSlider.Dispose ();
				BrightnessSlider = null;
			}
			if (CancelButton != null) {
				CancelButton.Dispose ();
				CancelButton = null;
			}
			if (ContrastSlider != null) {
				ContrastSlider.Dispose ();
				ContrastSlider = null;
			}
			if (EditImageView != null) {
				EditImageView.Dispose ();
				EditImageView = null;
			}
			if (OkButton != null) {
				OkButton.Dispose ();
				OkButton = null;
			}
			if (SaturationSlider != null) {
				SaturationSlider.Dispose ();
				SaturationSlider = null;
			}
			if (SliderValue != null) {
				SliderValue.Dispose ();
				SliderValue = null;
			}
		}
	}
}
