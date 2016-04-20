using System;
using System.Drawing;
using System.ServiceModel;
using System.Net;
using System.ServiceModel.Channels;

namespace Fleet.Lattice
{
	[ServiceContract]
	public interface ILatticeCommunication {
		[OperationContract]
		Boolean SendText (String text);

		[OperationContract]
		Boolean SendBitmap (Bitmap bmp);
	}

	public class LatticeCommunicationService {

		private Uri address;
		private BasicHttpBinding binding;

		private ServiceHost host;

		public LatticeCommunicationService (Int16 port = 8080) {
			this.host = new ServiceHost (typeof(LatticeCommunicationServiceHost));
			this.binding = new BasicHttpBinding ();
			this.address = new Uri ("http://localhost:" + port);

			host.AddServiceEndpoint (typeof(ILatticeCommunication), binding, address);
		}

		public Boolean RegisterServer () {
			host.Open ();

			return true;
		}

		public Boolean DeregisterServer () {
			host.Close ();

			return false;
		}
	}

	// Communication Server & Host

	public class LatticeCommunicationServiceHost: ILatticeCommunication {

		public delegate void DidReceiveEvent (Object o, EventArgs args);

		public static event DidReceiveEvent DidReceiveText = delegate {};
		public static event DidReceiveEvent DidReceiveBitmap = delegate {};

		public Boolean SendText (String text) {
			Console.WriteLine ("Recevied Text: " + text);

			DidReceiveText (text, new EventArgs ());

			return true;
		}

		public Boolean SendBitmap (Bitmap img) {
			Console.WriteLine ("Received Bitmap: " + img);

			DidReceiveBitmap (img, new EventArgs ());

			return true;
		}
	}

	public class LatticeCommunicationServiceClient: ClientBase<ILatticeCommunication>, ILatticeCommunication {

		public LatticeCommunicationServiceClient (Binding binding, EndpointAddress address) : base (binding, address) {}

		public Boolean SendText (String text) {
			return Channel.SendText (text);
		}

		public Boolean SendBitmap (Bitmap bmp) {
			return Channel.SendBitmap (bmp);
		}
	}
}

