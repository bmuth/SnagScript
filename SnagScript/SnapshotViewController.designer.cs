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
	[Register ("SnapshotViewController")]
	partial class SnapshotViewController
	{
		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIButton CancelButton { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIImageView LiveCameraStream { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIButton TakePhoto { get; set; }

		[Action ("CancelButton_TouchUpInside:")]
		[GeneratedCode ("iOS Designer", "1.0")]
		partial void CancelButton_TouchUpInside (UIButton sender);

		void ReleaseDesignerOutlets ()
		{
			if (CancelButton != null) {
				CancelButton.Dispose ();
				CancelButton = null;
			}
			if (LiveCameraStream != null) {
				LiveCameraStream.Dispose ();
				LiveCameraStream = null;
			}
			if (TakePhoto != null) {
				TakePhoto.Dispose ();
				TakePhoto = null;
			}
		}
	}
}
