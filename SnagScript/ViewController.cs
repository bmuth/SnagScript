using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using CoreGraphics;

using UIKit;

namespace SnagScript
{
	[Serializable]
	public class OptionData
	{
		public string EmailAddress;
		public string DoctorName;
		public string CollegeNumber;
	}

	public partial class ViewController : UIViewController
	{
		private UITapGestureRecognizer SingleTapPatientCalloutGesture = null;
		private UITapGestureRecognizer SingleTapMedsCalloutGesture = null;
		private UITapGestureRecognizer SingleTapPatientImageGesture = null;
		private UITapGestureRecognizer SingleTapMedsImageGesture = null;


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
				/*						p.ImageForEditing = imagePatient.Image;

					p.ModalTransitionStyle = UIModalTransitionStyle.PartialCurl;

					p.PhotoUpdated += (UIImage image) => {
						{
							imagePatient.Image = image;
						}
					};

					this.PresentViewController (p, true, null);*/
			}
			else
			{
				p.ModalTransitionStyle = UIModalTransitionStyle.PartialCurl;

				p.PhotoUpdated += (UIImage image) => {
					{
						imagePatient.Image = image;
						labTipPatientID.Hidden = true;
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
		}

		/************************************************
		 * 
		 * ViewDidLayoutSubviews
		 * 
		 * *********************************************/

		public override void ViewDidLayoutSubviews()
		{
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
					}
					else
					{
						PatientCalloutView.Image = PatientIDCallout.ImageOfPatientCallout (UIColor.Black);
					}
					bIfContrastPatient = !bIfContrastPatient;
				}
				else if (pt.X > PatientCalloutView.Bounds.Width * 2.0 / 3.0)
				{
					Console.WriteLine ("Details");
				}
				else
				{
					Console.WriteLine ("Camera");
					HidePatientCallout ();
					TakePhotoPatientID ();
				}
					
				if (PatientCalloutView.Bounds.Contains (pt))
				{
					Console.WriteLine ("point is contained in PatientCallout");
				}
				else
				{
					//HidePatientCallout ();
				}
			});

			PatientCalloutView.Image = PatientIDCallout.ImageOfPatientCallout (UIColor.Black);      
			;
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
				var documentsPath = Environment.GetFolderPath (Environment.SpecialFolder.Personal);
				var filePath = Path.Combine (documentsPath, "SnagScript.xml");

				XmlSerializer ser = new XmlSerializer (typeof (OptionData));	
				using (TextWriter writer = new StreamWriter (filePath))
				{
					ser.Serialize (writer, _OptionData);
					writer.Close ();
				}

				labProviderName.Text = _OptionData.DoctorName;
				labCollegeNo.Text = _OptionData.CollegeNumber;
			};

			this.PresentViewController (p, true, null);	
		}
	}
}

