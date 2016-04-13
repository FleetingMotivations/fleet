using System;
using Mono.Zeroconf;
using System.Runtime.Remoting.Channels.Tcp;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting;
using log4net;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections;

namespace Fleet.Lattice {
	public class LatticeServiceManager {

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

		// Service Configuration
		private String ServiceName { get; set;}
		private Int16 Port { get; set; }

		// Zeroconf Service
		private IRegisterService zeroconfService;

		// Remoting Channel
		private TcpChannel remotingChannel;

		//	==	==	==	==
		// 	Constructor	==
		//	==	==	==	==

		public LatticeServiceManager (String serviceName = "Fleet Workstation", Int16 port = 8080) {
			this.ServiceName = serviceName;
			this.Port = port;
		}

		~LatticeServiceManager () {
			if (this.zeroconfService != null)
				this.zeroconfService.Dispose ();
		}

		//	==	==	==	==	==	==
		//	Zeroconf Management	==
		//	==	==	==	==	==	==

		public Boolean RegisterZeroconfService() {

			if (this.zeroconfService == null) {
				var service = new RegisterService ();

				service.Name 		= this.ServiceName;
				service.RegType 	= "_lattice._tcp";
				service.ReplyDomain = "local.";
				service.Port 		= this.Port;

				//service.TxtRecord = new TxtRecord ();

				service.Register ();

				this.zeroconfService = service;
				return true;
			}

			return false;
		}

		public Boolean DeregisterZeroconfService() {
			if (this.zeroconfService != null) {
				this.zeroconfService.Dispose ();
				this.zeroconfService = null;
			}

			return false;
		}

		//	==	==	==	==	==	==
		//	Remoting Management	==
		//	==	==	==	==	==	==

		public Boolean RegisterRemotingService(Boolean secure = false) {

			if (this.remotingChannel == null) {
				var provider = new BinaryServerFormatterSinkProvider ();
				provider.TypeFilterLevel = System.Runtime.Serialization.Formatters.TypeFilterLevel.Full;
				var props = new Hashtable ();
				props ["port"] = this.Port;

				var channel = new TcpChannel (props, null, provider);
				var interfaceType = Type.GetType ("Fleet.Lattice.LatticeCommunicator");

				ChannelServices.RegisterChannel (channel, secure);
				RemotingConfiguration.RegisterWellKnownServiceType (interfaceType, "LatticeCommunicator", WellKnownObjectMode.SingleCall);

				var record = new TxtRecordItem ("Lattice_ServiceName", "LatticeCommunicator");
				this.zeroconfService.TxtRecord?.Add (record);

				this.remotingChannel = channel;
			}

			return false;
		}

		public Boolean DeregisterRemotingService() {

			if (this.remotingChannel != null) {
				ChannelServices.UnregisterChannel (this.remotingChannel);
				this.remotingChannel = null;
			}

			return false;
		}
	}

	//	==	==	==	==	==
	//	Remoting Object	==
	//	==	==	==	==	==

	public interface ILatticeCommunicator {
		Boolean ShareImage (Image img);
		Boolean ShareString (String str);
		Boolean ShareStream (Stream stream);
	}

	internal class LatticeCommunicator: MarshalByRefObject, ILatticeCommunicator {

		public Boolean ShareImage (Image img) {

			Console.WriteLine ("Received Image: " + img);

			return true;
		}

		public Boolean ShareString(String text) {

			Console.WriteLine ("Received Text: " + text);

			return true;
		}

		public Boolean ShareStream (Stream stream) {

			Console.WriteLine ("Received Stream: " + stream);

			return true;
		}

	}

	//	==	==	==	==	==	==
	//	Service Discovery	==
	//	==	==	==	==	==	==

	public class LatticeServiceDiscovery {

		public void doBrowsing() {
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
			ILatticeCommunicator communicator = (ILatticeCommunicator)Activator.GetObject (comType, "tcp://" + host + ":" + port + "/LatticeCommunicator");

			communicator.ShareString ("Hello World");

			var dialog = new OpenFileDialog ();
			dialog.ShowDialog ();
			var filename = dialog.FileName;

			var img = Image.FromFile (filename);
			communicator.ShareImage (img);
		}
	}
}
