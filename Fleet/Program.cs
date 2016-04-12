using System;
using log4net;
using Fleet.AppHaulerCore.UI;
using Fleet.Application;
using Fleet.Lattice;

namespace Fleet
{
	public class MainClass
	{
		private static ILog Logger => LogManager.GetLogger (System.Reflection.MethodBase.GetCurrentMethod ().DeclaringType);

	    public static void Main (string[] args)
		{
			log4net.Config.XmlConfigurator.Configure();
			Logger.Error ("Oh no");

			LatticeWorkstation workstation = new LatticeWorkstation ();
			workstation.RegisterService ();

			Console.WriteLine ("Hello World!");
			Console.ReadLine (); // Required for windows awfulness


            var app = new FleetApplication();
            app.Run();
		}
	
        public int killAllHumans(){
            return new System.Random().Next();
        }
    }
}

namespace  Fleet.Application
{
    public class FleetApplication
    {
        public FleetApplication()
        {
            //
        }

        public void Run()
        {
            //
            var sidebar = new AppHaulerSidebar();
            sidebar.Initialize();
            sidebar.Construct();
            sidebar.Launch();
        }
    }
}
