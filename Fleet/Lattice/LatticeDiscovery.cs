using System;
using Mono.Zeroconf;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using log4net;
using log4net.Core;

namespace Fleet.Lattice {
	//	==	==	==	==	==	==
	//	Service Discovery	==
	//	==	==	==	==	==	==

	public class LatticeServiceDiscovery {

		//	==	==	==
		// Logger	==
		//	==	==	==

		private static ILog _logger;
		private static ILog Logger {
			get {
				if (_logger == null)
					_logger = LogManager.GetLogger (System.Reflection.MethodBase.GetCurrentMethod ().DeclaringType);
				return _logger;
			}
		}

		//	==	==	==
		//	Members	==
		//	==	==	==

		private IDictionary<String, ServiceRecord> records;
		private ServiceBrowser browser;
		private Object @lock = new Object ();

		public IReadOnlyDictionary<String, ServiceRecord> CurrentRecords {
			get {
				lock (@lock) {
					return new ReadOnlyDictionary<String, ServiceRecord> (this.records);
				}
			}
		}

		//	==	==	==	==
		//	Constructor	==
		//	==	==	==	==

		public LatticeServiceDiscovery () {
			this.records = new Dictionary<String, ServiceRecord> ();
		}

		~LatticeServiceDiscovery () {
			this.browser?.Dispose ();
		}

		//	==	==	==	==	==	==	==
		//	Discovery Management	==
		//	==	==	==	==	==	==	==

		public Boolean StartBrowsing (String regtype = "_lattice._tcp", String domain = "local") {

			if (this.browser == null) {
				var browser = new ServiceBrowser ();

				browser.ServiceAdded += OnServiceAdded;
				browser.ServiceRemoved += OnServiceRemoved;

				browser.Browse (regtype, domain);
				this.browser = browser;

				return true;
			}

			return false;
		}

		public Boolean StopBrowsing () {

			if (this.browser != null) {
				this.browser.Dispose ();
				this.browser = null;

				return true;
			}

			return false;
		}

		//	==	==	==	==	==	==	==	==
		//	Service Delegate Methods	==
		//	==	==	==	==	==	==	==	==
			
		private void OnServiceAdded (Object o, ServiceBrowseEventArgs args) {
			var service = args.Service;
			var record = new ServiceRecord ();

			record.Hostname = service.HostEntry.AddressList[0].ToString ();
			record.Port = service.Port;
			record.ServiceName = service.Name;

			var key = record.Hostname + ":" + record.Port;

			lock (@lock) {
				this.records [key] = record;
			}

			Logger.Debug ("Added Service: " + record);
		}

		private void OnServiceRemoved (Object o, ServiceBrowseEventArgs args) {

			var hostname = args.Service.HostEntry.AddressList [0].ToString ();
			var port = args.Service.Port;

			var key = hostname + ":" + port;

			lock (@lock) {
				var record = this.records [key];
				Logger.Debug ("Removed Service: " + record);

				this.records.Remove (key);
			}
		}
	}

	//	==	==	==	==	==
	//	Service Record	==
	//	==	==	==	==	==

	public struct ServiceRecord {
		public String Hostname { get; set; }
		public Int16  Port { get; set; }
		public String ServiceName { get; set; }

		override public String ToString () {
			return this.ServiceName + " - " + this.Hostname + ":" + this.Port;
		}
	}

	//	==	==
	//	Old	==
	//	==	==

	public class LatticeDiscovery {

		public void DoBrowsing() {
			var browser = new ServiceBrowser ();

			browser.ServiceAdded += OnServiceAdded;

			browser.Browse ("_lattice._tcp", "local");
		}

		private void OnServiceAdded (Object o, ServiceBrowseEventArgs args) {
			var service = args.Service;
			Console.WriteLine ("Resolved Service: " + service.FullName);

			service.Resolved += OnServiceResolved;

			service.Resolve ();
		}

		private void OnServiceResolved(Object o, ServiceResolvedEventArgs args) {
			var service = args.Service;
			Int16  port = service.Port;
			String host = service.HostEntry.AddressList [0].ToString ();

			Console.WriteLine ("Resolved Service: {0} - {1}:{2} ({3} Text Record(s))", service.FullName, host, port, service.TxtRecord.Count);

			Type comType = Type.GetType ("Fleet.Lattice.ILatticeCommunicator");
			ILatticeCommunicator communicator = (ILatticeCommunicator) Activator.GetObject (comType, "tcp://" + host + ":" + port + "/LatticeCommunicator");

			communicator.ShareString ("Hello World");

			var dialog = new OpenFileDialog ();
			dialog.ShowDialog ();
			var filename = dialog.FileName;

			var img = Image.FromFile (filename);
			communicator.ShareImage (img);
		}
	}
}
