using System;
using System.Linq;
using Fleet.Lattice;
using System.Net;
using System.ServiceModel;
using System.Drawing;
using System.Windows.Forms;

namespace Lattice.Test
{
	class MainClass
	{
		private static LatticeServiceDiscovery discovery;
		private static LatticeServiceManager manager;

		public static void Main (string[] args)
		{
			Console.WriteLine ("Lattice Test Console");

			LatticeCommunicationServiceHost.DidReceiveBitmap += (o, arg) => {

				try {
					var bmp = (Bitmap) o;

					var saveDialog = new SaveFileDialog ();
					saveDialog.ShowDialog ();

					bmp.Save (saveDialog.FileName);
				} catch (Exception e) {
					Console.WriteLine (e.ToString ());
				}

			};

			while (true) {
				PrintOptions ();

				try {
					String input = Console.ReadLine ();
					Int16 option = Convert.ToInt16 (input);

					switch (option) {
					case 1: SystemStatus ();
							break;
					case 2: ToggleBroadcast ();
							break;
					case 3: ToggleDiscovery ();
							break;
					case 4: SendText ();
							break;
					case 5: SendImage ();
							break;
					case 6: PrintResolvedServices ();
							break;
					case 0: Environment.Exit (0);
						break;
					default:Console.WriteLine ("Invalid Input");
							break;
					}

				} catch (Exception e) {
					Console.WriteLine (e.ToString ());
				}


			}
		}

		private static void PrintOptions () {
			Console.WriteLine ("(1) - System Status");
			Console.WriteLine ("(2) - Toggle Broadcast");
			Console.WriteLine ("(3) - Toggle Discovery");
			Console.WriteLine ("(4) - Send Text");
			Console.WriteLine ("(5) - Send Image");
			Console.WriteLine ("(6) - Print Resolved Services");
			Console.WriteLine ("(0) - Exit");

			Console.Write ("--> ");
		}

		private static void SystemStatus () {
			Console.WriteLine ("** System Status **");

			if (manager == null)
				Console.WriteLine ("Broadcast: off");
			else
				Console.WriteLine ("Broadcast: on");

			if (discovery == null) 
				Console.WriteLine ("Discovery: off");
			else
				Console.WriteLine ("Discovery: on");

			Console.WriteLine ();
		}

		private static void ToggleBroadcast () {
			Console.WriteLine ("** Toggle Broadcast **");

			if (manager == null) {
				manager = new LatticeServiceManager ();
				manager.RegisterZeroconfService ();
				//manager.RegisterRemotingService ();
				manager.RegisterCommunicationService ();

				Console.WriteLine ("Broadcast Started");

			} else {
				manager.DeregisterZeroconfService ();
				//manager.DeregisterRemotingService ();
				manager.DeregisterCommunicationService ();
				manager = null;

				Console.WriteLine ("Broadcast Stopped");
			}

			Console.WriteLine ();
		}

		private static void ToggleDiscovery () {
			Console.WriteLine ("** Toggle Discovery **");

			if (discovery == null) {
				discovery = new LatticeServiceDiscovery ();
				discovery.StartBrowsing ();

				Console.WriteLine ("Discovery Started");

			} else {
				discovery.StopBrowsing ();
				discovery = null;

				Console.WriteLine ("Discovery Stopped");
			}

			Console.WriteLine ();
		}

		private static ServiceRecord GetServiceRecord (Int16 serviceIndex) {
			var services = discovery.CurrentRecords;
			if (serviceIndex >= services.Count) {
				Console.WriteLine ("That is not a valid input");
				return new ServiceRecord ();
			}

			ServiceRecord service;
			Int16 counter = 0;

			foreach (var record in services) {
				if (counter == serviceIndex) {
					service = record.Value;
				}
				counter++;
			}

			return service;
		}

		private static void SendText () {
			Console.WriteLine ("** Send Text **");

			Console.WriteLine ("Please enter a line of text to send");
			Console.Write ("--> ");

			String text = Console.ReadLine ();

			Console.WriteLine ();
			Console.WriteLine ("Please select a service out of the following list");
			PrintResolvedServices ();

			String serviceInput = Console.ReadLine ();
			Int16 serviceIndex = Convert.ToInt16 (serviceInput);

			ServiceRecord service = GetServiceRecord (serviceIndex);

			//Type comType = typeof(Fleet.Lattice.ILatticeCommunicator);
			//ILatticeCommunicator communicator = (ILatticeCommunicator) Activator.GetObject (comType, "tcp://" + service.Hostname + ":" + service.Port + "/LatticeCommunicator");

			var address = new EndpointAddress ("http://" + service.Hostname + ":" + service.Port);
			var binding = new BasicHttpBinding ();

			var client = new LatticeCommunicationServiceClient (binding, address);

			Console.WriteLine ("Sending to service " + service);

			//communicator.ShareString (text);
			client.SendText (text);
			
			Console.WriteLine ();
		}

		private static void SendImage () {
			Console.WriteLine ("** Send Image **");

			Console.WriteLine ("Please select a service out of the following list");
			PrintResolvedServices ();

			String serviceInput = Console.ReadLine ();
			Int16 serviceIndex = Convert.ToInt16 (serviceInput);

			ServiceRecord service = GetServiceRecord (serviceIndex);

			var address = new EndpointAddress ("http://" + service.Hostname + ":" + service.Port);
			var binding = new BasicHttpBinding ();

			var client = new LatticeCommunicationServiceClient (binding, address);

			var openFileDialog = new OpenFileDialog ();
			openFileDialog.ShowDialog ();

			var bmp = (Bitmap) Bitmap.FromFile (openFileDialog.FileName);

			Console.WriteLine ("Sending to service " + service);

			client.SendBitmap (bmp);

			Console.WriteLine ();
		
		}

		private static void PrintResolvedServices () {
			Console.WriteLine ("** Discovered Services **");

			if (discovery != null) {
				var services = discovery.CurrentRecords;

				Int16 i = 0;
				foreach (var recordpair in services) {
					var record = recordpair.Value;
					Console.WriteLine (i++ + " - " + record.ToString ());
				}

			} else {
				Console.WriteLine ("Discovery service not started");
			}

			Console.WriteLine ();
		}
	}
}
