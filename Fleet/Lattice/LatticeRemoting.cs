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
}