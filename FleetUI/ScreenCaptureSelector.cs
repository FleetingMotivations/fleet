using System;
using Gdk;
using Cairo;
using Gtk;

namespace FleetUI
{
	public partial class ScreenCaptureSelector : Gtk.Window
	{
		public ScreenCaptureSelector (System.Func<Tuple<double, double, double, double>, bool> callback) 
            : base (Gtk.WindowType.Toplevel)
		{
			this.Build ();
		    this.Callback = callback;
		    this.captureArea.ExposeEvent += Draw;
            // Add the events
            this.captureArea.AddEvents(
                (int)EventMask.ButtonPressMask      | 
                (int)EventMask.ButtonReleaseMask);

		    this.captureArea.ButtonPressEvent += delegate(object o, ButtonPressEventArgs args)
		    {
		        if (!TouchdownComplete)
		        {
		            TouchdownComplete = true;
		            TouchdownCoords = new Tuple<double, double>(args.Event.X, args.Event.Y);
		        }
		        else
		        {
		            CompleteCoords = new Tuple<double, double>(args.Event.X, args.Event.Y);
                    // TODO: Deal with result
		            Callback(new Tuple<double, double, double, double>(
                        TouchdownCoords.Item1, 
                        TouchdownCoords.Item2,
                        CompleteCoords.Item1,
                        CompleteCoords.Item2));

                    this.Destroy();
		        }
		        
		    };
		}

	    private Cairo.Context DrawingArea => Gdk.CairoHelper.Create(this.captureArea.GdkWindow);

        private bool TouchdownComplete { get; set; }
        private Tuple<double, double> TouchdownCoords { get; set; }
        private Tuple<double, double> CompleteCoords { get; set; } 

        private System.Func<Tuple<double, double, double, double>, bool> Callback { get; set; }

		public void Show() {
			base.Show();
			this.Opacity = 0.6;
			this.Fullscreen ();
			this.Decorated = false;
		}

	    public void Draw(object o, Gtk.ExposeEventArgs args)
	    {
	        // TODO
	    }

	}
}

