using System;
using System.IO;
using System.Drawing;

namespace Fleet.Lattice {
	//	==	==	==	==	==
	//	Remoting Object	==
	//	==	==	==	==	==

	public interface ILatticeCommunicator {
		Boolean ShareImage (Image img);
		Boolean ShareString (String str);
		Boolean ShareStream (Stream stream);
	}

	internal class LatticeCommunicator: MarshalByRefObject, ILatticeCommunicator {

		public delegate void ReceiveDelegate (Object o, EventArgs args);

		public static event ReceiveDelegate DidReceiveImage;
		public static event ReceiveDelegate DidReceiveString;
		public static event ReceiveDelegate DidReceiveStream;

		public Boolean ShareImage (Image img) {

			Console.WriteLine ("Received Image: " + img);

			DidReceiveImage (img, null);

			return true;
		}

		public Boolean ShareString(String text) {

			Console.WriteLine ("Received Text: " + text);

			DidReceiveString (text, null);

			return true;
		}

		public Boolean ShareStream (Stream stream) {

			Console.WriteLine ("Received Stream: " + stream);

			DidReceiveStream (stream, null);

			return true;
		}

	}
}