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
			log4net.Config.XmlConfigurator.Configure ();
			Logger.Error ("Oh no");

			Int16 port = 8080;

			if (args.Length == 1)
				port = Convert.ToInt16(args[0]);

			LatticeServiceManager workstation = new LatticeServiceManager ("Lattice Workstation", port);
			workstation.RegisterZeroconfService ();
			workstation.RegisterRemotingService ();

			Console.WriteLine ("Broadcasting Service. Press enter to consume service.");
			Console.ReadLine (); // Required for windows awfulness


            var app = new FleetApplication();
            app.Run();
			Console.WriteLine ("Bowsing Service");
			LatticeServiceDiscovery discovery = new LatticeServiceDiscovery ();
			discovery.doBrowsing ();

			Console.ReadLine ();
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
