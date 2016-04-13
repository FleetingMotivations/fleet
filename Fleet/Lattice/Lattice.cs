using System;
using Mono.Zeroconf;
using System.Runtime.Remoting.Channels.Tcp;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting;

namespace Fleet.Lattice {
	public class LatticeManager {

		//	==	==	==
		// Logger	==
		//	==	==	==



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

		public LatticeManager (String serviceName = "Fleet Workstation", Int16 port = 8080) {
			this.ServiceName = serviceName;
			this.Port = port;
		}

		~LatticeManager () {
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
				var channel = new TcpChannel (this.Port);
				var interfaceType = Type.GetType ("Fleet.Lattice.ILatticeCommunicator");

				ChannelServices.RegisterChannel (channel, secure);
				RemotingConfiguration.RegisterWellKnownServiceType (interfaceType, "LatticeCommuicator", WellKnownObjectMode.SingleCall);

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

	}

	internal class LatticeCommunicator: ILatticeCommunicator {

	}
}
