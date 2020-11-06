﻿using Foundation;
using UIKit;

using Xamarin.Forms;

namespace C1DataCollection101.iOS
{
	// The UIApplicationDelegate for the application. This class is responsible for launching the
	// User Interface of the application, as well as listening (and optionally responding) to application events from iOS.
	[Register ("AppDelegate")]
	public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
	{
		public override bool FinishedLaunching (UIApplication application, NSDictionary launchOptions)
		{
			global::Xamarin.Forms.Forms.Init();

			LoadApplication(new App()); 

			return base.FinishedLaunching(application, launchOptions);
		}
	}
}


