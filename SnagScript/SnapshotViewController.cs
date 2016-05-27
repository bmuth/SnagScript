using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;
using AVFoundation;
using CoreMedia;
using System.Threading.Tasks;
using CoreGraphics;
using CoreAnimation;
using System.Drawing;

namespace SnagScript
{
	partial class SnapshotViewController : UIViewController
	{
		public double fHeightWidthRatio;


		private double TopOffset;
		private double LeftOffset;
		private double WidthShrink;
		private double HeightShrink;

		public delegate void PhotoUpdatedHandler (UIImage image);
		public event PhotoUpdatedHandler PhotoUpdated;

		AVCaptureSession captureSession;
		AVCaptureDeviceInput captureDeviceInput;
		AVCaptureStillImageOutput stillImageOutput;
		AVCaptureVideoPreviewLayer videoPreviewLayer;

		CALayer CaptureLayer;
		public SnapshotViewController (IntPtr handle) : base (handle)
		{
		}

		partial void CancelButton_TouchUpInside (UIButton sender)
		{
			this.DismissViewControllerAsync (true);
		}

	}
}
