using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;
using CoreImage;

namespace SnagScript
{
	partial class EnhanceController : UIViewController
	{
		public delegate void PhotoUpdatedHandler (UIImage image);
		public event PhotoUpdatedHandler PhotoUpdated;
		public delegate void ContrastUpdateHandler (float contrast, float saturation, float brightness);
		public event ContrastUpdateHandler ContrastUpdated;

		public UIImage ImageForEditing;

		private CIColorControls colorCtrls = null;
		private CIContext context = null;

		public EnhanceController (IntPtr handle) : base (handle)
		{
		}

		public override bool ShouldAutorotate ()
		{
			return true;
		}

		public override UIInterfaceOrientationMask GetSupportedInterfaceOrientations ()
		{
			return UIInterfaceOrientationMask.Portrait;
		}

		/*************************************
		 * 
		 * ViewDidLoad ()
		 * 
		 * **********************************/

		public override void ViewDidLoad ()
		{
			EditImageView.Image = ImageForEditing;
			ImageForEditing = null;


			ContrastSlider.MinValue = 0;
			ContrastSlider.MaxValue = 4;
			SaturationSlider.MinValue = 0;
			SaturationSlider.MaxValue = 1;
			BrightnessSlider.MinValue = -1;
			BrightnessSlider.MaxValue = 1;

			ContrastSlider.Value = 2.5f;
			SaturationSlider.Value = .25f;
			BrightnessSlider.Value = .17f;

			ContrastSlider.ValueChanged += Slider_ValueChanged;
			BrightnessSlider.ValueChanged += Slider_ValueChanged;
			SaturationSlider.ValueChanged += Slider_ValueChanged;

			Slider_ValueChanged (ContrastSlider, null);
		}

		void Slider_ValueChanged (object sender, EventArgs e)
		{
			UISlider sl = (UISlider)sender;
			SliderValue.Text = sl.Value.ToString ();

			// use the low-res version
			if (colorCtrls == null) {
				colorCtrls = new CIColorControls ();
				colorCtrls.Image = CIImage.FromCGImage (EditImageView.Image.CGImage);
			} 

			// re-use context for efficiency
			if (context == null) {
				context = CIContext.FromOptions (null);
			}

			colorCtrls.Brightness = BrightnessSlider.Value;
			colorCtrls.Saturation = SaturationSlider.Value;
			colorCtrls.Contrast = ContrastSlider.Value;

			ContrastUpdated (ContrastSlider.Value, SaturationSlider.Value, BrightnessSlider.Value);

			// do the transformation
			using (var outputImage = colorCtrls.OutputImage) 
			{
				var result = context.CreateCGImage (outputImage, outputImage.Extent);
				// display the result in the UIImageView
				EditImageView.Image = UIImage.FromImage (result);
			}
		}

		/**************************************
		 * 
		 * Ok button pressed
		 * 
		 * ***********************************/

		partial void OkButton_TouchUpInside (UIButton sender)
		{
			PhotoUpdated (EditImageView.Image);

			EditImageView.Dispose ();
			colorCtrls.Dispose ();
			context.Dispose ();
			this.DismissViewControllerAsync (true);

		}

		/**************************************
		 * 
		 * Cancel button pressed
		 * 
		 * ***********************************/

		partial void CancelButton_TouchUpInside (UIButton sender)
		{
			EditImageView.Dispose ();
			colorCtrls.Dispose ();
			context.Dispose ();
			this.DismissViewControllerAsync (true);
		}

	}
}
