using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace SnagScript
{
	partial class OptionViewController : UIViewController
	{
		public delegate void OptionsUpdatedHandler ();
		public event OptionsUpdatedHandler OptionsUpdated;

		public OptionViewController (IntPtr handle) : base (handle)
		{
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			tbProviderName.Text = ViewController._OptionData.DoctorName;
			tbCollegeNo.Text = ViewController._OptionData.CollegeNumber;
			tbEmailAddress.Text = ViewController._OptionData.EmailAddress;

			tbProviderName.BecomeFirstResponder ();

			tbProviderName.EditingDidEndOnExit += (object sender, EventArgs e) => 
			{
				tbProviderName.ResignFirstResponder ();
			};
			tbCollegeNo.EditingDidEndOnExit += (object sender, EventArgs e) => {
				tbCollegeNo.ResignFirstResponder ();
			};
			tbEmailAddress.EditingDidEndOnExit += (object sender, EventArgs e) => {
				tbEmailAddress.ResignFirstResponder ();
			};

		}

		/****************************************
		 * 
		 * Ok Button pressed
		 * 
		 * *************************************/

		partial void OkButton_TouchUpInside (UIButton sender)
		{
			ViewController._OptionData.DoctorName = tbProviderName.Text;
			ViewController._OptionData.CollegeNumber = tbCollegeNo.Text;
			ViewController._OptionData.EmailAddress = tbEmailAddress.Text;

			OptionsUpdated ();

			this.DismissViewControllerAsync (true);
		}

		/***************************************
		 * 
		 * Cancel button pressed
		 * 
		 * ************************************/

		partial void CancelButton_TouchUpInside (UIButton sender)
		{
			this.DismissViewControllerAsync (true);
		}

	}
}
