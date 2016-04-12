using System;
using log4net;

namespace Fleet
{
	class MainClass
	{
		private static ILog Logger {
			get {
				return LogManager.GetLogger (System.Reflection.MethodBase.GetCurrentMethod ().DeclaringType);

			}
		}

		public static void Main (string[] args)
		{
			log4net.Config.XmlConfigurator.Configure();
			Logger.Error ("Oh no");

			Console.WriteLine ("Hello World!");
			Console.ReadLine (); // Required for windows awfulness
		}
	}
}
