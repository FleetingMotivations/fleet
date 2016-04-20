using System;
using System.Collections;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels.Tcp;
using System.Runtime.Remoting.Channels;
using log4net;
using Mono.Zeroconf;

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
		//private TcpChannel remotingChannel;

		// WCF Service
		private LatticeCommunicationService service;

		//	==	==	==	==
		// 	Constructor	==
		//	==	==	==	==

		public LatticeServiceManager (String serviceName = "Fleet Workstation", Int16 port = 8080) {
			this.ServiceName = serviceName;
			this.Port = port;

			this.service = new LatticeCommunicationService (port);
		}

		~LatticeServiceManager () {
			if (this.zeroconfService != null)
				this.zeroconfService.Dispose ();
		}

		//	==	==	==	==	==	==
		//	Zeroconf Management	==
		//	==	==	==	==	==	==

		public Boolean RegisterZeroconfService(String regtype = "_lattice._tcp", String replydomain = "local.") {

			if (this.zeroconfService == null) {
				var service = new RegisterService ();

				service.Name 		= this.ServiceName;
				service.RegType		= regtype;
				service.ReplyDomain = replydomain;
				service.Port 		= this.Port;

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

		/*public Boolean RegisterRemotingService(Boolean secure = false) {

			if (this.remotingChannel == null) {
				var provider = new BinaryServerFormatterSinkProvider ();
				provider.TypeFilterLevel = System.Runtime.Serialization.Formatters.TypeFilterLevel.Full;
				var props = new Hashtable ();
				props ["port"] = this.Port;

				var channel = new TcpChannel (props, null, provider);
				var interfaceType = typeof(Fleet.Lattice.LatticeCommunicator);

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
		}*/

		//	==	==	==	==	==	==	==
		//	Lattice Communication	==
		//	==	==	==	==	==	==	==

		public Boolean RegisterCommunicationService () {
			return this.service.RegisterServer ();
		}

		public Boolean DeregisterCommunicationService () {
			return this.service.DeregisterServer ();
		}
	}
}