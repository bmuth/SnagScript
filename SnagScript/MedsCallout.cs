//
//  Bubble1.cs
//  SnagScript
//
//  Created by B. Muth on 2016-05-29.
//  Copyright (c) 2016 . All rights reserved.
//
//  Generated by PaintCode (www.paintcodeapp.com)
//



using System;
using System.Drawing;
using Foundation;
using UIKit;
using CoreGraphics;

namespace SnagScript
{
    public class MedsCallout : NSObject
    {

        //// Initialization

        static MedsCallout()
        {
        }

        //// Drawing Methods

		public static void DrawMedsCallout(UIColor contrastFill)
        {
            //// Color Declarations
            var cameraLine = UIColor.FromRGBA(0.000f, 0.000f, 0.000f, 1.000f);
            var cameraInterior = UIColor.FromRGBA(1.000f, 1.000f, 1.000f, 1.000f);
            var whiteLens = UIColor.FromRGBA(1.000f, 1.000f, 1.000f, 1.000f);
            var contrastBlue = contrastFill;
            var orangeFill = UIColor.FromRGBA(0.992f, 0.840f, 0.520f, 1.000f);

            //// Bezier 2 Drawing
            UIBezierPath bezier2Path = new UIBezierPath();
            bezier2Path.MoveTo(new CGPoint(182.0f, 55.0f));
            bezier2Path.AddCurveToPoint(new CGPoint(123.55f, 84.25f), new CGPoint(182.0f, 68.51f), new CGPoint(157.59f, 80.01f));
            bezier2Path.AddCurveToPoint(new CGPoint(32.0f, 125.0f), new CGPoint(106.0f, 108.0f), new CGPoint(32.0f, 125.0f));
            bezier2Path.AddCurveToPoint(new CGPoint(57.08f, 83.03f), new CGPoint(32.0f, 125.0f), new CGPoint(66.0f, 98.0f));
            bezier2Path.AddCurveToPoint(new CGPoint(7.0f, 55.0f), new CGPoint(27.47f, 78.06f), new CGPoint(7.0f, 67.38f));
            bezier2Path.AddCurveToPoint(new CGPoint(80.2f, 24.41f), new CGPoint(7.0f, 39.61f), new CGPoint(38.67f, 26.83f));
            bezier2Path.AddCurveToPoint(new CGPoint(94.5f, 24.0f), new CGPoint(84.85f, 24.14f), new CGPoint(89.63f, 24.0f));
            bezier2Path.AddCurveToPoint(new CGPoint(182.0f, 55.0f), new CGPoint(142.82f, 24.0f), new CGPoint(182.0f, 37.88f));
            bezier2Path.ClosePath();
            orangeFill.SetFill();
            bezier2Path.Fill();
            UIColor.Gray.SetStroke();
            bezier2Path.LineWidth = 2.5f;
            bezier2Path.Stroke();


            //// Rectangle 2 Drawing
            UIBezierPath rectangle2Path = new UIBezierPath();
            rectangle2Path.MoveTo(new CGPoint(81.58f, 46.56f));
            rectangle2Path.AddLineTo(new CGPoint(84.54f, 46.56f));
            rectangle2Path.AddCurveToPoint(new CGPoint(88.17f, 40.82f), new CGPoint(84.54f, 46.56f), new CGPoint(86.61f, 41.64f));
            rectangle2Path.AddCurveToPoint(new CGPoint(100.07f, 40.82f), new CGPoint(89.72f, 40.0f), new CGPoint(99.04f, 40.41f));
            rectangle2Path.AddCurveToPoint(new CGPoint(103.18f, 46.56f), new CGPoint(101.11f, 41.23f), new CGPoint(103.18f, 46.56f));
            rectangle2Path.AddLineTo(new CGPoint(106.92f, 46.56f));
            rectangle2Path.AddCurveToPoint(new CGPoint(110.05f, 46.75f), new CGPoint(108.53f, 46.56f), new CGPoint(109.33f, 46.56f));
            rectangle2Path.AddLineTo(new CGPoint(110.2f, 46.78f));
            rectangle2Path.AddCurveToPoint(new CGPoint(112.23f, 48.36f), new CGPoint(111.14f, 47.04f), new CGPoint(111.88f, 47.62f));
            rectangle2Path.AddCurveToPoint(new CGPoint(112.5f, 50.9f), new CGPoint(112.5f, 49.03f), new CGPoint(112.5f, 49.65f));
            rectangle2Path.AddLineTo(new CGPoint(112.5f, 59.66f));
            rectangle2Path.AddCurveToPoint(new CGPoint(112.26f, 62.1f), new CGPoint(112.5f, 60.91f), new CGPoint(112.5f, 61.53f));
            rectangle2Path.AddLineTo(new CGPoint(112.23f, 62.21f));
            rectangle2Path.AddCurveToPoint(new CGPoint(110.2f, 63.79f), new CGPoint(111.88f, 62.94f), new CGPoint(111.14f, 63.52f));
            rectangle2Path.AddCurveToPoint(new CGPoint(106.92f, 64.0f), new CGPoint(109.33f, 64.0f), new CGPoint(108.53f, 64.0f));
            rectangle2Path.AddLineTo(new CGPoint(81.58f, 64.0f));
            rectangle2Path.AddCurveToPoint(new CGPoint(78.45f, 63.81f), new CGPoint(79.97f, 64.0f), new CGPoint(79.17f, 64.0f));
            rectangle2Path.AddLineTo(new CGPoint(78.3f, 63.79f));
            rectangle2Path.AddCurveToPoint(new CGPoint(76.27f, 62.21f), new CGPoint(77.36f, 63.52f), new CGPoint(76.62f, 62.94f));
            rectangle2Path.AddCurveToPoint(new CGPoint(76.0f, 59.66f), new CGPoint(76.0f, 61.53f), new CGPoint(76.0f, 60.91f));
            rectangle2Path.AddLineTo(new CGPoint(76.0f, 50.9f));
            rectangle2Path.AddCurveToPoint(new CGPoint(76.24f, 48.46f), new CGPoint(76.0f, 49.65f), new CGPoint(76.0f, 49.03f));
            rectangle2Path.AddLineTo(new CGPoint(76.27f, 48.36f));
            rectangle2Path.AddCurveToPoint(new CGPoint(78.3f, 46.78f), new CGPoint(76.62f, 47.62f), new CGPoint(77.36f, 47.04f));
            rectangle2Path.AddCurveToPoint(new CGPoint(81.58f, 46.56f), new CGPoint(79.17f, 46.56f), new CGPoint(79.97f, 46.56f));
            rectangle2Path.ClosePath();
            cameraInterior.SetFill();
            rectangle2Path.Fill();
            cameraLine.SetStroke();
            rectangle2Path.LineWidth = 2.0f;
            rectangle2Path.Stroke();


            //// Oval 5 Drawing
            var oval5Path = UIBezierPath.FromOval(new CGRect(86.5f, 47.5f, 15.0f, 14.0f));
            whiteLens.SetFill();
            oval5Path.Fill();
            cameraLine.SetStroke();
            oval5Path.LineWidth = 1.0f;
            oval5Path.Stroke();


            //// Oval 6 Drawing
            UIBezierPath oval6Path = new UIBezierPath();
            oval6Path.MoveTo(new CGPoint(90.0f, 54.5f));
            oval6Path.AddCurveToPoint(new CGPoint(93.5f, 51.0f), new CGPoint(90.0f, 52.57f), new CGPoint(91.57f, 51.0f));
            oval6Path.LineCapStyle = CGLineCap.Round;

            UIColor.LightGray.SetStroke();
            oval6Path.LineWidth = 1.0f;
            oval6Path.Stroke();


            //// Oval 7 Drawing
            UIBezierPath oval7Path = new UIBezierPath();
            oval7Path.MoveTo(new CGPoint(59.0f, 54.5f));
            oval7Path.AddCurveToPoint(new CGPoint(44.5f, 69.0f), new CGPoint(59.0f, 62.51f), new CGPoint(52.51f, 69.0f));
            oval7Path.AddCurveToPoint(new CGPoint(30.0f, 54.5f), new CGPoint(36.49f, 69.0f), new CGPoint(30.0f, 62.51f));
            oval7Path.AddCurveToPoint(new CGPoint(44.5f, 40.0f), new CGPoint(30.0f, 46.49f), new CGPoint(36.49f, 40.0f));
            oval7Path.AddCurveToPoint(new CGPoint(59.0f, 54.5f), new CGPoint(52.51f, 40.0f), new CGPoint(59.0f, 46.49f));
            oval7Path.ClosePath();
            whiteLens.SetFill();
            oval7Path.Fill();
            UIColor.Black.SetStroke();
            oval7Path.LineWidth = 2.0f;
            oval7Path.Stroke();


            //// Oval 8 Drawing
            UIBezierPath oval8Path = new UIBezierPath();
            oval8Path.MoveTo(new CGPoint(55.0f, 54.5f));
            oval8Path.AddCurveToPoint(new CGPoint(44.5f, 65.0f), new CGPoint(55.0f, 60.3f), new CGPoint(50.3f, 65.0f));
            oval8Path.AddCurveToPoint(new CGPoint(44.5f, 54.5f), new CGPoint(44.5f, 65.0f), new CGPoint(44.5f, 60.3f));
            oval8Path.AddCurveToPoint(new CGPoint(44.5f, 44.0f), new CGPoint(44.5f, 48.7f), new CGPoint(44.5f, 44.0f));
            oval8Path.AddCurveToPoint(new CGPoint(55.0f, 54.5f), new CGPoint(50.3f, 44.0f), new CGPoint(55.0f, 48.7f));
            oval8Path.ClosePath();
            contrastBlue.SetFill();
            oval8Path.Fill();


            //// Bezier Drawing
            UIBezierPath bezierPath = new UIBezierPath();
            bezierPath.MoveTo(new CGPoint(143.64f, 45.62f));
            bezierPath.AddLineTo(new CGPoint(145.2f, 51.0f));
            bezierPath.AddLineTo(new CGPoint(150.65f, 52.54f));
            bezierPath.AddLineTo(new CGPoint(157.66f, 45.62f));
            bezierPath.AddCurveToPoint(new CGPoint(149.09f, 57.54f), new CGPoint(160.0f, 51.0f), new CGPoint(154.9f, 57.54f));
            bezierPath.AddCurveToPoint(new CGPoint(144.95f, 56.7f), new CGPoint(147.62f, 57.54f), new CGPoint(146.22f, 57.24f));
            bezierPath.AddCurveToPoint(new CGPoint(137.61f, 64.44f), new CGPoint(142.71f, 59.55f), new CGPoint(137.61f, 64.44f));
            bezierPath.AddCurveToPoint(new CGPoint(134.29f, 66.0f), new CGPoint(137.61f, 64.44f), new CGPoint(135.63f, 66.0f));
            bezierPath.AddCurveToPoint(new CGPoint(130.0f, 61.77f), new CGPoint(131.92f, 66.0f), new CGPoint(130.0f, 64.11f));
            bezierPath.AddCurveToPoint(new CGPoint(132.05f, 58.16f), new CGPoint(130.0f, 60.24f), new CGPoint(130.82f, 58.91f));
            bezierPath.AddCurveToPoint(new CGPoint(139.94f, 52.28f), new CGPoint(132.52f, 57.88f), new CGPoint(137.12f, 54.26f));
            bezierPath.AddCurveToPoint(new CGPoint(138.57f, 47.15f), new CGPoint(139.07f, 50.77f), new CGPoint(138.57f, 49.02f));
            bezierPath.AddCurveToPoint(new CGPoint(149.09f, 39.46f), new CGPoint(138.57f, 41.42f), new CGPoint(143.64f, 38.69f));
            bezierPath.AddLineTo(new CGPoint(143.64f, 45.62f));
            bezierPath.ClosePath();
            whiteLens.SetFill();
            bezierPath.Fill();
            UIColor.Black.SetStroke();
            bezierPath.LineWidth = 2.0f;
            bezierPath.Stroke();
        }

        //// Generated Images

		public static UIImage ImageOfMedsCallout (UIColor clr)
		{
			UIGraphics.BeginImageContextWithOptions (new CGSize (188.0f, 137.0f), false, 0);
			DrawMedsCallout (clr);

			UIImage imageOfMedsCallout = UIGraphics.GetImageFromCurrentImageContext ();
			UIGraphics.EndImageContext ();

			return imageOfMedsCallout;
		}
    }
}
