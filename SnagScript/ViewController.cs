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

		public ViewController (IntPtr handle) : base (handle)
		{
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			LoadPersistedData ();

			labCollegeNo.Text = _OptionData.CollegeNumber;
			labProviderName.Text = _OptionData.DoctorName;
		}

		private void LoadPersistedData ()
		{
			var documentsPath = Environment.GetFolderPath (Environment.SpecialFolder.Personal);
			var filePath = Path.Combine (documentsPath, "SnagScript.xml");
			// Create an instance of the XmlSerializer specifying type and namespace.
			XmlSerializer serializer = new XmlSerializer (typeof (OptionData));

			try
			{
				FileStream fs = new FileStream (filePath, FileMode.Open);
				XmlReader reader = XmlReader.Create (fs);

				// Use the Deserialize method to restore the object's state.
				_OptionData = (OptionData) serializer.Deserialize (reader);
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
			};

			this.PresentViewController (p, true, null);		}
	}
}

