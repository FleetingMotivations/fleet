using System;
using Gtk;
using System.ServiceModel;
using Fleet.Lattice;
using Fleet.Lattice.IPC;
using Fleet.Lattice.Discovery;
using System.IO;

namespace FleetUI
{
	class MainClass
	{
        // Service Discovery
		public static LatticeDiscovery discovery;

        // IPC Management
        public static ServiceHost ipcHost;

		public static void Main (String[] args)
		{
            // Start service discovery
			try {
				discovery = new LatticeDiscovery();
				discovery.StartBrowsing();
			} catch (Exception e) {
				Console.WriteLine (e.ToString ());
			}

            // Receive Image Event
            LatticeIPCHost.DidPassImage += (bmp, arg) =>
            {
                Console.WriteLine("Did Receive Image");
               
                // Save image to temp file
                var filename = Path.GetTempFileName() + ".jpg";
                bmp.Save(filename);
                Console.WriteLine(filename);
                
                // Load temp as pixelbuffer (GTK)
                var pixbuff = new Gdk.Pixbuf(filename);

                // Invoke new window on GTK main thread
                Application.Invoke(delegate
                {
                    var showWindow = new MainWindow();
                    showWindow.DisplayImage(pixbuff);
                    showWindow.ShowNow();
                });
                
            };

            // Register with local server. If this fails, will not be able to receive
            try
            {
                ipcHost = LatticeUtil.MakeIPCHost("LatticeViewer");
                ipcHost.Open();

                var client = LatticeUtil.MakeIPCClient();
                client.RegisterClient("LatticeViewer");
            } catch (Exception ex)
            {
                Console.WriteLine("Could not initialise IPC");
                Console.WriteLine(ex.ToString());
            }

            // Initialise GTK and start the application
            Application.Init();
            MainWindow win = new MainWindow ();
			win.Show ();
			Application.Run ();
		}
	}
}
