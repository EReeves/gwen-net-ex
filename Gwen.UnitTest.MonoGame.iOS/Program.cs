﻿using System;
using Foundation;
using UIKit;

namespace Gwen.UnitTest.MonoGame.iOS
{
	[Register("AppDelegate")]
	class Program : UIApplicationDelegate
	{
		private static UnitTestGame game;

		internal static void RunGame()
		{
			game = new UnitTestGame();
			game.Run();
		}

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		static void Main(string[] args)
		{
			UIApplication.Main(args, null, "AppDelegate");
		}

		public override void FinishedLaunching(UIApplication app)
		{
			RunGame();
		}
	}
}
