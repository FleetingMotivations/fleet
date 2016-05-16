using System;

namespace FleetUI
{
	public partial class ShareSelectionWindow : Gtk.Window
	{
		public ShareSelectionWindow () :
			base (Gtk.WindowType.Toplevel)
		{
			this.Build ();
		}
	}
}

