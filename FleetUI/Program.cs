using System;
using Gtk;
using Fleet.Lattice.Discovery;

namespace FleetUI
{
	class MainClass
	{
		public static LatticeDiscovery discovery;

		public static void Main (String[] args)
		{
			try {
				discovery = new LatticeDiscovery();
				discovery.StartBrowsing();
			} catch (Exception e) {
				Console.WriteLine (e.ToString ());
			}
				
			Application.Init ();
			MainWindow win = new MainWindow ();
			win.Show ();
			Application.Run ();
		}
	}
}
