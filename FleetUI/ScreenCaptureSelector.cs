using System;
using Gdk;
using Cairo;
using Gtk;

namespace FleetUI
{
	public partial class ScreenCaptureSelector : Gtk.Window
	{
		public ScreenCaptureSelector (Func<ScreenCaptureCoords, bool> callback) 
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
		            TouchdownCoords = new Tuple<double, double>(args.Event.XRoot, args.Event.YRoot);
                    Console.WriteLine("Touchdown Point: " + TouchdownCoords);
		        }
		        else
		        {
                    this.Hide();
                    Console.WriteLine("Complete Coords: " + new Tuple<double, double>(args.Event.XRoot, args.Event.YRoot));
		            Callback(new ScreenCaptureCoords
		            {
		                TouchdownX = (int)TouchdownCoords.Item1,
                        TouchdownY = (int)TouchdownCoords.Item2,
                        CompleteX = (int)args.Event.XRoot,
                        CompleteY = (int)args.Event.YRoot
		            });

                    this.Destroy();
		        }
		    };
		}

	    private Cairo.Context DrawingArea => Gdk.CairoHelper.Create(this.captureArea.GdkWindow);

        private bool TouchdownComplete { get; set; }
        private Tuple<double, double> TouchdownCoords { get; set; }

        /// <summary>
        /// double 1 and 2 are the initial point, double
        /// </summary>
        private Func<ScreenCaptureCoords, bool> Callback { get; set; }

        public struct ScreenCaptureCoords
        {
            public int TouchdownX { get; set; }
            public int TouchdownY { get; set; }

            public int CompleteY { get; set; }
            public int CompleteX { get; set; }
        }

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

