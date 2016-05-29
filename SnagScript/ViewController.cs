using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using CoreGraphics;
using CoreImage;

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
	}

	public partial class ViewController : UIViewController
	{
		private UITapGestureRecognizer SingleTapPatientCalloutGesture = null;
		private UITapGestureRecognizer SingleTapMedsCalloutGesture = null;
		private UITapGestureRecognizer SingleTapPatientImageGesture = null;
		private UITapGestureRecognizer SingleTapMedsImageGesture = null;

		private UIImage imagePatientOriginal = null;
		private UIImage imagePatientEnhanced = null;

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

			if (imagePatient.Image != null)
			{
				ShowPatientCallout ();
			}
			else
			{
				p.ModalTransitionStyle = UIModalTransitionStyle.PartialCurl;

				p.PhotoUpdated += (UIImage image) => {
					{
						imagePatientOriginal = image;
						imagePatient.Image = image;
						labTipPatientID.Hidden = true;
						bIfContrastPatient = false;
						PatientCalloutView.Image = PatientIDCallout.ImageOfPatientCallout (UIColor.Black);
						Console.WriteLine ("Contrast false");
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

		/***************************
		 * WireUpMedsTap
		 * ************************/

		private void WireUpMedsTap ()
		{
			SingleTapMedsImageGesture = new UITapGestureRecognizer (() => {
				if (PatientCalloutView.Hidden == false)
				{
					HidePatientCallout ();
					return;
				}
					
				var storyboard = this.Storyboard;
				var p = (SnapshotViewController)storyboard.InstantiateViewController ("SnapshotViewController");
				p.fHeightWidthRatio = fHeightWidthRatioMeds;

				if (imageMeds.Image != null)
				{
/*						p.ImageForEditing = imageMeds.Image;

					p.ModalTransitionStyle = UIModalTransitionStyle.PartialCurl;

					p.PhotoUpdated += (UIImage image) => {
						{
							imageMeds.Image = image;
							labDate.Text = DateTime.Today.ToString ("MMM dd, yyyy");
						}
					};

					this.PresentViewController (p, true, null);*/
				}
				else
				{
					p.ModalTransitionStyle = UIModalTransitionStyle.PartialCurl;

					p.PhotoUpdated += (UIImage image) => {
						{
							imageMeds.Image = image;
							labDate.Text = DateTime.Today.ToString ("MMM dd, yyyy");
							labTipMed.Hidden = true;
						}
					};

					this.PresentViewController (p, true, null);					}
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

			labCollegeNo.Text = _OptionData.CollegeNumber;
			labProviderName.Text = _OptionData.DoctorName;

			base.ViewDidLayoutSubviews();

			WireUpPatientTap ();
			WireUpMedsTap ();	

			SingleTapPatientCalloutGesture = new UITapGestureRecognizer (() => {
				CGPoint pt = SingleTapPatientCalloutGesture.LocationInView (PatientCalloutView);
				Console.WriteLine ("PatientCallout touched at {0}", pt);

				if (pt.X < PatientCalloutView.Bounds.Width / 3)
				{
					Console.WriteLine ("Contrast");
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
					Console.WriteLine ("contrast is now {0}", bIfContrastPatient);
				}
				else if (pt.X > PatientCalloutView.Bounds.Width * 2.0 / 3.0)
				{
					Console.WriteLine ("Details");
					var storyboard = this.Storyboard;
					var p = (EnhanceController) storyboard.InstantiateViewController ("EnhanceController");

					p.ImageForEditing = imagePatientOriginal;

					p.PhotoUpdated += (image) =>  
					{
						imagePatientEnhanced = image;
						imagePatient.Image = image;
						bIfContrastPatient = true;
						PatientCalloutView.Image = PatientIDCallout.ImageOfPatientCallout (contrastBlue);  
						Console.WriteLine ("Contrast now true");

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
					Console.WriteLine ("Camera");
					HidePatientCallout ();
					imagePatientEnhanced.Dispose ();
					imagePatientOriginal.Dispose ();
					imagePatientEnhanced = null;
					imagePatientOriginal = null;
					imagePatient.Image.Dispose ();
					imagePatient.Image = null;

					TakePhotoPatientID ();
				}
			});

			PatientCalloutView.Image = PatientIDCallout.ImageOfPatientCallout (UIColor.Black);      
			bIfContrastPatient = false;
			Console.WriteLine ("contrast now false");

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
				labProviderName.Text = _OptionData.DoctorName;
				labCollegeNo.Text = _OptionData.CollegeNumber;
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
	}
}

