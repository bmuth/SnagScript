using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace SnagScript
{
	partial class OptionViewController : UIViewController
	{
		bool bChanged = false;

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
		}

		/****************************************
		 * 
		 * Ok Button pressed
		 * 
		 * *************************************/

		partial void OkButton_TouchUpInside (UIButton sender)
		{
			if (bChanged)
			{
				ViewController._OptionData.DoctorName = tbProviderName.Text;
				ViewController._OptionData.CollegeNumber = tbCollegeNo.Text;
				ViewController._OptionData.EmailAddress = tbEmailAddress.Text;

				OptionsUpdated ();
			}

			this.DismissViewControllerAsync (true);
		}

		/****************************************
		 * 
		 * TextFieldChanged
		 * 
		 * *************************************/

		partial void TextFieldChanged (UITextField sender)
		{
			bChanged = true;
		}
	}
}
