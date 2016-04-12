using System;
using Mono.Zeroconf;

namespace Fleet.Lattice {
	public class LatticeWorkstation {

		//	==	==	==
		//	Members	==
		//	==	==	==

		// Service Configuration
		private String ServiceName { get; set;}
		private Int16 Port { get; set; }

		// Zeroconf Service
		private IRegisterService zeroconfService;

		//	==	==	==	==
		// 	Constructor	==
		//	==	==	==	==

		public LatticeWorkstation (String serviceName = "Fleet Workstation", Int16 port = 8080) {
			this.ServiceName = serviceName;
			this.Port = port;
		}

		~LatticeWorkstation () {
			if (this.zeroconfService != null)
				this.zeroconfService.Dispose ();
		}

		//	==	==	==	==	==	==
		//	Zeroconf Management	==
		//	==	==	==	==	==	==

		public Boolean RegisterService() {

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

		public Boolean DeregisterService() {
			if (this.zeroconfService != null) {
				this.zeroconfService.Dispose ();
				this.zeroconfService = null;
			}

			return false;
		}

		//	==	==	==	==	==	==
		//	Remoting Management	==
		//	==	==	==	==	==	==


	}
}
