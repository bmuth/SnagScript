// WARNING
//
// This file has been generated automatically by Xamarin Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace SnagScript
{
	[Register ("ViewController")]
	partial class ViewController
	{
		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIButton buttonEmail { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIButton buttonOptions { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIImageView imageMeds { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIImageView imagePatient { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIImageView imageviewScript { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UILabel labDate { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UILabel labDoctorName { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UILabel labDoctorNo { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UILabel labSignature { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UILabel labTipMed { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UILabel labTipPatientID { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIImageView MedsCalloutView { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIImageView PatientCalloutView { get; set; }

		[Action ("ButtonEmail_TouchUpInside:")]
		[GeneratedCode ("iOS Designer", "1.0")]
		partial void ButtonEmail_TouchUpInside (UIButton sender);

		[Action ("ButtonOptions_TouchUpInside:")]
		[GeneratedCode ("iOS Designer", "1.0")]
		partial void ButtonOptions_TouchUpInside (UIButton sender);

		void ReleaseDesignerOutlets ()
		{
			if (buttonEmail != null) {
				buttonEmail.Dispose ();
				buttonEmail = null;
			}
			if (buttonOptions != null) {
				buttonOptions.Dispose ();
				buttonOptions = null;
			}
			if (imageMeds != null) {
				imageMeds.Dispose ();
				imageMeds = null;
			}
			if (imagePatient != null) {
				imagePatient.Dispose ();
				imagePatient = null;
			}
			if (imageviewScript != null) {
				imageviewScript.Dispose ();
				imageviewScript = null;
			}
			if (labDate != null) {
				labDate.Dispose ();
				labDate = null;
			}
			if (labDoctorName != null) {
				labDoctorName.Dispose ();
				labDoctorName = null;
			}
			if (labDoctorNo != null) {
				labDoctorNo.Dispose ();
				labDoctorNo = null;
			}
			if (labSignature != null) {
				labSignature.Dispose ();
				labSignature = null;
			}
			if (labTipMed != null) {
				labTipMed.Dispose ();
				labTipMed = null;
			}
			if (labTipPatientID != null) {
				labTipPatientID.Dispose ();
				labTipPatientID = null;
			}
			if (MedsCalloutView != null) {
				MedsCalloutView.Dispose ();
				MedsCalloutView = null;
			}
			if (PatientCalloutView != null) {
				PatientCalloutView.Dispose ();
				PatientCalloutView = null;
			}
		}
	}
}
