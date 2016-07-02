using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using CoreGraphics;
using CoreImage;
using MessageUI;
using UIKit;

namespace SnagScript
{
	[Serializable]
	public class OptionData
	{
		public string EmailAddress;
		public string DoctorName;
		public string CollegeNumber;
		public float BrightnessValue;
		public float ContrastValue;
		public float SaturationValue;
		public double SmallCropSizeHeight;
		public double SmallCropSizeWidth;
		public double LargeCropSizeHeight;
		public double LargeCropSizeWidth;
	}

	public partial class ViewController : UIViewController
	{
		private UITapGestureRecognizer SingleTapPatientCalloutGesture = null;
		private UITapGestureRecognizer SingleTapMedsCalloutGesture = null;
		private UITapGestureRecognizer SingleTapPatientImageGesture = null;
		private UITapGestureRecognizer SingleTapMedsImageGesture = null;

		private UIImage imagePatientOriginal = null;
		private UIImage imagePatientEnhanced = null;
		private UIImage imageMedsOriginal = null;
		private UIImage imageMedsEnhanced = null;

		static public OptionData _OptionData = new OptionData {EmailAddress = "email address here", DoctorName = "Provider Name", CollegeNumber = "College No. here"};

		const double fHeightWidthRatioPatient = 78.0 / 183.0;
		const double fHeightWidthRatioMeds = 244.0 / 274.0;

		private UIColor contrastBlue = UIColor.FromRGBA (0.000f, 0.235f, 1.000f, 0.617f);
		private bool bIfContrastPatient = false;
		private bool bIfContrastMeds = false;

		public ViewController (IntPtr handle) : base (handle)
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

		/****************************
		 * WireUpPatientTap
		 * *************************/

		private void WireUpPatientTap ()
		{
			SingleTapPatientImageGesture = new UITapGestureRecognizer (() => {
				if (PatientCalloutView.Hidden == false)
				{
					HidePatientCallout ();
					return;
				}

				TakePhotoPatientID ();
			});

			SingleTapPatientImageGesture.NumberOfTapsRequired = 1;
			imagePatient.AddGestureRecognizer (SingleTapPatientImageGesture);
		}

		/**********************************
		 * 
		 * TakePhotoPatientID
		 * 
		 * *******************************/

		private void TakePhotoPatientID ()
		{
			var storyboard = this.Storyboard;
			var p = (SnapshotViewController) storyboard.InstantiateViewController ("SnapshotViewController");
			p.fHeightWidthRatio = fHeightWidthRatioPatient;
			p.CropSize = new CGSize (_OptionData.SmallCropSizeWidth, _OptionData.SmallCropSizeHeight);

			if (imagePatient.Image != null)
			{
				ShowPatientCallout ();
			}
			else
			{
				//p.ModalTransitionStyle = UIModalTransitionStyle.PartialCurl;

				p.CropSizeUpdated += (CGSize sz) => {
					_OptionData.SmallCropSizeWidth = sz.Width;
					_OptionData.SmallCropSizeHeight = sz.Height;
					SavePersistedData ();
				};

				p.PhotoUpdated += (UIImage image) => {
					{
						imagePatientOriginal = image;
						imagePatient.Image = image;
						labTipPatientID.Hidden = true;
						bIfContrastPatient = false;
						PatientCalloutView.Image = PatientIDCallout.ImageOfPatientCallout (UIColor.Black);
//						Console.WriteLine ("Contrast false");
					}
				};

//				imagePatient.RemoveGestureRecognizer (SingleTapPatientImageGesture);
//				imageMeds.RemoveGestureRecognizer (SingleTapMedsImageGesture);
				//var p2 = (SnapshotViewController2) storyboard.InstantiateViewController ("SnapshotViewController2");
				this.PresentViewController (p, true, null);
			}
		}

		/**********************************
		 * 
		 * TakePhotoMeds
		 * 
		 * *******************************/

		private void TakePhotoMeds ()
		{
			var storyboard = this.Storyboard;
			var p = (SnapshotViewController) storyboard.InstantiateViewController ("SnapshotViewController");
			p.fHeightWidthRatio = fHeightWidthRatioMeds;
			p.CropSize = new CGSize (_OptionData.LargeCropSizeWidth, _OptionData.LargeCropSizeHeight);

			if (imageMeds.Image != null)
			{
        				ShowMedsCallout ();
			}
			else
			{
				//p.ModalTransitionStyle = UIModalTransitionStyle.PartialCurl;

				p.CropSizeUpdated += (CGSize sz) => {
					_OptionData.LargeCropSizeWidth = sz.Width;
					_OptionData.LargeCropSizeHeight = sz.Height;
					SavePersistedData ();
				};
					
				p.PhotoUpdated += (UIImage image) => {
					{
						imageMedsOriginal = image;
						imageMeds.Image = image;
						labTipMed.Hidden = true;
						bIfContrastMeds = false;
						MedsCalloutView.Image = MedsCallout.ImageOfMedsCallout (UIColor.Black);
//						Console.WriteLine ("Contrast false");
					}
				};

				this.PresentViewController (p, true, null);
			}
		}

		/*******************************************
		 * 
		 * ShowPatientCallout
		 * 
		 * ****************************************/

		private void ShowPatientCallout ()
		{
			if (MedsCalloutView.Hidden == false) {
				HideMedsCallout ();
			}

			PatientCalloutView.Hidden = false;
			PatientCalloutView.AddGestureRecognizer (SingleTapPatientCalloutGesture);
		}

		/*******************************************
		 * 
		 * HidePatientCallout
		 * 
		 * ****************************************/

		private void HidePatientCallout ()
		{
			PatientCalloutView.RemoveGestureRecognizer (SingleTapPatientCalloutGesture);
			PatientCalloutView.Hidden = true;
		}
		/*******************************************
		 * 
		 * ShowMedsCallout
		 * 
		 * ****************************************/

		private void ShowMedsCallout ()
		{
			if (PatientCalloutView.Hidden == false) {
				HidePatientCallout ();
			}

			MedsCalloutView.Hidden = false;
			MedsCalloutView.AddGestureRecognizer (SingleTapMedsCalloutGesture);
		}

		/*******************************************
		 * 
		 * HideMedsCallout
		 * 
		 * ****************************************/

		private void HideMedsCallout ()
		{
			MedsCalloutView.RemoveGestureRecognizer (SingleTapMedsCalloutGesture);
			MedsCalloutView.Hidden = true;
		}

		/***************************
		 * WireUpMedsTap
		 * ************************/

		private void WireUpMedsTap ()
		{
			SingleTapMedsImageGesture = new UITapGestureRecognizer (() => 
			{
				if (MedsCalloutView.Hidden == false)
					{
						HideMedsCallout ();
						return;
					}

				TakePhotoMeds ();
			});

			SingleTapMedsImageGesture.NumberOfTapsRequired = 1;
			imageMeds.AddGestureRecognizer (SingleTapMedsImageGesture);
		}

		/*******************************************
		 * 
		 * Apply contrast, brightness, saturation
		 * 
		 * ****************************************/

		private UIImage ApplyContrast (UIImage original)
		{
			CIColorControls colorCtrls = new CIColorControls ();

			colorCtrls.Image = CIImage.FromCGImage (original.CGImage);
			CIContext context = CIContext.FromOptions (null);

			colorCtrls.Brightness = _OptionData.BrightnessValue;
			colorCtrls.Saturation = _OptionData.SaturationValue;
			colorCtrls.Contrast = _OptionData.ContrastValue;

			// do the transformation
			UIImage contrastImage = null;
			using (var outputImage = colorCtrls.OutputImage) 
			{
				var result = context.CreateCGImage (outputImage, outputImage.Extent);
				// display the result in the UIImageView
				contrastImage = UIImage.FromImage (result);
			}
			//context.Dispose ();
			//colorCtrls.Dispose ();
			return contrastImage;
		}

		/***************************************
		 * 
		 * ViewDidLoad ()
		 * 
		 * ************************************/

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			LoadPersistedData ();

			labDoctorNo.Text = _OptionData.CollegeNumber;
			labDoctorName.Text = _OptionData.DoctorName;

			WireUpPatientTap ();
			WireUpMedsTap ();	

			SingleTapPatientCalloutGesture = new UITapGestureRecognizer (() => {
				CGPoint pt = SingleTapPatientCalloutGesture.LocationInView (PatientCalloutView);
//				Console.WriteLine ("PatientCallout touched at {0}", pt);

				if (pt.X < PatientCalloutView.Bounds.Width / 3)
				{
//					Console.WriteLine ("patient Contrast");
					if (!bIfContrastPatient)
					{
						PatientCalloutView.Image = PatientIDCallout.ImageOfPatientCallout (contrastBlue);  

						if (imagePatientEnhanced == null)
						{
							imagePatientEnhanced = ApplyContrast (imagePatientOriginal);
						}
						imagePatient.Image = imagePatientEnhanced;
					}
					else
					{
						PatientCalloutView.Image = PatientIDCallout.ImageOfPatientCallout (UIColor.Black);
						imagePatient.Image = imagePatientOriginal;
					}
					bIfContrastPatient = !bIfContrastPatient;
//					Console.WriteLine ("patient contrast is now {0}", bIfContrastPatient);
				}
				else if (pt.X > PatientCalloutView.Bounds.Width * 2.0 / 3.0)
				{
//					Console.WriteLine ("Details");
					var storyboard = this.Storyboard;
					var p = (EnhanceController) storyboard.InstantiateViewController ("EnhanceController");

					p.ImageForEditing = imagePatientOriginal;

					p.PhotoUpdated += (image) =>  
					{
						imagePatientEnhanced = image;
						imagePatient.Image = image;
						bIfContrastPatient = true;
						PatientCalloutView.Image = PatientIDCallout.ImageOfPatientCallout (contrastBlue);  
						Console.WriteLine ("patient Contrast now true");

						HidePatientCallout ();
						SavePersistedData ();
					};

					p.ContrastUpdated += (contrast, saturation, brightness) => 
					{
						_OptionData.ContrastValue = contrast;
						_OptionData.SaturationValue = saturation;
						_OptionData.BrightnessValue = brightness;
					};

					this.PresentViewController (p, true, null);	

				}
				else
				{
//					Console.WriteLine ("Camera");
					HidePatientCallout ();
					if (imagePatientEnhanced != null)
					{
						imagePatientEnhanced.Dispose ();
					}
					if (imagePatientOriginal != null)
					{
						imagePatientOriginal.Dispose ();
					}
					imagePatientEnhanced = null;
					imagePatientOriginal = null;
					imagePatient.Image.Dispose ();
					imagePatient.Image = null;

					TakePhotoPatientID ();
				}
			});

			PatientCalloutView.Image = PatientIDCallout.ImageOfPatientCallout (UIColor.Black);      
			bIfContrastPatient = false;
//			Console.WriteLine ("patient contrast now false");

			/* Now, same thing for Meds Tap Gesture
			 * ------------------------------------ */

			SingleTapMedsCalloutGesture = new UITapGestureRecognizer (() => {
				CGPoint pt = SingleTapMedsCalloutGesture.LocationInView (MedsCalloutView);
//				Console.WriteLine ("MedsCallout touched at {0}", pt);

				if (pt.X < MedsCalloutView.Bounds.Width / 3)
				{
//					Console.WriteLine ("Contrast");
					if (!bIfContrastMeds)
					{
						MedsCalloutView.Image = MedsCallout.ImageOfMedsCallout (contrastBlue);  

						if (imageMedsEnhanced == null)
						{
							imageMedsEnhanced = ApplyContrast (imageMedsOriginal);
						}
						imageMeds.Image = imageMedsEnhanced;
					}
					else
					{
						MedsCalloutView.Image = MedsCallout.ImageOfMedsCallout (UIColor.Black);
						imageMeds.Image = imageMedsOriginal;
					}
					bIfContrastMeds = !bIfContrastMeds;
//					Console.WriteLine ("meds contrast is now {0}", bIfContrastMeds);
				}
				else if (pt.X > PatientCalloutView.Bounds.Width * 2.0 / 3.0)
				{
//					Console.WriteLine ("Details");
					var storyboard = this.Storyboard;
					var p = (EnhanceController) storyboard.InstantiateViewController ("EnhanceController");

					p.ImageForEditing = imageMedsOriginal;

					p.PhotoUpdated += (image) =>  
					{
						imageMedsEnhanced = image;
						imageMeds.Image = image;
						bIfContrastPatient = true;
						MedsCalloutView.Image = MedsCallout.ImageOfMedsCallout (contrastBlue);  
//						Console.WriteLine ("meds Contrast now true");

						HideMedsCallout ();
						SavePersistedData ();
					};

					p.ContrastUpdated += (contrast, saturation, brightness) => 
					{
						_OptionData.ContrastValue = contrast;
						_OptionData.SaturationValue = saturation;
						_OptionData.BrightnessValue = brightness;
					};

					this.PresentViewController (p, true, null);	

				}
				else
				{
//					Console.WriteLine ("Camera");
					HideMedsCallout ();
					imageMedsEnhanced.Dispose ();
					imageMedsOriginal.Dispose ();
					imageMedsEnhanced = null;
					imageMedsOriginal = null;
					imageMeds.Image.Dispose ();
					imageMeds.Image = null;

					TakePhotoMeds ();
				}
			});

			MedsCalloutView.Image = MedsCallout.ImageOfMedsCallout (UIColor.Black);      
			bIfContrastMeds = false;
//			Console.WriteLine ("meds contrast now false");
		}

		/************************************************
		 * 
		 * LoadPersistData ()
		 * 
		 * *********************************************/

		private void LoadPersistedData ()
		{
			var documentsPath = Environment.GetFolderPath (Environment.SpecialFolder.Personal);
			var filePath = Path.Combine (documentsPath, "SnagScript.xml");
			// Create an instance of the XmlSerializer specifying type and namespace.
			XmlSerializer serializer = new XmlSerializer (typeof (OptionData));

			try
			{
				using (FileStream fs = new FileStream (filePath, FileMode.Open))
				{
					XmlReader reader = XmlReader.Create (fs);

					// Use the Deserialize method to restore the object's state.
					_OptionData = (OptionData) serializer.Deserialize (reader);
				}
			}
			catch (Exception e) {
				Console.WriteLine ("Failed to load SnagScript.xml. {0}", e.Message);
			}
		}

		public override void DidReceiveMemoryWarning ()
		{
			base.DidReceiveMemoryWarning ();
			// Release any cached data, images, etc that aren't in use.
		}

		/*******************************************
	 	* 
	 	* Options Button pressed
	 	* 
	 	* *****************************************/

		partial void ButtonOptions_TouchUpInside (UIButton sender)
		{
			var storyboard = this.Storyboard;
			var p = (OptionViewController)storyboard.InstantiateViewController ("OptionViewController");


			p.OptionsUpdated += () => 
			{
				SavePersistedData ();
				labDoctorName.Text = _OptionData.DoctorName;
				labDoctorNo.Text = _OptionData.CollegeNumber;
			};

			this.PresentViewController (p, true, null);	
		}

		/*******************************************
		 * 
		 * SavePersistedData
		 * 
		 * ****************************************/

		private void SavePersistedData ()
		{
			var documentsPath = Environment.GetFolderPath (Environment.SpecialFolder.Personal);
			var filePath = Path.Combine (documentsPath, "SnagScript.xml");

			XmlSerializer ser = new XmlSerializer (typeof (OptionData));	
			using (TextWriter writer = new StreamWriter (filePath))
			{
				ser.Serialize (writer, _OptionData);
				writer.Close ();
			}
		}

		/******************************************
		 * 
		 * ButtonEmail clicked
		 * 
		 * ***************************************/

		partial void ButtonEmail_TouchUpInside (UIButton sender)
		{
			if (imageMeds.Image == null || imagePatient.Image == null)
			{
				AlertViewController.PresentOKAlert ("Not ready", "The prescription is incomplete.", this);
				return;
			}

			UIImage script = UIImage.FromFile ("PrescriptionTemplate3.png");
			CGSize sz = script.Size;

			var colorSpace = CoreGraphics.CGColorSpace.CreateDeviceRGB ();

			/* create a bitmap that roughly is the shape of PrescriptionTemplate3.png
			 * ----------------------------------------------------------------------
			 * but has the same aspect ratio of imageViewScript */


			nint BitmapWidth = (nint) (imageviewScript.Bounds.Width * ScaleUpFactor);
			nint BitmapHeight = (nint) (imageviewScript.Bounds.Height * ScaleUpFactor);

			var context = new CGBitmapContext(null, BitmapWidth, BitmapHeight, 8, 0, colorSpace, CGImageAlphaInfo.PremultipliedFirst);


			/*	UIImage transformedImage; //Your new image
			UIGraphics.BeginImageContext (imagePatient.Frame.Size);
			imagePatientView.DrawViewHierarchy (imagePatient.Frame, true);
			transformedImage = UIGraphics.GetImageFromCurrentImageContext ();
			UIGraphics.EndImageContext ();*/

			/* Figure out the offset
			* ---------------------*/

			nfloat fLeft = (nfloat) ((BitmapWidth - sz.Width) / 2.0);
			nfloat fBottom = (nfloat) ((BitmapHeight - sz.Height) / 2.0);

			context.DrawImage (new CGRect (fLeft, fBottom, sz.Width, sz.Height), script.CGImage);

			// debug - fill imagePatient

/*			{
				CGRect rectangle = MapRect (imagePatient.Frame);
				context.SetFillColor (UIColor.Red.CGColor);
				context.SetStrokeColor (UIColor.Blue.CGColor);
				context.FillRect (rectangle);
				context.StrokeRect (rectangle);

				rectangle = MapRect (imageMeds.Frame);
				context.FillRect (rectangle);

				context.StrokeRectWithWidth (rectangle, 3);
			}*/

			/* Now draw patient id
			 * ------------------- */

			{
				UIImage patient = imagePatient.Image;

				nfloat ScaledPatientHeight;
				nfloat ScaledPatientWidth;

				if (patient.Size.Width / patient.Size.Height > imagePatient.Bounds.Width / imagePatient.Bounds.Height)
				{
					/* must be some space above and below image
					 * ---------------------------------------- */

					ScaledPatientHeight = imagePatient.Bounds.Width / patient.Size.Width * patient.Size.Height;
					ScaledPatientWidth = imagePatient.Bounds.Width;
				}
				else
				{
					/* must be space left and right of image
					 * ------------------------------------- */

					ScaledPatientHeight = imagePatient.Bounds.Height;
					ScaledPatientWidth = imagePatient.Bounds.Height / patient.Size.Height * patient.Size.Width;
				}

				nfloat FromLeft = imagePatient.Frame.Left;
				FromLeft += (nfloat) ((imagePatient.Bounds.Width - ScaledPatientWidth) / 2.0);


				nfloat FromTop = imagePatient.Frame.Top;
				FromTop += (nfloat) ((imagePatient.Bounds.Height - ScaledPatientHeight) / 2.0);

				CGRect r = MapRect (new CGRect (FromLeft, FromTop, ScaledPatientWidth, ScaledPatientHeight));

/*				context.SetFillColor (UIColor.Yellow.CGColor);
				context.SetStrokeColor (UIColor.Black.CGColor);
				context.FillRect (r);
				context.StrokeRectWithWidth (r, 3);*/

				context.DrawImage (new CGRect (r.Left, r.Top, ScaledPatientWidth * ScaleUpFactor, ScaledPatientHeight * ScaleUpFactor), patient.CGImage);
			}
			{
				UIImage meds = imageMeds.Image;

				nfloat ScaledMedsHeight;
				nfloat ScaledMedsWidth;

				if (meds.Size.Width / meds.Size.Height > imageMeds.Bounds.Width / imageMeds.Bounds.Height)
				{
					/* must be some space above and below image
					 * ---------------------------------------- */

					ScaledMedsHeight = imageMeds.Bounds.Width / meds.Size.Width * meds.Size.Height;
					ScaledMedsWidth = imageMeds.Bounds.Width;
				}
				else
				{
					/* must be space left and right of image
					 * ------------------------------------- */

					ScaledMedsHeight = imageMeds.Bounds.Height;
					ScaledMedsWidth = imageMeds.Bounds.Height / meds.Size.Height * meds.Size.Width;
				}

				nfloat FromLeft = imageMeds.Frame.Left;
				FromLeft += (nfloat) ((imageMeds.Bounds.Width - ScaledMedsWidth) / 2.0);

				nfloat FromTop = imageMeds.Frame.Top;
				FromTop += (nfloat) ((imageMeds.Bounds.Height - ScaledMedsHeight) / 2.0);

				CGRect r = MapRect (new CGRect (FromLeft, FromTop, ScaledMedsWidth, ScaledMedsHeight));

/*				context.SetFillColor (UIColor.Yellow.CGColor);
				context.SetStrokeColor (UIColor.Black.CGColor);
				context.FillRect (r);
				context.StrokeRectWithWidth (r, 3);*/

				context.DrawImage (new CGRect (r.Left, r.Top, ScaledMedsWidth * ScaleUpFactor, ScaledMedsHeight * ScaleUpFactor), meds.CGImage);
			}

			context.SelectFont ("Helvetica", 50, CGTextEncoding.MacRoman);
			context.SetTextDrawingMode (CGTextDrawingMode.FillStroke);
			context.SetStrokeColor (UIColor.Black.CGColor);
			context.SetFillColor (UIColor.Black.CGColor);

			{
				CGRect r = labSignature.Frame;
				CGPoint p = MapPoint (r);
				context.ShowTextAtPoint (p.X, p.Y, labSignature.Text);
			}

			{
				CGRect r = labDoctorName.Frame;
				CGPoint p = MapPoint (r);


				//draw in invisible mode, get the size then subtract from width of rect to get left hand x of the text
				context.SetTextDrawingMode(CGTextDrawingMode.Invisible);
				context.ShowTextAtPoint (p.X, p.Y, labDoctorName.Text);

				//Then get the position of the text and set the mode back to visible:            
				CGPoint pt = context.TextPosition;      

				nfloat FromLeft = labDoctorName.Frame.Left + labDoctorName.Frame.Width - imageviewScript.Frame.Left;
				FromLeft *= ScaleUpFactor;

				//Draw at new position
				context.SetTextDrawingMode(CGTextDrawingMode.Fill);
				context.ShowTextAtPoint (p.X + (FromLeft - pt.X), p.Y, labDoctorName.Text);
			}

			{
				CGRect r = labDoctorNo.Frame;
				CGPoint p = MapPoint (r);

				//draw in invisible mode, get the size then subtract from width of rect to get left hand x of the text
				context.SetTextDrawingMode(CGTextDrawingMode.Invisible);
				context.ShowTextAtPoint (p.X, p.Y, labDoctorNo.Text);

				//Then get the position of the text and set the mode back to visible:            
				CGPoint pt = context.TextPosition;      

				nfloat FromLeft = labDoctorNo.Frame.Left + labDoctorNo.Frame.Width - imageviewScript.Frame.Left;
				FromLeft *= ScaleUpFactor;

				//Draw at new position
				context.SetTextDrawingMode(CGTextDrawingMode.Fill);
				context.ShowTextAtPoint (p.X + (FromLeft - pt.X), p.Y, labDoctorNo.Text);
			}

			{
				CGPoint p = MapPoint (labDate.Frame);

				//draw in invisible mode, get the size then subtract from width of rect to get left hand x of the text
				context.SetTextDrawingMode(CGTextDrawingMode.Invisible);
				context.ShowTextAtPoint (p.X, p.Y, labDate.Text);

				//Then get the position of the text and set the mode back to visible:            
				CGPoint pt = context.TextPosition;      

				nfloat FromLeft = labDate.Frame.Left + labDate.Frame.Width - imageviewScript.Frame.Left;
				FromLeft *= ScaleUpFactor;

				//Draw at new position
				context.SetTextDrawingMode(CGTextDrawingMode.Fill);
				context.ShowTextAtPoint (p.X + (FromLeft - pt.X), p.Y, labDate.Text);
			}

			var image = new UIImage(context.ToImage(), 1.0f, UIImageOrientation.Up);

			context.Dispose();
			colorSpace.Dispose();
			script.Dispose ();


			// Create an email
			var _mailController = new MFMailComposeViewController();
			_mailController.SetToRecipients(new[] {_OptionData.EmailAddress});
			_mailController.SetSubject("Send prescription for printing");
			_mailController.SetMessageBody("Copyright 2016 Dr. B.Muth", false);

			// Add the screenshot as an attachment
			_mailController.AddAttachmentData(image.AsPNG(),"image/png", "Prescription.png");

			// Handle the action to take when the user completes sending the email
			_mailController.Finished += ( object s, MFComposeResultEventArgs args) => {
				System.Console.WriteLine (args.Result.ToString ());
				args.Controller.DismissViewController (true, null);
			};

			// Show the email view
			PresentViewController (_mailController, true, null);
		}

		/****************************
		 * 
		 * MapPoint
		 * 
		 * *************************/

		private CGPoint MapPoint (CGRect frame)
		{
			nfloat FromLeft = frame.Left - imageviewScript.Frame.Left;
			FromLeft *= ScaleUpFactor;

			nfloat FromTop = frame.Top - imageviewScript.Frame.Top;
			nfloat FromBottom = imageviewScript.Frame.Height - FromTop;
			FromBottom -= frame.Height;
			FromBottom *= ScaleUpFactor;

			return new CGPoint (FromLeft, FromBottom);
		}

		/********************************
		 * 
		 * MapRect
		 * 
		 * ******************************/

		private CGRect MapRect (CGRect frame)
		{
			CGPoint pt = MapPoint (frame);
			CGSize sz = new CGSize (frame.Size.Width * ScaleUpFactor, frame.Size.Height * ScaleUpFactor);
			return new CGRect (pt, sz);
		}

		/********************************************************
		 * 
		 * ScaleUpFactor
		 * 
		 * *****************************************************/

		private nfloat _scaleUpFactor = 0.0f;

		private nfloat ScaleUpFactor {
			get {
				if (_scaleUpFactor > 0.0f) {
					return _scaleUpFactor;
				} else {
					UIImage script = UIImage.FromFile ("PrescriptionTemplate3.png");
					CGSize sz = script.Size;

					nfloat scaley = sz.Height / imageviewScript.Bounds.Height;
					nfloat scalex = sz.Width / imageviewScript.Bounds.Width;

					_scaleUpFactor = (nfloat) Math.Max (scalex, scaley);

					script.Dispose ();

					return _scaleUpFactor;
				}
			}
		}

	}
}

