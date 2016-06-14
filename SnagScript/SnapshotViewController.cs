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
		public CGSize CropSize;

		private bool bScaleWidth;
		private bool bScaleHeight;

		public delegate void PhotoUpdatedHandler (UIImage image);
		public event PhotoUpdatedHandler PhotoUpdated;
		public delegate void CropSizeUpdatedHandler (CGSize sz);
		public event CropSizeUpdatedHandler CropSizeUpdated;


		AVCaptureSession captureSession;
		AVCaptureDeviceInput captureDeviceInput;
		AVCaptureStillImageOutput stillImageOutput;
		AVCaptureVideoPreviewLayer videoPreviewLayer;

		private CALayer CaptureLayer = null;
		private CGSize scale;

		UIPinchGestureRecognizer PinchGesture;

		public SnapshotViewController (IntPtr handle) : base (handle)
		{
		}

		/**********************************************
		 * 
		 * ViewDidLoad
		 * 
		 * *******************************************/

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();


			TakePhotoButton.TouchUpInside += async (object sender, EventArgs e) => 
			{
				await TakePhotoButtonTapped ();
			};

			FlashToggleButton.TouchUpInside += (object sender, EventArgs e) => 
			{
				FlashButtonTapped ();
			};

			PinchGesture = new UIPinchGestureRecognizer (this.OnPinchGesture);
			this.View.AddGestureRecognizer (PinchGesture);


		}

		/************************************************
		 * 
		 * ViewDidLayoutSubviews
		 * 
		 * *********************************************/

		public override async void ViewDidLayoutSubviews()
		{
			base.ViewDidLayoutSubviews();

			if (CropSize.Width == 0) {
				CGSize s = LiveCameraStream.Frame.Size;
				s.Width -= 6;
				CropSize = new CGSize (s.Width, s.Width * fHeightWidthRatio);	
			}

			await AuthorizeCameraUse ();
			SetupLiveCameraStream ();		
		}

		/*****************************************
		 * 
		 * InitializeCaptureLayer
		 * 
		 * **************************************/

		private void InitializeCaptureLayer (CGPoint position)
		{
			if (CaptureLayer != null) {
				CaptureLayer.Dispose ();
				CaptureLayer = null;
			}

			CaptureLayer = new CALayer ();
			CaptureLayer.Bounds = new CGRect (0, 0, CropSize.Width, CropSize.Height);
			CaptureLayer.Position = position;

//			Console.WriteLine ("starting CropLayer bounds={0}, frame={1}", CaptureLayer.Bounds, CaptureLayer.Frame);

			/* Draw the crop layer
			 * ------------------- */

			//layer.ContentsGravity = CALayer.GravityResizeAspect;
			CaptureLayer.BorderWidth = 1.5f;
			CaptureLayer.CornerRadius = 5;
			CaptureLayer.BorderColor = UIColor.Blue.CGColor;
			//layer.BackgroundColor = UIColor.Purple.CGColor;
			LiveCameraStream.Layer.AddSublayer (CaptureLayer);

			/* reset the scale to start from here
			 * ---------------------------------- */

			scale = new CGSize (1.0, 1.0);
		}
		/*************************************
		 * 
		 * OnPinchGesture
		 * 
		 * ***********************************/

		private void OnPinchGesture (UIPinchGestureRecognizer pinch)
		{
			switch (pinch.State)
			{
			case UIGestureRecognizerState.Began:

				bScaleWidth = false;
				bScaleHeight = false;

//				Console.WriteLine ("no of touches {0}", pinch.NumberOfTouches);
				for (int i = 0; i < pinch.NumberOfTouches; i++) {
					CGPoint p = pinch.LocationOfTouch (i, LiveCameraStream);
//					Console.WriteLine ("touch={0} {1}", p.X, p.Y);
				}
				if (pinch.NumberOfTouches >= 2) {
					CGPoint p1 = pinch.LocationOfTouch (0, LiveCameraStream);
					CGPoint p2 = pinch.LocationOfTouch (1, LiveCameraStream);
					double m = Math.Abs ((p2.Y - p1.Y) / (p2.X - p1.X));
//					Console.WriteLine ("slope={0}", m);

					double angle = Math.Atan (m);
					if (angle < Math.PI / 6 && angle > -(Math.PI / 6)) {
						bScaleWidth = true;
						bScaleHeight = false;
					} else if (angle > Math.PI / 3 && angle < Math.PI * 4 / 3) {
						bScaleWidth = false;
						bScaleHeight = true;
					} else {
						bScaleWidth = true;
						bScaleHeight = true;
					}

//					Console.WriteLine ("{0} {1}", bScaleWidth, bScaleHeight);	
				}

//				Console.WriteLine ("begin scale={0}", pinch.Scale);
				break;

			case UIGestureRecognizerState.Changed:
//				Console.WriteLine ("scale={0}", pinch.Scale);

				scale.Width = bScaleWidth ? pinch.Scale : (nfloat)1.0;
				scale.Height = bScaleHeight ? pinch.Scale : (nfloat)1.0;

				CGAffineTransform sc = CGAffineTransform.MakeScale (scale.Width, scale.Height);
				CaptureLayer.Transform = CATransform3D.MakeFromAffine (sc);	

				//Console.WriteLine ("croplayer frame={0}", CropLayer.Frame.ToString ());
				break;

			case UIGestureRecognizerState.Ended:
//				Console.WriteLine ("end scale=({0}, {1}. Bounds={2} Frame={3}", scale.Width, scale.Height, CaptureLayer.Bounds, CaptureLayer.Frame);
				CGPoint position = CaptureLayer.Position;
				CropSize = CaptureLayer.Frame.Size;
				CaptureLayer.RemoveFromSuperLayer ();
				InitializeCaptureLayer (position);
				break;
			}
		}

		/**********************************
		 * AuthorizeCameraUse
		 * *******************************/

		async Task AuthorizeCameraUse ()
		{
			var authorizationStatus = AVCaptureDevice.GetAuthorizationStatus (AVMediaType.Video);

			if (authorizationStatus != AVAuthorizationStatus.Authorized) {
				await AVCaptureDevice.RequestAccessForMediaTypeAsync (AVMediaType.Video);
			}
		}

		/**********************************
		 * Crop the image
		 * *******************************/

		private UIImage CropImage (UIImage sourceImage)
		{
			CGSize SourceImageSize = sourceImage.Size; 
			nfloat expand = SourceImageSize.Width / LiveCameraStream.Bounds.Width;

			/* Crop the image
			 * -------------- */

			CGRect r = CaptureLayer.Frame;

			nfloat NewWidth = (nfloat) (r.Width * expand);
			nfloat NewHeight = (nfloat) (r.Height * expand);
			nfloat NewTop = r.Top;
			NewTop *= expand;

			nfloat NewLeft = r.Left;
			NewLeft *= expand;

			CGSize sz = new CGSize (NewWidth, NewHeight);
			UIGraphics.BeginImageContext (sz);
			var context = UIGraphics.GetCurrentContext ();
			var clippedRect = new CGRect (0, 0, NewWidth, NewHeight);
			context.ClipToRect (clippedRect);
			var drawRect = new CGRect (-NewLeft, -NewTop, SourceImageSize.Width, SourceImageSize.Height);

			sourceImage.Draw (drawRect);
			var modifiedImage = UIGraphics.GetImageFromCurrentImageContext ();
			UIGraphics.EndImageContext ();

			return modifiedImage;
		}

		/************************************
		 * TakePhotoButtonTapped
		 * *********************************/

		async Task TakePhotoButtonTapped ()
		{
			var videoConnection = stillImageOutput.ConnectionFromMediaType (AVMediaType.Video);
			var sampleBuffer = await stillImageOutput.CaptureStillImageTaskAsync (videoConnection);

			var jpegImageAsNsData = AVCaptureStillImageOutput.JpegStillToNSData (sampleBuffer);
			//var jpegAsByteArray = jpegImageAsNsData.ToArray ();
			UIImage image = UIImage.LoadFromData (jpegImageAsNsData);

			image = CropImage (image);
			PhotoUpdated (image);
			SnapshotExit ();
		}

		/***************************************
		 * 
		 * FlashButtonTapped
		 * 
		 * ************************************/

		void FlashButtonTapped ()
		{
			var device = captureDeviceInput.Device;

			var err = new NSError ();
			if (device.HasFlash) 
			{
				if (device.FlashMode == AVCaptureFlashMode.On) {
					device.LockForConfiguration (out err);
					device.FlashMode = AVCaptureFlashMode.Off;
					device.UnlockForConfiguration ();
					FlashToggleButton.SetBackgroundImage (UIImage.FromFile ("FlashOff.png"), UIControlState.Normal);
				} else {
					device.LockForConfiguration (out err);
					device.FlashMode = AVCaptureFlashMode.On;
					device.UnlockForConfiguration ();
					FlashToggleButton.SetBackgroundImage (UIImage.FromFile ("FlashOn.png"), UIControlState.Normal);
				}
			}
		}


		/***********************************
		 * SetupLiveCameraStream
		 * ********************************/

		public void SetupLiveCameraStream ()
		{
			captureSession = new AVCaptureSession ();

			var viewLayer = LiveCameraStream.Layer;
			videoPreviewLayer = new AVCaptureVideoPreviewLayer (captureSession) {
				Frame = this.View.Frame
			};
			LiveCameraStream.Layer.AddSublayer (videoPreviewLayer);

			InitializeCaptureLayer (new CGPoint (((int) LiveCameraStream.Bounds.Width) >> 1, ((int) LiveCameraStream.Bounds.Height) >> 1));

			var captureDevice = AVCaptureDevice.DefaultDeviceWithMediaType (AVMediaType.Video);
			ConfigureCameraForDevice (captureDevice);
			captureDeviceInput = AVCaptureDeviceInput.FromDevice (captureDevice);
			captureSession.AddInput (captureDeviceInput);

			stillImageOutput = new AVCaptureStillImageOutput ();
			stillImageOutput.OutputSettings = NSDictionary.FromObjectAndKey (new NSString ("AVVideoCodecKey"), new NSString ("AVVideoCodecJPEG"));

			captureSession.AddOutput (stillImageOutput);
			captureSession.StartRunning ();
		}

		/*****************************
		 * 
		 * ConfigureCameraForDevice
		 * 
		 * ***************************/

		void ConfigureCameraForDevice (AVCaptureDevice device)
		{
			var error = new NSError ();
			if (device.IsFocusModeSupported (AVCaptureFocusMode.ContinuousAutoFocus)) {
				device.LockForConfiguration (out error);
				device.FocusMode = AVCaptureFocusMode.ContinuousAutoFocus;
				device.UnlockForConfiguration ();
			} else if (device.IsExposureModeSupported (AVCaptureExposureMode.ContinuousAutoExposure)) {
				device.LockForConfiguration (out error);
				device.ExposureMode = AVCaptureExposureMode.ContinuousAutoExposure;
				device.UnlockForConfiguration ();
			} else if (device.IsWhiteBalanceModeSupported (AVCaptureWhiteBalanceMode.ContinuousAutoWhiteBalance)) {
				device.LockForConfiguration (out error);
				device.WhiteBalanceMode = AVCaptureWhiteBalanceMode.ContinuousAutoWhiteBalance;
				device.UnlockForConfiguration ();
			}
		}

		/**************************************
		 * 
		 * SnapshotExit
		 * 
		 * ************************************/

		void SnapshotExit ()
		{
			this.View.RemoveGestureRecognizer (PinchGesture);

			if (CaptureLayer != null)
			{
				CaptureLayer.RemoveFromSuperLayer ();
				CaptureLayer.Dispose ();
				CaptureLayer = null;
			}
			captureSession.StopRunning ();
			captureSession.Dispose ();
			stillImageOutput.Dispose ();
			captureDeviceInput.Dispose ();
			videoPreviewLayer.Dispose ();
			PinchGesture.Dispose ();

			CropSizeUpdated (CropSize); 
		
			this.DismissViewControllerAsync (true);
		}

		partial void CancelButton_TouchUpInside (UIButton sender)
		{
			SnapshotExit ();
		}

	}
}
