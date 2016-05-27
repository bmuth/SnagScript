using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

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
		static public OptionData _OptionData = new OptionData {EmailAddress = "email address here", DoctorName = "Provider Name", CollegeNumber = "College No. here"};

		const double fHeightWidthRatioPatient = 78.0 / 183.0;
		const double fHeightWidthRatioMeds = 244.0 / 274.0;

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
			UITapGestureRecognizer DoubleTapGesture = null;
			UITapGestureRecognizer SingleTapGesture = null;

			{
				DoubleTapGesture = new UITapGestureRecognizer (() => {
					var storyboard = this.Storyboard;
					var p = (SnapshotViewController)storyboard.InstantiateViewController ("SnapshotViewController");
					p.fHeightWidthRatio = fHeightWidthRatioPatient;

					p.ModalTransitionStyle = UIModalTransitionStyle.PartialCurl;

					p.PhotoUpdated += (UIImage image) => {
						{
							imagePatient.Image = image;
						}
					};

					this.PresentViewController (p, true, null);
				});
				DoubleTapGesture.NumberOfTapsRequired = 2;
				imagePatient.AddGestureRecognizer (DoubleTapGesture);			
			}

	/*		{
				SingleTapGesture = new UITapGestureRecognizer (() => {
					var storyboard = this.Storyboard;
					var p = (EditPhotoViewController)storyboard.InstantiateViewController ("EditPhotoViewController");
					p.fHeightWidthRatio = fHeightWidthRatioPatient;

					if (imagePatient.Image != null)
					{
						p.ImageForEditing = imagePatient.Image;

						p.ModalTransitionStyle = UIModalTransitionStyle.PartialCurl;

						p.PhotoUpdated += (UIImage image) => {
							{
								imagePatient.Image = image;
							}
						};

						this.PresentViewController (p, true, null);
					}
				});
				SingleTapGesture.NumberOfTapsRequired = 1;
				SingleTapGesture.RequireGestureRecognizerToFail (DoubleTapGesture);
				imagePatient.AddGestureRecognizer (SingleTapGesture);
			}*/
		}

		/***************************
		 * WireUpMedsTap
		 * ************************/

		private void WireUpMedsTap ()
		{
			UITapGestureRecognizer DoubleTapGesture = null;
			UITapGestureRecognizer SingleTapGesture = null;

			{
				DoubleTapGesture = new UITapGestureRecognizer (() => {
					var storyboard = this.Storyboard;
					var p = (SnapshotViewController)storyboard.InstantiateViewController ("SnapshotViewController");
					p.fHeightWidthRatio = fHeightWidthRatioMeds;

					p.ModalTransitionStyle = UIModalTransitionStyle.PartialCurl;

					p.PhotoUpdated += (UIImage image) => {
						{
							imageMeds.Image = image;
						}
					};

					this.PresentViewController (p, true, null);
				});
				DoubleTapGesture.NumberOfTapsRequired = 2;
				imageMeds.AddGestureRecognizer (DoubleTapGesture);			
			}

	/*		{
				SingleTapGesture = new UITapGestureRecognizer (() => {
					var storyboard = this.Storyboard;
					var p = (EditPhotoViewController)storyboard.InstantiateViewController ("EditPhotoViewController");
					p.fHeightWidthRatio = fHeightWidthRatioMeds;

					if (imageMeds.Image != null)
					{
						p.ImageForEditing = imageMeds.Image;

						p.ModalTransitionStyle = UIModalTransitionStyle.PartialCurl;

						p.PhotoUpdated += (UIImage image) => {
							{
								imageMeds.Image = image;
								labDate.Text = DateTime.Today.ToString ("MMM dd, yyyy");
							}
						};

						this.PresentViewController (p, true, null);
					}
				});
				SingleTapGesture.NumberOfTapsRequired = 1;
				SingleTapGesture.RequireGestureRecognizerToFail (DoubleTapGesture);
				imageMeds.AddGestureRecognizer (SingleTapGesture);
			}*/
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

			WireUpPatientTap ();
			WireUpMedsTap ();
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

		partial void UIButton5_TouchUpInside (UIButton sender)
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

